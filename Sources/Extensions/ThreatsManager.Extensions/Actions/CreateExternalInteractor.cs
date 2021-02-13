using System;
using System.Drawing;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("2F7A97D4-D5EE-4DF8-A467-EE7F22B451F1", "Create an External Interactor Context Aware Action", 15, ExecutionMode.Simplified)]
    public class CreateExternalInteractor : IIdentityContextAwareAction, IIdentityAddingRequiredAction
    {
        public Scope Scope => Scope.Diagram | Scope.ThreatModel;
        public string Label => "Create an External Interactor...";
        public string Group => "EntityCreation";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<IDiagram, IIdentity, PointF, SizeF> IdentityAddingRequired;

        public bool Execute(object item)
        {
            bool result = false;

            if (item is IIdentity identity)
                result = Execute(identity);

            return result;
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (identity is IDiagram diagram)
            {
                var interactor = diagram.Model?.AddEntity<IExternalInteractor>();
                IdentityAddingRequired?.Invoke(diagram, interactor, PointF.Empty, SizeF.Empty);
                result = true;
            } 
            else if (identity is IThreatModel threatModel)
            {
                threatModel.AddEntity<IExternalInteractor>();
                result = true;
            }

            return result;
        }
    }
}