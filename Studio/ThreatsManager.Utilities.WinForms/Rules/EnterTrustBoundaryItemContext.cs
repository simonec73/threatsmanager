using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class EnterTrustBoundaryItemContext : ButtonItemContext
    {
        public EnterTrustBoundaryItemContext() : base(Scope.Object)
        {
        }

        public EnterTrustBoundaryItemContext(Scope scope) : base(scope)
        {
        }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is bool boolValue)
            {
                result = new EnterTrustBoundaryRuleNode(node.Text, boolValue) { Scope = Scope };
            }

            return result;
        }
    }
}