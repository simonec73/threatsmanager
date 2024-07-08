using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    internal class MitigationRule : Rule
    {
        internal MitigationRule(int row, [NotNull] IWorksheet worksheet, [NotNull] RuleSheetSettings sheet, int key) : base(row, worksheet, sheet, key)
        {
            if (sheet.KeyColumn > 0)
                Id = worksheet[row, sheet.KeyColumn].DisplayText?.Trim('\'', ' ');

            Name = GetString(row, worksheet, sheet, RuleFieldType.Name);
            Level = GetString(row, worksheet, sheet, RuleFieldType.Level);
            Service = GetBoolean(row, worksheet, sheet, RuleFieldType.Service) ?? false;
            Common = GetBoolean(row, worksheet, sheet, RuleFieldType.Common) ?? false;
            if (Enum.TryParse<SecurityControlType>(GetString(row, worksheet, sheet, RuleFieldType.ControlType), out var controlType))
                ControlType = controlType;
            else
                ControlType = SecurityControlType.Unknown;
            Top = GetBoolean(row, worksheet, sheet, RuleFieldType.Top) ?? false;
            if (Enum.TryParse<MitigationStatus>(GetString(row, worksheet, sheet, RuleFieldType.Status), out var status))
                Status = status;
            var namingRuleString = GetString(row, worksheet, sheet, RuleFieldType.NamingRule);
            if (namingRuleString.StartsWith(NamingRule.Replace.ToString()))
            {
                var parts = namingRuleString.Replace(NamingRule.Replace.ToString(), "").Split(',');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out var start) && start >= 0 &&
                    int.TryParse(parts[1], out var len) && len > 0)
                {
                    Naming = NamingRule.Replace;
                    ReplaceStart = start;
                    ReplaceLength = len;
                }
                else
                    Naming = NamingRule.Parenthesis;
            }
            else if (Enum.TryParse<NamingRule>(namingRuleString, out var namingRule))
            {
                Naming = namingRule;
            }
            else
            {
                Naming = NamingRule.Parenthesis;
            }

            var threats = GetString(row, worksheet, sheet, RuleFieldType.Threats);
            if (!string.IsNullOrWhiteSpace(threats))
            {
                var dict = new Dictionary<string, DefaultStrength>();
                using (var reader = new StringReader(threats))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var split = line.Split('(');
                        if (split.Length == 2)
                        {
                            var name = split[0].TrimEnd();
                            if (Enum.TryParse<DefaultStrength>(split[1].TrimEnd(')'), false, out var strength))
                            {
                                if (!dict.ContainsKey(name))
                                    dict.Add(name, strength);
                            }
                        }
                    }
                }
                Threats = dict;
            }
        }

        public override bool IsValid => base.IsValid && !string.IsNullOrWhiteSpace(Name) && ControlType != SecurityControlType.Unknown;

        public string Name { get; private set; }
        public string Level { get; private set; }
        public bool Service { get; private set; }
        public bool Common { get; private set; }
        public SecurityControlType ControlType { get; private set; }
        public bool Top { get; private set; }
        public MitigationStatus? Status { get; private set; }
        public NamingRule Naming { get; private set; }
        public int ReplaceStart { get; private set; }
        public int ReplaceLength { get; private set; }
        public IDictionary<string, DefaultStrength> Threats { get; private set; }

        public string GetSpecificName(string specifier)
        {
            string result;

            if (string.IsNullOrWhiteSpace(specifier))
            {
                result = Name;
            }
            else
            {
                switch (Naming)
                {
                    case NamingRule.For:
                        result = $"{Name} for {specifier}";
                        break;
                    case NamingRule.On:
                        result = $"{Name} on {specifier}";
                        break;
                    case NamingRule.Replace:
                        if (ReplaceLength > 0)
                        {
                            result = ReplaceText(specifier, ReplaceStart, ReplaceLength);
                        }
                        else
                        {
                            result = $"{Name} ({specifier})";
                        }
                        break;
                    case NamingRule.With:
                        result = $"{Name} with {specifier}";
                        break;
                    default:
                        result = $"{Name} ({specifier})";
                        break;
                }
            }

            return result;
        }

        private string ReplaceText(string text, int start, int nSpaces)
        {
            string result = null;

            if (nSpaces > 0)
            { 
                var spaces = new List<int>();
                int current = 0;
                do
                {
                    current = Name.IndexOf(' ', current);
                    if (current >= 0)
                    {
                        spaces.Add(current);
                        current++;
                    }
                } while (current != -1);

                if (spaces.Count >= start + nSpaces - 1)
                {
                    var builder = new StringBuilder();

                    if (start <= 0)
                    {
                        builder.Append(text);
                    }
                    else
                    {
                        builder.Append(Name.Substring(0, spaces[start - 1]));
                        builder.Append(' ');
                        builder.Append(text);
                    }
                    
                    if (spaces.Count > start + nSpaces - 1)
                    {
                        builder.Append(' ');
                        builder.Append(Name.Substring(spaces[start + nSpaces - 1] + 1));
                    }

                    result = builder.ToString();
                }
            }

            return result;
        }
    }
}
