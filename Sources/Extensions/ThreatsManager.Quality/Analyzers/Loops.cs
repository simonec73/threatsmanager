using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("8ED1AAA3-2A7A-45D8-A282-374317C5DE5F", "Loops Quality Analyzer", 20, ExecutionMode.Simplified)]
    public class Loops : IQualityAnalyzer
    {
        public string Label => "Loops";
        public string Description => "Loops between two Entities should be avoided, but in some situation may be necessary.\nConsider the possibility to use Flow Type to assign the required semantics.";
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
            minYellow = Double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0;

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

            var entities = model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                var flows = model.DataFlows?.Where(x => !isFalsePositive(this, x)).ToArray();

                if (flows?.Any() ?? false)
                {
                    List<object> found = new List<object>();

                    foreach (var entity in entities)
                    {
                        var loopFlow = flows.FirstOrDefault(x => x.SourceId == entity.Id &&
                                                      flows.Any(y =>
                                                          y.SourceId == x.TargetId && y.TargetId == x.SourceId));
                        if (loopFlow != null)
                        {
                            found.Add(loopFlow);
                        }
                    }

                    result = found.Count;
                    if (found.Any())
                        instances = found;
                }
            }

            return result;
        }
    }
}
