using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoGenRules.Schemas;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using Svg;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    public class Importer
    {
        private ExcelEngine _engine;
        private IThreatModel _model;
        private ImportSettings _settings;
        private string _path;
        private Dictionary<string, IItemTemplate> _itemTemplates = new Dictionary<string, IItemTemplate>();
        private Dictionary<string, string> _aliases;
        private IEnumerable<ItemDetails> _itemDetails;
        private AutoGenRulesPropertySchemaManager _schemaManager;
        private const string AND = "AND";
        private const string OR = "OR";
        private const string CROSSTB = "Crosses Trust Boundary";

        public event Func<string, ArtifactInfo> GetArtifactInfo;

        #region Constructors.
        public Importer([NotNull] IThreatModel model, [Required] string settingsFile)
        {
            _model = model;
            _settings = LoadSettings(settingsFile);
            _path = Path.GetDirectoryName(settingsFile);
        }
        #endregion

        #region Public members.
        public bool Import([Required] string fileName, Dictionary<string, string> aliases = null, 
            IEnumerable<ParameterValue> parameterValues = null)
        {
            bool result = false;

            if (!File.Exists(fileName))
            {
                EventsDispatcher.RaiseEvent("ShowWarning", $"File '{fileName}' does not exist.");
            }
            else if (_settings != null && _settings.Direction == Direction.Import && _settings.Version == 3)
            {
                try
                {
                    if (parameterValues?.Any() ?? false)
                        ParameterManager.Initialize(_settings.Parameters, parameterValues);

                    _aliases = aliases;
                    result = ImportExcelFile(fileName, GetRules());

                    if (result)
                        UpdateDescription();
                }
                finally
                {
                    ParameterManager.Release();
                }
            }
            else
            {
                EventsDispatcher.RaiseEvent("ShowWarning", "The Structure Description file is not valid for this operation.");
            }
            
            return result;
        }

        public bool ImportItem([Required] string itemName, IEnumerable<ParameterValue> parameterValues = null)
        {
            var result = false;

            if (_settings != null && _settings.Direction == Direction.Import && _settings.Version > 1)
            {
                try
                {
                    ParameterManager.Initialize(_settings.Parameters, parameterValues);

                    var rules = GetRules();
                    _itemDetails = ImportItemMapping()?.Items;
                    var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, itemName) == 0);
                    if (itemDetails != null)
                    {
                        if (!string.IsNullOrWhiteSpace(itemDetails.Alias))
                            itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemDetails.Alias, x.Name) == 0);

                        var entityTemplates = _model.EntityTemplates?.ToArray();
                        var flowTemplates = _model.FlowTemplates?.ToArray();
                        var trustBoundaryTemplates = _model.TrustBoundaryTemplates?.ToArray();
                        CreateArtifact(ArtifactType.ItemTemplate, itemName, entityTemplates, flowTemplates, trustBoundaryTemplates);
                        result = CreateAdditionalControls(itemName, itemDetails, rules, HitPolicy.Replace);
                    }
                }
                finally
                {
                    ParameterManager.Release();
                }
            }

            return result;
        }

        private IEnumerable<Rule> GetRules()
        {
            IEnumerable<Rule> result = null;

            if (!string.IsNullOrWhiteSpace(_settings.RuleFile) &&
                (_settings.RuleFileSheets?.Any() ?? false))
            {
                var fullPath = Path.Combine(_path, _settings.RuleFile);

                if (File.Exists(fullPath))
                {
                    try
                    {
                        result = ImportRulesFile(fullPath, _settings.RuleFileSheets);
                    }
                    catch (IOException)
                    {
                        EventsDispatcher.RaiseEvent("ShowWarning", "The Excel Rules file is in use. Please close it and retry.");
                        result = null;
                        //MessageBox.Show(Form.ActiveForm, "The Excel Rules file is in use. Please close it and retry.",
                        //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    EventsDispatcher.RaiseEvent("ShowWarning", $"The Excel Rules file '{fullPath}' does not exist.");
                }
            }

            return result;
        }
        #endregion

        #region Import Json Settings file.
        private static ImportSettings LoadSettings([Required] string pathName)
        {
            ImportSettings result = null;

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
                            result = serializer.Deserialize<ImportSettings>(reader);
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region Import the main Excel file.
        private bool ImportExcelFile([Required] string fileName, IEnumerable<Rule> rules)
        {
            bool result = false;

            IWorkbook workbook = null;

            try
            {
                _engine = new ExcelEngine();
                var excel = _engine.Excel;
                using (var fileStream = File.OpenRead(fileName))
                {
                    workbook = excel.Workbooks.Open(fileStream,
                        ExcelParseOptions.DoNotParseCharts | ExcelParseOptions.DoNotParsePivotTable, true, null);
                }

                if (!_settings.StrictValidation || Validate(workbook))
                {
                    result = Load(workbook, rules);
                }
                else
                {
                    EventsDispatcher.RaiseEvent("ShowWarning", "The Excel file does not match the expected structure.");
                }
            }
            catch (IOException exc)
            {
                EventsDispatcher.RaiseEvent("ShowWarning", $"The following error has been raised when importing an Excel file: '{exc.Message}'.");
                result = false;
                //MessageBox.Show(Form.ActiveForm, $"The following error has been raised when importing an Excel file: '{exc.Message}'.",
                //    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                workbook?.Close();
                _engine?.Dispose();
                _engine = null;
            }

            return result;
        }

        private bool Validate([NotNull] IWorkbook workbook)
        {
            var result = false;

            var sheets = _settings.Sheets?.ToArray();
            if (sheets?.Any() ?? false)
            {
                result = true;

                foreach (var sheet in sheets)
                {
                    var worksheet =
                        workbook.Worksheets.FirstOrDefault(x => string.CompareOrdinal(x.Name, sheet.Name) == 0);
                    if (worksheet == null)
                    {
                        result = false;
                        break;
                    }
                    else
                    {
                        var columns = sheet.Columns?.ToArray();
                        if (columns?.Any() ?? false)
                        {
                            if (columns.Any(x => x.Index <= 0))
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private bool Load([NotNull] IWorkbook workbook, IEnumerable<Rule> rules)
        {
            var result = false;

            var sheets = _settings.Sheets?
                .OrderBy(x => (int) x.ObjectType)
                .ToArray();
            if (sheets?.Any() ?? false)
            {
                result = true;

                foreach (var sheet in sheets)
                {
                    var worksheet =
                        workbook.Worksheets.FirstOrDefault(x => string.CompareOrdinal(x.Name, sheet.Name) == 0);
                    if (worksheet != null)
                    {
                        result |= Load(sheet, worksheet, rules);
                    }
                }

                if (_settings.AutoCleanup)
                {
                    var threatTypes = _model.ThreatTypes?.Where(x => !(x.Mitigations?.Any() ?? false)).ToArray();
                    if (threatTypes?.Any() ?? false)
                    {
                        foreach (var tt in threatTypes)
                        {
                            _model.RemoveThreatType(tt.Id);
                        }
                    }

                    var mitigations = _model.Mitigations?
                        .Where(x => !(_model.ThreatTypes?.Any(y =>
                            y.Mitigations?.Any(z => z.MitigationId == x.Id) ?? false) ?? false))
                        .ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var m in mitigations)
                        {
                            _model.RemoveMitigation(m.Id);
                        }
                    }
                }
            }

            return result;
        }

        private bool Load([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<Rule> rules)
        {
            var result = false;

            _itemDetails = ImportItemMapping()?.Items?.ToArray();
            CreateArtifacts(sheet, worksheet);

            switch (sheet.ObjectType)
            {
                case ObjectType.ThreatType:
                    result = LoadThreatTypes(sheet, worksheet, rules);
                    break;
                case ObjectType.Mitigation:
                    result = LoadMitigations(sheet, worksheet, rules);
                    break;
                case ObjectType.ThreatTypeMitigation:
                    result = LoadThreatTypeMitigations(sheet, worksheet, rules);
                    break;
                case ObjectType.EntityTemplate:
                    result = LoadEntityTemplates(sheet, worksheet, rules);
                    break;
                case ObjectType.SpecializedMitigation:
                    result = LoadSpecializedMitigations(sheet, worksheet, rules);
                    break;
            }

            return result;
        }
        #endregion

        #region Import Rules file.
        private IEnumerable<Rule> ImportRulesFile([Required] string fileName, [NotNull] IEnumerable<RuleSheetSettings> sheets)
        {
            IEnumerable<Rule> result = null;

            ExcelEngine engine = null;
            IWorkbook workbook = null;

            try
            {
                engine = new ExcelEngine();
                var excel = engine.Excel;
                using (var fileStream = File.OpenRead(fileName))
                {
                    workbook = excel.Workbooks.Open(fileStream,
                        ExcelParseOptions.DoNotParseCharts | ExcelParseOptions.DoNotParsePivotTable, true, null);
                }

                result = LoadRules(workbook);
            }
            finally
            {
                workbook?.Close();
                engine?.Dispose();
            }

            return result;
        }

        private IEnumerable<Rule> LoadRules([NotNull] IWorkbook workbook)
        {
            IEnumerable<Rule> result = null;

            var sheets = _settings.RuleFileSheets?
                .OrderBy(x => (int)x.ObjectType)
                .ToArray();
            if (sheets?.Any() ?? false)
            {
                var rules = new List<Rule>();

                foreach (var sheet in sheets)
                {
                    var worksheet =
                        workbook.Worksheets.FirstOrDefault(x => string.CompareOrdinal(x.Name, sheet.Name) == 0);
                    if (worksheet != null)
                    {
                        LoadRules(rules, sheet, worksheet);
                    }
                }

                if (rules.Any())
                    result = rules.AsReadOnly();
            }

            return result;
        }

        private void LoadRules([NotNull] List<Rule> rules, [NotNull] RuleSheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            IEnumerable<Rule> loaded = null;

            switch (sheet.ObjectType)
            {
                case ObjectType.ThreatType:
                    loaded = LoadThreatTypeRules(sheet, worksheet);
                    break;
                case ObjectType.Mitigation:
                    loaded = LoadMitigationRules(sheet, worksheet);
                    break;
            }

            if (loaded?.Any() ?? false)
                rules.AddRange(loaded);
        }

        private IEnumerable<Rule> LoadThreatTypeRules([NotNull] RuleSheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            IEnumerable<Rule> result = null;

            if (sheet.Columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var rules = new List<Rule>();

                do
                {
                    if (string.IsNullOrEmpty(GetValue(worksheet, row, sheet.Columns[0])))
                        break;

                    if (CheckFilter(sheet, worksheet, row))
                    {
                        var rule = new ThreatTypeRule(row, worksheet, sheet, sheet.KeyColumn);
                        if (rule.IsValid)
                            rules.Add(rule);
                    }

                    row++;
                } while (true);

                if (rules.Any())
                    result = rules.AsReadOnly();
            }

            return result;
        }

        private IEnumerable<Rule> LoadMitigationRules([NotNull] RuleSheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            IEnumerable<Rule> result = null;

            if (sheet.Columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var rules = new List<Rule>();

                do
                {
                    if (string.IsNullOrEmpty(GetValue(worksheet, row, sheet.Columns[0])))
                        break;

                    if (CheckFilter(sheet, worksheet, row))
                    {
                        var rule = new MitigationRule(row, worksheet, sheet, sheet.KeyColumn);
                        if (rule.IsValid)
                            rules.Add(rule);
                    }

                    row++;
                } while (true);

                if (rules.Any())
                    result = rules.AsReadOnly();
            }

            return result;
        }

        private bool CheckFilter([NotNull] RuleSheetSettings sheet, [NotNull] IWorksheet worksheet, int row)
        {
            bool result = !sheet.DefaultExclude;

            var filters = sheet.Filters?.ToArray();
            if (filters?.Any() ?? false)
            {
                foreach (var filter in filters)
                {
                    var value = worksheet[row, filter.Index].DisplayText?.Trim('\'', ' ');
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var regex = new Regex(filter.Regex,
                            filter.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
                        var match = regex.IsMatch(value);
                        if (match && filter.StopIfMatch || !match && filter.StopIfNotMatch)
                        {
                            result = filter.Include && match || !match && !filter.Include;
                            break;
                        }
                    }
                    else if (filter.IsEmpty)
                    {
                        if (filter.StopIfMatch)
                        {
                            result = filter.Include;
                            break;
                        }
                    }
                    else if (filter.StopIfNotMatch)
                    {
                        result = !filter.Include;
                    }
                }
            }

            return result;
        }

        private string GetValue([NotNull] IWorksheet worksheet, int row, [NotNull] RuleColumnSettings settings)
        {
            return worksheet[row, settings.Index].DisplayText?.Trim('\'', ' ');
        }
        #endregion

        #region Artifacts management.
        private ItemMappingSettings ImportItemMapping()
        {
            ItemMappingSettings result = null;

            if (!string.IsNullOrWhiteSpace(_settings.ItemTemplateRules))
            {
                var fullPath = Path.Combine(_path, _settings.ItemTemplateRules);

                if (File.Exists(fullPath))
                {
                    using (var file = File.OpenText(fullPath))
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
                                result = serializer.Deserialize<ItemMappingSettings>(reader);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void CreateArtifacts([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            var columns = sheet.Columns?
                .Where(x => (x.Artifact?.ArtifactType ?? ArtifactType.Undefined) != ArtifactType.Undefined)
                .ToArray();
            if (columns?.Any() ?? false)
            {
                foreach (var column in columns)
                {
                    CreateArtifacts(column, sheet, worksheet);
                }
            }
            else
            {
                var specifier = GetCalculatedName(sheet, out var artifactType);
                if (!string.IsNullOrWhiteSpace(specifier))
                {
                    CreateArtifact(artifactType, specifier,
                       _model.EntityTemplates?.ToArray(),
                       _model.FlowTemplates?.ToArray(),
                       _model.TrustBoundaryTemplates?.ToArray());
                }
            }
        }

        private void CreateArtifacts([NotNull] ColumnSettings column, [NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            var uniqueValues = GetUniqueValues(column, sheet, worksheet);

            if (uniqueValues?.Any() ?? false)
            {
                var entityTemplates = _model.EntityTemplates?.ToArray();
                var flowTemplates = _model.FlowTemplates?.ToArray();
                var trustBoundaryTemplates = _model.TrustBoundaryTemplates?.ToArray();

                foreach (var value in uniqueValues)
                {
                    CreateArtifact(column.Artifact.ArtifactType, value, entityTemplates, flowTemplates, trustBoundaryTemplates);
                }
            }
        }

        private void CreateArtifact(ArtifactType artifactType, [Required] string serviceName,
            IEnumerable<IEntityTemplate> entityTemplates, IEnumerable<IFlowTemplate> flowTemplates, 
            IEnumerable<ITrustBoundaryTemplate> trustBoundaryTemplates)
        {
            var name = serviceName;
            if (_aliases?.ContainsKey(serviceName) ?? false)
                name = _aliases[serviceName];

            var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(name, x.Name) == 0);
            string iconPath = null;
            if (itemDetails != null)
            {
                if (!string.IsNullOrWhiteSpace(itemDetails.Alias))
                    itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemDetails.Alias, x.Name) == 0);

                if (!string.IsNullOrWhiteSpace(itemDetails.Icon))
                    iconPath = Path.Combine(_path, Path.Combine(Path.GetDirectoryName(_settings.ItemTemplateRules), itemDetails.Icon));
            }

            switch (artifactType)
            {
                case ArtifactType.ItemTemplate:
                    if (itemDetails != null)
                    {
                        var itemTemplates = GetItemTemplates(itemDetails.ItemType,
                            entityTemplates, flowTemplates, trustBoundaryTemplates);
                        AddEntity(name, itemDetails.Name, itemDetails.Description, itemDetails.ItemType,
                            itemTemplates, itemDetails.Properties, iconPath);
                    }
                    else
                    {
                        var artifactInfo = GetArtifactInfo?.Invoke(name);
                        if (artifactInfo != null)
                        {
                            var itemTemplates = GetItemTemplates(artifactInfo.ItemType,
                                entityTemplates, flowTemplates, trustBoundaryTemplates);
                            AddEntity(name, artifactInfo.Name, artifactInfo.Description, 
                                artifactInfo.ItemType, itemTemplates);
                        }
                    }
                    break;
                case ArtifactType.ExternalInteractorTemplate:
                    AddEntity(name, name, null, ItemType.ExternalInteractor,
                        entityTemplates.Where(x => x.EntityType == EntityType.ExternalInteractor));
                    break;
                case ArtifactType.ProcessTemplate:
                    AddEntity(name, name, null, ItemType.Process,
                        entityTemplates.Where(x => x.EntityType == EntityType.Process));
                    break;
                case ArtifactType.DataStoreTemplate:
                    AddEntity(name, name, null, ItemType.DataStore,
                        entityTemplates.Where(x => x.EntityType == EntityType.DataStore));
                    break;
                case ArtifactType.FlowTemplate:
                    AddEntity(name, name, null, ItemType.Flow, flowTemplates);
                    break;
                case ArtifactType.TrustBoundaryTemplate:
                    AddEntity(name, name, null, ItemType.Flow, trustBoundaryTemplates);
                    break;
                default:
                    break;
            }

        }

        private IItemTemplate GetArtifact([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, int row)
        {
            IItemTemplate result = null;

            var artifactSettings = sheet.Columns?.
                FirstOrDefault(x => (x.Artifact?.ArtifactType ?? ArtifactType.Undefined) != ArtifactType.Undefined);

            if (artifactSettings != null)
            {
                var key = GetValue(worksheet, row, artifactSettings);
                if (_itemTemplates.TryGetValue(key, out var itemTemplate))
                {
                    result = itemTemplate;
                }
            }

            return result;
        }

        private IEnumerable<IItemTemplate> GetItemTemplates(ItemType itemType, 
            IEnumerable<IEntityTemplate> entityTemplates, 
            IEnumerable<IFlowTemplate> flowTemplates,
            IEnumerable<ITrustBoundaryTemplate> trustBoundaryTemplates)
        {
            IEnumerable<IItemTemplate> result = null;

            switch (itemType)
            {
                case ItemType.ExternalInteractor:
                    result = entityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor);
                    break;
                case ItemType.Process:
                    result = entityTemplates?.Where(x => x.EntityType == EntityType.Process);
                    break;
                case ItemType.DataStore:
                    result = entityTemplates?.Where(x => x.EntityType == EntityType.DataStore);
                    break;
                case ItemType.Flow:
                    result = flowTemplates;
                    break;
                case ItemType.TrustBoundary:
                    result = trustBoundaryTemplates;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private void AddEntity([Required] string key, [Required] string name, string description, 
            ItemType itemType, IEnumerable<IItemTemplate> itemTemplates, 
            IEnumerable<Property> properties = null, string iconFile = null)
        {
            var existing = itemTemplates?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);

            if (existing == null)
            {
                IItemTemplate itemTemplate;

                Bitmap smallImage = null;
                Bitmap mediumImage = null;
                Bitmap largeImage = null;
                if (iconFile != null && GetImage(iconFile, out var small, out var medium, out var large))
                {
                    smallImage = small;
                    mediumImage = medium;
                    largeImage = large;
                }

                switch (itemType)
                {
                    case ItemType.ExternalInteractor:
                        itemTemplate = _model.AddEntityTemplate(name, description, 
                            largeImage, mediumImage, smallImage, EntityType.ExternalInteractor);
                        break;
                    case ItemType.Process:
                        itemTemplate = _model.AddEntityTemplate(name, description,
                            largeImage, mediumImage, smallImage, EntityType.Process);
                        break;
                    case ItemType.DataStore:
                        itemTemplate = _model.AddEntityTemplate(name, description,
                            largeImage, mediumImage, smallImage, EntityType.DataStore);
                        break;
                    case ItemType.Flow:
                        itemTemplate = _model.AddFlowTemplate(name, description);
                        break;
                    case ItemType.TrustBoundary:
                        itemTemplate = _model.AddTrustBoundaryTemplate(name, description);
                        break;
                    default:
                        itemTemplate = null;
                        break;
                }

                if (itemTemplate != null)
                {
                    _itemTemplates.Add(key, itemTemplate);

                    var ps = properties?.ToArray();
                    if (ps?.Any() ?? false)
                    {
                        foreach (var p in ps)
                        {
                            var propertyType = _model.GetSchema(p.Schema, p.Namespace)?.GetPropertyType(p.Name);
                            if (propertyType != null)
                                itemTemplate.AddProperty(propertyType, p.Value);
                        }
                    }
                }
            }
        }

        private IEnumerable<string> GetUniqueValues([NotNull] ColumnSettings column, 
            [NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            IEnumerable<string> result = null;

            var uniqueValues = new List<string>();
            var row = sheet.FirstRow;
            string value;

            do
            {
                value = GetMappedValue(worksheet, row, column);
                if (!string.IsNullOrWhiteSpace(value) && !uniqueValues.Contains(value))
                    uniqueValues.Add(value);
                row++;
            } while (!string.IsNullOrWhiteSpace(value));

            if (uniqueValues.Count > 0)
                result = uniqueValues.AsReadOnly();

            return result;
        }

        private bool GetImage(string path, out Bitmap small, out Bitmap medium, out Bitmap large)
        {
            bool result = false;
            small = null;
            medium = null;
            large = null;

            if (File.Exists(path))
            {
                if (string.CompareOrdinal(Path.GetExtension(path)?.ToLower(), ".svg") == 0)
                {
                    SvgDocument svg = SvgDocument.Open(path);
                    if (CalculateBorder(svg, 5, 512, out var left, out var right, out var top, out var bottom))
                    {
                        small = ConvertSvg(svg, 16, 512, left, right, top, bottom);
                        medium = ConvertSvg(svg, 32, 512, left, right, top, bottom);
                        large = ConvertSvg(svg, 64, 512, left, right, top, bottom);
                        result = true;
                    }
                }
                else
                {
                    var image = Bitmap.FromFile(path);
                    small = new Bitmap(image, 16, 16);
                    medium = new Bitmap(image, 32, 32);
                    large = new Bitmap(image, 64, 64);
                    result = true;
                }
            }

            return result;
        }

        private Bitmap ConvertSvg([NotNull] SvgDocument svg, int targetHeight, int referenceHeight, int left, int right, int top, int bottom)
        {
            Bitmap result = null;

            if (targetHeight > 0)
            {
                var factor = ((float)targetHeight) / ((float)(referenceHeight - top - bottom));
                var height = (int)(targetHeight + ((top + bottom) * factor));
                var bitmap = svg.Draw(0, height);

                var fullHeight = bitmap.Height;
                var topCrop = (int)(top * factor * height / fullHeight);
                var bottomCrop = (int)(bottom * factor * height / fullHeight);
                var leftCrop = (int)(left * factor * height / fullHeight);
                var rightCrop = (int)(right * factor * height / fullHeight);


                Rectangle cropRect = new Rectangle(leftCrop, topCrop, bitmap.Width - leftCrop - rightCrop, targetHeight);
                result = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(result))
                {
                    g.DrawImage(bitmap, new Rectangle(0, 0, result.Width, result.Height),
                        cropRect,
                        GraphicsUnit.Pixel);
                }
            }

            return result;
        }

        private bool CalculateBorder([NotNull] SvgDocument svg, int pixelMargin, int referenceHeight,
            out int left, out int right, out int top, out int bottom)
        {
            bool result = false;

            left = -1;
            right = -1;
            top = -1;
            bottom = -1;

            var bitmap = svg.Draw(0, referenceHeight);

            int fullHeight = bitmap.Height;
            int fullWidth = bitmap.Width;

            int? leftEmpty = null;
            int? rightEmpty = null;
            int? topEmpty = null;
            int? bottomEmpty = null;

            for (int y = 0; y < fullHeight; y++)
            {
                for (int x = 0; x < fullWidth; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    if (color != Color.White && !color.A.Equals(0))
                    {
                        if (!leftEmpty.HasValue || leftEmpty.Value > x)
                            leftEmpty = x;
                        if (!rightEmpty.HasValue || rightEmpty.Value < x)
                            rightEmpty = x;
                        if (!topEmpty.HasValue || topEmpty.Value > y)
                            topEmpty = y;
                        if (!bottomEmpty.HasValue || bottomEmpty.Value < y)
                            bottomEmpty = y;
                    }
                }
            }

            if (leftEmpty.HasValue && rightEmpty.HasValue && topEmpty.HasValue && bottomEmpty.HasValue)
            {
                if (topEmpty.Value > pixelMargin && bottomEmpty.Value < fullHeight - pixelMargin)
                {
                    top = topEmpty.Value - pixelMargin;
                    bottom = fullHeight - bottomEmpty.Value - pixelMargin;
                }
                else
                {
                    top = topEmpty.Value;
                    bottom = fullHeight - bottomEmpty.Value;
                }

                if (leftEmpty.Value > pixelMargin && rightEmpty.Value < fullWidth - pixelMargin)
                {
                    left = leftEmpty.Value - pixelMargin;
                    right = fullWidth - rightEmpty.Value - pixelMargin;
                }
                else
                {
                    left = leftEmpty.Value;
                    right = fullWidth - rightEmpty.Value;
                }

                var netHeight = fullHeight - bottom - top + 1;
                var netWidth = fullWidth - right - left + 1;

                if (netHeight > netWidth)
                {
                    var deltaWidth = netHeight - netWidth;

                    if (netWidth + deltaWidth <= fullWidth)
                    {
                        // Expand Width to square.
                        left = left - (deltaWidth / 2);
                        right = right - (deltaWidth / 2);
                    }
                }
                else if (netHeight < netWidth)
                {
                    var deltaHeight = netWidth - netHeight;

                    if (netHeight + deltaHeight <= fullHeight)
                    {
                        // Expand Height to square.
                        top = top - (deltaHeight / 2);
                        bottom = bottom - (deltaHeight / 2);
                    }
                }

                result = true;
            }

            return result;
        }

        private void UpdateDescription()
        {
            if (_model?.Description?.StartsWith("Knowledge Base") ?? false)
            {
                var builder = new StringBuilder();

                using (var reader = new StringReader(_model.Description))
                {
                    builder.AppendLine(reader.ReadLine());
                }

                var entityTemplates = _model.EntityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor);
                AppendTemplates(builder, entityTemplates);
                entityTemplates = _model.EntityTemplates?.Where(x => x.EntityType == EntityType.Process);
                AppendTemplates(builder, entityTemplates);
                entityTemplates = _model.EntityTemplates?.Where(x => x.EntityType == EntityType.DataStore);
                AppendTemplates(builder, entityTemplates);
                AppendTemplates(builder, _model.FlowTemplates);
                AppendTemplates(builder, _model.TrustBoundaryTemplates);

                _model.Description = builder.ToString();
            }
        }

        private void AppendTemplates([NotNull] StringBuilder builder, IEnumerable<IItemTemplate> templates)
        {
            var itemTemplates = templates?.OrderBy(x => x.Name).ToArray();
            if (itemTemplates?.Any() ?? false)
            {
                foreach (var t in itemTemplates)
                {
                    string type = null;
                    if (t is IEntityTemplate e)
                    {
                        type = e.EntityType.GetEnumLabel();
                    } else if (t is IFlowTemplate)
                    {
                        type = "Flow";
                    } else if (t is ITrustBoundaryTemplate)
                    {
                        type = "Trust Boundary";
                    }

                    if (type != null)
                    {
                        builder.AppendLine($"- [{type}] {t.Name}");
                    }
                }
            }
        }
        #endregion

        #region Load tables.
        private bool LoadThreatTypes([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<Rule> rules)
        {
            var result = false;

            var columns = sheet.Columns?.ToArray();

            if (columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var nameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Name);
                var descSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Description);
                var sevSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Severity);

                if (row > 0 && nameSettings != null && nameSettings.Index > 0 && sevSettings != null && sevSettings.Index > 0)
                {
                    if (sheet.ForceClear)
                    {
                        var threatTypes = _model.ThreatTypes?.ToArray();
                        if (threatTypes?.Any() ?? false)
                        {
                            foreach (var threatType in threatTypes)
                            {
                                _model.RemoveThreatType(threatType.Id, true);
                            }
                        }
                    }

                    do
                    {
                        var name = GetValue(worksheet, row, nameSettings);
                        if (string.IsNullOrWhiteSpace(name))
                            break;
                        else
                        {
                            if (CheckFilter(sheet, worksheet, row))
                            {
                                string description = null;
                                if (descSettings != null && descSettings.Index > 0)
                                    description = GetValue(worksheet, row, descSettings);

                                var key = GetKey(sheet, worksheet, row);
                                if (key == null)
                                {
                                    result = false;
                                    break;
                                }

                                var rule = GetRule<MitigationRule>(key, rules);

                                var severity = _model.GetMappedSeverity(_model.Severities?.FirstOrDefault(x =>
                                        string.CompareOrdinal(x.Name,
                                            GetMappedValue(worksheet, row, sevSettings)) == 0)?
                                    .Id ?? 75);

                                var existing =
                                    _model.ThreatTypes?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);

                                if (existing != null && sheet.HitPolicy != HitPolicy.Add)
                                {
                                    switch (sheet.HitPolicy)
                                    {
                                        case HitPolicy.Skip:
                                            break;
                                        case HitPolicy.Replace:
                                            existing.Description = description;
                                            existing.Severity = severity;
                                            LoadProperties(sheet, worksheet, row, existing, rule);
                                            result = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    var threatType = _model.AddThreatType(name, severity);
                                    threatType.Description = description;
                                    LoadProperties(sheet, worksheet, row, threatType, rule);
                                    result = true;
                                }
                            }
                        }

                        row++;
                    } while (true);
                }
            }

            return result;
        }

        private bool LoadMitigations([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<Rule> rules)
        {
            var result = false;

            var columns = sheet.Columns?.ToArray();
            if (columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var nameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Name);
                var descSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Description);

                if (row > 0 && nameSettings != null && nameSettings.Index > 0)
                {
                    if (sheet.ForceClear)
                    {
                        var mitigations = _model.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                _model.RemoveMitigation(mitigation.Id, true);
                            }
                        }
                    }

                    Dictionary<string, IWorkbook> workbooks = new Dictionary<string, IWorkbook>();
                    ArtifactType artifactType = ArtifactType.Undefined;

                    try
                    {
                        do
                        {
                            var name = GetValue(worksheet, row, nameSettings);
                            if (string.IsNullOrWhiteSpace(name))
                                break;
                            else
                            {
                                if (CheckFilter(sheet, worksheet, row))
                                {
                                    bool skip = false;
                                    MitigationRule rule = null;

                                    var specifierSettings = columns.FirstOrDefault(x => x.IsSpecifier);
                                    string specifier = null;
                                    if (specifierSettings != null)
                                    {
                                        specifier = GetValue(worksheet, row, specifierSettings);
                                        if (specifier != null)
                                        {
                                            if (_aliases?.ContainsKey(specifier) ?? false)
                                                specifier = _aliases[specifier];
                                        }
                                    }

                                    var key = GetKey(sheet, worksheet, row);

                                    if (rules?.Any() ?? false)
                                    {
                                        if (key == null)
                                        {
                                            result = false;
                                            break;
                                        }

                                        rule = GetRule<MitigationRule>(key, rules.OfType<MitigationRule>()
                                            .Where(x => string.IsNullOrWhiteSpace(_settings.Level) || string.CompareOrdinal(x.Level, _settings.Level) == 0));
                                        if (rule != null)
                                        {
                                            if (!string.IsNullOrWhiteSpace(specifier))
                                            {
                                                ItemDetails itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, specifier) == 0);
                                                string specificName;
                                                if (string.IsNullOrWhiteSpace(itemDetails?.Alias))
                                                    specificName = rule.GetSpecificName(specifier);
                                                else
                                                    specificName = rule.GetSpecificName(itemDetails.Alias);
                                                if (!string.IsNullOrWhiteSpace(specificName))
                                                    name = specificName;
                                            }
                                            else
                                            {
                                                specifier = GetCalculatedName(sheet, out artifactType);
                                                if (!string.IsNullOrWhiteSpace(specifier))
                                                {
                                                    var specificName = rule.GetSpecificName(specifier);
                                                    if (!string.IsNullOrWhiteSpace(specificName))
                                                        name = specificName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            skip = true;
                                        }
                                    }

                                    if (!skip)
                                    {
                                        string description = null;
                                        if (descSettings != null && descSettings.Index > 0)
                                            description = GetValue(worksheet, row, descSettings);

                                        var existing =
                                            _model.Mitigations?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);

                                        if (existing != null && sheet.HitPolicy != HitPolicy.Add)
                                        {
                                            switch (sheet.HitPolicy)
                                            {
                                                case HitPolicy.Skip:
                                                    break;
                                                case HitPolicy.Replace:
                                                    existing.Description = description;
                                                    LoadProperties(sheet, worksheet, row, existing, rule);
                                                    LoadCalculatedColumns(sheet, existing, workbooks, key);
                                                    AssociateThreats(existing, rule, rules, specifier);
                                                    CreateGenerationRules(sheet, specifier, row, existing, rules, rule);
                                                    result = true;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            var mitigation = _model.AddMitigation(name);
                                            mitigation.Description = description;
                                            LoadProperties(sheet, worksheet, row, mitigation, rule);
                                            LoadCalculatedColumns(sheet, mitigation, workbooks, key);
                                            ApplyControlType(worksheet, columns, row, mitigation, rule);
                                            AssociateThreats(mitigation, rule, rules, specifier);
                                            CreateGenerationRules(sheet, specifier, row, mitigation, rules, rule);

                                            result = true;
                                        }
                                    }
                                }
                            }

                            row++;
                        } while (true);

                        if (_itemTemplates?.Any() ?? false)
                        {
                            foreach (var itemTemplatePair in _itemTemplates)
                            {
                                var itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemTemplatePair.Key, x.Name) == 0);
                                if (itemDetails != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(itemDetails.Alias))
                                        itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemDetails.Alias, x.Name) == 0);

                                    result = CreateAdditionalControls(itemTemplatePair.Key, itemDetails, rules, sheet.HitPolicy);
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (workbooks.Any())
                        {
                            foreach (var workbook in workbooks)
                            {
                                workbook.Value.Close();
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool LoadThreatTypeMitigations([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<Rule> rules)
        {
            var result = false;

            var columns = sheet.Columns?.ToArray();

            if (columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var ttNameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.ThreatTypeRef);
                var mNameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.MitigationRef);
                var strSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Strength);

                if (row > 0 && ttNameSettings != null && ttNameSettings.Index > 0 && mNameSettings != null && mNameSettings.Index > 0)
                {
                    var knownThreatTypes = new List<Guid>();

                    do
                    {
                        var ttName = GetMappedValue(worksheet, row, ttNameSettings);
                        var mName = GetMappedValue(worksheet, row, mNameSettings);
                        if (string.IsNullOrWhiteSpace(ttName) || string.IsNullOrWhiteSpace(mName))
                            break;
                        else
                        {
                            if (CheckFilter(sheet, worksheet, row))
                            {
                                var tt = _model.ThreatTypes?.FirstOrDefault(x =>
                                    string.CompareOrdinal(x.Name, ttName) == 0);
                                var m = _model.Mitigations?.FirstOrDefault(x =>
                                    string.CompareOrdinal(x.Name, mName) == 0);

                                if (tt != null && m != null)
                                {
                                    IStrength strength;
                                    if (strSettings != null && strSettings.Index > 0)
                                    {
                                        strength = _model.Strengths?
                                                       .FirstOrDefault(x =>
                                                           string.CompareOrdinal(x.Name,
                                                               GetMappedValue(worksheet, row, strSettings)) ==
                                                           0) ??
                                                   _model.GetMappedStrength(50);
                                    }
                                    else
                                    {
                                        strength = _model.GetMappedStrength(50);
                                    }

                                    if (sheet.ForceClear && !knownThreatTypes.Contains(tt.Id))
                                    {
                                        knownThreatTypes.Add(tt.Id);
                                        var mitigations = tt.Mitigations?.ToArray();
                                        if (mitigations?.Any() ?? false)
                                        {
                                            foreach (var mitigation in mitigations)
                                            {
                                                tt.RemoveMitigation(mitigation.MitigationId);
                                            }
                                        }
                                    }

                                    var existing = tt.GetMitigation(m.Id);

                                    if (existing != null && sheet.HitPolicy != HitPolicy.Add)
                                    {
                                        switch (sheet.HitPolicy)
                                        {
                                            case HitPolicy.Skip:
                                                break;
                                            case HitPolicy.Replace:
                                                existing.Strength = strength;
                                                result = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        var threatTypeMitigation = tt.AddMitigation(m, strength);
                                        result = true;
                                    }
                                }
                            }
                        }

                        row++;
                    } while (true);
                }
            }

            return result;
        }

        private bool LoadEntityTemplates([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<Rule> rules)
        {
            var result = false;

            var columns = sheet.Columns?.ToArray();

            if (columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var nameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Name);
                var descSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Description);
                var entityTypeSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.EntityType);

                if (row > 0 && nameSettings != null && nameSettings.Index > 0 && entityTypeSettings != null && entityTypeSettings.Index > 0)
                {
                    if (sheet.ForceClear)
                    {
                        var entityTemplates = _model.EntityTemplates?.ToArray();
                        if (entityTemplates?.Any() ?? false)
                        {
                            foreach (var entityTemplate in entityTemplates)
                            {
                                _model.RemoveEntityTemplate(entityTemplate.Id);
                            }
                        }
                    }

                    do
                    {
                        var name = GetValue(worksheet, row, nameSettings);
                        if (string.IsNullOrWhiteSpace(name))
                            break;
                        else
                        {
                            if (CheckFilter(sheet, worksheet, row))
                            {
                                string description = null;
                                if (descSettings != null && descSettings.Index > 0)
                                    description = GetValue(worksheet, row, descSettings);

                                var key = GetKey(sheet, worksheet, row);
                                if (key == null)
                                {
                                    result = false;
                                    break;
                                }

                                var rule = GetRule<MitigationRule>(key, rules);

                                var existing =
                                    _model.ThreatTypes?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);

                                if (existing != null && sheet.HitPolicy != HitPolicy.Add)
                                {
                                    switch (sheet.HitPolicy)
                                    {
                                        case HitPolicy.Skip:
                                            break;
                                        case HitPolicy.Replace:
                                            existing.Description = description;
                                            LoadProperties(sheet, worksheet, row, existing, rule);
                                            result = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    if (Enum.TryParse<EntityType>(
                                        GetMappedValue(worksheet, row, entityTypeSettings), out var entityType))
                                    {
                                        var entityTemplate = _model.AddEntityTemplate(name, description, entityType);
                                        LoadProperties(sheet, worksheet, row, entityTemplate, rule);
                                        result = true;
                                    }
                                }
                            }
                        }

                        row++;
                    } while (true);
                }
            }

            return result;
        }

        private bool LoadSpecializedMitigations([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, IEnumerable<Rule> rules)
        {
            var result = false;

            var columns = sheet.Columns?.ToArray();
            if (columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var nameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Name);
                var descSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Description);

                if (row > 0 && nameSettings != null && nameSettings.Index > 0)
                {
                    if (sheet.ForceClear)
                    {
                        var mitigations = _model.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                var specializedMitigations = mitigation.Specialized?.ToArray();
                                if (specializedMitigations?.Any() ?? false)
                                {
                                    foreach (var specialized in specializedMitigations)
                                    {
                                        mitigation.RemoveSpecializedMitigation(specialized.TargetId);
                                    }
                                }
                            }
                        }
                    }

                    Dictionary<string, IWorkbook> workbooks = new Dictionary<string, IWorkbook>();
                    ArtifactType artifactType = ArtifactType.Undefined;

                    try
                    {
                        do
                        {
                            var name = GetMappedValue(worksheet, row, nameSettings);
                            if (string.IsNullOrWhiteSpace(name))
                                break;
                            else
                            {
                                if (CheckFilter(sheet, worksheet, row))
                                {
                                    bool skip = false;
                                    MitigationRule rule = null;

                                    var specifierSettings = columns.FirstOrDefault(x => x.IsSpecifier);
                                    string specifier = null;
                                    if (specifierSettings != null)
                                    {
                                        specifier = GetValue(worksheet, row, specifierSettings);
                                        if (specifier != null)
                                        {
                                            if (_aliases?.ContainsKey(specifier) ?? false)
                                                specifier = _aliases[specifier];
                                        }
                                    }

                                    var key = GetKey(sheet, worksheet, row);

                                    if (rules?.Any() ?? false)
                                    {
                                        if (key == null)
                                        {
                                            result = false;
                                            break;
                                        }

                                        rule = GetRule<MitigationRule>(key, rules.OfType<MitigationRule>()
                                            .Where(x => string.IsNullOrWhiteSpace(_settings.Level) || string.CompareOrdinal(x.Level, _settings.Level) == 0));
                                        if (rule != null)
                                        {
                                            if (!string.IsNullOrWhiteSpace(specifier))
                                            {
                                                ItemDetails itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, specifier) == 0);
                                                string specificName;
                                                if (string.IsNullOrWhiteSpace(itemDetails?.Alias))
                                                    specificName = rule.GetSpecificName(specifier);
                                                else
                                                    specificName = rule.GetSpecificName(itemDetails.Alias);
                                                if (!string.IsNullOrWhiteSpace(specificName))
                                                    name = specificName;
                                            }
                                            else
                                            {
                                                specifier = GetCalculatedName(sheet, out artifactType);
                                                if (!string.IsNullOrWhiteSpace(specifier))
                                                {
                                                    var specificName = rule.GetSpecificName(specifier);
                                                    if (!string.IsNullOrWhiteSpace(specificName))
                                                        name = specificName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            skip = true;
                                        }
                                    }

                                    if (!skip)
                                    {
                                        string description = null;
                                        if (descSettings != null && descSettings.Index > 0)
                                            description = GetValue(worksheet, row, descSettings);

                                        var existing =
                                            _model.Mitigations?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);

                                        if (existing != null && sheet.HitPolicy != HitPolicy.Add)
                                        {
                                            switch (sheet.HitPolicy)
                                            {
                                                case HitPolicy.Skip:
                                                    break;
                                                case HitPolicy.Replace:
                                                    existing.Description = description;
                                                    LoadProperties(sheet, worksheet, row, existing, rule);
                                                    LoadCalculatedColumns(sheet, existing, workbooks, key);
                                                    AssociateThreats(existing, rule, rules, specifier);
                                                    CreateGenerationRules(sheet, specifier, row, existing, rules, rule);
                                                    result = true;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            var mitigation = _model.AddMitigation(name);
                                            mitigation.Description = description;
                                            LoadProperties(sheet, worksheet, row, mitigation, rule);
                                            LoadCalculatedColumns(sheet, mitigation, workbooks, key);
                                            ApplyControlType(worksheet, columns, row, mitigation, rule);
                                            AssociateThreats(mitigation, rule, rules, specifier);
                                            CreateGenerationRules(sheet, specifier, row, mitigation, rules, rule);

                                            result = true;
                                        }
                                    }
                                }
                            }

                            row++;
                        } while (true);

                        if (_itemTemplates?.Any() ?? false)
                        {
                            foreach (var itemTemplatePair in _itemTemplates)
                            {
                                var itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemTemplatePair.Key, x.Name) == 0);
                                if (itemDetails != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(itemDetails.Alias))
                                        itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemDetails.Alias, x.Name) == 0);

                                    result = CreateAdditionalControls(itemTemplatePair.Key, itemDetails, rules, sheet.HitPolicy);
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (workbooks.Any())
                        {
                            foreach (var workbook in workbooks)
                            {
                                workbook.Value.Close();
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void LoadProperties([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet,
            int row, [NotNull] IPropertiesContainer container, Rule rule)
        {
            var properties = rule?.Properties?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    AddProperty(container, property.Schema, property.Namespace, property.Name, property.Value);
                }
            }

            var columns = sheet.Columns?
                .Where(x => x.FieldType == FieldType.Property && !(rule?.Properties?.Any(y => 
                    string.CompareOrdinal(y.Namespace, x.SchemaNamespace) == 0 &&
                    string.CompareOrdinal(y.Schema, x.SchemaName) == 0 &&
                    string.CompareOrdinal(y.Name, x.PropertyName) == 0) ?? false))
                .ToArray();

            if (columns?.Any() ?? false)
            {
                foreach (var column in columns)
                {
                    if (!string.IsNullOrWhiteSpace(column?.SchemaName) &&
                        !string.IsNullOrWhiteSpace(column.SchemaNamespace) &&
                        !string.IsNullOrWhiteSpace(column.PropertyName) &&
                        column.Index > 0)
                    {
                        AddProperty(container, column.SchemaName, column.SchemaNamespace, column.PropertyName,
                            GetMappedValue(worksheet, row, column));
                    }
                }
            }
        }

        private void LoadProperties([NotNull] IPropertiesContainer container, string specifier, IEnumerable<Property> properties)
        {
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if (property.Specifier)
                        AddProperty(container, property.Schema, property.Namespace, property.Name, specifier);
                    else
                        AddProperty(container, property.Schema, property.Namespace, property.Name, property.Value);
                }
            }
        }

        private void AddProperty([NotNull] IPropertiesContainer container, [Required] string schemaName, 
            [Required] string schemaNamespace, [Required] string propertyName, string value)
        {
            var propertyType = _model.GetSchema(schemaName, schemaNamespace)?.GetPropertyType(propertyName);
            if (propertyType != null)
            {
                if (container.HasProperty(propertyType))
                {
                    var property = container.GetProperty(propertyType);
                    property.StringValue = value;
                }
                else
                {
                    container.AddProperty(propertyType, value);
                }
            }
        }

        private void LoadCalculatedColumns([NotNull] SheetSettings sheet, [NotNull] IPropertiesContainer container,
            [NotNull] Dictionary<string, IWorkbook> workbooks, string key)
        {
            var calculatedColumns = sheet.Calculate?.ToArray();
            if (calculatedColumns?.Any() ?? false)
            {
                foreach (var col in calculatedColumns)
                {
                    switch (col.Source)
                    {
                        case SourceType.Parameter:
                            var value = ParameterManager.Instance.ApplyParameters(col.Value);
                            AddProperty(container, col.SchemaName, col.SchemaNamespace, col.PropertyName, value);
                            break;
                        case SourceType.ExcelFile:
                            var excelFile = ParameterManager.Instance.ApplyParameters(col.Location);
                            if (File.Exists(excelFile))
                            {
                                var fileName = Path.GetFileName(excelFile);
                                IWorkbook workbook;
                                if (!workbooks.TryGetValue(fileName, out workbook))
                                {
                                    using (var fileStream = File.OpenRead(excelFile))
                                    {
                                        workbook = _engine.Excel.Workbooks.Open(fileStream,
                                            ExcelParseOptions.DoNotParseCharts | ExcelParseOptions.DoNotParsePivotTable,
                                            false, null);
                                    }
                                    workbooks.Add(fileName, workbook);
                                }

                                if (workbook != null)
                                {
                                    var regex = new Regex(col.Sheet, RegexOptions.IgnoreCase);
                                    var worksheets = workbook.Worksheets.Where(x => regex.IsMatch(x.Name))?.ToArray();
                                    if (worksheets?.Any() ?? false)
                                    {
                                        bool found = false;

                                        foreach (var worksheet in worksheets)
                                        {
                                            var row = col.FirstRow;

                                            do
                                            {
                                                var keyValue = GetValue(worksheet, row, col.Key);
                                                if (string.IsNullOrEmpty(keyValue))
                                                    break;
                                                else if (string.CompareOrdinal(key, keyValue) == 0)
                                                {
                                                    found = true;

                                                    var colValue = GetValue(worksheet, row, col.Index);
                                                    AddProperty(container, col.SchemaName, col.SchemaNamespace, col.PropertyName, colValue);

                                                    break;
                                                }

                                                row++;
                                            } while (true);

                                            if (found)
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        case SourceType.Level:
                            AddProperty(container, col.SchemaName, col.SchemaNamespace, col.PropertyName, _settings.Level);
                            break;
                    }
                }
            }
        }

        private string GetCalculatedName([NotNull] SheetSettings sheet, out ArtifactType artifactType)
        {
            string result = null;
            artifactType = ArtifactType.Undefined;

            var calculatedColumns = sheet.Calculate?.ToArray();
            if (calculatedColumns?.Any() ?? false)
            {
                foreach (var col in calculatedColumns)
                {
                    if (col.Source == SourceType.Parameter && col.Specifier)
                    {
                        var value = ParameterManager.Instance.ApplyParameters(col.Value);
                        if (value != null)
                            result = value;
                        if (col.Artifact != null)
                            artifactType = col.Artifact.ArtifactType;

                        break;
                    }
                }
            }

            return result;
        }

        private bool CheckFilter([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, int row)
        {
            bool result = !sheet.DefaultExclude;

            var filters = sheet.Filters?.ToArray();
            if (filters?.Any() ?? false)
            {
                foreach (var filter in filters)
                {
                    var value = worksheet[row, filter.Index].DisplayText?.Trim('\'', ' ');
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var regex = new Regex(filter.Regex,
                            filter.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase);
                        var match = regex.IsMatch(value);
                        if (match && filter.StopIfMatch || !match && filter.StopIfNotMatch)
                        {
                            result = filter.Include && match || !match && !filter.Include;
                            break;
                        }
                    }
                    else if (filter.IsEmpty)
                    {
                        if (filter.StopIfMatch)
                        {
                            result = filter.Include;
                            break;
                        }
                    }
                    else if (filter.StopIfNotMatch)
                    {
                        result = !filter.Include;
                    }
                }
            }

            return result;
        }

        private string GetValue([NotNull] IWorksheet worksheet, int row, [NotNull] ColumnSettings settings)
        {
            return GetValue(worksheet, row, settings.Index);
        }

        private string GetValue([NotNull] IWorksheet worksheet, int row, int column)
        {
            return worksheet[row, column].DisplayText?.Trim('\'', ' ', '\n');
        }

        private string GetMappedValue([NotNull] IWorksheet worksheet, int row, [NotNull] ColumnSettings settings)
        {
            var text = GetValue(worksheet, row, settings);
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(text))
            {
                if (!string.IsNullOrWhiteSpace(settings.Prefix))
                    builder.Append(ParameterManager.Instance.ApplyParameters(settings.Prefix));

                if (settings.Mapping?.ContainsKey(text) ?? false)
                {
                    builder.Append(settings.Mapping[text]);
                }
                else
                {
                    builder.Append(text);
                }

                if (!string.IsNullOrWhiteSpace(settings.Suffix))
                    builder.Append(ParameterManager.Instance.ApplyParameters(settings.Suffix));
            }

            return builder.ToString();
        }

        private string GetKey([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet, int row)
        {
            string result = null;

            var column = sheet.Columns?.FirstOrDefault(x => x.IsKey);
            if (column == null)
                EventsDispatcher.RaiseEvent("ShowWarning", "The IsKey flag is missing. Please configure IsKey for at least a column for the imported Excel file.");
            else
                result = GetMappedValue(worksheet, row, column);

            return result;
        }

        private T GetRule<T>([Required] string key, IEnumerable<Rule> rules) where T : Rule
        {
            return rules?.OfType<T>().FirstOrDefault(x => string.CompareOrdinal(x.KeyValue, key) == 0);
        }

        private void ApplyControlType(IWorksheet worksheet, IEnumerable<ColumnSettings> columns, int row, IMitigation mitigation, MitigationRule rule)
        {
            if (rule != null && rule.ControlType != SecurityControlType.Unknown)
            {
                mitigation.ControlType = rule.ControlType;
            }
            else
            {
                var ctSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.ControlType);
                if (ctSettings != null && ctSettings.Index > 0 &&
                    Enum.TryParse<SecurityControlType>(GetMappedValue(worksheet, row, ctSettings), out var controlType))
                {
                    mitigation.ControlType = controlType;
                }
            }
        }

        private void AssociateThreats([NotNull] IMitigation mitigation, MitigationRule rule, 
            IEnumerable<Rule> rules, string serviceName)
        {
            var threats = rule?.Threats?.ToArray();

            if (threats?.Any() ?? false)
            {
                IItemTemplate itemTemplate = null;
                ItemDetails itemDetails = null;
                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    if  (_itemTemplates.TryGetValue(serviceName, out var it))
                        itemTemplate = it;
                    itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, serviceName) == 0);
                    if (!string.IsNullOrWhiteSpace(itemDetails?.Alias))
                        itemDetails = _itemDetails.FirstOrDefault(x => string.CompareOrdinal(itemDetails.Alias, x.Name) == 0);

                }

                foreach (var tDef in threats)
                {
                    if (!(itemDetails?.IgnoreThreatTypes?.Any(x => string.CompareOrdinal(tDef.Key, x) == 0) ?? false))
                    {
                        var threat = _model.ThreatTypes?.FirstOrDefault(x => string.CompareOrdinal(x.Name, tDef.Key) == 0);
                        var tRule = rules?.OfType<ThreatTypeRule>().FirstOrDefault(x => string.CompareOrdinal(x.Id, tDef.Key) == 0);
                        var strength = _model.GetStrength((int)tDef.Value);
                        if (threat != null && strength != null && (tRule == null || IsApplicable(itemTemplate, tRule)))
                        {
                            if (!(threat.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false))
                                threat.AddMitigation(mitigation, strength);
                        }
                    }
                }
            }
        }

        private void AssociateThreats([NotNull] IMitigation mitigation, [Required] string serviceName, 
            [NotNull] ItemDetails itemDetails, IEnumerable<Rule> rules, IEnumerable<Threat> threats)
        {
            if (threats?.Any() ?? false)
            {
                IItemTemplate itemTemplate = null;
                if (_itemTemplates.TryGetValue(serviceName, out var it))
                    itemTemplate = it;

                foreach (var threat in threats)
                {
                    if (!(itemDetails.IgnoreThreatTypes?.Any(x => string.CompareOrdinal(threat.Name, x) == 0) ?? false))
                    {
                        var t = _model.ThreatTypes?.FirstOrDefault(x => string.CompareOrdinal(x.Name, threat.Name) == 0);
                        var strength = _model.GetStrength((int)threat.Strength);
                        var tRule = rules?.OfType<ThreatTypeRule>().FirstOrDefault(x => string.CompareOrdinal(x.Id, threat.Name) == 0);
                        if (t != null && strength != null && (tRule == null || IsApplicable(itemTemplate, tRule)))
                        {
                            if (!(t.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false))
                                t.AddMitigation(mitigation, strength);
                        }
                    }
                }
            }
        }

        private bool IsApplicable(IItemTemplate itemTemplate, ThreatTypeRule rule)
        {
            bool result = false;

            if (rule != null)
            {
                switch (rule.Policy)
                {
                    case ThreatsPolicy.Processes:
                        result = ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) == EntityType.Process;
                        break;
                    case ThreatsPolicy.DataStores:
                        result = ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) == EntityType.DataStore;
                        break;
                    case ThreatsPolicy.Full:
                        result = ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) != EntityType.ExternalInteractor;
                        break;
                    case ThreatsPolicy.ProcessesFromEI:
                        result = ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) == EntityType.Process;
                        break;
                    case ThreatsPolicy.DataStoresFromEI:
                        result = ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) == EntityType.DataStore;
                        break;
                    case ThreatsPolicy.FullFromEI:
                        result = ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) != EntityType.ExternalInteractor;
                        break;
                    case ThreatsPolicy.FullCrossTB:
                        result = itemTemplate is ITrustBoundaryTemplate ||
                            ((itemTemplate as IEntityTemplate)?.EntityType ?? EntityType.ExternalInteractor) != EntityType.ExternalInteractor;
                        break;
                    case ThreatsPolicy.Undefined:
                    case ThreatsPolicy.Skip:
                        break;
                    default:
                        result = true;
                        break;
                }
            }

            return result;
        }

        private bool CreateAdditionalControls([Required] string serviceName, [NotNull] ItemDetails itemDetails, IEnumerable<Rule> rules, HitPolicy hitPolicy)
        {
            bool result = false;

            var controls = itemDetails.AdditionalControls?.ToArray();
            if (controls?.Any() ?? false)
            {
                foreach (var control in controls)
                {
                    var existing =
                        _model.Mitigations?.FirstOrDefault(x => string.CompareOrdinal(x.Name, control.Name) == 0);

                    if (existing != null && hitPolicy != HitPolicy.Add)
                    {
                        switch (hitPolicy)
                        {
                            case HitPolicy.Skip:
                                break;
                            case HitPolicy.Replace:
                                existing.Description = control.Description;
                                existing.ControlType = control.ControlType;
                                LoadProperties(existing, serviceName, control.Properties);
                                AssociateThreats(existing, serviceName, itemDetails, rules, control.Threats);
                                CreateGenerationRules(existing, control.Status, serviceName, rules, control.Threats);
                                result = true;
                                break;
                        }
                    }
                    else
                    {
                        var mitigation = _model.AddMitigation(control.Name);
                        mitigation.Description = control.Description;
                        mitigation.ControlType = control.ControlType;
                        LoadProperties(mitigation, serviceName, control.Properties);
                        AssociateThreats(mitigation, serviceName, itemDetails, rules, control.Threats);
                        CreateGenerationRules(mitigation, control.Status, serviceName, rules, control.Threats);

                        result = true;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Create Threat and Mitigation Generation rules.
        private void CreateGenerationRules([NotNull] SheetSettings sheet, string serviceName, int row, 
            [NotNull] IMitigation mitigation, IEnumerable<Rule> rules, MitigationRule rule)
        {
            var threats = _model.ThreatTypes?
                .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                .ToArray();

            if ((rules?.Any() ?? false) && rule != null && (threats?.Any() ?? false))
            {
                IItemTemplate itemTemplate = null;
                if (!string.IsNullOrWhiteSpace(serviceName) && _itemTemplates.TryGetValue(serviceName, out var it))
                    itemTemplate = it;

                foreach (var threat in threats)
                {
                    var ttRule = rules.OfType<ThreatTypeRule>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.Name, threat.Name) == 0);
                    if (ttRule != null)
                    {
                        try
                        {
                            var ttMitigation = threat.GetMitigation(mitigation.Id);
                            switch (ttRule.Policy)
                            {
                                case ThreatsPolicy.ThreatModel:
                                    AddThreatModelGenRule(ttMitigation, rule.Status);
                                    break;
                                case ThreatsPolicy.Processes:
                                    AddProcessesGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.DataStores:
                                    AddDataStoresGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.Full:
                                    AddFullGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.ProcessesFromEI:
                                    AddProcessesFromEIGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.DataStoresFromEI:
                                    AddDataStoresFromEIGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.FullFromEI:
                                    AddFullFromEIGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.AllProcesses:
                                    AddAllProcessesGenRule(ttMitigation, rule.Status);
                                    break;
                                case ThreatsPolicy.AllDataStores:
                                    AddAllDataStoresGenRule(ttMitigation, rule.Status);
                                    break;
                                case ThreatsPolicy.AllProcessesFromEI:
                                    AddAllProcessesFromEIGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.AllDataStoresFromEI:
                                    AddAllDataStoresFromEIGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.AllFromEI:
                                    AddAllFromEIGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                case ThreatsPolicy.CrossTB:
                                    AddCrossTBGenRule(ttMitigation, rule.Status);
                                    break;
                                case ThreatsPolicy.FullCrossTB:
                                    AddFullCrossTBGenRule(ttMitigation, itemTemplate, rule.Status);
                                    break;
                                default:
                                    break;
                            }

                            bool top = ttRule.Top;
                            SetTop(threat, top);
                            SetTop(ttMitigation, top && rule.Top);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
        }

        private void CreateGenerationRules([NotNull] IMitigation mitigation, MitigationStatus? status,
            [Required] string serviceName, IEnumerable<Rule> rules, IEnumerable<Threat> threats)
        {
            var modelTTs = _model.ThreatTypes?
                .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                .ToArray();

            if ((rules?.Any() ?? false) && (modelTTs?.Any() ?? false) && (threats?.Any() ?? false))
            {
                IItemTemplate itemTemplate = null;
                if (_itemTemplates.TryGetValue(serviceName, out var it))
                    itemTemplate = it;

                foreach (var tt in modelTTs)
                {
                    var ttRule = rules.OfType<ThreatTypeRule>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.Name, tt.Name) == 0);

                    var threat = threats.FirstOrDefault(x => string.CompareOrdinal(x.Name, tt.Name) == 0);

                    if (ttRule != null && threat != null)
                    {
                        try
                        {
                            var ttMitigation = tt.GetMitigation(mitigation.Id);
                            switch (ttRule.Policy)
                            {
                                case ThreatsPolicy.ThreatModel:
                                    AddThreatModelGenRule(ttMitigation, status);
                                    break;
                                case ThreatsPolicy.Processes:
                                    AddProcessesGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.DataStores:
                                    AddDataStoresGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.Full:
                                    AddFullGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.ProcessesFromEI:
                                    AddProcessesFromEIGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.DataStoresFromEI:
                                    AddDataStoresFromEIGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.FullFromEI:
                                    AddFullFromEIGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.AllProcesses:
                                    AddAllProcessesGenRule(ttMitigation, status);
                                    break;
                                case ThreatsPolicy.AllDataStores:
                                    AddAllDataStoresGenRule(ttMitigation, status);
                                    break;
                                case ThreatsPolicy.AllProcessesFromEI:
                                    AddAllProcessesFromEIGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.AllDataStoresFromEI:
                                    AddAllDataStoresFromEIGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.AllFromEI:
                                    AddAllFromEIGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                case ThreatsPolicy.CrossTB:
                                    AddCrossTBGenRule(ttMitigation, status);
                                    break;
                                case ThreatsPolicy.FullCrossTB:
                                    AddFullCrossTBGenRule(ttMitigation, itemTemplate, status);
                                    break;
                                default:
                                    break;
                            }

                            bool top = ttRule.Top;
                            SetTop(tt, top);
                            SetTop(ttMitigation, top && threat.Top);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
        }

        private void SetSelectionRule([NotNull] IPropertiesContainer container, [NotNull] SelectionRule rule)
        {
            if (_schemaManager == null)
                _schemaManager = new AutoGenRulesPropertySchemaManager(_model);

            if (rule is MitigationSelectionRule mitigationSelectionRule && !mitigationSelectionRule.Status.HasValue)
            {
                mitigationSelectionRule.Status = MitigationStatus.Undefined;
            }

            var propertyType = _schemaManager.GetPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property == null)
                    property = container.AddProperty(propertyType, null);

                if (property != null)
                {
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                    {
                        jsonSerializableObject.Value = rule;
                    }
                }
            }
        }

        private SelectionRule GetSelectionRule([NotNull] IPropertiesContainer container)
        {
            SelectionRule result = null;

            if (_schemaManager == null)
                _schemaManager = new AutoGenRulesPropertySchemaManager(_model);

            var propertyType = _schemaManager.GetPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    result = jsonSerializableObject.Value as SelectionRule;
                }
            }

            return result;
        }

        private void SetTop([NotNull] IPropertiesContainer container, bool top)
        {
            if (_schemaManager == null)
                _schemaManager = new AutoGenRulesPropertySchemaManager(_model);
            _schemaManager.SetTop(container, top);
        }

        private void AddThreatModelGenRule([NotNull] IThreatTypeMitigation ttMitigation, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            selectionRule.Root = new EnumValueRuleNode("Object Type", null, null, 
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Threat Model");
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var mr = new MitigationSelectionRule()
            {
                Root = new EnumValueRuleNode("Object Type", null, null,
                    new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Threat Model")
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddProcessesGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var templates = _model.GetEntityTemplates(EntityType.Process)?.Where(x => IsApplicable(threatType, x)).ToArray();
            if (template is IEntityTemplate entityTemplate && entityTemplate.EntityType == EntityType.Process)
            {
                MitigationSelectionRule mr = null;
                var related = GetRelatedItemTemplates(ttMitigation, entityTemplate, templates)?.OfType<IEntityTemplate>();
                if ((related?.Count() ?? 0) > 1)
                {
                    var or = new OrRuleNode() { Name = OR };
                    mr = new MitigationSelectionRule()
                    {
                        Root = or
                    };
                    foreach (var r in related)
                    {
                        or.Children.Add(new ProcessTemplateRuleNode(r));
                    }
                }
                else
                {
                    mr = new MitigationSelectionRule()
                    {
                        Root = new ProcessTemplateRuleNode(entityTemplate)
                    };
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    var child = new ProcessTemplateRuleNode(entityTemplate);
                    var selectionRule = GetSelectionRule(threatType);
                    if (selectionRule == null)
                    {
                        selectionRule = new SelectionRule();
                        selectionRule.Root = child;
                        SetSelectionRule(threatType, selectionRule);
                    }
                    else
                    {
                        var root = selectionRule.Root;
                        if (!IsChildIncluded(root, child))
                        {
                            var orNode = root as OrRuleNode;
                            if (orNode == null)
                            {
                                orNode = new OrRuleNode() { Name = OR };
                                orNode.Children.Add(root);
                                selectionRule.Root = orNode;
                            }
                            orNode.Children.Add(child);
                        }
                    }
                }
            }
        }

        private void AddDataStoresGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var templates = _model.GetEntityTemplates(EntityType.DataStore)?.Where(x => IsApplicable(threatType, x)).ToArray();
 
            if (template is IEntityTemplate entityTemplate && entityTemplate.EntityType == EntityType.DataStore)
            {
                MitigationSelectionRule mr = null;
                var related = GetRelatedItemTemplates(ttMitigation, entityTemplate, templates)?.OfType<IEntityTemplate>();
                if ((related?.Count() ?? 0) > 1)
                {
                    var or = new OrRuleNode() { Name = OR };
                    mr = new MitigationSelectionRule()
                    {
                        Root = or
                    };
                    foreach (var r in related)
                    {
                        or.Children.Add(new DataStoreTemplateRuleNode(r));
                    }
                }
                else
                {
                    mr = new MitigationSelectionRule()
                    {
                        Root = new DataStoreTemplateRuleNode(entityTemplate)
                    };
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    var child = new DataStoreTemplateRuleNode(entityTemplate);
                    var selectionRule = GetSelectionRule(threatType);
                    if (selectionRule == null)
                    {
                        selectionRule = new SelectionRule();
                        selectionRule.Root = child;
                        SetSelectionRule(threatType, selectionRule);
                    }
                    else
                    {
                        var root = selectionRule.Root;
                        if (!IsChildIncluded(root, child))
                        {
                            var orNode = root as OrRuleNode;
                            if (orNode == null)
                            {
                                orNode = new OrRuleNode() { Name = OR };
                                orNode.Children.Add(root);
                                selectionRule.Root = orNode;
                            }
                            orNode.Children.Add(child);
                        }
                    }
                }
            }
        }

        private void AddFullGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var pTemplates = _model.GetEntityTemplates(EntityType.Process)?.Where(x => IsApplicable(threatType, x)).ToArray();
            var dsTemplates = _model.GetEntityTemplates(EntityType.DataStore)?.Where(x => IsApplicable(threatType, x)).ToArray();

            if (template is IEntityTemplate entityTemplate)
            {
                MitigationSelectionRule mr = null;
                OrRuleNode or = null;
                IEntityTemplate pTemplate = null;
                if (entityTemplate.EntityType == EntityType.Process)
                    pTemplate = entityTemplate;
                var pRelated = GetRelatedItemTemplates(ttMitigation, pTemplate, pTemplates)?.OfType<IEntityTemplate>();
                IEntityTemplate dsTemplate = null;
                if (entityTemplate.EntityType == EntityType.DataStore)
                    dsTemplate = entityTemplate;
                var dsRelated = GetRelatedItemTemplates(ttMitigation, dsTemplate, dsTemplates)?.OfType<IEntityTemplate>();

                if ((pRelated?.Count() ?? 0) + (dsRelated?.Count() ?? 0) > 1)
                {
                    or = new OrRuleNode() { Name = OR };
                    mr = new MitigationSelectionRule()
                    {
                        Root = or
                    };
                }

                if ((pRelated?.Count() ?? 0) > 0)
                {
                    if (or != null)
                    {
                        foreach (var r in pRelated)
                        {
                            or.Children.Add(new ProcessTemplateRuleNode(r));
                        }
                    }
                    else
                    {
                        mr = new MitigationSelectionRule()
                        {
                            Root = new ProcessTemplateRuleNode(pRelated.First())
                        };
                    }
                }

                if ((dsRelated?.Count() ?? 0) > 0)
                {
                    if (or != null)
                    {
                        foreach (var r in dsRelated)
                        {
                            or.Children.Add(new DataStoreTemplateRuleNode(r));
                        }
                    }
                    else
                    {
                        mr = new MitigationSelectionRule()
                        {
                            Root = new DataStoreTemplateRuleNode(dsRelated.First())
                        };
                    }
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    EntityTemplateRuleNode child = null;
                    if (entityTemplate.EntityType == EntityType.Process)
                        child = new ProcessTemplateRuleNode(entityTemplate);
                    else if (entityTemplate.EntityType == EntityType.DataStore)
                        child = new DataStoreTemplateRuleNode(entityTemplate);
                    if (child != null)
                    {
                        var selectionRule = GetSelectionRule(threatType);
                        if (selectionRule == null)
                        {
                            selectionRule = new SelectionRule();
                            selectionRule.Root = child;
                            SetSelectionRule(threatType, selectionRule);
                        }
                        else
                        {
                            var root = selectionRule.Root;
                            if (!IsChildIncluded(root, child))
                            {
                                var orNode = root as OrRuleNode;
                                if (orNode == null)
                                {
                                    orNode = new OrRuleNode() { Name = OR };
                                    orNode.Children.Add(root);
                                    selectionRule.Root = orNode;
                                }
                                orNode.Children.Add(child);
                            }
                        }
                    }
                }
            }
        }

        private void AddProcessesFromEIGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var templates = _model.GetEntityTemplates(EntityType.Process)?.Where(x => IsApplicable(threatType, x)).ToArray();

            if (template is IEntityTemplate entityTemplate && entityTemplate.EntityType == EntityType.Process)
            {
                var andNode = new AndRuleNode() { Name = AND };
                andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));

                var mr = new MitigationSelectionRule()
                {
                    Root = andNode
                };

                var related = GetRelatedItemTemplates(ttMitigation, entityTemplate, templates)?.OfType<IEntityTemplate>();
                if ((related?.Count() ?? 0) > 1)
                {
                    var or = new OrRuleNode() { Name = OR };
                    andNode.Children.Add(or);
                    foreach (var r in related)
                    {
                        or.Children.Add(new ProcessTemplateRuleNode(r));
                    }
                }
                else
                {
                    andNode.Children.Add(new ProcessTemplateRuleNode(entityTemplate));
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    var child = new ProcessTemplateRuleNode(entityTemplate);
                    var selectionRule = GetSelectionRule(threatType);
                    if (selectionRule == null)
                    {
                        selectionRule = new SelectionRule();
                        andNode = new AndRuleNode() { Name = AND };
                        selectionRule.Root = andNode;
                        andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
                        andNode.Children.Add(child);
                        SetSelectionRule(threatType, selectionRule);
                    }
                    else
                    {
                        andNode = selectionRule.Root as AndRuleNode;
                        if (andNode != null && andNode.Children.Count == 2)
                        {
                            var secondChild = andNode.Children[1];
                            if (!IsChildIncluded(secondChild, child))
                            {
                                var orNode = secondChild as OrRuleNode;
                                if (orNode == null)
                                {
                                    orNode = new OrRuleNode() { Name = OR };
                                    orNode.Children.Add(andNode.Children[1]);
                                    andNode.Children[1] = orNode;
                                }
                                orNode.Children.Add(child);
                            }
                        }
                    }
                }
            }
        }

        private void AddDataStoresFromEIGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var templates = _model.GetEntityTemplates(EntityType.DataStore)?.Where(x => IsApplicable(threatType, x)).ToArray();

            if (template is IEntityTemplate entityTemplate && entityTemplate.EntityType == EntityType.DataStore)
            {
                var andNode = new AndRuleNode() { Name = AND };
                andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));

                var mr = new MitigationSelectionRule()
                {
                    Root = andNode
                };

                var related = GetRelatedItemTemplates(ttMitigation, entityTemplate, templates)?.OfType<IEntityTemplate>();
                if ((related?.Count() ?? 0) > 1)
                {
                    var or = new OrRuleNode() { Name = OR };
                    andNode.Children.Add(or);
                    foreach (var r in related)
                    {
                        or.Children.Add(new DataStoreTemplateRuleNode(r));
                    }
                }
                else
                {
                    andNode.Children.Add(new DataStoreTemplateRuleNode(entityTemplate));
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    var child = new DataStoreTemplateRuleNode(entityTemplate);
                    var selectionRule = GetSelectionRule(threatType);
                    if (selectionRule == null)
                    {
                        selectionRule = new SelectionRule();
                        andNode = new AndRuleNode() { Name = AND };
                        selectionRule.Root = andNode;
                        andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
                        andNode.Children.Add(child);
                        SetSelectionRule(threatType, selectionRule);
                    }
                    else
                    {
                        andNode = selectionRule.Root as AndRuleNode;
                        if (andNode != null && andNode.Children.Count == 2)
                        {
                            var secondChild = andNode.Children[1];
                            if (!IsChildIncluded(secondChild, child))
                            {
                                var orNode = secondChild as OrRuleNode;
                                if (orNode == null)
                                {
                                    orNode = new OrRuleNode() { Name = OR };
                                    orNode.Children.Add(andNode.Children[1]);
                                    andNode.Children[1] = orNode;
                                }
                                orNode.Children.Add(child);
                            }
                        }
                    }
                }
            }
        }

        private void AddFullFromEIGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var pTemplates = _model.GetEntityTemplates(EntityType.Process)?.Where(x => IsApplicable(threatType, x)).ToArray();
            var dsTemplates = _model.GetEntityTemplates(EntityType.DataStore)?.Where(x => IsApplicable(threatType, x)).ToArray();

            if (template is IEntityTemplate entityTemplate)
            {
                var andNode = new AndRuleNode() { Name = AND };
                andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));

                var mr = new MitigationSelectionRule()
                {
                    Root = andNode
                };

                OrRuleNode or = null;
                IEntityTemplate pTemplate = null;
                if (entityTemplate.EntityType == EntityType.Process)
                    pTemplate = entityTemplate;
                var pRelated = GetRelatedItemTemplates(ttMitigation, pTemplate, pTemplates)?.OfType<IEntityTemplate>();
                IEntityTemplate dsTemplate = null;
                if (entityTemplate.EntityType == EntityType.DataStore)
                    dsTemplate = entityTemplate;
                var dsRelated = GetRelatedItemTemplates(ttMitigation, dsTemplate, dsTemplates)?.OfType<IEntityTemplate>();

                if ((pRelated?.Count() ?? 0) + (dsRelated?.Count() ?? 0) > 1)
                {
                    or = new OrRuleNode() { Name = OR };
                    andNode.Children.Add(or);
                }

                if ((pRelated?.Count() ?? 0) > 0)
                {
                    if (or != null)
                    {
                        foreach (var r in pRelated)
                        {
                            or.Children.Add(new ProcessTemplateRuleNode(r));
                        }
                    }
                    else
                    {
                        andNode.Children.Add(new ProcessTemplateRuleNode(pRelated.First()));
                    }
                }

                if ((dsRelated?.Count() ?? 0) > 0)
                {
                    if (or != null)
                    {
                        foreach (var r in dsRelated)
                        {
                            or.Children.Add(new DataStoreTemplateRuleNode(r));
                        }
                    }
                    else
                    {
                        andNode.Children.Add(new DataStoreTemplateRuleNode(dsRelated.First()));
                    }
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    EntityTemplateRuleNode child = null;
                    if (entityTemplate.EntityType == EntityType.Process)
                        child = new ProcessTemplateRuleNode(entityTemplate);
                    else if (entityTemplate.EntityType == EntityType.DataStore)
                        child = new DataStoreTemplateRuleNode(entityTemplate);
                    if (child != null)
                    { 
                        var selectionRule = GetSelectionRule(threatType);
                        if (selectionRule == null)
                        {
                            selectionRule = new SelectionRule();
                            andNode = new AndRuleNode() { Name = AND };
                            selectionRule.Root = andNode;
                            andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
                            andNode.Children.Add(child);
                            SetSelectionRule(threatType, selectionRule);
                        }
                        else
                        {
                            andNode = selectionRule.Root as AndRuleNode;
                            if (andNode != null && andNode.Children.Count == 2)
                            {
                                var secondChild = andNode.Children[1];
                                if (!IsChildIncluded(secondChild, child))
                                {
                                    var orNode = secondChild as OrRuleNode;
                                    if (orNode == null)
                                    {
                                        orNode = new OrRuleNode() { Name = OR };
                                        orNode.Children.Add(andNode.Children[1]);
                                        andNode.Children[1] = orNode;
                                    }
                                    orNode.Children.Add(child);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddAllProcessesGenRule([NotNull] IThreatTypeMitigation ttMitigation, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            selectionRule.Root = new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Process");
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var mr = new MitigationSelectionRule()
            {
                Root = new EnumValueRuleNode("Object Type", null, null,
                    new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Process")
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddAllDataStoresGenRule([NotNull] IThreatTypeMitigation ttMitigation, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            selectionRule.Root = new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Data Store");
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var mr = new MitigationSelectionRule()
            {
                Root = new EnumValueRuleNode("Object Type", null, null,
                    new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Data Store")
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddAllProcessesFromEIGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            var andNode = new AndRuleNode() { Name = AND };
            selectionRule.Root = andNode;
            andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
            andNode.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Process"));
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var andNode2 = new AndRuleNode() { Name = AND };
            andNode2.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
            andNode2.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Process"));

            var mr = new MitigationSelectionRule()
            {
                Root = andNode2
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddAllDataStoresFromEIGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            var andNode = new AndRuleNode() { Name = AND };
            selectionRule.Root = andNode;
            andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
            andNode.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Data Store"));
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var andNode2 = new AndRuleNode() { Name = AND };
            andNode2.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
            andNode2.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Data Store"));

            var mr = new MitigationSelectionRule()
            {
                Root = andNode2
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddAllFromEIGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            var andNode = new AndRuleNode() { Name = AND };
            selectionRule.Root = andNode;
            andNode.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
            var orNode = new OrRuleNode() { Name = OR };
            andNode.Children.Add(orNode);
            orNode.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Process"));
            orNode.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Data Store"));
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var andNode2 = new AndRuleNode() { Name = AND };
            andNode2.Children.Add(new HasIncomingRuleNode(EntityType.ExternalInteractor));
            var orNode2 = new OrRuleNode() { Name = OR };
            andNode2.Children.Add(orNode);
            orNode2.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Process"));
            orNode2.Children.Add(new EnumValueRuleNode("Object Type", null, null,
                new[] { "External Interactor", "Process", "Data Store", "Flow", "Threat Model" }, "Data Store"));

            var mr = new MitigationSelectionRule()
            {
                Root = andNode2
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddCrossTBGenRule([NotNull] IThreatTypeMitigation ttMitigation, MitigationStatus? status)
        {
            var selectionRule = new SelectionRule();
            selectionRule.Root = new CrossTrustBoundaryRuleNode(CROSSTB, true);
            SetSelectionRule(ttMitigation.ThreatType, selectionRule);

            var mr = new MitigationSelectionRule()
            {
                Root = new CrossTrustBoundaryRuleNode(CROSSTB, true)
            };
            if (status != null)
                mr.Status = status;
            SetSelectionRule(ttMitigation, mr);
        }

        private void AddFullCrossTBGenRule([NotNull] IThreatTypeMitigation ttMitigation, IItemTemplate template, MitigationStatus? status)
        {
            var threatType = ttMitigation.ThreatType;
            var pTemplates = _model.GetEntityTemplates(EntityType.Process)?.Where(x => IsApplicable(threatType, x)).ToArray();
            var dsTemplates = _model.GetEntityTemplates(EntityType.DataStore)?.Where(x => IsApplicable(threatType, x)).ToArray();
            var tbTemplates = _model.TrustBoundaryTemplates?.Where(x => IsApplicable(threatType, x)).ToArray();

            if (!(template is IFlowTemplate))
            {
                var andNode = new AndRuleNode() { Name = AND };
                andNode.Children.Add(new CrossTrustBoundaryRuleNode(CROSSTB, true));

                var mr = new MitigationSelectionRule()
                {
                    Root = andNode
                };

                OrRuleNode or = null;

                var entityTemplate = template as IEntityTemplate;
                IEntityTemplate pTemplate = null;
                if (entityTemplate != null && entityTemplate.EntityType == EntityType.Process)
                    pTemplate = entityTemplate;
                var pRelated = GetRelatedItemTemplates(ttMitigation, pTemplate, pTemplates)?.OfType<IEntityTemplate>();
                IEntityTemplate dsTemplate = null;
                if (entityTemplate != null && entityTemplate.EntityType == EntityType.DataStore)
                    dsTemplate = entityTemplate;
                var dsRelated = GetRelatedItemTemplates(ttMitigation, dsTemplate, dsTemplates)?.OfType<IEntityTemplate>();

                IEnumerable<ITrustBoundaryTemplate> tbRelated;
                if (template is ITrustBoundaryTemplate tbTemplate)
                    tbRelated = GetRelatedItemTemplates(ttMitigation, tbTemplate, tbTemplates)?.OfType<ITrustBoundaryTemplate>();
                else
                    tbRelated = GetRelatedItemTemplates(ttMitigation, null, tbTemplates)?.OfType<ITrustBoundaryTemplate>();

                if (((pRelated?.Count() ?? 0) * 2) + (dsRelated?.Count() ?? 0) + (tbRelated?.Count() ?? 0) > 1)
                {
                    or = new OrRuleNode() { Name = OR };
                    andNode.Children.Add(or);
                }

                if ((pRelated?.Count() ?? 0) > 0)
                {
                    foreach (var r in pRelated)
                    {
                        or.Children.Add(new ProcessTemplateRuleNode(r) { Scope = Scope.Source });
                        or.Children.Add(new ProcessTemplateRuleNode(r) { Scope = Scope.Target });
                    }
                }

                if ((dsRelated?.Count() ?? 0) > 0)
                {
                    if (or != null)
                    {
                        foreach (var r in dsRelated)
                        {
                            or.Children.Add(new DataStoreTemplateRuleNode(r) { Scope = Scope.Target });
                        }
                    }
                    else
                    {
                        andNode.Children.Add(new DataStoreTemplateRuleNode(dsRelated.First()) { Scope = Scope.Target });
                    }
                }

                if ((tbRelated?.Count() ?? 0) > 0)
                {
                    if (or != null)
                    {
                        foreach (var r in tbRelated)
                        {
                            or.Children.Add(new TrustBoundaryTemplateRuleNode(r));
                        }
                    }
                    else
                    {
                        andNode.Children.Add(new TrustBoundaryTemplateRuleNode(tbRelated.First()));
                    }
                }

                if (mr != null)
                {
                    if (status != null)
                        mr.Status = status;
                    SetSelectionRule(ttMitigation, mr);

                    SelectionRuleNode child = null;
                    if (entityTemplate != null && entityTemplate.EntityType == EntityType.Process)
                    {
                        var orNode = new OrRuleNode() { Name = OR };
                        orNode.Children.Add(new ProcessTemplateRuleNode(entityTemplate) { Scope = Scope.Source });
                        orNode.Children.Add(new ProcessTemplateRuleNode(entityTemplate) { Scope = Scope.Target });
                        child = orNode;
                    }
                    else if (entityTemplate != null && entityTemplate.EntityType == EntityType.DataStore)
                        child = new DataStoreTemplateRuleNode(entityTemplate) { Scope = Scope.Target };
                    else if (template is ITrustBoundaryTemplate trustBoundaryTemplate)
                        child = new TrustBoundaryTemplateRuleNode(trustBoundaryTemplate);
                    if (child != null)
                    {
                        var selectionRule = GetSelectionRule(threatType);
                        if (selectionRule == null)
                        {
                            selectionRule = new SelectionRule();
                            andNode = new AndRuleNode() { Name = AND };
                            selectionRule.Root = andNode;
                            andNode.Children.Add(new CrossTrustBoundaryRuleNode(CROSSTB, true));
                            andNode.Children.Add(child);
                            SetSelectionRule(threatType, selectionRule);
                        }
                        else
                        {
                            andNode = selectionRule.Root as AndRuleNode;
                            if (andNode != null && andNode.Children.Count == 2)
                            {
                                var secondChild = andNode.Children[1];
                                if (!IsChildIncluded(secondChild, child))
                                {
                                    var orNode = secondChild as OrRuleNode;
                                    if (orNode == null)
                                    {
                                        orNode = new OrRuleNode() { Name = OR };
                                        orNode.Children.Add(andNode.Children[1]);
                                        andNode.Children[1] = orNode;
                                    }
                                    if (child is OrRuleNode childOr)
                                    {
                                        var children = childOr.Children?.ToArray();
                                        if (children?.Any() ?? false)
                                        {
                                            foreach (var c in children)
                                            {
                                                orNode.Children.Add(c);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        orNode.Children.Add(child);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<IItemTemplate> GetRelatedItemTemplates([NotNull] IThreatTypeMitigation mitigation,
            IItemTemplate reference, IEnumerable<IItemTemplate> population)
        {
            IEnumerable<IItemTemplate> result = null;

            var selected = new List<IItemTemplate>();
            if (population?.Any() ?? false)
            {
                foreach (var item in population)
                {
                    var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, item.Name) == 0);
                    if (!string.IsNullOrWhiteSpace(itemDetails?.Alias))
                        itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, itemDetails.Alias) == 0);
                    if (itemDetails?.AdditionalControls?.Any(x => string.CompareOrdinal(x.Name, mitigation.Mitigation.Name) == 0) ?? false)
                    {
                        selected.Add(item);
                    }
                }
            }

            if (!selected.Any() && reference != null && IsApplicable(mitigation.ThreatType, reference))
                selected.Add(reference);
            
            if (selected.Any())
                result = selected.AsEnumerable();

            return result;
        }

        private bool IsApplicable([NotNull] IThreatType threatType, IItemTemplate template)
        {
            bool result = true;

            var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, template.Name) == 0);
            if (!string.IsNullOrWhiteSpace(itemDetails?.Alias))
                itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, itemDetails.Alias) == 0);
            if (itemDetails?.IgnoreThreatTypes?.Contains(threatType.Name) ?? false)
                result = false;

            return result;
        }

        private bool IsChildIncluded(SelectionRuleNode parent, SelectionRuleNode child)
        {
            bool result = true;

            if (child is EntityTemplateRuleNode entityTemplateRuleNode)
            {
                result = IsChildIncluded(parent, entityTemplateRuleNode);
            }
            else if (child is OrRuleNode orRuleNode)
            {
                var children = orRuleNode.Children?.ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var c in children)
                    {
                        result = result && IsChildIncluded(parent, c);
                    }
                }
            }
            else if (child is TrustBoundaryTemplateRuleNode trustBoundaryTemplateRuleNode)
            {
                result = IsChildIncluded(parent, trustBoundaryTemplateRuleNode);
            }

            return result;
        }

        private bool IsChildIncluded(SelectionRuleNode parent, EntityTemplateRuleNode child)
        {
            bool result;

            if (child == null)
            {
                result = true;
            }
            else if (parent is NaryRuleNode naryRuleNode)
            {
                result = naryRuleNode.Children?.OfType<EntityTemplateRuleNode>().Any(x => x.EntityTemplate == child.EntityTemplate) ?? false;
            }
            else if (parent is EntityTemplateRuleNode entityTemplateRuleNode)
            {
                result = entityTemplateRuleNode.EntityTemplate == child.EntityTemplate;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool IsChildIncluded(SelectionRuleNode parent, TrustBoundaryTemplateRuleNode child)
        {
            bool result;

            if (child == null)
            {
                result = true;
            }
            else if (parent is NaryRuleNode naryRuleNode)
            {
                result = naryRuleNode.Children?.OfType<TrustBoundaryTemplateRuleNode>().Any(x => x.TrustBoundaryTemplate == child.TrustBoundaryTemplate) ?? false;
            }
            else if (parent is TrustBoundaryTemplateRuleNode trustBoundaryTemplateRuleNode)
            {
                result = trustBoundaryTemplateRuleNode.TrustBoundaryTemplate == child.TrustBoundaryTemplate;
            }
            else
            {
                result = false;
            }

            return result;
        }
        #endregion
    }
}
