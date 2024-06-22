using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.ThreatModel
{
    public partial class ThreatModelPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.ThreatModel;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}