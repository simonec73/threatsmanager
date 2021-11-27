using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("5F752B4B-A4E1-41F4-A7DC-FC9FBD6A774F", "Threat Events Summary Table", 47, ExecutionMode.Business)]
    public class TableSummaryThreatEventsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableSummaryThreatEvents";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableSummaryThreatEventsPlaceholder();
        }
    }
    public class TableSummaryThreatEventsPlaceholder : ITablePlaceholder
    {
        public string Name => "ThreatEventsSummary";
        public string Label => "Threat Events Summary";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Icons.Resources.threat_events_small;

        public TableItem GetTable([NotNull] IThreatModel model)
        {
            TableItem result = null;

            var threatEvents = model.GetThreatEvents()?
                .OrderByDescending(x => x.Severity, new SeverityComparer())
                .ThenBy(x => x.Parent.Name)
                .ThenBy(x => x.Name)
                .ToArray();
            if (threatEvents?.Any() ?? false)
            {
                result = new TableItem(Name, new[]
                {
                    new TableColumn("Threat Event", 200),
                    new TableColumn("Severity", 50),
                    new TableColumn("Mitigations", 300)
                }, GetCells(threatEvents));
            }

            return result;
        }

        private IEnumerable<Cell> GetCells([NotNull] IEnumerable<IThreatEvent> threatEvents)
        {
            var result = new List<Cell>();

            foreach (var threatEvent in threatEvents)
            {
                result.Add(new Cell(threatEvent.Name, null, 
                    $" on [{threatEvent.Model.GetIdentityTypeInitial(threatEvent.Parent)}] {threatEvent.Parent.Name}", 
                    new []{threatEvent.Id, threatEvent.ThreatTypeId}));
                result.Add(new Cell(threatEvent.Severity.Name, threatEvent.Severity.TextColor, threatEvent.Severity.BackColor, false, true));
                result.Add(new Cell(threatEvent.Mitigations?
                    .OrderBy(x => x.ThreatEvent.Parent.Name)
                    .ThenBy(x => x.Mitigation.Name)
                    .Select(x =>
                        new Line(x.Mitigation.Name, null, null, new[] {x.MitigationId}, x.Status.ToString()))));
            }

            return result;
        }
    }
}
