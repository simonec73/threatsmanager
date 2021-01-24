using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    public partial class PropertySchemaListPanelFactory
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.PropertySchema;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions;
        }
    }
}