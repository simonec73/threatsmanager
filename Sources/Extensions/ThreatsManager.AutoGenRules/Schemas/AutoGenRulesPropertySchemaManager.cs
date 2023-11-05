using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoGenRules.Schemas
{
    public class AutoGenRulesPropertySchemaManager
    {
        private const string SchemaName = "Automatic Generation Rules";

        private readonly IThreatModel _model;

        public AutoGenRulesPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{SchemaName}' schema"))
            {
                result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
                result.AppliesTo = Scope.All;
                result.AutoApply = false;
                result.Priority = 10;
                result.Visible = false;
                result.System = true;
                result.NotExportable = false;
                result.Description = Resources.AutoGenRulePropertySchemaDescription;
                scope?.Complete();
            }

            return result;
        }

        public IPropertyType GetPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get AutoGenRule property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType("AutoGenRule") ?? schema.AddPropertyType("AutoGenRule", PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.PropertyAutoGenRule;
                    scope?.Complete();
                }
            }

            return result;
        }

        public IPropertyType GetTopPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get Top property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType("Top") ?? schema.AddPropertyType("Top", PropertyValueType.Boolean);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.PropertyTop;
                    scope?.Complete();
                }
            }

            return result;
        }

        public bool IsTop([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetTopPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property is IPropertyBool boolProperty)
                {
                    result = boolProperty.Value;
                }
            }

            return result;
        }

        public void SetTop([NotNull] IPropertiesContainer container, bool top)
        {
            using (var scope = UndoRedoManager.OpenScope("Set Top property"))
            {
                var propertyType = GetTopPropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType) ?? container.AddProperty(propertyType, null);
                    if (property is IPropertyBool boolProperty)
                    {
                        boolProperty.Value = top;
                        scope?.Complete();
                    }
                }
            }
        }
    }
}