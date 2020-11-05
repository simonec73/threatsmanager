using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Schemas
{
    public class WordPropertySchemaManager
    {
        private const string SchemaName = "Word";

        private readonly IThreatModel _model;

        public WordPropertySchemaManager([NotNull] IThreatModel model)
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
                result.Description = Properties.Resources.WordPropertySchemaDescription;
            }

            var ignoredListFields = result.GetPropertyType("IgnoredListFields");
            if (ignoredListFields == null)
            {
                ignoredListFields = result.AddPropertyType("IgnoredListFields", PropertyValueType.Array);
                ignoredListFields.Visible = false;
                ignoredListFields.Description = Resources.PropertyIgnoredListFields;
            }

            var columnWidth = result.GetPropertyType("ColumnWidth");
            if (columnWidth == null)
            {
                columnWidth = result.AddPropertyType("ColumnWidth", PropertyValueType.Array);
                columnWidth.Visible = false;
                columnWidth.Description = Resources.PropertyColumnWidths;
            }

            return result;
        }
    }
}