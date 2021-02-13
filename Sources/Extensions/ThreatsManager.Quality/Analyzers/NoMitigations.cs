using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("9696B01B-F8EA-4034-8069-AC44DCAC8CB1", "Missing Mitigations Quality Analyzer", 40, ExecutionMode.Simplified)]
    public class NoMitigations : IQualityAnalyzer
    {
        public string Label => "Missing Mitigations";
        public string Description => "All Threat Events should have at least an associated Mitigation.";
        public double MultiplicationFactor => 1.0;

        public override string ToString()
        {
            return Label;
        }

        public bool GetThresholds([NotNull] IThreatModel model, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out double minRed, out double maxRed, out double minYellow, out double maxYellow,
            out double minGreen, out double maxGreen)
        {
            bool result = false;
            minRed = 0;
            maxRed = 0;
            minYellow = double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Entities?.Select(x => x.ThreatEvents?.Where(y => !isFalsePositive(this, y)).Count() ?? 0).Sum() ?? 0) +
                        (model.DataFlows?.Select(x => x.ThreatEvents?.Where(y => !isFalsePositive(this, y)).Count() ?? 0).Sum() ?? 0) +
                        (model.ThreatEvents?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

            if (count > 0)
            {
                minGreen = 0;
                maxGreen = 0.49;
                minRed = 0.51;
                maxRed = 1;

                result = true;
            }
            else
            {
                minGreen = 0;
                maxGreen = 1;
                minRed = double.NaN;
                maxRed = double.NaN;
            }

            return result;
        }

        public double Analyze([NotNull] IThreatModel model, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            List<object> items = new List<object>();

            AnalyzeContainer(model, isFalsePositive, items);

            var entities = model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    AnalyzeContainer(entity, isFalsePositive, items);
                }
            }

            var flows = model.DataFlows?.ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    AnalyzeContainer(flow, isFalsePositive, items);
                }
            }

            if (items.Any())
            {
                result = items.Count;
                instances = items;
            }

            return result;
        }

        private void AnalyzeContainer([NotNull] IThreatEventsContainer container, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            [NotNull] IList<object> list)
        {
            var threatEvents = container.ThreatEvents?
                .Where(x => !isFalsePositive(this, x) && !(x.Mitigations?.Any() ?? false)).ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var te in threatEvents)
                    list.Add(te);
            }
        }
    }
}
