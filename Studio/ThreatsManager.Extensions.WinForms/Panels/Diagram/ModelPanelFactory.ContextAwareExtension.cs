using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.Diagram | Scope.Entity | Scope.Group | Scope.DataFlow | Scope.ThreatEvent | 
                                        Scope.Link | Scope.EntityShape | Scope.GroupShape;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}