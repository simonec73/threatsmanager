using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("7B677E73-6A14-4BCC-BE20-FA1B0F578FD9", "Objects with No Name Quality Analyzer", 6, ExecutionMode.Simplified)]
    public class NoName : IQualityAnalyzer
    {
        public string Label => "No Name Objects";
        public string Description =>
            "Objects with no name should be considered a mistake because they do not allow to understand their meaning.";

        public double MultiplicationFactor => 1.0;

        public override string ToString()
        {
            return Label;
        }

        public bool GetThresholds(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, out double minRed, out double maxRed, out double minYellow,
            out double maxYellow, out double minGreen, out double maxGreen)
        {
            bool result = false;
            minRed = 0;
            maxRed = 0;
            minYellow = Double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Entities?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                        (model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

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

        public double Analyze(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            var found = new List<IIdentity>();

            var entities = model.Entities?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    if (string.IsNullOrWhiteSpace(entity.Name))
                        found.Add(entity);
                }
            }

            var flows = model.DataFlows?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    if (string.IsNullOrWhiteSpace(flow.Name))
                        found.Add(flow);
                }
            }

            result = found.Count;
            instances = found;

            return result;
        }
    }
}