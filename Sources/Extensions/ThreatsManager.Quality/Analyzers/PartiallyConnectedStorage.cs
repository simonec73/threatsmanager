using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("DAEB7324-13B8-4827-8655-F295E96D99E4", "Partially Connected Data Store Quality Analyzer", 12, ExecutionMode.Simplified)]
    public class PartiallyConnectedStorage : IQualityAnalyzer
    {
        public string Label => "Partially Connected Data Stores";
        public string Description => "Data Stores should both be used to save and read data.\nPlease check the Flow Type for the incoming Flows.";
        public double MultiplicationFactor => 1.0;
        
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

            var count = model.Entities?.OfType<IDataStore>()
                .Where(x => !isFalsePositive(this, x)).Count() ?? 0;

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

            var entities = model.Entities?.OfType<IDataStore>()
                .Where(x => !isFalsePositive(this, x)).ToArray();
            if (entities?.Any() ?? false)
            {
                var flows = model.DataFlows?.ToArray();

                if (flows?.Any() ?? false)
                {
                    List<object> found = new List<object>();

                    foreach (var entity in entities)
                    {
                        var read = flows.Any(x => x.TargetId == entity.Id &&
                                                  (x.FlowType == FlowType.Read ||
                                                   x.FlowType == FlowType.ReadWriteCommand));
                        var write = flows.Any(x => x.TargetId == entity.Id &&
                                                  (x.FlowType == FlowType.WriteCommand ||
                                                   x.FlowType == FlowType.ReadWriteCommand));

                        if (read ^ write)
                            found.Add(entity);
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
