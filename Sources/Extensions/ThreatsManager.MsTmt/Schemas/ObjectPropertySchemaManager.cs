using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Schemas
{
    public class ObjectPropertySchemaManager
    {
        public const string ThreatModelObjectId = "ObjectId";
        public const string ThreatModelInstanceId = "InstanceId";

        private readonly IThreatModel _model;

        public ObjectPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(Resources.TmtObjectPropertySchema, Resources.DefaultNamespace) ?? _model.AddSchema(Resources.TmtObjectPropertySchema, Resources.DefaultNamespace);
            result.AppliesTo = Scope.Entity | Scope.DataFlow | Scope.TrustBoundary;
            result.Priority = 30;
            result.Visible = false;
            result.System = true;
            result.AutoApply = false;
            result.Description = Resources.TmtObjectPropertySchemaDescription;

            var id = result.GetPropertyType(ThreatModelObjectId) ?? result.AddPropertyType(ThreatModelObjectId, PropertyValueType.String);
            id.Visible = false;
            id.Description = Resources.ThreatModelObjectIdDescription;

            var instanceId = result.GetPropertyType(ThreatModelInstanceId) ?? result.AddPropertyType(ThreatModelInstanceId, PropertyValueType.String);
            instanceId.Visible = false;
            instanceId.Description = Resources.ThreatModelInstanceIdDescription;

            return result;
        }

        public string GetObjectId([NotNull] IPropertiesContainer container)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelObjectId);
            var property = container.GetProperty(propertyType);

            return property?.StringValue;
        }

        public void SetObjectId([NotNull] IPropertiesContainer container, [Required] string value)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelObjectId);
            var property = container.GetProperty(propertyType);

            if (property == null)
            {
                container.AddProperty(propertyType, value);
            }
            else
            {
                property.StringValue = value;
            }

        }

        public string GetInstanceId([NotNull] IPropertiesContainer container)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelInstanceId);
            var property = container.GetProperty(propertyType);

            return property?.StringValue;
        }

        public void SetInstanceId([NotNull] IPropertiesContainer container, [Required] string value)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelInstanceId);
            var property = container.GetProperty(propertyType);

            if (property == null)
            {
                container.AddProperty(propertyType, value);
            }
            else
            {
                property.StringValue = value;
            }

        }
    }
}