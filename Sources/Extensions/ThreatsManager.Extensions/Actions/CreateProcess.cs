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
    [Extension("E10940D4-952E-4E47-B9F6-4404E58878F4", "Create a Process Context Aware Action", 16, ExecutionMode.Simplified)]
    public class CreateProcess : IIdentityContextAwareAction, IIdentityAddingRequiredAction
    {
        public Scope Scope => Scope.Diagram | Scope.ThreatModel;
        public string Label => "Create a Process...";
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
                var process = diagram.Model?.AddEntity<IProcess>();
                IdentityAddingRequired?.Invoke(diagram, process, PointF.Empty, SizeF.Empty);
                result = true;
            } 
            else if (identity is IThreatModel threatModel)
            {
                threatModel.AddEntity<IProcess>();
                result = true;
            }

            return result;
        }
    }
}