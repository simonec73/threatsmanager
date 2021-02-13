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
    [Extension("8C5691D1-EA92-4311-8497-39EAEA94D4C4", "Single Mitigation Quality Analyzer", 42, ExecutionMode.Simplified)]
    public class SingleMitigation : IQualityAnalyzer
    {
        public string Label => "Single Mitigation";
        public string Description => "On average, Threat Events should have more than only one associated Mitigation.\nThe analyzer takes in account only the Threat Events with at least a Mitigation.";
        public double MultiplicationFactor => 0.5;
        
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

            var count = (model.Entities?.Select(x => x.ThreatEvents?
                            .Where(y => !isFalsePositive(this, y) && (y.Mitigations?.Any() ?? false)).Count() ?? 0).Sum() ?? 0) +
                        (model.DataFlows?.Select(x => x.ThreatEvents?
                            .Where(y => !isFalsePositive(this, y) && (y.Mitigations?.Any() ?? false)).Count() ?? 0).Sum() ?? 0) +
                        (model.ThreatEvents?.Where(y => !isFalsePositive(this, y) && (y.Mitigations?.Any() ?? false)).Count() ?? 0);

            if (count >= 10)
            {
                minGreen = 0.0;
                maxGreen = count * 0.5;
                minYellow = Math.Floor(maxGreen * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxYellow = count * 0.75;
                minRed = Math.Floor(maxYellow * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxRed = count;

                result = true;
            }
            else if (count > 0)
            {
                minGreen = 0.0;
                maxGreen = count * 0.75;
                minRed = maxGreen + count * 0.02;
                maxRed = count;

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
                .Where(x => !isFalsePositive(this, x) && (x.Mitigations?.Count() ?? 0) == 1).ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var te in threatEvents)
                    list.Add(te);
            }
        }
    }
}
