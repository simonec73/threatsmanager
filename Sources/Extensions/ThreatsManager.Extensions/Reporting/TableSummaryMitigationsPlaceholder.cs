using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

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
                .OrderBy(x => x.Name)
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
                result.Add(new Cell(mitigation.Name, null, null, new []{mitigation.Id}));
                var model = mitigation.Model;
                var tems = model.GetThreatEventMitigations(mitigation)?.ToArray();
                result.Add(new Cell(tems?
                    .OrderBy(x => x.ThreatEvent.Parent.Name)
                    .ThenBy(x => x.ThreatEvent.Name)
                    .Select(x =>
                        new Line(x.ThreatEvent.Name, $"[{model.GetIdentityTypeInitial(x.ThreatEvent.Parent)}] {x.ThreatEvent.Parent.Name}: ", 
                            null, new[] {x.ThreatEvent.Id, x.ThreatEvent.ThreatTypeId}, x.Status.ToString()))));
            }

            return result;
        }
    }
}
