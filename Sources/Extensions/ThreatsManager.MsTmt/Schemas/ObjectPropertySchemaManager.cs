using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Schemas
{
    public class ObjectPropertySchemaManager
    {
        private const string ThreatModelObjectId = "MSTMT_ThreatModel_ObjectId";

        private readonly IThreatModel _model;

        public ObjectPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(Resources.TmtObjectPropertySchema, Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(Resources.TmtObjectPropertySchema, Resources.DefaultNamespace);
                result.AppliesTo = Scope.Entity | Scope.DataFlow | Scope.TrustBoundary;
                result.Priority = 30;
                result.Visible = false;
                result.System = true;
                result.AutoApply = false;
                result.Description = Resources.TmtObjectPropertySchemaDescription;
            }

            var id = result.GetPropertyType(ThreatModelObjectId);
            if (id == null)
            {
                id = result.AddPropertyType(ThreatModelObjectId, PropertyValueType.String);
                id.Visible = false;
                id.Description = Resources.ThreatModelObjectIdDescription;
            }

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
    }
}