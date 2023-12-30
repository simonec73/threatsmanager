using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("8B35C893-C304-418A-BD30-7C9CE88317AB", "Add Missing Flows Context Aware Action", 28, ExecutionMode.Simplified)]
    public class AddMissingFlows : IShapeContextAwareAction
    {
        public Scope Scope => Scope.Entity;
        public string Label => "Add Missing Flows";
        public string Group => "ItemActions";
        public Bitmap Icon => Properties.Resources.graph_star_big;
        public Bitmap SmallIcon => Properties.Resources.graph_star;
        public Shortcut Shortcut => Shortcut.None;

        public bool Execute([NotNull] object item)
        {
            return false;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute([NotNull] IShape shape)
        {
            var result = false;

            if (shape is IEntityShape entityShape && shape is IThreatModelChild child)
            {
                var diagram = child.Model?.Diagrams?
                    .FirstOrDefault(x => x.Entities.Contains(entityShape));
                if (diagram != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Add missing Flows"))
                    {
                        var outFlows = child.Model?.DataFlows?
                            .Where(x => x.SourceId == entityShape.AssociatedId &&
                                        !(diagram.Links?.Any(y => x.Id == y.AssociatedId) ?? false));
                        result = AddFlows(diagram, outFlows);

                        var inFlows = child.Model?.DataFlows?
                            .Where(x => x.TargetId == entityShape.AssociatedId &&
                                        !(diagram.Links?.Any(y => x.Id == y.AssociatedId) ?? false));
                        result |= AddFlows(diagram, inFlows);

                        if (result)
                        {
                            scope?.Complete();
                        }
                    }
                }
            }

            return result;
        }

        private bool AddFlows([NotNull] IDiagram diagram, IEnumerable<IDataFlow> flows)
        {
            bool result = false;

            if (flows?.Any() ?? false)
            {
                result = true;
                foreach (var flow in flows)
                {
                    diagram.AddLink(flow);
                }
            }

            return result;
        }
    }
}
