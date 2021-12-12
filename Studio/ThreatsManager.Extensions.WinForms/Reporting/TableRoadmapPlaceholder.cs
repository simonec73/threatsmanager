using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("A5E06C0E-E8AC-4853-8C29-0D81B88B744F", "Roadmap Table", 53, ExecutionMode.Business)]
    public class TableRoadmapPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableRoadmap";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableRoadmapPlaceholder();
        }
    }
    public class TableRoadmapPlaceholder : ITablePlaceholder
    {
        public string Name => "Roadmap";
        public string Label => "Roadmap";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Properties.Resources.roadmap_small;

        public TableItem GetTable([NotNull] IThreatModel model)
        {
            TableItem result = null;

            var mitigations = model.GetUniqueMitigations()?.ToArray();
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

            Append(mitigations, RoadmapStatus.ShortTerm, result);
            Append(mitigations, RoadmapStatus.MidTerm, result);
            Append(mitigations, RoadmapStatus.LongTerm, result);

            return result;
        }

        private void Append(IEnumerable<IMitigation> mitigations, RoadmapStatus phase, List<Cell> destination)
        {
            if (mitigations?.Any() ?? false)
            {
                var selected = GetMitigations(mitigations, phase);
                if (selected?.Any() ?? false)
                {
                    var cells = GetCells(selected, phase)?.ToArray();
                    if (cells?.Any() ?? false)
                        destination.AddRange(cells);
                }
            }
        }

        private IEnumerable<IMitigation> GetMitigations([NotNull] IEnumerable<IMitigation> mitigations, RoadmapStatus status)
        {
            return mitigations
                .Where(x => x.GetStatus() == status)
                .OrderBy(x => x.Name);
        }

        private IEnumerable<Cell> GetCells([NotNull] IEnumerable<IMitigation> mitigations, RoadmapStatus status)
        {
            IEnumerable<Cell> result = null;

            if (mitigations.Any())
            {
                var list = new List<Cell>();

                list.Add(new Cell(status.GetEnumLabel(), null, null, true));
                list.Add(new Cell(string.Empty));

                foreach (var mitigation in mitigations)
                {
                    list.Add(new Cell(mitigation.Name, null, null, new [] {mitigation.Id}));
                    list.Add(new Cell(GetLines(mitigation)));
                }

                result = list;
            }

            return result;
        }

        private IEnumerable<Line> GetLines([NotNull] IMitigation mitigation)
        {
            IEnumerable<Line> result = null;

            var model = mitigation.Model;
            var threatEventMitigations = model.GetThreatEventMitigations(mitigation)?
                .Where(x => x.Status != MitigationStatus.Existing && x.Status != MitigationStatus.Implemented && x.Status != MitigationStatus.Undefined)
                .OrderBy(x => x.Status.ToString())
                .ThenBy(x => x.ThreatEvent.Parent.Name)
                .ThenBy(x => x.ThreatEvent.Name)
                .ToArray();

            if (threatEventMitigations?.Any() ?? false)
            {
                var list = new List<Line>();

                foreach (var tem in threatEventMitigations)
                {
                    list.Add(new Line(tem.ThreatEvent.Name, 
                        $"[{model.GetIdentityTypeInitial(tem.ThreatEvent.Parent)}] {tem.ThreatEvent.Parent.Name}: ",
                        null, new []{tem.ThreatEvent.Id, tem.ThreatEvent.ThreatTypeId}, tem.Status.ToString()));    
                }

                result = list;
            }

            return result;
        }
    }
}
