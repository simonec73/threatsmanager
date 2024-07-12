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
        private IEnumerable<ItemDetails> _itemDetails;

        public event Func<string, ItemTemplateInfo> GetItemTemplateInfo;

        #region Constructors.
        public Importer([NotNull] IThreatModel model, [Required] string settingsFile) : this(model)
        {
            _settings = LoadSettings(settingsFile);
            _path = Path.GetDirectoryName(settingsFile);
        }

        public Importer([NotNull] IThreatModel model)
        {
            _model = model;
        }
        #endregion

        #region Public members.
        public bool Import([Required] string fileName, IEnumerable<ParameterValue> parameterValues = null)
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

                    result = ImportExcelFile(fileName);

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

                    _itemDetails = ImportItemMapping()?.Items;
                    var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, itemName) == 0);
                    if (itemDetails != null)
                    {
                        var entityTemplates = _model.EntityTemplates?.ToArray();
                        var flowTemplates = _model.FlowTemplates?.ToArray();
                        var trustBoundaryTemplates = _model.TrustBoundaryTemplates?.ToArray();
                        result = CreateItemTemplate(itemName, entityTemplates, flowTemplates, trustBoundaryTemplates) != null;
                    }
                }
                finally
                {
                    ParameterManager.Release();
                }
            }

            return result;
        }

        public bool ImportItem([Required] string itemName)
        {
            var result = false;

            _itemDetails = ImportItemMapping()?.Items;
            var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(x.Name, itemName) == 0);
            if (itemDetails != null)
            {
                var entityTemplates = _model.EntityTemplates?.ToArray();
                var flowTemplates = _model.FlowTemplates?.ToArray();
                var trustBoundaryTemplates = _model.TrustBoundaryTemplates?.ToArray();
                result = CreateItemTemplate(itemName, entityTemplates, flowTemplates, trustBoundaryTemplates) != null;
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
        private bool ImportExcelFile([Required] string fileName)
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
                        ExcelParseOptions.DoNotParseCharts | ExcelParseOptions.DoNotParsePivotTable);
                }

                if (!_settings.StrictValidation || Validate(workbook))
                {
                    result = Load(workbook);
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

        private bool Load([NotNull] IWorkbook workbook)
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
                        result |= Load(sheet, worksheet);
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

        private bool Load([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            var result = false;

            _itemDetails = ImportItemMapping()?.Items?.ToArray();

            switch (sheet.ObjectType)
            {
                case ObjectType.ThreatType:
                    result = LoadThreatTypes(sheet, worksheet);
                    break;
                case ObjectType.Mitigation:
                    result = LoadMitigations(sheet, worksheet);
                    break;
                case ObjectType.ThreatTypeMitigation:
                    result = LoadThreatTypeMitigations(sheet, worksheet);
                    break;
                case ObjectType.EntityTemplate:
                    result = LoadEntityTemplates(sheet, worksheet);
                    break;
                case ObjectType.SpecializedMitigation:
                    result = LoadSpecializedMitigations(sheet, worksheet);
                    break;
            }

            RemoveMitigations(sheet);
            AssignControlType(sheet);
            AssignDefaultControlType(sheet);
            MapThreatTypesWithMitigations(sheet);

            return result;
        }

        private void RemoveMitigations([NotNull] SheetSettings sheet)
        {
            var identifiers = sheet.MitigationsToBeRemoved?.Identifiers?.ToArray();
            if (identifiers?.Any() ?? false)
            {
                IPropertyType mPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.MitigationsToBeRemoved.SchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.MitigationsToBeRemoved.SchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.MitigationsToBeRemoved.PropertyName))
                {
                    mPropertyType = _model.GetSchema(sheet.MitigationsToBeRemoved.SchemaName,
                        sheet.MitigationsToBeRemoved.SchemaNamespace)?
                        .GetPropertyType(sheet.MitigationsToBeRemoved.PropertyName);
                }

                IPropertyType levelPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.MitigationsToBeRemoved.LevelSchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.MitigationsToBeRemoved.LevelSchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.MitigationsToBeRemoved.LevelPropertyName))
                {
                    levelPropertyType = _model.GetSchema(sheet.MitigationsToBeRemoved.LevelSchemaName,
                        sheet.MitigationsToBeRemoved.LevelSchemaNamespace)?
                        .GetPropertyType(sheet.MitigationsToBeRemoved.LevelPropertyName);
                }

                foreach (var identifier in identifiers)
                {
                    var mitigation = GetMitigation(identifier, mPropertyType, levelPropertyType);
                    if (mitigation != null)
                        _model.RemoveMitigation(mitigation.Id);
                }
            }
        }

        private void AssignControlType([NotNull] SheetSettings sheet)
        {
            var associations = sheet.ControlTypeAssociations?.Associations?.ToArray();
            if (associations?.Any() ?? false)
            {
                IPropertyType mPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.ControlTypeAssociations.MitigationSchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.ControlTypeAssociations.MitigationSchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.ControlTypeAssociations.MitigationPropertyName))
                {
                    mPropertyType = _model.GetSchema(sheet.ControlTypeAssociations.MitigationSchemaName,
                        sheet.ControlTypeAssociations.MitigationSchemaNamespace)?
                        .GetPropertyType(sheet.ControlTypeAssociations.MitigationPropertyName);
                }

                IPropertyType levelPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.ControlTypeAssociations.LevelSchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.ControlTypeAssociations.LevelSchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.ControlTypeAssociations.LevelPropertyName))
                {
                    levelPropertyType = _model.GetSchema(sheet.ControlTypeAssociations.LevelSchemaName,
                        sheet.ControlTypeAssociations.LevelSchemaNamespace)?
                        .GetPropertyType(sheet.ControlTypeAssociations.LevelPropertyName);
                }

                foreach (var association in associations)
                {
                    var mitigation = GetMitigation(association.Identifier, mPropertyType, levelPropertyType);
                    if (mitigation != null)
                        mitigation.ControlType = association.ControlType;
                }
            }
        }

        private void AssignDefaultControlType([NotNull] SheetSettings sheet)
        {
            var controlType = sheet.ControlTypeAssociations?.Default ?? SecurityControlType.Unknown;
            if (controlType != SecurityControlType.Unknown)
            {
                var mitigations = _model.Mitigations?
                        .Where(x => x.ControlType == SecurityControlType.Unknown).ToArray();
                if (mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in mitigations)
                    {
                        mitigation.ControlType = controlType;
                    }
                }
            }
        }

        private void MapThreatTypesWithMitigations([NotNull] SheetSettings sheet)
        {
            var mappings = sheet.ThreatTypeMitigationMappings?.Mappings?.ToArray();
            if (mappings?.Any() ?? false)
            {
                IPropertyType ttPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.ThreatTypeSchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.ThreatTypeSchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.ThreatTypePropertyName))
                {
                    ttPropertyType = _model.GetSchema(sheet.ThreatTypeMitigationMappings.ThreatTypeSchemaName,
                        sheet.ThreatTypeMitigationMappings.ThreatTypeSchemaNamespace)?
                        .GetPropertyType(sheet.ThreatTypeMitigationMappings.ThreatTypePropertyName);
                }

                IPropertyType mPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.MitigationSchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.MitigationSchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.MitigationPropertyName))
                {
                    mPropertyType = _model.GetSchema(sheet.ThreatTypeMitigationMappings.MitigationSchemaName,
                        sheet.ThreatTypeMitigationMappings.MitigationSchemaNamespace)?
                        .GetPropertyType(sheet.ThreatTypeMitigationMappings.MitigationPropertyName);
                }

                IPropertyType levelPropertyType = null;
                if (!string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.LevelSchemaName) &&
                    !string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.LevelSchemaNamespace) &&
                    !string.IsNullOrWhiteSpace(sheet.ThreatTypeMitigationMappings.LevelPropertyName))
                {
                    levelPropertyType = _model.GetSchema(sheet.ThreatTypeMitigationMappings.LevelSchemaName,
                        sheet.ThreatTypeMitigationMappings.LevelSchemaNamespace)?
                        .GetPropertyType(sheet.ThreatTypeMitigationMappings.LevelPropertyName);
                }

                foreach (var mapping in mappings)
                {
                    DoMapping(mapping, ttPropertyType, mPropertyType, levelPropertyType);
                }
            }
        }

        private void DoMapping([NotNull] ThreatTypeMitigationMapping mapping, IPropertyType ttPropertyType,
            IPropertyType mPropertyType, IPropertyType levelPropertyType)
        {
            if (string.IsNullOrWhiteSpace(mapping.BulkThreats))
            {
                if (string.IsNullOrWhiteSpace(mapping.BulkMitigation))
                {
                    var threatType = GetThreatType(mapping, ttPropertyType);
                    var mitigation = GetMitigation(mapping.MitigationIdentifier, mPropertyType, levelPropertyType);
                    var strength = _model.GetStrength((int) mapping.Strength);
                    CreateMapping(threatType, mitigation, strength);
                }
                else
                {
                    var threatType = GetThreatType(mapping, ttPropertyType);
                    var list = SplitBulkList(mapping.BulkMitigation);
                    IEnumerable<IMitigation> mitigations;
                    if (levelPropertyType == null)
                    {
                        mitigations = _model.Mitigations?.ToArray();
                    }
                    else
                    {
                        mitigations = _model.Mitigations?
                            .Where(x => string.CompareOrdinal(x.GetProperty(levelPropertyType)?.StringValue, _settings.Level) == 0)
                            .ToArray();
                    }

                    if ((list?.Any() ?? false) && (mitigations?.Any() ?? false))
                    {
                        foreach (var item in list)
                        {
                            var mitigation = mitigations.FirstOrDefault(x => string.CompareOrdinal(x.Name, item.Key) == 0);
                            if (threatType != null)
                            {
                                CreateMapping(threatType, mitigation, item.Value);
                            }
                        }
                    }
                }
            }
            else
            {
                var mitigation = GetMitigation(mapping.MitigationIdentifier, mPropertyType, levelPropertyType);
                var list = SplitBulkList(mapping.BulkThreats);
                var threatTypes = _model.ThreatTypes?.ToArray();
                if ((list?.Any() ?? false) && (threatTypes?.Any() ?? false))
                {
                    foreach (var item in list)
                    {
                        var threatType = threatTypes.FirstOrDefault(x => string.CompareOrdinal(x.Name, item.Key) == 0);
                        if (threatType != null)
                        {
                            CreateMapping(threatType, mitigation, item.Value);
                        }
                    }
                }
            }
         }

        private void CreateMapping(IThreatType threatType, IMitigation mitigation, IStrength strength)
        {
            if (threatType != null && mitigation != null)
            {
                threatType.AddMitigation(mitigation, strength);
            }
        }

        private IThreatType GetThreatType([NotNull] ThreatTypeMitigationMapping mapping, 
            IPropertyType ttPropertyType)
        {
            IThreatType threatType = null;
            if (ttPropertyType == null)
            {
                threatType = _model?.ThreatTypes?
                    .FirstOrDefault(x => string.CompareOrdinal(x.Name, mapping.ThreatTypeIdentifier) == 0);
            }
            else
            {
                threatType = _model.ThreatTypes?
                    .FirstOrDefault(x => string.CompareOrdinal(x.GetProperty(ttPropertyType)?.StringValue, mapping.ThreatTypeIdentifier) == 0);
            }

            return threatType;
        }

        private IMitigation GetMitigation(string identifier, 
            IPropertyType mPropertyType, IPropertyType levelPropertyType)
        {
            IMitigation mitigation = null;
            if (mPropertyType == null)
            {
                mitigation = _model?.Mitigations?
                    .FirstOrDefault(x => string.CompareOrdinal(x.Name, identifier) == 0);
            }
            else
            {
                if (levelPropertyType == null)
                {
                    mitigation = _model.Mitigations?
                        .FirstOrDefault(x => string.CompareOrdinal(x.GetProperty(mPropertyType)?.StringValue, identifier) == 0);
                }
                else
                {
                    mitigation = _model.Mitigations?
                        .FirstOrDefault(x => string.CompareOrdinal(x.GetProperty(mPropertyType)?.StringValue, identifier) == 0 &&
                                             string.CompareOrdinal(x.GetProperty(levelPropertyType)?.StringValue, _settings.Level) == 0);
                }
            }

            return mitigation;
        }

        private IEnumerable<KeyValuePair<string, IStrength>> SplitBulkList(string bulkList)
        {
            IEnumerable<KeyValuePair<string, IStrength>> result = null;

            if (!string.IsNullOrWhiteSpace(bulkList))
            {
                var lines = bulkList.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Any())
                {
                    var list = new List<KeyValuePair<string, IStrength>>();
                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"^(?<name>.+)\s+\((?<strength>.+)\)$");
                        if (match.Success)
                        {
                            var name = match.Groups["name"].Value;
                            var strength = _model.Strengths.FirstOrDefault(x => string.CompareOrdinal(x.Name, match.Groups["strength"].Value) == 0);
                            if (strength != null)
                                list.Add(new KeyValuePair<string, IStrength>(name, strength));
                        }
                    }

                    if (list.Any())
                        result = list.AsReadOnly();
                }
            }

            return result;
        }
        #endregion

        #region Item Templates management.
        private ItemMappingSettings ImportItemMapping()
        {
            ItemMappingSettings result = null;

            if (!string.IsNullOrWhiteSpace(_settings.ItemTemplates))
            {
                var fullPath = Path.Combine(_path, _settings.ItemTemplates);

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

        private IItemTemplate GetItemTemplate([NotNull] SheetSettings sheet)
        {
            IItemTemplate result = null;

            var targetItem = ParameterManager.Instance.ApplyParameters(sheet.TargetItem);

            if (targetItem != null)
            {
                if (_itemTemplates.TryGetValue(targetItem, out var itemTemplate))
                {
                    result = itemTemplate;
                }
                else
                {
                    CreateItemTemplate(targetItem, _model.EntityTemplates, _model.FlowTemplates, _model.TrustBoundaryTemplates);
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


        private IItemTemplate CreateItemTemplate([Required] string serviceName,
            IEnumerable<IEntityTemplate> entityTemplates, IEnumerable<IFlowTemplate> flowTemplates, 
            IEnumerable<ITrustBoundaryTemplate> trustBoundaryTemplates)
        {
            IItemTemplate result = null;

            var name = serviceName;

            var itemDetails = _itemDetails?.FirstOrDefault(x => string.CompareOrdinal(name, x.Name) == 0);
            string iconPath = null;
            if (itemDetails != null)
            {
                if (!string.IsNullOrWhiteSpace(itemDetails.Icon))
                    iconPath = Path.Combine(_path, 
                        Path.Combine(Path.GetDirectoryName(_settings.ItemTemplates), itemDetails.Icon));
            }

            if (itemDetails != null)
            {
                var itemTemplates = GetItemTemplates(itemDetails.ItemType,
                    entityTemplates, flowTemplates, trustBoundaryTemplates);
                result = AddItemTemplate(name, itemDetails.Name, itemDetails.Description, itemDetails.ItemType,
                    itemTemplates, itemDetails.Properties, iconPath);
                CreateAdditionalControls(name, itemDetails);
            }
            else
            {
                var itemTemplateInfo = GetItemTemplateInfo?.Invoke(name);
                if (itemTemplateInfo != null)
                {
                    var itemTemplates = GetItemTemplates(itemTemplateInfo.ItemType,
                        entityTemplates, flowTemplates, trustBoundaryTemplates);
                    result = AddItemTemplate(name, itemTemplateInfo.Name, itemTemplateInfo.Description, 
                        itemTemplateInfo.ItemType, itemTemplates);
                }
            }

            return result;
        }

        private IItemTemplate AddItemTemplate([Required] string key, [Required] string name, string description, 
            ItemType itemType, IEnumerable<IItemTemplate> itemTemplates, 
            IEnumerable<Property> properties = null, string iconFile = null)
        {
            IItemTemplate result = null;

            var existing = itemTemplates?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);

            if (existing == null)
            {
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
                        result = _model.AddEntityTemplate(name, description, 
                            largeImage, mediumImage, smallImage, EntityType.ExternalInteractor);
                        break;
                    case ItemType.Process:
                        result = _model.AddEntityTemplate(name, description,
                            largeImage, mediumImage, smallImage, EntityType.Process);
                        break;
                    case ItemType.DataStore:
                        result = _model.AddEntityTemplate(name, description,
                            largeImage, mediumImage, smallImage, EntityType.DataStore);
                        break;
                    case ItemType.Flow:
                        result = _model.AddFlowTemplate(name, description);
                        break;
                    case ItemType.TrustBoundary:
                        result = _model.AddTrustBoundaryTemplate(name, description);
                        break;
                    default:
                        result = null;
                        break;
                }

                if (result != null)
                {
                    _itemTemplates.Add(key, result);

                    var ps = properties?.ToArray();
                    if (ps?.Any() ?? false)
                    {
                        foreach (var p in ps)
                        {
                            var propertyType = _model.GetSchema(p.Schema, p.Namespace)?.GetPropertyType(p.Name);
                            if (propertyType != null)
                                result.AddProperty(propertyType, p.Value);
                        }
                    }
                }
            }

            return result;
        }

        private bool CreateAdditionalControls([Required] string serviceName, [NotNull] ItemDetails itemDetails)
        {
            bool result = false;

            var controls = itemDetails.AdditionalControls?.ToArray();
            if (controls?.Any() ?? false)
            {
                foreach (var control in controls)
                {
                    var existing =
                        _model.Mitigations?.FirstOrDefault(x => string.CompareOrdinal(x.Name, control.Name) == 0);

                    if (existing != null)
                    {
                        existing.Description = control.Description;
                        existing.ControlType = control.ControlType;
                        LoadProperties(existing, serviceName, control.Properties);
                        result = true;
                    }
                    else
                    {
                        var mitigation = _model.AddMitigation(control.Name);
                        mitigation.Description = control.Description;
                        mitigation.ControlType = control.ControlType;
                        LoadProperties(mitigation, serviceName, control.Properties);

                        result = true;
                    }
                }
            }

            return result;
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
        private bool LoadThreatTypes([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
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
                                            LoadProperties(sheet, worksheet, row, existing);
                                            result = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    var threatType = _model.AddThreatType(name, severity);
                                    threatType.Description = description;
                                    AddLevelInfo(threatType, sheet);
                                    LoadProperties(sheet, worksheet, row, threatType);
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

        private bool LoadMitigations([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
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
                                    var key = GetKey(sheet, worksheet, row);

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
                                                LoadProperties(sheet, worksheet, row, existing);
                                                LoadCalculatedColumns(sheet, existing, workbooks, key);
                                                result = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        var mitigation = _model.AddMitigation(name);
                                        mitigation.Description = description;
                                        AddLevelInfo(mitigation, sheet);
                                        LoadProperties(sheet, worksheet, row, mitigation);
                                        LoadCalculatedColumns(sheet, mitigation, workbooks, key);
                                        result = true;
                                    }
                                }
                            }

                            row++;
                        } while (true);
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

        private bool LoadThreatTypeMitigations([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
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
                                        AddLevelInfo(threatTypeMitigation, sheet);
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

        private bool LoadEntityTemplates([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
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
                                            LoadProperties(sheet, worksheet, row, existing);
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
                                        AddLevelInfo(entityTemplate, sheet);
                                        LoadProperties(sheet, worksheet, row, entityTemplate);
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

        private bool LoadSpecializedMitigations([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet)
        {
            var result = false;

            var columns = sheet.Columns?.ToArray();
            if (columns?.Any() ?? false)
            {
                int row = sheet.FirstRow;
                var nameSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Name);
                var descSettings = columns.FirstOrDefault(x => x.FieldType == FieldType.Description);
                var keySettings = columns.FirstOrDefault(x => x.IsKey || x.FieldType == FieldType.Key);
                var mitigations = _model.Mitigations?.ToArray();
                var itemTemplate = GetItemTemplate(sheet);

                if (row > 0 && itemTemplate != null && 
                    keySettings != null && keySettings.Index > 0 && 
                    (mitigations?.Any() ?? false))
                {
                    if (sheet.ForceClear)
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

                    var schema = _model.GetSchema(keySettings.SchemaName, keySettings.SchemaNamespace);
                    if (schema != null)
                    {
                        var propertyType = schema.GetPropertyType(keySettings.PropertyName);

                        if (propertyType != null)
                        {
                            do
                            {
                                if (CheckFilter(sheet, worksheet, row))
                                {
                                    var key = GetKey(sheet, worksheet, row);
                                    if (!string.IsNullOrWhiteSpace(key))
                                    {
                                        var mitigation = mitigations
                                            .FirstOrDefault(x => string.CompareOrdinal(x.GetProperty(propertyType)?.StringValue, key) == 0);

                                        if (mitigation != null)
                                        {
                                            var name = GetValue(worksheet, row, nameSettings);
                                            var description = GetValue(worksheet, row, descSettings);
                                            mitigation.AddSpecializedMitigation(itemTemplate, name, description);
                                        }
                                    }
                                }

                                row++;
                            } while (true);
                        }
                    }
                }
            }

            return result;
        }

        private void AddLevelInfo([NotNull] IPropertiesContainer container, [NotNull] SheetSettings sheet)
        {
            if (!string.IsNullOrWhiteSpace(sheet.LevelPropertyName) &&
                !string.IsNullOrWhiteSpace(sheet.LevelSchemaName) &&
                !string.IsNullOrWhiteSpace(sheet.LevelSchemaNamespace) &&
                !string.IsNullOrWhiteSpace(_settings?.Level))
            {
                AddProperty(container, sheet.LevelSchemaName, sheet.LevelSchemaNamespace, sheet.LevelPropertyName, _settings.Level);
            }
        }

        private void LoadProperties([NotNull] SheetSettings sheet, [NotNull] IWorksheet worksheet,
            int row, [NotNull] IPropertiesContainer container)
        {
            var columns = sheet.Columns?
                .Where(x => x.FieldType == FieldType.Property)
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
                                            ExcelParseOptions.DoNotParseCharts | ExcelParseOptions.DoNotParsePivotTable);
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

            var column = sheet.Columns?.FirstOrDefault(x => x.IsKey || x.FieldType == FieldType.Key);
            if (column == null)
                EventsDispatcher.RaiseEvent("ShowWarning", "The IsKey flag is missing. Please configure IsKey for at least a column for the imported Excel file.");
            else
                result = GetMappedValue(worksheet, row, column);

            return result;
        }
        #endregion
    }
}
