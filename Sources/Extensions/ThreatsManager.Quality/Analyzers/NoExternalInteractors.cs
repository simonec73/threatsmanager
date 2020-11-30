using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("37F972FB-B069-4BB3-B95D-A7E6C9F4D6E0", "No External Interactors Quality Analyzer", 26, ExecutionMode.Simplified)]
    public class NoExternalInteractors : IQualityAnalyzer
    {
        public string Label => "Diagrams Missing External Interactors";

        public string Description =>
            "Diagrams should have External Interactors, but occasionally they may be not necessary.";

        public double MultiplicationFactor => 0.75;

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

            var count = (model.Diagrams?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

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

            var diagrams = model.Diagrams?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (diagrams?.Any() ?? false)
            {
                List<IDiagram> list = new List<IDiagram>();

                foreach (var diagram in diagrams)
                {
                    if (!(diagram.Entities?.Any(x => x.Identity is IExternalInteractor) ?? false))
                    {
                        list.Add(diagram);
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