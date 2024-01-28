using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    public partial class ThreatTypeListPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.Diagram | Scope.DataFlow | Scope.Entity |
            Scope.TrustBoundary | Scope.FlowTemplate | Scope.EntityTemplate |
            Scope.TrustBoundaryTemplate | Scope.Mitigation | Scope.Severity |
            Scope.Strength | Scope.ThreatActor | Scope.ThreatEvent |
            Scope.ThreatType | Scope.ThreatTypeMitigation | Scope.ThreatTypeWeakness |
            Scope.Vulnerability | Scope.Weakness | Scope.WeaknessMitigation;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}