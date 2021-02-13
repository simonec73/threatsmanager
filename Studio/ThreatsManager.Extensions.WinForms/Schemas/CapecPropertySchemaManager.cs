using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Schemas
{
    public class CapecPropertySchemaManager
    {
        private const string SchemaName = "Capec";

        private readonly IThreatModel _model;

        public CapecPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            result.AppliesTo = Scope.Threats;
            result.Priority = 12;
            result.Visible = true;
            result.System = true;
            result.AutoApply = false;
            result.NotExportable = true;
            result.Description = Resources.CapecPropertySchemaDescription;
           
            return result;
        }

        public IPropertyType GetHiddenPropertiesPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType("HiddenProperties") ?? schema.AddPropertyType("HiddenProperties", PropertyValueType.Tokens);
                result.Visible = false;
                result.DoNotPrint = true;
                result.Description = Resources.PropertyHiddenProperties;
            }

            return result;
        }
    }
}