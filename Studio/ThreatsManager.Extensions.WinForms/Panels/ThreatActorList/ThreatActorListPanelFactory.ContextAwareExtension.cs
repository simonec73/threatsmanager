using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.ThreatActorList
{
    public partial class ThreatActorListPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.Undefined;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}