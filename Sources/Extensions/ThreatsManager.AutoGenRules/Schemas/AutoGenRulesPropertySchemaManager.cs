using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

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
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
                result.AppliesTo = Scope.All;
                result.AutoApply = false;
                result.Priority = 10;
                result.Visible = false;
                result.System = true;
                result.NotExportable = false;
                result.Description = Resources.AutoGenRulePropertySchemaDescription;
            }

            return result;
        }

        public IPropertyType GetPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType("AutoGenRule");
                if (result == null)
                {
                    result = schema.AddPropertyType("AutoGenRule", PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Resources.PropertyAutoGenRule;
                }
            }

            return result;
        }
    }
}