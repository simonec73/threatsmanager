using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using System.Collections.Generic;
using System.Linq;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    internal abstract class Rule
    {
        protected Rule(int row, [NotNull] IWorksheet worksheet, [NotNull] RuleSheetSettings sheet, int key)
        {
            ImportProperties(row, worksheet, sheet);

            KeyValue = GetString(row, key, worksheet);
        }

        public string Id { get; protected set; }

        public IEnumerable<Property> Properties { get; private set; }

        public virtual bool IsValid => !string.IsNullOrWhiteSpace(Id);

        public string KeyValue { get; private set; }

        protected string GetString(int row, [NotNull] IWorksheet worksheet, [NotNull] RuleSheetSettings sheet, RuleFieldType fieldType)
        {
            string result = null;

            var settings = sheet.Columns?.FirstOrDefault(x => x.FieldType == fieldType);
            if (settings != null)
            {
                result = GetString(row, settings.Index, worksheet);
            }

            return result;
        }

        protected string GetString(int row, int column, [NotNull] IWorksheet worksheet)
        {
            return  worksheet[row, column].DisplayText?.Trim('\'', ' ', '\n');
        }

        protected bool? GetBoolean(int row, [NotNull] IWorksheet worksheet, [NotNull] RuleSheetSettings sheet, RuleFieldType fieldType)
        {
            bool? result = null;

            var settings = sheet.Columns?.FirstOrDefault(x => x.FieldType == fieldType);
            if (settings != null)
            {
                result = GetBoolean(row, settings.Index, worksheet);
            }

            return result;
        }

        protected bool? GetBoolean(int row, int column, [NotNull] IWorksheet worksheet)
        {
            return worksheet[row, column].Value2 as bool?;
        }

        protected int? GetInteger(int row, int column, [NotNull] IWorksheet worksheet)
        {
            return worksheet[row, column].Value2 as int?;
        }

        private void ImportProperties(int row, [NotNull] IWorksheet worksheet, [NotNull] RuleSheetSettings sheet)
        {
            var columns = sheet.Columns?.Where(x => x.FieldType == RuleFieldType.Property).ToArray();
            if (columns?.Any() ?? false)
            {
                Properties = columns.Select(x => new Property(x, GetString(row, x.Index, worksheet))).ToArray();
            }
        }
    }
}
