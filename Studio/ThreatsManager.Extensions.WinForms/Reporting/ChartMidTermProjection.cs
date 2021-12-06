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
    [Extension("300C2EFE-D7B6-4ECE-9B2C-5DB4A493E8AD", "Chart Mid Term Projection Placeholder", 57, ExecutionMode.Business)]
    public class ChartMidTermProjectionPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ChartMidTermProjection";

        public IPlaceholder Create(string parameters = null)
        {
            return new ChartMidTermProjectionPlaceholder();
        }
    }

    public class ChartMidTermProjectionPlaceholder : IChartPlaceholder
    {
        public string Name => "MidTermProjection";
        public string Label => "Mid Term Projection";
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
