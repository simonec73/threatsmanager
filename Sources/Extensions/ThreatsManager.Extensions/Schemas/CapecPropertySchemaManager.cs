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
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
                result.AppliesTo = Scope.Threats;
                result.Priority = 12;
                result.Visible = true;
                result.System = true;
                result.AutoApply = false;
                result.Description = Properties.Resources.CapecPropertySchemaDescription;
            }

            var hiddenProperties = result.GetPropertyType("HiddenProperties");
            if (hiddenProperties == null)
            {
                hiddenProperties = result.AddPropertyType("HiddenProperties", PropertyValueType.Tokens);
                hiddenProperties.Visible = false;
                hiddenProperties.Description = Resources.PropertyHiddenProperties;
            }

            return result;
        }
    }
}