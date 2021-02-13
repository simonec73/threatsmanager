using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine
{
    internal enum RowType
    {
        Normal,
        Merged,
        ThreatType,
        ThreatEvent,
        Mitigation
    }

    internal class Row
    {
        public Row([Required] string value)
        {
            RowType = RowType.Merged;
            Values = new[] {value};
        }
        
        public Row([NotNull] IEnumerable<string> values)
        {
            RowType = RowType.Normal;
            Values = values;
        }

        public Row([NotNull] IThreatType threatType, [NotNull] ISeverity severity, IEnumerable<IThreatEventMitigation> mitigations)
        {
            RowType = RowType.ThreatType;
            Values = new [] {threatType.Name, severity.Name, null};
            Identity = threatType;
            SeverityTextColor = severity.TextColor;
            SeverityBackColor = severity.BackColor;
            Mitigations = mitigations;
        }

        public Row([NotNull] IThreatEvent threatEvent)
        {
            RowType = RowType.ThreatEvent;
            Values = new[] { threatEvent.Name, threatEvent.Severity.Name, null };
            Identity = threatEvent;
            SeverityTextColor = threatEvent.Severity.TextColor;
            SeverityBackColor = threatEvent.Severity.BackColor;
            Mitigations = threatEvent.Mitigations;
        }

        public Row([NotNull] IMitigation mitigation, IEnumerable<IThreatEventMitigation> mitigations)
        {
            RowType = RowType.Mitigation;
            Values = new[] { mitigation.Name, null };
            Identity = mitigation;
            Mitigations = mitigations;
        }

        public RowType RowType { get; protected set; }

        public IEnumerable<string> Values { get; protected set; }

        public IIdentity Identity { get; private set; }

        public KnownColor SeverityTextColor { get; private set; }

        public KnownColor SeverityBackColor { get; private set; }

        public IEnumerable<IThreatEventMitigation> Mitigations { get; private set; }

        public int Count => Values?.Count() ?? 0;
    }
}