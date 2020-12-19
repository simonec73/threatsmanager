using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Schemas
{
    public class DevOpsConfigPropertySchemaManager
    {
        private IThreatModel _model;

        public DevOpsConfigPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetPropertySchema()
        {
            IPropertySchema result = _model.GetSchema(Properties.Resources.DevOpsConfigPropertySchema,
                Properties.Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(Properties.Resources.DevOpsConfigPropertySchema,
                    Properties.Resources.DefaultNamespace);
                result.Description = Properties.Resources.DevOpsConfigPropertySchemaDescription;
                result.Visible = false;
                result.AppliesTo = Scope.ThreatModel;
                result.System = true;
                result.AutoApply = false;
                result.NotExportable = true;
            }

            return result;
        }

        public IPropertyType GetPropertyType()
        {
            IPropertyType result = null;

            var schema = GetPropertySchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Properties.Resources.DevOpsConfig);
                if (result == null)
                {
                    result = schema.AddPropertyType(Properties.Resources.DevOpsConfig, PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Properties.Resources.DevOpsConfigDescription;
                }
            }

            return result;
        }

        public IDevOpsConnector GetDevOpsConnector([NotNull] IThreatModel model, out string url, out string project)
        {
            IDevOpsConnector result = null;
            url = null;
            project = null;

            var propertyType = GetPropertyType();

            if (propertyType != null)
            {
                var property = model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is DevOpsConnection connection)
                {
                    result = ExtensionUtils.GetExtension<IDevOpsConnectorFactory>(connection.ExtensionId)?.Create();
                    if (result != null)
                    {
                        url = connection.Url;
                        project = connection.Project;

                        result.MasterParent = connection.MasterParent;
                        result.Tag = connection.Tag;
                        result.WorkItemType = connection.WorkItemType;

                        var workItemStateMappings = connection.WorkItemStateMappings;
                        if (workItemStateMappings?.Any() ?? false)
                        {
                            foreach (var mapping in workItemStateMappings)
                            {
                                result.SetWorkItemStateMapping(mapping.Field, mapping.Status);
                            }
                        }

                        var workItemFieldMappings = connection.WorkItemFieldMappings;
                        if (workItemFieldMappings?.Any() ?? false)
                        {
                            foreach (var mapping in workItemFieldMappings)
                            {
                                result.SetWorkItemFieldMapping(mapping.Field, mapping.GetMappedField(model));
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void RegisterConnection([NotNull] IThreatModel model, [NotNull] IDevOpsConnector connector)
        {
            var propertyType = GetPropertyType();

            if (propertyType != null)
            {
                var property = model.GetProperty(propertyType) ?? model.AddProperty(propertyType, null);

                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    jsonSerializableObject.Value = new DevOpsConnection(connector);
                }
            }
        }

        public void UnregisterConnection([NotNull] IThreatModel model)
        {
            var propertyType = GetPropertyType();

            if (propertyType != null)
            {
                if (model.GetProperty(propertyType) is IPropertyJsonSerializableObject property)
                {
                    property.Value = null;
                }
            }
        }
    }
}