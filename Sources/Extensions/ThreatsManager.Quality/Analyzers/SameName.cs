using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("EB78DFE2-B125-49EF-AFF6-D15BC553618F", "Objects with the Same Name Quality Analyzer", 8, ExecutionMode.Simplified)]
    public class SameName : IQualityAnalyzer
    {
        public string Label => "Same Name Objects";

        public string Description =>
            "Objects with the same name make more difficult to understand the model and thus shall be avoided.";

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
            minYellow = Double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Entities?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                (model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                (model.Groups?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

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

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
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
                    if (entities.Any(x => (entity.Id != x.Id) &&
                                           entity.GetEntityType() == x.GetEntityType() &&
                                           string.CompareOrdinal(entity.Name, x.Name) == 0) && 
                        !found.OfType<IEntity>().Any(x => string.CompareOrdinal(x.Name, entity.Name) == 0))
                        found.Add(entity);
                }
            }

            var flows = model.DataFlows?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    if (flows.Any(x => (flow.Id != x.Id) &&
                                           string.CompareOrdinal(flow.Name, x.Name) == 0) && 
                        !found.OfType<IDataFlow>().Any(x => string.CompareOrdinal(x.Name, flow.Name) == 0))
                        found.Add(flow);
                }
            }
            
            var groups = model.Groups?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    if (groups.Any(x => (group.Id != x.Id) &&
                                         string.CompareOrdinal(group.Name, x.Name) == 0) && 
                        !found.OfType<IGroup>().Any(x => string.CompareOrdinal(x.Name, group.Name) == 0))
                        found.Add(group);
                }
            }

            result = found.Count;
            instances = found;

            return result;
        }
    }
}
