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
            var result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
            result.AppliesTo = Scope.ThreatModel;
            result.AutoApply = true;
            result.Priority = 10;
            result.Visible = false;
            result.System = true;
            result.NotExportable = true;
            result.Description = Resources.ReportingConfigPropertySchemaDescription;

            var excelSelectedFields = result.GetPropertyType("ExcelSelectedFields") ?? result.AddPropertyType("ExcelSelectedFields", PropertyValueType.Array);
            excelSelectedFields.Visible = false;
            excelSelectedFields.DoNotPrint = true;
            excelSelectedFields.Description = Resources.PropertyExcelSelectedFields;

            var wordDocumentPath = result.GetPropertyType("WordDocumentPath") ?? result.AddPropertyType("WordDocumentPath", PropertyValueType.SingleLineString);
            wordDocumentPath.Visible = false;
            wordDocumentPath.DoNotPrint = true;
            wordDocumentPath.Description = Resources.PropertyWordDocumentPath;

            var wordSections = result.GetPropertyType("WordSections") ?? result.AddPropertyType("WordSections", PropertyValueType.JsonSerializableObject);
            wordSections.Visible = false;
            wordSections.DoNotPrint = true;
            wordSections.Description = Resources.PropertyWordSections;

            return result;
        }
    }
}