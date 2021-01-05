using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
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

        public IPropertyType GetPropertyType()
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

        public int GetDevOpsId([NotNull] IMitigation mitigation, [NotNull] IDevOpsConnector connector)
        {
            int result = -1;

            var info = GetInfo(mitigation);
            if (info != null)
            {
                var connectionInfo = GetConnectionInfo<DevOpsWorkItemConnectionInfo>(info, connector);
                if (connectionInfo != null)
                {
                    result = connectionInfo.Id;
                }
            }

            return result;
        }

        public WorkItemStatus GetDevOpsStatus([NotNull] IMitigation mitigation, [NotNull] IDevOpsConnector connector)
        {
            var result = WorkItemStatus.Unknown;

            var info = GetInfo(mitigation);
            if (info != null)
            {
                var connectionInfo = GetConnectionInfo<DevOpsWorkItemConnectionInfo>(info, connector);
                if (connectionInfo != null)
                {
                    result = connectionInfo.Status;
                }
            }

            return result;
        }

        public void SetDevOpsStatus([NotNull] IMitigation mitigation, 
            [NotNull] IDevOpsConnector connector, int id, WorkItemStatus status)
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
                        ConnectorId = connector.GetExtensionId(),
                        Url = connector.Url,
                        Project = connector.Project,
                        Status = status
                    });
                }
            }
            else
            {
                SetInfo(mitigation, new DevOpsWorkItemConnectionInfo()
                {
                    Id = id,
                    ConnectorId = connector.GetExtensionId(),
                    Url = connector.Url,
                    Project = connector.Project,
                    Status = status
                });
            }
        }

        private DevOpsInfo GetInfo([NotNull] IPropertiesContainer container)
        {
            DevOpsInfo result = null;

            var propertyType = GetPropertyType();
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
                var propertyType = GetPropertyType();
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
                .FirstOrDefault(x => string.CompareOrdinal(x.ConnectorId, connector.GetExtensionId()) == 0 &&
                                     string.CompareOrdinal(x.Url, connector.Url) == 0 &&
                                     string.CompareOrdinal(x.Project, connector.Project) == 0) as T;
        }
    }
}