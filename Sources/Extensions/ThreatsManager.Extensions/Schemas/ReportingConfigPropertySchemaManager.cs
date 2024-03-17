using PostSharp.Patterns.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

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
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{SchemaName}' schema"))
            {
                result = _model.GetSchema(SchemaName, Resources.DefaultNamespace) ?? _model.AddSchema(SchemaName, Resources.DefaultNamespace);
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
                
                scope?.Complete();
            }

            return result;
        }

        #region Multiple documents generation support.
        public IPropertyType GetPropertyTypeTemplates()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get Templates property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType(Properties.Resources.PropertyTemplates) ??
                             schema.AddPropertyType(Properties.Resources.PropertyTemplates, PropertyValueType.JsonSerializableObject);
                    result.Visible = false;
                    result.DoNotPrint = true;
                    result.Description = Properties.Resources.PropertyTemplatesDescription;
                    scope?.Complete();
                }
            }

            return result;
        }

        public IEnumerable<WordReportDefinition> GetWordReportDefinitions()
        {
            IEnumerable<WordReportDefinition> result = null;

            var propertyType = GetPropertyTypeTemplates();
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is WordReportDefinitions collection)
                {
                    result = collection.Reports;
                }
            }

            return result;
        }

        public void StoreWordReportDefinition(WordReportDefinition definition)
        {
            var propertyType = GetPropertyTypeTemplates();
            if (propertyType != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Set Word Report Definition"))
                {
                    if (_model.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is WordReportDefinitions collection)
                    {
                        collection.AddReport(definition); 
                    }
                    else
                    {
                        collection = new WordReportDefinitions();
                        collection.AddReport(definition);
                        var property = _model.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject jsonObject)
                        {
                            jsonObject.Value = collection;
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        public void StoreWordReportDefinitions(IEnumerable<WordReportDefinition> definitions)
        {
            var propertyType = GetPropertyTypeTemplates();
            if (propertyType != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Set Word Report Definitions"))
                {
                    if (_model.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is WordReportDefinitions collection)
                    {
                        collection.AddReports(definitions);
                    }
                    else
                    {
                        collection = new WordReportDefinitions();
                        collection.AddReports(definitions);
                        var property = _model.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject jsonObject)
                        {
                            jsonObject.Value = collection;
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        public bool RemoveWordReportDefinition(WordReportDefinition definition)
        {
            bool result = false;

            var propertyType = GetPropertyTypeTemplates();
            if (propertyType != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Word Report Definition"))
                {
                    if (_model.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is WordReportDefinitions collection)
                    {
                        result = collection.RemoveReport(definition);
                    }

                    scope?.Complete();
                }
            }

            return result;
        }
        #endregion
    }
}