using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("BF79DB1A-15D1-443D-AE1E-6BEA8137E64A", "Complex Diagrams Quality Analyzer", 21, ExecutionMode.Simplified)]
    public class ComplexDiagrams : IQualityAnalyzer
    {
        public string Label => "Too Complex Diagrams";
        public string Description => "Diagrams with too many objects are not easily understood and shall be avoided.";
        public double MultiplicationFactor => 0.75;

        public override string ToString()
        {
            return Label;
        }

        public bool GetThresholds([NotNull] IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive,
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

            var count = model.Diagrams?.Where(x => !isFalsePositive(this, x)).Count() ?? 0;

            if (count > 0)
            {
                minGreen = 0;
                maxGreen = 0.95;
                minYellow = 0.97;
                maxYellow = 1.58;
                minRed = 1.60;
                maxRed = 2;

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

        public double Analyze([NotNull] IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            var diagrams = model.Diagrams?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (diagrams?.Any() ?? false)
            {
                List<IDiagram> list = new List<IDiagram>();

                foreach (var diagram in diagrams)
                {
                    var count = (diagram?.Entities?.Count() ?? 0) +
                                (diagram?.Groups?.Count() ?? 0) +
                                (diagram?.Links?.Count() ?? 0);
                    if (count > 80)
                    {
                        list.Add(diagram);
                        result += 2.0;
                    }
                    else if (count > 60)
                    {
                        list.Add(diagram);
                        result += 1.5;
                    }
                    else if (count > 40)
                    {
                        list.Add(diagram);
                        result += 1.0;
                    }
                }

                if (list.Any())
                    instances = list;
            }

            return result;
        }
    }
}
