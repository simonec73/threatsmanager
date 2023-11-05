using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.All;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}