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
    [Extension("CE92020F-11E4-4837-811B-F0FCB4C40467", "Objects Not In Any Diagram Quality Analyzer", 22, ExecutionMode.Simplified)]
    public class NotInAnyDiagram : IQualityAnalyzer
    {
        public string Label => "Not In Any Diagram";
        public string Description => "Objects should be present in at least a Diagram.";
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
            minYellow = double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Entities?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) + 
                        (model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                        (model.Groups?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                        (model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

            if (count >= 10)
            {
                minGreen = 0.0;
                maxGreen = count * 0.25;
                minYellow = Math.Floor(maxGreen * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxYellow = count * 0.75;
                minRed = Math.Floor(maxYellow * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxRed = count;

                result = true;
            }
            else if (count > 0)
            {
                minGreen = 0.0;
                maxGreen = count * 0.75;
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

        public double Analyze([NotNull] IThreatModel model, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            List<object> items = new List<object>();

            var entities = model.Entities?
                .Where(x => !isFalsePositive(this, x) && !IsInDiagram(x, model)).ToArray();
            if (entities?.Any() ?? false)
            {
                items.AddRange(entities);
            }

            var flows = model.DataFlows?
                .Where(x => !isFalsePositive(this, x) && !IsInDiagram(x, model)).ToArray();
            if (flows?.Any() ?? false)
            {
                items.AddRange(flows);
            }

            var groups = model.Groups?
                .Where(x => !isFalsePositive(this, x) && !IsInDiagram(x, model)).ToArray();
            if (groups?.Any() ?? false)
            {
                items.AddRange(groups);
            }

            if (items.Any())
            {
                result = items.Count;
                instances = items;
            }

            return result;
        }

        private bool IsInDiagram([NotNull] IEntity entity, [NotNull] IThreatModel model)
        {
            return model.Diagrams?
                       .Any(x => x.Entities?.Any(y => y.AssociatedId == entity.Id) ?? false) ?? false;
        }

        private bool IsInDiagram([NotNull] IDataFlow flow, [NotNull] IThreatModel model)
        {
            return model.Diagrams?
                       .Any(x => x.Links?.Any(y => y.AssociatedId == flow.Id) ?? false) ?? false;
        }

        private bool IsInDiagram([NotNull] IGroup group, [NotNull] IThreatModel model)
        {
            return model.Diagrams?
                       .Any(x => x.Groups?.Any(y => y.AssociatedId == group.Id) ?? false) ?? false;
        }
    }
}
