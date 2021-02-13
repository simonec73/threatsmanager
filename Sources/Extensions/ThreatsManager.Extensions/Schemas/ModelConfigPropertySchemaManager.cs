using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Schemas
{
    public class ModelConfigPropertySchemaManager
    {
        private const string SchemaName = "Standard Extensions Configuration";

        private readonly IThreatModel _model;

        public ModelConfigPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            result.AppliesTo = Scope.ThreatModel;
            result.AutoApply = true;
            result.Priority = 10;
            result.Visible = false;
            result.System = true;
            result.NotExportable = true;
            result.Description = Resources.ThreatModelConfigPropertySchemaDescription;

            var horizontalSpacing = result.GetPropertyType("Diagram Layout Horizontal Spacing") ?? result.AddPropertyType("Diagram Layout Horizontal Spacing", PropertyValueType.Integer);
            horizontalSpacing.Visible = false;
            horizontalSpacing.DoNotPrint = true;
            horizontalSpacing.Description = Resources.PropertyHorizontalSpacing;

            var verticalSpacing = result.GetPropertyType("Diagram Layout Vertical Spacing") ?? result.AddPropertyType("Diagram Layout Vertical Spacing", PropertyValueType.Integer);
            verticalSpacing.Visible = false;
            verticalSpacing.DoNotPrint = true;
            verticalSpacing.Description = Resources.PropertyVerticalSpacing;

            return result;
        }
    }
}