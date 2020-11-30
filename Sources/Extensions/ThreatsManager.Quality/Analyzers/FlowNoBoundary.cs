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
    [Extension("B6D77089-F2E3-4175-B870-FA3333593A08", "Flow Missing Trust Boundary Quality Analyzer", 17, ExecutionMode.Simplified)]
    public class FlowNoBoundary : IQualityAnalyzer
    {
        public string Label => "Flow Missing Trust Boundary";
        public string Description => "Flows between an External Interactor and a Process or Data Store must cross a Trust Boundary.";
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

            var list = new List<IDataFlow>();

            var flowsSources = model.DataFlows?
                .Where(x => !isFalsePositive(this, x) && 
                            x.Source is IExternalInteractor &&
                            (x.Target is IProcess || x.Target is IDataStore) &&
                            x.Source.ParentId == x.Target.ParentId)
                .ToArray();
            if (flowsSources?.Any() ?? false)
            {
                list.AddRange(flowsSources);
            }

            var flowsTargets = model.DataFlows?
                .Where(x => !isFalsePositive(this, x) && 
                            x.Target is IExternalInteractor &&
                            (x.Source is IProcess || x.Source is IDataStore) &&
                            x.Source.ParentId == x.Target.ParentId)
                .ToArray();
            if (flowsTargets?.Any() ?? false)
            {
                list.AddRange(flowsTargets);
            }

            if (list.Any())
            {
                instances = list;
                result = list.Count;
            }

            return result;
        }
    }
}
