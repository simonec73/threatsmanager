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
    [Extension("DF2DB940-E3C4-44B2-A7B8-C14CAB1FCE80", "Flow Between Data Stores Quality Analyzer", 15, ExecutionMode.Simplified)]
    public class FlowBetweenDataStores : IQualityAnalyzer
    {
        public string Label => "Flow Between Data Stores";
        public string Description => "Flows between Data Stores must be avoided: a Process between them is in order.";
        public double MultiplicationFactor => 1.0;

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

        public double Analyze([NotNull] IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            var flows = model.DataFlows?
                .Where(x => !isFalsePositive(this, x) && 
                            x.Source is IDataStore && x.Target is IDataStore)
                .ToArray();

            if (flows?.Any() ?? false)
            {
                result = flows.Length;
                instances = flows;
            }

            return result;
        }
    }
}
