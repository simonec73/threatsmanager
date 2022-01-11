using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Review;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Schemas
{
    public class DevOpsPropertySchemaManager
    {
        private IThreatModel _model;

        public DevOpsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetPropertySchema()
        {
            IPropertySchema result = _model.GetSchema(Properties.Resources.DevOpsPropertySchema,
                Properties.Resources.DefaultNamespace) ?? _model.AddSchema(Properties.Resources.DevOpsPropertySchema,
                Properties.Resources.DefaultNamespace);
            result.Description = Properties.Resources.DevOpsPropertySchemaDescription;
            result.Visible = false;
            result.AppliesTo = Scope.Mitigation;
            result.System = true;
            result.AutoApply = false;
            result.NotExportable = true;

            return result;
        }

        #region DevOpsInfo.
        public IPropertyType GetDevOpsInfoPropertyType()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.DevOpsInfo) ?? schema.AddPropertyType(Properties.Resources.DevOpsInfo, PropertyValueType.JsonSerializableObject);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Properties.Resources.DevOpsInfoDescription;
            }

            return result;
        }

        public DevOpsWorkItemConnectionInfo GetDevOpsInfo([NotNull] IMitigation mitigation, [NotNull] IDevOpsConnector connector)
        {
            DevOpsWorkItemConnectionInfo result = null;

            var info = GetInfo(mitigation);
            if (info != null)
            {
                result = GetConnectionInfo<DevOpsWorkItemConnectionInfo>(info, connector);
            }

            return result;
        }

        public void RemoveDevOpsInfos([NotNull] IMitigation mitigation)
        {
            var propertyType = GetDevOpsInfoPropertyType();
            if (propertyType != null)
            {
                mitigation.RemoveProperty(propertyType);
            }
        }

        public void SetDevOpsStatus([NotNull] IMitigation mitigation, 
            [NotNull] IDevOpsConnector connector, int id, string itemUrl, string assignedTo, WorkItemStatus status)
        {
            var info = GetInfo(mitigation);
            if (info != null)
            {
                var connectionInfo = GetConnectionInfo<DevOpsWorkItemConnectionInfo>(info, connector);
                if (connectionInfo != null)
                {
                    connectionInfo.Status = status;
                    connectionInfo.AssignedTo = assignedTo;
                }
                else
                {
                    SetInfo(mitigation, new DevOpsWorkItemConnectionInfo()
                    {
                        Id = id,
                        ConnectorId = connector.FactoryId,
                        Url = connector.Url,
                        AssignedTo = assignedTo,
                        Project = connector.Project,
                        Status = status,
                        ItemUrl = itemUrl
                    });
                }
            }
            else
            {
                SetInfo(mitigation, new DevOpsWorkItemConnectionInfo()
                {
                    Id = id,
                    ConnectorId = connector.FactoryId,
                    Url = connector.Url,
                    AssignedTo = assignedTo,
                    Project = connector.Project,
                    Status = status,
                    ItemUrl = itemUrl
                });
            }
        }

        public void SetDevOpsStatus([NotNull] IMitigation mitigation, 
            [NotNull] IDevOpsConnector connector, int id, string itemUrl, WorkItemStatus status)
        {
            var info = GetInfo(mitigation);
            if (info != null)
            {
                var connectionInfo = GetConnectionInfo<DevOpsWorkItemConnectionInfo>(info, connector);
                if (connectionInfo != null)
                {
                    connectionInfo.Status = status;
                }
                else
                {
                    SetInfo(mitigation, new DevOpsWorkItemConnectionInfo()
                    {
                        Id = id,
                        ConnectorId = connector.FactoryId,
                        Url = connector.Url,
                        Project = connector.Project,
                        Status = status,
                        ItemUrl = itemUrl
                    });
                }
            }
            else
            {
                SetInfo(mitigation, new DevOpsWorkItemConnectionInfo()
                {
                    Id = id,
                    ConnectorId = connector.FactoryId,
                    Url = connector.Url,
                    Project = connector.Project,
                    Status = status,
                    ItemUrl = itemUrl
                });
            }
        }

        private DevOpsInfo GetInfo([NotNull] IPropertiesContainer container)
        {
            DevOpsInfo result = null;

            var propertyType = GetDevOpsInfoPropertyType();
            if (propertyType != null)
            {
                result = (container.GetProperty(propertyType) as IPropertyJsonSerializableObject)?.Value as DevOpsInfo;
            }

            return result;
        }

        private void SetInfo([NotNull] IPropertiesContainer container, DevOpsConnectionInfo connectionInfo)
        {
            var info = GetInfo(container);
            if (info == null)
            {
                var propertyType = GetDevOpsInfoPropertyType();
                if (propertyType != null)
                {
                    var property = (container.GetProperty(propertyType) as IPropertyJsonSerializableObject) ??
                                   container.AddProperty(propertyType, null) as IPropertyJsonSerializableObject;
                    if (property != null)
                    {
                        info = new DevOpsInfo();
                        info.Infos = new List<DevOpsConnectionInfo>();
                        info.Infos.Add(connectionInfo);
                        property.Value = info;
                    }
                }
            }
            else
            {
                var existing = info.Infos?
                    .FirstOrDefault(x => string.CompareOrdinal(x.ConnectorId, connectionInfo.ConnectorId) == 0 &&
                                         string.CompareOrdinal(x.Url?.ToLower(), connectionInfo.Url?.ToLower()) == 0 &&
                                         string.CompareOrdinal(x.Project, connectionInfo.Project) == 0);
                if (existing != null)
                    info.Infos.Remove(existing);
                if (info.Infos == null)
                    info.Infos = new List<DevOpsConnectionInfo>();
                info.Infos.Add(connectionInfo);
            }
        }

        private T GetConnectionInfo<T>([NotNull] DevOpsInfo devOpsInfo, [NotNull] IDevOpsConnector connector) where T : DevOpsConnectionInfo
        {
            return devOpsInfo?.Infos?
                .FirstOrDefault(x => string.CompareOrdinal(x.ConnectorId, connector.FactoryId) == 0 &&
                                     string.CompareOrdinal(x.Url, connector.Url) == 0 &&
                                     string.CompareOrdinal(x.Project, connector.Project) == 0) as T;
        }
        #endregion

        #region Review.
        public IPropertyType GetReviewPropertyType()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.Review) ?? schema.AddPropertyType(Properties.Resources.Review, PropertyValueType.JsonSerializableObject);
                result.Visible = true;
                result.DoNotPrint = true;
                result.CustomPropertyViewer = "Backlog Review Property Viewer";
                result.Description = Properties.Resources.ReviewDescription;
            }

            return result;
        }

        public ReviewInfo GetReview([NotNull] IMitigation mitigation)
        {
            ReviewInfo result = null;

            var propertyType = GetReviewPropertyType();
            if (propertyType != null)
            {
                var property = mitigation.GetProperty(propertyType) ?? mitigation.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is ReviewInfo reviewInfo)
                {
                    result = reviewInfo;
                }
            }

            return result;
        }

        public void SetReview([NotNull] IMitigation mitigation, [NotNull] ReviewInfo info)
        {
            ReviewInfo result = null;

            var propertyType = GetReviewPropertyType();
            if (propertyType != null)
            {
                var property = mitigation.GetProperty(propertyType) ?? mitigation.AddProperty(propertyType, null);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    jsonSerializableObject.Value = info;
                }
            }
        }
        #endregion

        #region Iterations.
        public IPropertyType GetIterationFirstSeenPropertyType()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.IterationFirstSeen) ?? schema.AddPropertyType(Properties.Resources.IterationFirstSeen, PropertyValueType.JsonSerializableObject);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Properties.Resources.IterationFirstSeenDescription;
            }

            return result;
        }

        public IPropertyType GetMitigationExposurePropertyType()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.IterationMitigationExposure) ?? schema.AddPropertyType(Properties.Resources.IterationMitigationExposure, PropertyValueType.SingleLineString);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Properties.Resources.IterationMitigationExposureDescription;
            }

            return result;
        }

        public IterationInfo GetFirstSeenOn([NotNull] IMitigation mitigation)
        {
            IterationInfo result = null;

            var propertyType = GetIterationFirstSeenPropertyType();
            if (propertyType != null)
            {
                var property = mitigation.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is IterationInfo iterationInfo && iterationInfo.IsValid)
                {
                    result = iterationInfo;
                }
            }

            return result;
        }

        public void SetFirstSeenOn([NotNull] IMitigation mitigation, Iteration info)
        {
            var iterationFirstSeenPropertyType = GetIterationFirstSeenPropertyType();
            var mitigationExposurePropertyType = GetMitigationExposurePropertyType();
            if (iterationFirstSeenPropertyType != null && mitigationExposurePropertyType != null)
            {
                var propertyFirstSeen = mitigation.GetProperty(iterationFirstSeenPropertyType) ?? mitigation.AddProperty(iterationFirstSeenPropertyType, null);
                var propertyMitigationExposure = mitigation.GetProperty(mitigationExposurePropertyType) ?? mitigation.AddProperty(mitigationExposurePropertyType, null);
                if (propertyFirstSeen is IPropertyJsonSerializableObject firstSeen &&
                    propertyMitigationExposure is IPropertySingleLineString mitigationExposure)
                {
                    if (info == null)
                    {
                        firstSeen.StringValue = null;
                        mitigationExposure.StringValue = null;
                    }
                    else
                    {
                        firstSeen.Value = new IterationInfo(info);
                        mitigationExposure.StringValue = GetMitigationExposure(mitigation);
                    }
                }
            }
        }

        public bool HasChanged([NotNull] IMitigation mitigation)
        {
            var result = false;

            var mitigationExposurePropertyType = GetMitigationExposurePropertyType();
            if (mitigationExposurePropertyType != null)
            {
                var propertyMitigationExposure = mitigation.GetProperty(mitigationExposurePropertyType);
                if (propertyMitigationExposure is IPropertySingleLineString mitigationExposure)
                {
                    result = string.CompareOrdinal(mitigationExposure.StringValue, GetMitigationExposure(mitigation)) != 0;
                }
            }

            return result;
        }

        private string GetMitigationExposure([NotNull] IMitigation mitigation)
        {
            return _model.GetThreatEventMitigations(mitigation)?
                .Select(x => x.ThreatEvent.Id.ToString("N"))
                .OrderBy(x => x)
                .TagConcat();
        }
        #endregion
    }
}