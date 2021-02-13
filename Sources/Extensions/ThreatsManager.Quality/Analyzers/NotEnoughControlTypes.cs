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
    [Extension("81BF8B32-C6C7-4E4E-A605-772907067A8A", "Not Enough Control Types Quality Analyzer", 44, ExecutionMode.Simplified)]
    public class NotEnoughControlTypes : IQualityAnalyzer
    {
        public string Label => "Not Enough Control Types";
        public string Description => "On average, Threat Events should have associated Mitigations belonging to multiple Control Types.\nThe analyzer takes in account only the Threat Events with more than a Mitigation.";
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
                            .Where(y => !isFalsePositive(this, y) && (y.Mitigations?.Count() ?? 0) > 1).Count() ?? 0).Sum() ?? 0) +
                        (model.DataFlows?.Select(x => x.ThreatEvents?
                            .Where(y => !isFalsePositive(this, y) && (y.Mitigations?.Count() ?? 0) > 1).Count() ?? 0).Sum() ?? 0) +
                        (model.ThreatEvents?.Where(y => !isFalsePositive(this, y) && (y.Mitigations?.Count() ?? 0) > 1).Count() ?? 0);

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
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, [NotNull] IList<object> list)
        {
            var threatEvents = container.ThreatEvents?
                .Where(x => !isFalsePositive(this, x)).ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var te in threatEvents)
                {
                    var mitigations = te.Mitigations?.ToArray();
                    if ((mitigations?.Length ?? 0) >= 2)
                    {
                        SecurityControlType? controlType = null;
                        bool found = false;
                        foreach (var m in mitigations)
                        {
                            if (!controlType.HasValue)
                            {
                                controlType = m.Mitigation?.ControlType;
                            }
                            else
                            {
                                if (controlType != m.Mitigation?.ControlType)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if (!found)
                            list.Add(te);
                    }
                }
            }
        }
    }
}
