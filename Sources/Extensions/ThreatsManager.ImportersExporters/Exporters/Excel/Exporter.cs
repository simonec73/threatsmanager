using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    public class Exporter
    {
        private IThreatModel _model;
        private ExportSettings _settings;

        #region Constructors.
        public Exporter([NotNull] IThreatModel model, [NotNull] ExportSettings settings)
        {
            _model = model;
            _settings = settings;
        }

        public Exporter([NotNull] IThreatModel model, [Required] string settingsFile)
        {
            _model = model;
            _settings = LoadSettings(settingsFile);
        }
        #endregion

        #region Public members.
        public bool Export([Required] string fileName)
        {
            bool result = false;

            if (_settings != null && _settings.Direction == Direction.Export && _settings.Version > 1)
            {
                ExcelEngine engine = null;
                IWorkbook workbook = null;

                try
                {
                    engine = new ExcelEngine();
                    var excel = engine.Excel;
                    if (File.Exists(fileName))
                    {
                        using (var file = File.OpenRead(fileName))
                        {
                            workbook = excel.Workbooks.Open(file);
                        }
                    }
                    else
                    {
                        workbook = excel.Workbooks.Create(0);
                    }

                    if (Save(workbook))
                    {
                        using (var file = File.OpenWrite(fileName))
                        {
                            workbook.SaveAs(file);
                        }
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    workbook?.Close();
                    engine?.Dispose();
                }
            }
            
            return result;
        }
        #endregion

        #region Private auxiliary members.
        private static ExportSettings LoadSettings([Required] string pathName)
        {
            ExportSettings result = null;

            if (File.Exists(pathName))
            {
                using (var file = File.OpenText(pathName))
                {
                    var json = file.ReadToEnd();
                    if (json.Length > 0)
                    {
                        using (var textReader = new StringReader(json))
                        using (var reader = new JsonTextReader(textReader))
                        {
                            var serializer = new JsonSerializer
                            {
                                TypeNameHandling = TypeNameHandling.None
                            };
                            result = serializer.Deserialize<ExportSettings>(reader);
                        }
                    }
                }
            }

            return result;
        }

        private bool Save([NotNull] IWorkbook workbook)
        {
            var result = false;

            var sheets = _settings?.Sheets?.ToArray();
            if (sheets?.Any() ?? false)
            {
                foreach (var sheet in sheets)
                {
                    var worksheet =
                        workbook.Worksheets.FirstOrDefault(x => string.CompareOrdinal(sheet.Name, x.Name) == 0);
                    if (worksheet != null)
                    {
                        worksheet.Clear();
                    }
                    else
                    {
                        worksheet = workbook.Worksheets.Create(sheet.Name);
                    }

                    Save(sheet, worksheet);
                }

                result = true;
            }

            return result;
        }

        private void Save([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            var columns = sheet.Columns?.ToArray();
            if (columns?.Any() ?? false)
            {
                foreach (var column in columns)
                {
                    worksheet[1, column.Index].Value = column.Label;
                    worksheet[1, column.Index].CellStyle.Font.Bold = true;
                    worksheet.Columns[column.Index - 1].ColumnWidth = column.Width;
                    worksheet.Columns[column.Index - 1].WrapText = true;
                }

                IEnumerable<IIdentity> identities = null;
                switch (sheet.ObjectType)
                {
                    case ObjectType.ThreatType:
                        identities = _model.ThreatTypes;
                        break;
                    case ObjectType.Mitigation:
                        identities = _model.Mitigations;
                        break;
                    case ObjectType.ThreatTypeMitigation:
                        identities = _model.Mitigations;
                        break;
                }

                var sort = sheet.SortRule?.ToArray();
                if (sort?.Any() ?? false)
                {
                    foreach (var s in sort)
                    {
                        identities = Sort(identities, s.Descending, s.FieldType, 
                            s.SchemaName, s.SchemaNamespace, s.PropertyName);
                    }
                }

                Save(sheet, worksheet, identities);
            }
        }

        private IEnumerable<IIdentity> Sort(IEnumerable<IIdentity> identities, bool descending, FieldType fieldType, 
            string schemaName, string schemaNamespace, string propertyName)
        {
            IEnumerable<IIdentity> result = identities;

            if (identities?.Any() ?? false)
            {
                switch (fieldType)
                {
                    case FieldType.Name:
                        if (identities is IOrderedEnumerable<IIdentity> ordered1)
                        {
                            result = descending ? ordered1.ThenByDescending(x => x.Name) : ordered1.ThenBy(x => x.Name);
                        }
                        else
                        {
                            result = descending ? identities.OrderByDescending(x => x.Name) : identities.OrderBy(x => x.Name);
                        }
                        break;
                    case FieldType.Description:
                        if (identities is IOrderedEnumerable<IIdentity> ordered2)
                        {
                            result = descending ? ordered2.ThenByDescending(x => x.Description) : ordered2.ThenBy(x => x.Description);
                        }
                        else
                        {
                            result = descending ? identities.OrderByDescending(x => x.Description) : identities.OrderBy(x => x.Description);
                        }
                        break;
                    case FieldType.Severity:
                        if (identities is IOrderedEnumerable<IIdentity> ordered3)
                        {
                            result = descending ? ordered3.ThenByDescending(x => (x as IThreatEvent)?.SeverityId ?? 0) :
                                ordered3.ThenBy(x => (x as IThreatEvent)?.SeverityId ?? 0);
                        }
                        else
                        {
                            result = descending ? identities.OrderByDescending(x => (x as IThreatEvent)?.SeverityId ?? 0) :
                                identities.OrderBy(x => (x as IThreatEvent)?.SeverityId ?? 0);
                        }
                        break;
                    case FieldType.Property:
                        var propertyType = _model.GetSchema(schemaName, schemaNamespace)?.GetPropertyType(propertyName);
                        if (propertyType != null)
                        {
                            if (identities is IOrderedEnumerable<IIdentity> ordered)
                            {
                                result = descending ? ordered
                                        .ThenByDescending(x => (x as IPropertiesContainer)?.GetProperty(propertyType)?.StringValue) :
                                    ordered.ThenBy(x => (x as IPropertiesContainer)?.GetProperty(propertyType)?.StringValue);
                            }
                            else
                            {
                                result = descending ? identities
                                        .OrderByDescending(x => (x as IPropertiesContainer)?.GetProperty(propertyType)?.StringValue) :
                                    identities.OrderBy(x => (x as IPropertiesContainer)?.GetProperty(propertyType)?.StringValue);
                            }
                        }
                        break;
                    case FieldType.ControlType:
                        if (identities is IOrderedEnumerable<IIdentity> ordered4)
                        {
                            result = descending ?
                                ordered4.ThenByDescending(x => (x as IMitigation)?.ControlType ?? SecurityControlType.Unknown) :
                                ordered4.ThenBy(x => (x as IMitigation)?.ControlType ?? SecurityControlType.Unknown);
                        }
                        else
                        {
                            result = descending ?
                                identities.OrderByDescending(x => (x as IMitigation)?.ControlType ?? SecurityControlType.Unknown) :
                                identities.OrderBy(x => (x as IMitigation)?.ControlType ?? SecurityControlType.Unknown);
                        }
                        break;
                }
            }

            return result;
        }

        private void Save([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<IIdentity> identities)
        {
            var columns = sheet.Columns?.ToArray();

            if (columns?.Any() ?? false)
            {
                int row = 2;

                var array = identities?
                    .Where(x => ShallProcess(sheet, x))
                    .ToArray();
                if (array?.Any() ?? false)
                {
                    var topPropertyType = _model
                        .GetSchema("Automatic Generation Rules", "https://www.simoneonsecurity.com/tm/2018")?
                        .GetPropertyType("Top");

                    foreach (var item in array)
                    {
                        foreach (var column in columns)
                        {
                            switch (column.FieldType)
                            {
                                case FieldType.LabelOnly:
                                    break;
                                case FieldType.Name:
                                    worksheet[row, column.Index].Value = $"'{item.Name}";
                                    break;
                                case FieldType.Description:
                                    worksheet[row, column.Index].Value = $"'{item.Description}";
                                    break;
                                case FieldType.Severity:
                                    if (sheet.ObjectType == ObjectType.ThreatType && item is IThreatType threatType)
                                        worksheet[row, column.Index].Value = $"'{threatType.Severity.Name}";
                                    break;
                                case FieldType.Property:
                                    if (item is IPropertiesContainer container)
                                    {
                                        var propertyType = _model.GetSchema(column.SchemaName, column.SchemaNamespace)?
                                            .GetPropertyType(column.PropertyName);
                                        if (propertyType != null)
                                        {
                                            var property = container.GetProperty(propertyType);
                                            if (!string.IsNullOrWhiteSpace(property?.StringValue))
                                                worksheet[row, column.Index].Value = $"'{property.StringValue}";
                                        }
                                    }

                                    break;
                                case FieldType.ControlType:
                                    if (item is IMitigation mitigation)
                                        worksheet[row, column.Index].Value = $"'{mitigation.ControlType.ToString()}";
                                    break;
                                case FieldType.Top:
                                    if (item is IMitigation mitigation3)
                                    {
                                        var threats = _model.GetThreatTypeMitigations(mitigation3)?
                                            .Where(x => x.ThreatType != null)
                                            .OrderBy(x => x.ThreatType.Name);
                                        if (threats?.Any() ?? false)
                                        {
                                            bool isTop = false;

                                            if (topPropertyType != null)
                                            {
                                                foreach (var threat in threats)
                                                {
                                                    if ((threat.GetProperty(topPropertyType) as IPropertyBool)?.Value ?? false)
                                                    {
                                                        isTop = true;
                                                        break;
                                                    }
                                                }

                                                worksheet[row, column.Index].Value = isTop.ToString();
                                            }
                                        }
                                    }
                                    break;
                                case FieldType.Threats:
                                    if (item is IMitigation mitigation2)
                                    {
                                        var threats = _model.GetThreatTypeMitigations(mitigation2)?
                                            .Where(x => x.ThreatType != null)
                                            .OrderBy(x => x.ThreatType.Name);
                                        if (threats?.Any() ?? false)
                                        {
                                            var builder = new StringBuilder();
                                            bool first = true;

                                            foreach (var threat in threats)
                                            {
                                                if (first)
                                                    first = false;
                                                else
                                                    builder.AppendLine();

                                                builder.Append($"{threat.ThreatType.Name} ({threat.Strength.ToString()})");
                                            }

                                            worksheet[row, column.Index].Value = $"'{builder.ToString()}";
                                        }
                                    }    
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        row++;
                    }
                }
            }
        }

        private bool ShallProcess([NotNull] SheetSettings sheet, [NotNull] object item)
        {
            bool result = false;

            var filter = sheet.Filter;
            if (filter != null)
            {
                bool single = true;
                string value = null;
                IEnumerable<string> values = null;

                if (item is IMitigation mitigation)
                {
                    switch (filter.ReferencedItem)
                    {
                        case ReferencedItemType.Current:
                            value = GetValue(filter, item);
                            break;
                        case ReferencedItemType.ThreatTypeMitigation:
                            values = _model.GetThreatTypeMitigations(mitigation)?
                                .Select(x => GetValue(filter, x));
                            single = false;
                            break;
                        case ReferencedItemType.ThreatEventMitigation:
                            values = _model.GetThreatEventMitigations(mitigation)?
                                .Select(x => GetValue(filter, x));
                            single = false;
                            break;
                    }
                }
                else
                {
                    value = GetValue(filter, item);
                }

                if (single)
                {
                    result = Evaluate(filter, value);
                }
                else
                {
                    result = values?.Any(x => Evaluate(filter, x)) ?? false;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        private string GetValue([NotNull] FilterSettings filter, [NotNull] object item)
        {
            string result = null;

            switch (filter.FieldType)
            {
                case FieldType.Name:
                    result = (item as IIdentity)?.Name; 
                    break;
                case FieldType.Description:
                    result = (item as IIdentity)?.Description; 
                    break;
                case FieldType.Severity:
                    result = (item as IThreatType)?.Severity?.Name; 
                    break;
                case FieldType.Property:
                    if (item is IPropertiesContainer container)
                    {
                        var propertyType = _model.GetSchema(filter.SchemaName, filter.SchemaNamespace)?
                            .GetPropertyType(filter.PropertyName);
                        if (propertyType != null)
                            result = container.GetProperty(propertyType)?.StringValue;
                    }
                    break;
            }

            return result;
        }

        private bool Evaluate([NotNull] FilterSettings filter, string value)
        {
            bool result = !filter.Include;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var regex = new Regex(filter.Regex,
                    filter.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
                var match = regex.IsMatch(value);
                result = filter.Include && match || !match && !filter.Include;
            }
            else if (filter.IsEmpty)
            {
                result = filter.Include;
            }

            return result;
        }
        #endregion
    }
}
