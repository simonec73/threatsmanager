using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("A099EB5E-9768-45D5-89D3-B370A5D48109", "Single Threat Events Quality Analyzer", 32, ExecutionMode.Simplified)]
    public class SingleThreatEvent : IQualityAnalyzer
    {
        public string Label => "Single Threat Event";
        public string Description => "On average, Entities and Flows should have more than only one associated Threat Event.\nThe analyzer takes in account only the objects with at least a Threat Event.";
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

            var count = (isFalsePositive(this, model) ? 0 : ((model.ThreatEvents?.Any() ?? false) ? 1 : 0)) +
                        (model.Entities?
                            .Where(y => !isFalsePositive(this, y) && (y.ThreatEvents?.Any() ?? false)).Count() ?? 0) +
                        (model.DataFlows?
                            .Where(y => !isFalsePositive(this, y) && (y.ThreatEvents?.Any() ?? false)).Count() ?? 0);

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

            if (!isFalsePositive(this, model) && ((model.ThreatEvents?.Count() ?? 0) == 1))
                items.Add(model);

            var entities = model.Entities?
                .Where(x => !isFalsePositive(this, x) && ((x.ThreatEvents?.Count() ?? 0) == 1)).ToArray();
            if (entities?.Any() ?? false)
            {
                items.AddRange(entities);
            }

            var flows = model.DataFlows?
                .Where(x => !isFalsePositive(this, x) && ((x.ThreatEvents?.Count() ?? 0) == 1)).ToArray();
            if (flows?.Any() ?? false)
            {
                items.AddRange(flows);
            }

            if (items.Any())
            {
                result = items.Count;
                instances = items;
            }

            return result;
        }
    }
}
