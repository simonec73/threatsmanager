using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Export(typeof(IContextAwareAction))]
    [ExportMetadata("Id", "140EA130-FA6C-4745-B609-28B5A47AF834")]
    [ExportMetadata("Label", "Remove from Diagram Context Aware Action")]
    [ExportMetadata("Priority", 50)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
    public class RemoveFromDiagram : IShapesContextAwareAction, 
        IShapeContextAwareAction, ILinkContextAwareAction, 
        IEntityGroupRemovingRequiredAction, IDataFlowRemovingRequiredAction
    {
        public Scope Scope => Scope.Entity | Scope.Group | Scope.DataFlow;
        public string Label => "Remove from Diagram";
        public string Group => "Remove";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<IShape> EntityGroupRemovingRequired;
        public event Action<ILink> DataFlowRemovingRequired;

        public bool Execute([NotNull] object item)
        {
            return false;
        }
        
        public bool Execute([NotNull] ILink link)
        {
            return Execute(null, new[] {link});
        }

        public bool Execute([NotNull] IShape shape)
        {
            return Execute(new[] {shape}, null);
        }

        public bool Execute(IEnumerable<IShape> shapes, IEnumerable<ILink> links)
        {
            var shapesArray = shapes?.ToArray();
            var linksArray = links?.ToArray();

            if (linksArray?.Any() ?? false)
            {
                foreach (var link in linksArray)
                {
                    DataFlowRemovingRequired?.Invoke(link);
                }
            }

            if (shapesArray?.Any() ?? false)
            {
                foreach (var shape in shapesArray)
                {
                    EntityGroupRemovingRequired?.Invoke(shape);
                }
            }

            return true;
        }
    }
}