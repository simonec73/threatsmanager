using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Configuration;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("31C8D017-8480-4FFD-A725-336511C082EE", "Chart Roadmap Placeholder", 37, ExecutionMode.Business)]
    public class ChartRoadmapPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ChartRoadmap";

        public IPlaceholder Create(string parameters = null)
        {
            return new ChartRoadmapPlaceholder();
        }
    }

    public class ChartRoadmapPlaceholder : IChartPlaceholder
    {
        public string Name => "Roadmap";
        public string Label => "Roadmap";
        public PlaceholderSection Section => PlaceholderSection.Chart;
        public Bitmap Image => Properties.Resources.roadmap_small;

        public ChartType StandardChartType => ChartType.Column;

        public IEnumerable<ChartItem> GetChart([NotNull] IThreatModel model)
        {
            IEnumerable<ChartItem> result = null;

            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var estimator = schemaManager.SelectedEstimator;
            if (estimator != null)
            {
                var mitigations = model.Mitigations?.ToArray();
                if ((mitigations?.Any() ?? false))
                {
                    var selectedMitigations = new List<Guid>();

                    var currentRisk = estimator.Estimate(model, null, out var currentMin, out var currentMax);
                    var current = 100f;

                    var shortTermMitigations = mitigations
                        .Where(x => x.GetStatus() == RoadmapStatus.ShortTerm)
                        .Select(x => x.Id)
                        .ToArray();
                    if (shortTermMitigations.Any())
                        selectedMitigations.AddRange(shortTermMitigations);
                    var shortTerm = estimator.Estimate(model, selectedMitigations, out var shortTermMin, out var shortTermMax) * 100f / currentRisk;

                    var midTermMitigations = mitigations
                        .Where(x => x.GetStatus() == RoadmapStatus.MidTerm)
                        .Select(x => x.Id)
                        .ToArray();
                    if (midTermMitigations.Any())
                        selectedMitigations.AddRange(midTermMitigations);
                    var midTerm = estimator.Estimate(model, selectedMitigations, out var midTermMin, out var midTermMax) * 100f / currentRisk;

                    var longTermMitigations = mitigations
                        .Where(x => x.GetStatus() == RoadmapStatus.LongTerm)
                        .Select(x => x.Id)
                        .ToArray();
                    if (longTermMitigations.Any())
                        selectedMitigations.AddRange(longTermMitigations);
                    var longTerm = estimator.Estimate(model, selectedMitigations, out var longTermMin, out var longTermMax) * 100f / currentRisk;

                    float acceptableRisk;
                    var parameters = schemaManager.Parameters?.ToArray();
                    if (parameters?.Any() ?? false)
                    {
                        var infinite = schemaManager.Infinite;
                        if (infinite < 0)
                            infinite = estimator.DefaultInfinite;

                        var normalizationReference = 
                            (new ExtensionConfigurationManager(model, (new ConfigurationPanelFactory()).GetExtensionId())).NormalizationReference;

                        var p = parameters.ToDictionary(x => x.Name, x => x.Value);
                        acceptableRisk = estimator.GetAcceptableRisk(model, p, infinite, normalizationReference) * 100f / currentRisk;
                    }
                    else
                    {
                        acceptableRisk = 0f;
                    }

                    result = new List<ChartItem>
                    {
                        new ChartItem("Current", current,
                            current <= acceptableRisk ? KnownColor.Green : KnownColor.Red),
                        new ChartItem(RoadmapStatus.ShortTerm.GetEnumLabel(), shortTerm,
                            shortTerm <= acceptableRisk ? KnownColor.Green : KnownColor.Red),
                        new ChartItem(RoadmapStatus.MidTerm.GetEnumLabel(), midTerm,
                            midTerm <= acceptableRisk ? KnownColor.Green : KnownColor.Red),
                        new ChartItem(RoadmapStatus.LongTerm.GetEnumLabel(), longTerm,
                            longTerm <= acceptableRisk ? KnownColor.Green : KnownColor.Red)
                    };
                }
            }

            return result;
        }
    }
}
