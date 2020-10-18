using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Schemas
{
    public class ThreatsPropertySchemaManager
    {
        private const string SchemaName = "Threats";

        private readonly IThreatModel _model;

        public ThreatsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
                result.AppliesTo = Scope.ThreatType | Scope.ThreatEvent;
                result.AutoApply = true;
                result.Priority = 10;
                result.Visible = true;
                result.System = true;
                result.RequiredExecutionMode = ExecutionMode.Expert;
                result.Description = Properties.Resources.ThreatsPropertySchemaDescription;
            }

            var keywords = result.GetPropertyType("Keywords");
            if (keywords == null)
            {
                keywords = result.AddPropertyType("Keywords", PropertyValueType.Tokens);
                keywords.Visible = true;
                keywords.Description = Resources.PropertyKeywords;
            }

            return result;
        }
    }
}