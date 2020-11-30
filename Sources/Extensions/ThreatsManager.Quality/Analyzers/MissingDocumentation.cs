using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("5477244B-BF14-4C2C-99AE-24C19F674058", "Missing Documentation Quality Analyzer", 60, ExecutionMode.Simplified)]
    public class MissingDocumentation : IQualityAnalyzer
    {
        public string Label => "Missing Documentation";
        public string Description => "On average, most objects in the Threat Model should be documented.\nThe analyzer does not take in account Threat Type, Threat Events or Mitigations.";
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

            var count = (isFalsePositive(this, model) ? 0 : 1) + 
                        (model.Entities?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) + 
                        (model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                        (model.Groups?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) + 
                        (model.Diagrams?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

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

            if (string.IsNullOrWhiteSpace(model.Description) && !isFalsePositive(this, model))
                items.Add(model);

            AnalyzeContainer(model.Entities?.Where(x => !isFalsePositive(this, x)), items);
            AnalyzeContainer(model.DataFlows?.Where(x => !isFalsePositive(this, x)), items);
            AnalyzeContainer(model.Groups?.Where(x => !isFalsePositive(this, x)), items);
            AnalyzeContainer(model.Diagrams?.Where(x => !isFalsePositive(this, x)), items);

            if (items.Any())
            {
                result = items.Count;
                instances = items;
            }

            return result;
        }

        private void AnalyzeContainer(IEnumerable<IIdentity> identities, [NotNull] IList<object> list)
        {
            var items = identities?.ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrWhiteSpace(item.Description))
                        list.Add(item);
                }
            }
        }
    }
}
