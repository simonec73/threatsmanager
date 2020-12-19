using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("AA8E43D1-695D-49DB-930E-DB913A798126", "Redundant Trust Boundary Quality Analyzer", 18, ExecutionMode.Simplified)]
    public class RedundantTrustBoundary : IQualityAnalyzer
    {
        public string Label => "Redundant Trust Boundaries";
        public string Description => "Nested Trust Boundaries may be redundant and should be avoided.";
        public double MultiplicationFactor => 0.5;

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
            minYellow = double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Groups?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

            if (count >= 10)
            {
                minGreen = 0.0;
                maxGreen = count * 0.2;
                minYellow = Math.Floor(maxGreen * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxYellow = count * 0.5;
                minRed = Math.Floor(maxYellow * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxRed = count;

                result = true;
            }
            else if (count > 0)
            {
                minGreen = 0.0;
                maxGreen = count * 0.5;
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

        public double Analyze(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            var trustBoundaries = model.Groups?.OfType<ITrustBoundary>()
                .Where(x => !isFalsePositive(this, x)).ToArray();
            if (trustBoundaries?.Any() ?? false)
            {
                var list = new List<ITrustBoundary>();

                foreach (var trustBoundary in trustBoundaries)
                {
                    var parent = trustBoundary.Parent;
                    if (parent != null &&
                        !(parent.Entities?.Any() ?? false) &&
                        (parent.Groups?.Count() ?? 0) == 1)
                    {
                        list.Add(trustBoundary);
                    }
                }

                if (list.Any())
                {
                    instances = list;
                    result = list.Count;
                }
            }

            return result;
        }
    }
}