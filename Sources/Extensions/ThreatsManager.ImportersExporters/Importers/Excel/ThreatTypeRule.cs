using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO;
using System;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    internal class ThreatTypeRule : Rule
    {
        internal ThreatTypeRule(int row, [NotNull] IWorksheet worksheet, [NotNull] RuleSheetSettings sheet, int key) : base(row, worksheet, sheet, key)
        {
            if (sheet.KeyColumn > 0)
                Id = worksheet[row, sheet.KeyColumn].DisplayText?.Trim('\'', ' ');

            Name = GetString(row, worksheet, sheet, RuleFieldType.Name);
            Top = GetBoolean(row, worksheet, sheet, RuleFieldType.Top) ?? false;
            if (Enum.TryParse<ThreatsPolicy>(GetString(row, worksheet, sheet, RuleFieldType.ServicePolicy), out var servicePolicy))
                ServicePolicy = servicePolicy;
            else
                ServicePolicy = ThreatsPolicy.Undefined;
            if (Enum.TryParse<ThreatsPolicy>(GetString(row, worksheet, sheet, RuleFieldType.CommonPolicy), out var commonPolicy))
                CommonPolicy = commonPolicy;
            else
                CommonPolicy = ThreatsPolicy.Undefined;
            ThreatPolicy = sheet.ThreatPolicy;
        }

        public override bool IsValid => base.IsValid && !string.IsNullOrWhiteSpace(Name) && 
            ServicePolicy != ThreatsPolicy.Undefined && CommonPolicy != ThreatsPolicy.Undefined;

        public string Name { get; private set; }
        public bool Top { get; private set; }
        public ThreatsPolicy ServicePolicy { get; private set; }
        public ThreatsPolicy CommonPolicy { get; private set; }
        public ThreatPolicyType ThreatPolicy { get; private set; }

        public ThreatsPolicy Policy
        {
            get
            {
                var result = ThreatsPolicy.Undefined;
                switch (ThreatPolicy)
                {
                    case ThreatPolicyType.Service:
                        result = ServicePolicy;
                        break;
                    case ThreatPolicyType.Common:
                        result = CommonPolicy;
                        break;
                }

                return result;
            }
        }
    }
}
