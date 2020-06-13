using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Schemas
{
    public class EntitiesPropertySchemaManager
    {
        private const string SchemaName = "MS TMT Entity Properties";
        private const string ThreatModelEntityId = "MSTMT_ThreatModel_EntityId";

        private readonly IThreatModel _model;

        public EntitiesPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Resources.DefaultNamespace);
                result.AppliesTo = Scope.Entity;
                result.Priority = 30;
                result.Visible = true;
                result.System = true;
                result.AutoApply = false;
                result.Description = Resources.EntitiesPropertySchemaDescription;
            }

            var id = result.GetPropertyType(ThreatModelEntityId);
            if (id == null)
            {
                id = result.AddPropertyType(ThreatModelEntityId, PropertyValueType.String);
                id.Visible = false;
                id.Description = Resources.ThreatModelEntityIdDescription;
            }

            return result;
        }

        public string GetMsTmtEntityId([NotNull] IEntity entity)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelEntityId);
            var property = entity.GetProperty(propertyType);

            return property?.StringValue;
        }

        public void SetMsTmtEntityId([NotNull] IEntity entity, [Required] string value)
        {
            var schema = GetSchema();
            var propertyType = schema.GetPropertyType(ThreatModelEntityId);
            var property = entity.GetProperty(propertyType);

            if (property == null)
            {
                property = entity.AddProperty(propertyType, value);
            }
            else
            {
                property.StringValue = value;
            }

        }
    }
}