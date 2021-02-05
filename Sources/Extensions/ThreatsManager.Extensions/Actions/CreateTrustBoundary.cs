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
    [Extension("E6BA9412-00C0-4547-9D62-A55BF881F3E7", "Create a Trust Boundary Context Aware Action", 18, ExecutionMode.Simplified)]
    public class CreateTrustBoundary : IIdentityContextAwareAction, IIdentityAddingRequiredAction
    {
        public Scope Scope => Scope.Diagram | Scope.ThreatModel;
        public string Label => "Create a Trust Boundary...";
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
                var trustBoundary = diagram.Model?.AddGroup<ITrustBoundary>();
                IdentityAddingRequired?.Invoke(diagram, trustBoundary, PointF.Empty, new SizeF(600, 300));
                result = true;
            } 
            else if (identity is IThreatModel threatModel)
            {
                threatModel.AddGroup<ITrustBoundary>();
                result = true;
            }

            return result;
        }
    }
}