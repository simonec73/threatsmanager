using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("0138A1D4-CFF6-4AF2-97D8-E2AB90A42C03", "Threat Types Summary Table", 45, ExecutionMode.Business)]
    public class TableSummaryThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableSummaryThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableSummaryThreatTypesPlaceholder();
        }
    }
    public class TableSummaryThreatTypesPlaceholder : ITablePlaceholder
    {
        public string Name => "ThreatTypesSummary";
        public string Label => "Threat Types Summary";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Icons.Resources.threat_types_small;

        public TableItem GetTable([PostSharp.Patterns.Contracts.NotNull] IThreatModel model)
        {
            TableItem result = null;

            var threatEvents = model.GetThreatEvents()?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                result = new TableItem(Name, new[]
                {
                    new TableColumn("Threat Type", 200),
                    new TableColumn("Severity", 50),
                    new TableColumn("Mitigations", 300)
                }, GetCells(threatEvents));
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private IEnumerable<Cell> GetCells([PostSharp.Patterns.Contracts.NotNull] IEnumerable<IThreatEvent> threatEvents)
        {
            var result = new List<Cell>();

            if (threatEvents.Any())
            {
                var model = threatEvents.First().Model;
                var threatEventMitigations = model.GetThreatEventMitigations();

                var threatTypes = threatEvents.Select(x => x.ThreatType).Distinct()
                    .OrderByDescending(x => x.GetTopSeverity(), new SeverityComparer())
                    .ThenBy(x => x.Name)
                    .ToArray();

                foreach (var threatType in threatTypes)
                {
                    var mitigations = threatEventMitigations.Where(x => x.ThreatEvent.ThreatTypeId == threatType.Id).ToArray();

                    result.Add(new Cell(threatType.Name, null, null, new[] {threatType.Id}));
                    var topSeverity = threatType.GetTopSeverity() ?? threatType.Severity;
                    result.Add(new Cell(topSeverity.Name, topSeverity.TextColor, topSeverity.BackColor, false, true));
                    result.Add(new Cell(mitigations
                        .OrderBy(x => x.ThreatEvent.Parent.Name)
                        .ThenBy(x => x.Mitigation.Name)
                        .Select(x =>
                            new Line(x.Mitigation.Name, $"[{model.GetIdentityTypeInitial(x.ThreatEvent.Parent)}] {x.ThreatEvent.Parent.Name}: ", 
                                null, new[] {x.MitigationId}, x.Status.ToString()))));
                }
            }

            return result;
        }
    }
}
