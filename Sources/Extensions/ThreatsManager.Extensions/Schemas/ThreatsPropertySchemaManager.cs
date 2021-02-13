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
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            result.AppliesTo = Scope.ThreatType | Scope.ThreatEvent;
            result.AutoApply = true;
            result.Priority = 10;
            result.Visible = true;
            result.System = true;
            result.NotExportable = false;
            result.RequiredExecutionMode = ExecutionMode.Expert;
            result.Description = Resources.ThreatsPropertySchemaDescription;

            var keywords = result.GetPropertyType("Keywords") ?? result.AddPropertyType("Keywords", PropertyValueType.Tokens);
            keywords.Visible = true;
            keywords.DoNotPrint = true;
            keywords.Description = Resources.PropertyKeywords;

            return result;
        }
    }
}