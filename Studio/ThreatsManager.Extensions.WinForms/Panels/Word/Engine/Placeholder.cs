using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Extensions.Panels.Word.Engine.Fields;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine
{
    internal class Placeholder
    {
        private readonly Dictionary<string, float> _tablePropWidth = new Dictionary<string, float>();
        private readonly IThreatModel _model;
        private readonly IPropertyType _propertyType;

        public Placeholder([Required] string text, [NotNull] IThreatModel model)
        {
            _model = model;

            var regex = new Regex(
                @"\[ThreatsManagerPlatform:(?<type>Model|Counter|Chart|List|Table)(?<placeholder>[\w]*)\]");
            var match = regex.Match(text);

            if (match.Success && Enum.TryParse<PlaceholderSection>(match.Groups["type"].Value, out var section))
            {
                Section = section;
                Code = match.Groups["placeholder"].Value;
                ItemType = ItemType.Undefined;

                var schemaManager = new WordPropertySchemaManager(model);
                var schema = schemaManager.GetSchema();

                switch (section)
                {
                    case PlaceholderSection.Model:
                        if (string.CompareOrdinal(Code, "Name") == 0)
                        {
                            Image = Properties.Resources.tag_small;
                        }
                        else if (string.CompareOrdinal(Code, "Description") == 0)
                        {
                            Image = Properties.Resources.text_small;
                        }
                        else if (string.CompareOrdinal(Code, "Owner") == 0)
                        {
                            Image = Properties.Resources.caesar_small;
                        }
                        else if (string.CompareOrdinal(Code, "Contributors") == 0)
                        {
                            Image = Properties.Resources.businesspeople_alt_small;
                        }
                        else if (string.CompareOrdinal(Code, "Dependencies") == 0)
                        {
                            Image = Properties.Resources.dependencies_small;
                        }
                        else if (string.CompareOrdinal(Code, "Assumptions") == 0)
                        {
                            Image = Properties.Resources.hint_small;
                        }
                        Name = Code;
                        break;
                    case PlaceholderSection.Counter:
                        if (string.CompareOrdinal(Code, "ThreatTypes") == 0)
                        {
                            Name = "Threat Types";
                            Image = Resources.threat_type_small;
                            ItemType = ItemType.ThreatType;
                        }
                        else if (string.CompareOrdinal(Code, "ThreatEvents") == 0)
                        {
                            Name = "Threat Events";
                            Image = Resources.threat_events_small;
                            ItemType = ItemType.ThreatEvent;
                        }
                        else if (string.CompareOrdinal(Code, "Mitigations") == 0)
                        {
                            Name = "Mitigations";
                            Image = Resources.mitigations_small;
                            ItemType = ItemType.Mitigation;
                        }
                        else if (string.CompareOrdinal(Code, "CriticalThreatTypes") == 0)
                        {
                            Name = "Critical Threat Types";
                            var critical = _model.GetSeverity((int) DefaultSeverity.Critical);
                            if (critical != null)
                                Image = CreateColorBitmap(critical.BackColor);
                        }
                        else if (string.CompareOrdinal(Code, "HighThreatTypes") == 0)
                        {
                            Name = "High Threat Types";
                            var high = _model.GetSeverity((int) DefaultSeverity.High);
                            if (high != null)
                                Image = CreateColorBitmap(high.BackColor);
                        }
                        else if (string.CompareOrdinal(Code, "MediumThreatTypes") == 0)
                        {
                            Name = "Medium Threat Types";
                            var medium = _model.GetSeverity((int) DefaultSeverity.Medium);
                            if (medium != null)
                                Image = CreateColorBitmap(medium.BackColor);
                        }
                        else if (string.CompareOrdinal(Code, "LowThreatTypes") == 0)
                        {
                            Name = "Low Threat Types";
                            var low = _model.GetSeverity((int) DefaultSeverity.Low);
                            if (low != null)
                                Image = CreateColorBitmap(low.BackColor);
                        }
                        else if (string.CompareOrdinal(Code, "InfoThreatTypes") == 0)
                        {
                            Name = "Info Threat Types";
                            var info = _model.GetSeverity((int) DefaultSeverity.Info);
                            if (info != null)
                                Image = CreateColorBitmap(info.BackColor);
                        }
                        else
                        {
                            Name = Code;
                        }
                        break;
                    case PlaceholderSection.Chart:
                        if (string.CompareOrdinal(Code, "ThreatTypes") == 0)
                        {
                            Name = "Threat Types";
                            Image = Properties.Resources.tt_chart_pie_small;
                            ItemType = ItemType.ThreatType;
                        }
                        else if (string.CompareOrdinal(Code, "ThreatEvents") == 0)
                        {
                            Name = "Threat Events";
                            Image = Properties.Resources.red_chart_pie_small;
                            ItemType = ItemType.ThreatEvent;
                        }
                        else if (string.CompareOrdinal(Code, "Mitigations") == 0)
                        {
                            Name = "Mitigations";
                            Image = Properties.Resources.chart_pie_small;
                            ItemType = ItemType.Mitigation;
                        }
                        else if (string.CompareOrdinal(Code, "Roadmap") == 0)
                        {
                            Name = "Roadmap";
                            Image = Properties.Resources.roadmap_small;
                        }
                        else
                        {
                            Name = Code;
                        }
                        break;
                    case PlaceholderSection.List:
                        if (string.CompareOrdinal(Code, "Diagrams") == 0)
                        {
                            Name = "Diagrams";
                            Image = Resources.model_small;
                        }
                        else if (string.CompareOrdinal(Code, "ExternalInteractors") == 0)
                        {
                            Name = "External Interactors";
                            Fields = Field.GetFields(ItemType.ExternalInteractor, model);
                            Image = Resources.external_small;
                            ItemType = ItemType.ExternalInteractor;
                        }
                        else if (string.CompareOrdinal(Code, "Processes") == 0)
                        {
                            Name = "Processes";
                            Fields = Field.GetFields(ItemType.Process, model);
                            Image = Resources.process_small;
                            ItemType = ItemType.Process;
                        }
                        else if (string.CompareOrdinal(Code, "Storages") == 0)
                        {
                            Name = "Data Stores";
                            Fields = Field.GetFields(ItemType.DataStore, model);
                            Image = Resources.storage_small;
                            ItemType = ItemType.DataStore;
                        }
                        else if (string.CompareOrdinal(Code, "Flows") == 0)
                        {
                            Name = "Flows";
                            Fields = Field.GetFields(ItemType.DataFlow, model);
                            Image = Resources.flow_small;
                            ItemType = ItemType.DataFlow;
                        }
                        else if (string.CompareOrdinal(Code, "TrustBoundaries") == 0)
                        {
                            Name = "Trust Boundaries";
                            Fields = Field.GetFields(ItemType.TrustBoundary, model);
                            Image = Resources.trust_boundary_small;
                            ItemType = ItemType.TrustBoundary;
                        }
                        else if (string.CompareOrdinal(Code, "ThreatTypes") == 0)
                        {
                            Name = "Threat Types";
                            Fields = Field.GetFields(ItemType.ThreatType, model);
                            Image = Resources.threat_types_small;
                            ItemType = ItemType.ThreatType;
                        }
                        else if (string.CompareOrdinal(Code, "ThreatEvents") == 0)
                        {
                            Name = "Threat Events";
                            Fields = Field.GetFields(ItemType.ThreatEvent, model);
                            Image = Resources.threat_events_small;
                            ItemType = ItemType.ThreatEvent;
                        }
                        else if (string.CompareOrdinal(Code, "Mitigations") == 0)
                        {
                            Name = "Mitigations";
                            Fields = Field.GetFields(ItemType.Mitigation, model);
                            Image = Resources.mitigations_small;
                            ItemType = ItemType.Mitigation;
                        }
                        else
                        {
                            Name = Code;
                        }

                        _propertyType = schema?.GetPropertyType("IgnoredListFields");
                        if (_propertyType != null)
                        {
                            var property = model.GetProperty(_propertyType) as IPropertyArray;
                            var values = property?.Value?.Where(x=> x.StartsWith($"{Code}#")).ToArray();
                            if (values?.Any() ?? false)
                            {
                                _ignoredFields.AddRange(values.Select(x => x.Replace($"{Code}#", "")));
                            }
                        }
                        break;
                    case PlaceholderSection.Table:
                        if (string.CompareOrdinal(Code, "Severities") == 0)
                        {
                            Name = "Severities";
                            Image = Resources.severity_small;
                            _tablePropWidth.Add("Severity", 0.0f);
                            _tablePropWidth.Add("Description", 0.0f);
                            TableType = TableType.Severities;
                        }
                        else if (string.CompareOrdinal(Code, "ThreatActors") == 0)
                        {
                            Name = "Threat Actors";
                            Image = Resources.actor_small;
                            _tablePropWidth.Add("Threat Actor", 0.0f);
                            _tablePropWidth.Add("Description", 0.0f);
                            TableType = TableType.ThreatActors;
                        }
                        else if (string.CompareOrdinal(Code, "Strengths") == 0)
                        {
                            Name = "Strengths";
                            Image = Resources.strength_small;
                            _tablePropWidth.Add("Strength", 0.0f);
                            _tablePropWidth.Add("Description", 0.0f);
                            TableType = TableType.Strengths;
                        }
                        else if (string.CompareOrdinal(Code, "ControlTypes") == 0)
                        {
                            Name = "Control Types";
                            Image = Properties.Resources.firewall_small;
                            _tablePropWidth.Add("Control Type", 0.0f);
                            _tablePropWidth.Add("Description", 0.0f);
                            TableType = TableType.ControlTypes;
                        }
                        else if (string.CompareOrdinal(Code, "MitigationStatus") == 0)
                        {
                            Name = "Mitigation States";
                            Image = Resources.mitigations_small;
                            _tablePropWidth.Add("Status Name", 0.0f);
                            _tablePropWidth.Add("Description", 0.0f);
                            TableType = TableType.MitigationStatus;
                        }
                        else if (string.CompareOrdinal(Code, "SummaryThreatTypes") == 0)
                        {
                            Name = "Threat Types Summary";
                            Image = Resources.threat_types_small;
                            _tablePropWidth.Add("Threat Type", 0.0f);
                            _tablePropWidth.Add("Severity", 0.0f);
                            _tablePropWidth.Add("Mitigations", 0.0f);
                            TableType = TableType.SummaryThreatTypes;
                        }
                        else if (string.CompareOrdinal(Code, "SummaryThreatEvents") == 0)
                        {
                            Name = "Threat Events Summary";
                            Image = Resources.threat_events_small;
                            _tablePropWidth.Add("Threat Event", 0.0f);
                            _tablePropWidth.Add("Severity", 0.0f);
                            _tablePropWidth.Add("Mitigations", 0.0f);
                            TableType = TableType.SummaryThreatEvents;
                        }
                        else if (string.CompareOrdinal(Code, "SummaryMitigations") == 0)
                        {
                            Name = "Mitigations Summary";
                            Image = Resources.mitigations_small;
                            _tablePropWidth.Add("Mitigations", 0.0f);
                            _tablePropWidth.Add("Related Threats", 0.0f);
                            TableType = TableType.SummaryMitigations;
                        }
                        else if (string.CompareOrdinal(Code, "Roadmap") == 0)
                        {
                            Name = "Roadmap";
                            Image = Properties.Resources.signpost_two_small;
                            _tablePropWidth.Add("Mitigations", 0.0f);
                            _tablePropWidth.Add("Related Threats", 0.0f);
                            TableType = TableType.Roadmap;
                        }
                        else
                        {
                            Name = Code;
                        }

                        _propertyType = schema?.GetPropertyType("ColumnWidth");
                        if (_propertyType != null)
                        {
                            var property = model.GetProperty(_propertyType) as IPropertyArray;
                            var values = property?.Value?.ToArray();
                            if (values?.Any() ?? false)
                            {
                                foreach (var value in values)
                                {
                                    var items = value.Split('#');
                                    if (items.Length == 3)
                                    {
                                        if (string.CompareOrdinal(items[0], Code) == 0 &&
                                            float.TryParse(items[2], out var floatValue))
                                        {
                                            Set(items[1], floatValue);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private Bitmap CreateColorBitmap(KnownColor color)
        {
            Bitmap result = new Bitmap(16, 16);
            using (Graphics graph = Graphics.FromImage(result))
            {
                Rectangle ImageSize = new Rectangle(0,0,16,16);
                var brush = new SolidBrush(Color.FromKnownColor(color));
                graph.FillRectangle(brush, ImageSize);
            }

            return result;
        }

        public string Name { get; }

        public string Code { get; }

        public PlaceholderSection Section { get; }

        public ItemType ItemType { get; }

        public TableType TableType { get; }

        private readonly List<string> _ignoredFields = new List<string>();

        public IEnumerable<Field> Fields { get; }

        public IEnumerable<Field> Selected => Fields?.Where(x => !_ignoredFields.Contains(x.Label));

        public IEnumerable<Field> Ignored => Fields?.Where(x => _ignoredFields.Contains(x.Label));

        public IEnumerable<KeyValuePair<string, float>> PropertyWidths => _tablePropWidth;

        public Bitmap Image { get; }

        public void Set([NotNull] Field field, bool status)
        {
            if (Fields.Contains(field))
            {
                if (status && _ignoredFields.Contains(field.Label))
                {
                    _ignoredFields.Remove(field.Label);
                } else if (!status && !_ignoredFields.Contains(field.Label))
                {
                    _ignoredFields.Add(field.Label);
                }

                if (_propertyType != null)
                {
                    var property = _model.GetProperty(_propertyType) as IPropertyArray;
                    if (property == null)
                    {
                        property = _model.AddProperty(_propertyType, null) as IPropertyArray;
                    }

                    if (property != null)
                    {
                        var list = new List<string>();
                        var current = property.Value?.Where(x => !x.StartsWith($"{Code}#")).ToArray();
                        if (current?.Any() ?? false)
                            list.AddRange(current);
                        list.AddRange(_ignoredFields.Select(x => $"{Code}#{x}"));
                        property.Value = list.ToArray();
                    }
                }
            }
        }

        public void Set([Required] string propertyName, float width)
        {
            if (_tablePropWidth.ContainsKey(propertyName))
            {
                _tablePropWidth[propertyName] = width;

                if (_propertyType != null)
                {
                    var newValue = $"{Code}#{propertyName}#{width.ToString()}";
                    var property = _model.GetProperty(_propertyType) as IPropertyArray;
                    var values = property?.Value?.ToArray();
                    if (values?.Any() ?? false)
                    {
                        bool found = false;
                        for (int i = 0; i < values.Length; i++)
                        {
                            var items = values[i].Split('#');
                            if (items.Length == 3)
                            {
                                if (string.CompareOrdinal(items[0], Code) == 0 &&
                                    string.CompareOrdinal(items[1], propertyName) == 0)
                                {
                                    values[i] = newValue;
                                    property.Value = values.ToArray();
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if (!found)
                        {
                            var list = new List<string>();
                            var current = property.Value?.ToArray();
                            if (current?.Any() ?? false)
                                list.AddRange(current);
                            list.Add(newValue);
                            property.Value = list.ToArray();
                        }
                    }
                    else
                    {
                        property = _model.AddProperty(_propertyType, null) as IPropertyArray;
                        if (property != null)
                            property.Value = new [] { newValue };
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}