using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    public partial class DashboardFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.Entity | Scope.DataFlow | Scope.ThreatModel | 
                                        Scope.ThreatEvent | Scope.TrustBoundary | Scope.Diagram;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}