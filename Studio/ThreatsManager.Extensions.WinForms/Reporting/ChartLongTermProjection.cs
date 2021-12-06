using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("5AA3335B-DC54-461D-B36C-456359ABC6BE", "Chart Long Term Projection Placeholder", 58, ExecutionMode.Business)]
    public class ChartLongTermProjectionPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ChartLongTermProjection";

        public IPlaceholder Create(string parameters = null)
        {
            return new ChartLongTermProjectionPlaceholder();
        }
    }

    public class ChartLongTermProjectionPlaceholder : IChartPlaceholder
    {
        public string Name => "LongTermProjection";
        public string Label => "Long Term Projection";
        public PlaceholderSection Section => PlaceholderSection.Chart;
        public Bitmap Image => null;

        public ChartType StandardChartType => ChartType.Pie;

        public IEnumerable<ChartItem> GetChart([NotNull] IThreatModel model)
        {
            IEnumerable<ChartItem> result = null;

            var mitigations = model.Mitigations?.ToArray();

            if (mitigations?.Any() ?? false)
            {
                var selectedMitigations = new List<Guid>();
                UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.ShortTerm);
                UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.MidTerm);
                UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.LongTerm);

                var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
                var projected = schemaManager.SelectedEstimator?.GetProjectedThreatTypesResidualRisk(model, selectedMitigations);

                if (projected?.Any() ?? false)
                {
                    result = GetChart(model, projected);
                }
            }

            return result;
        }

        private static void UpdateSelectedMitigations(List<Guid> selectedMitigations,
            [NotNull] IEnumerable<IMitigation> mitigations, RoadmapStatus status)
        {
            var toBeAdded = mitigations
                    .Where(x => x.GetStatus() == status)
                    .Select(x => x.Id)
                    .ToArray();
            if (toBeAdded.Any())
                selectedMitigations.AddRange(toBeAdded);
        }

        public IEnumerable<ChartItem> GetChart([NotNull] IThreatModel model, [NotNull] IDictionary<Guid, int> projectedSeverities)
        {
            IEnumerable<ChartItem> result = null;

            var severities = model.Severities?.ToArray();
            if ((severities?.Any() ?? false) && projectedSeverities.Any())
            {
                var list = new List<ChartItem>();

                var total = model.AssignedThreatTypes;
                foreach (var severity in severities)
                {
                    var count = projectedSeverities.Count(x => x.Value == severity.Id);

                    if (count > 0)
                    {
                        list.Add(new ChartItem(severity.Name, count, severity.BackColor));
                    }
                }

                if (list.Any())
                    result = list;
            }

            return result;
        }
    }
}
