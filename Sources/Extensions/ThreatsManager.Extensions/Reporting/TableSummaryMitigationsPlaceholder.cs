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
    [Extension("6EDC50EF-62AD-47A5-8EF2-804A82594110", "Mitigations Summary Table", 46, ExecutionMode.Business)]
    public class TableSummaryMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableSummaryMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableSummaryMitigationsPlaceholder();
        }
    }
    public class TableSummaryMitigationsPlaceholder : ITablePlaceholder
    {
        public string Name => "MitigationsSummary";
        public string Label => "Mitigations Summary";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Icons.Resources.mitigations_small;

        public TableItem GetTable([NotNull] IThreatModel model)
        {
            TableItem result = null;

            var mitigations = model.GetUniqueMitigations()?
                .OrderBy(x => x.ToString())
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                result = new TableItem(Name, new[]
                {
                    new TableColumn("Mitigation", 200),
                    new TableColumn("Related Threats", 500)
                }, GetCells(mitigations));
            }

            return result;
        }

        private IEnumerable<Cell> GetCells([NotNull] IEnumerable<IMitigation> mitigations)
        {
            var result = new List<Cell>();

            foreach (var mitigation in mitigations)
            {
                result.Add(new Cell(mitigation.ToString(), null, null, new []{mitigation.Id}));
                var model = mitigation.Model;
                var tems = model.GetThreatEventMitigations(mitigation)?.ToArray();
                result.Add(new Cell(tems?
                    .OrderBy(x => x.ThreatEvent?.Parent?.ToString())
                    .ThenBy(x => x.ThreatEvent?.ToString())
                    .Select(x =>
                        new Line(x.ThreatEvent.ToString(), $"[{x.ThreatEvent?.Parent?.GetIdentityTypeInitial() ?? ThreatModelManager.Unknown}] {x.ThreatEvent?.Parent?.ToString() ?? ThreatModelManager.Unknown}: ", 
                            null, new[] {x.ThreatEvent?.Id ?? Guid.Empty, x.ThreatEvent?.ThreatTypeId ?? Guid.Empty}, x.Status.ToString()))));
            }

            return result;
        }
    }
}
