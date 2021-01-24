using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    public partial class KnownMitigationListPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.Mitigation | Scope.ThreatTypeMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}