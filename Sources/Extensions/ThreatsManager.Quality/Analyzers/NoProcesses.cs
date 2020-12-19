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
    [Extension("617694EB-9DAF-48D5-90D6-7CBD33F670BC", "No Processes Quality Analyzer", 24, ExecutionMode.Simplified)]
    public class NoProcesses : IQualityAnalyzer
    {
        public string Label => "Diagrams Missing Processes";

        public string Description =>
            "Diagrams must have Processes, otherwise they would miss components to process information.";

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

            var count = (model.Diagrams?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

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

            var diagrams = model.Diagrams?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (diagrams?.Any() ?? false)
            {
                List<IDiagram> list = new List<IDiagram>();

                foreach (var diagram in diagrams)
                {
                    if (!(diagram.Entities?.Any(x => x.Identity is IProcess) ?? false))
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