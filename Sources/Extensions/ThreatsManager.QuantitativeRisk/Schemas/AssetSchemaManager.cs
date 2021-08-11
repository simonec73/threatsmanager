using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.QuantitativeRisk.ListProvider;

namespace ThreatsManager.QuantitativeRisk.Schemas
{
    public class AssetSchemaManager
    {
        private const string SchemaName = "Asset";

        private readonly IThreatModel _model;

        public AssetSchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public IPropertySchema GetSchema()
        {
            var result = _model.GetSchema(SchemaName, Properties.Resources.DefaultNamespace) ?? 
                         _model.AddSchema(SchemaName, Properties.Resources.DefaultNamespace);
            result.AppliesTo = Scope.Process | Scope.DataStore;
            result.AutoApply = false;
            result.Priority = 10;
            result.Visible = true;
            result.System = true;
            result.NotExportable = true;
            result.RequiredExecutionMode = ExecutionMode.Pioneer;
            result.Description = Properties.Resources.AssetSchemaDescription;

            var descriptionPT = result.GetPropertyType("Asset Description") ?? 
                                result.AddPropertyType("Asset Description", PropertyValueType.String);
            descriptionPT.Visible = true;
            descriptionPT.Description = "Description of the Asset.";

            var minValuePT = result.GetPropertyType("Min Value") ?? result.AddPropertyType("Min Value", PropertyValueType.Decimal);
            minValuePT.Visible = true;
            minValuePT.Description =
                "Minimum estimated value of the Asset, expressed in the main currency for the Threat Model.";

            var maxValuePT = result.GetPropertyType("Max Value") ?? result.AddPropertyType("Max Value", PropertyValueType.Decimal);
            maxValuePT.Visible = true;
            maxValuePT.Description =
                "Maximum estimated value of the Asset, expressed in the main currency for the Threat Model.";

            var effects = result.GetPropertyType("Concerns");
            if (effects == null)
            {
                effects = result.AddPropertyType("Concerns", PropertyValueType.ListMulti);
                if (effects is IListMultiPropertyType listMultiPropertyType)
                {
                    listMultiPropertyType.SetListProvider(new EffectsListProvider());
                }
            }
            effects.Visible = true;
            effects.Description = "Main concerns for the Asset.";

            return result;
        }
    }
}