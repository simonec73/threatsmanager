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
    [Extension("C38FABD0-16CA-46E5-A6FC-50B40A02E8B5", "Data Store as a source for Flows Quality Analyzer", 14, ExecutionMode.Simplified)]
    public class StorageFlowsSource : IQualityAnalyzer
    {
        public string Label => "Data Store as a Source for Flows";
        public string Description => "Data Stores cannot be used as a Source for Flows.\nThis typically points to a missing Process.";
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
                        if (flows.Any(x => x.SourceId == entity.Id))
                            found.Add(entity);
                    }

                    result = found.Count;
                    if (result > 0)
                        instances = found;
                }
            }

            return result;
        }
    }
}
