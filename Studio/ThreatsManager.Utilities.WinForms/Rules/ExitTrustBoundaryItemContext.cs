using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class ExitTrustBoundaryItemContext : ButtonItemContext
    {
        public ExitTrustBoundaryItemContext() : base(Scope.Object)
        {
        }

        public ExitTrustBoundaryItemContext(Scope scope) : base(scope)
        {
        }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is bool boolValue)
            {
                result = new ExitTrustBoundaryRuleNode(node.Text, boolValue) { Scope = Scope };
            }

            return result;
        }
    }
}