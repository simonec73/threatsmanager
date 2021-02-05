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
    [Extension("D6B01BC8-E72A-400C-88C9-6EB83B41D936", "Create a Data Store Context Aware Action", 17, ExecutionMode.Simplified)]
    public class CreateDataStore : IIdentityContextAwareAction, IIdentityAddingRequiredAction
    {
        public Scope Scope => Scope.Diagram | Scope.ThreatModel;
        public string Label => "Create a Data Store...";
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
                var dataStore = diagram.Model?.AddEntity<IDataStore>();
                IdentityAddingRequired?.Invoke(diagram, dataStore, PointF.Empty, SizeF.Empty);
                result = true;
            } 
            else if (identity is IThreatModel threatModel)
            {
                threatModel.AddEntity<IDataStore>();
                result = true;
            }

            return result;
        }
    }
}