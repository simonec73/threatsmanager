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
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace);
            if (result == null)
            {
                result = _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
                result.AppliesTo = Scope.ThreatModel;
                result.AutoApply = true;
                result.Priority = 10;
                result.Visible = false;
                result.System = true;
                result.Description = Resources.ThreatModelConfigPropertySchemaDescription;
            }

            var horizontalSpacing = result.GetPropertyType("Diagram Layout Horizontal Spacing");
            if (horizontalSpacing == null)
            {
                horizontalSpacing =
                    result.AddPropertyType("Diagram Layout Horizontal Spacing", PropertyValueType.Integer);
                horizontalSpacing.Visible = false;
                horizontalSpacing.Description = Resources.PropertyHorizontalSpacing;
            }

            var verticalSpacing = result.GetPropertyType("Diagram Layout Vertical Spacing");
            if (verticalSpacing == null)
            {
                verticalSpacing =
                    result.AddPropertyType("Diagram Layout Vertical Spacing", PropertyValueType.Integer);
                verticalSpacing.Visible = false;
                verticalSpacing.Description = Resources.PropertyVerticalSpacing;
            }

            return result;
        }
    }
}