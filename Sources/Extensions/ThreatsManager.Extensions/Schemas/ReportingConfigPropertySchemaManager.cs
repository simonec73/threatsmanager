using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Schemas
{
    public class ReportingConfigPropertySchemaManager
    {
        private const string SchemaName = "Reporting Extensions Configuration";

        private readonly IThreatModel _model;

        public ReportingConfigPropertySchemaManager([NotNull] IThreatModel model)
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
                result.Description = Resources.ReportingConfigPropertySchemaDescription;
            }

            var excelSelectedFields = result.GetPropertyType("ExcelSelectedFields");
            if (excelSelectedFields == null)
            {
                excelSelectedFields = result.AddPropertyType("ExcelSelectedFields", PropertyValueType.Array);
                excelSelectedFields.Visible = false;
                excelSelectedFields.Description = Resources.PropertyExcelSelectedFields;
            }

            var wordDocumentPath = result.GetPropertyType("WordDocumentPath");
            if (wordDocumentPath == null)
            {
                wordDocumentPath = result.AddPropertyType("WordDocumentPath", PropertyValueType.SingleLineString);
                wordDocumentPath.Visible = false;
                wordDocumentPath.Description = Resources.PropertyWordDocumentPath;
            }

            var wordSections = result.GetPropertyType("WordSections");
            if (wordSections == null)
            {
                wordSections = result.AddPropertyType("WordSections", PropertyValueType.JsonSerializableObject);
                wordSections.Visible = false;
                wordSections.Description = Resources.PropertyWordSections;
            }

            return result;
        }
    }
}