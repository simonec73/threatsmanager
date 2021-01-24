using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class CrossTrustBoundaryItemContext : ButtonItemContext
    {
        public CrossTrustBoundaryItemContext() : base(Scope.Object)
        {
        }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is bool boolValue)
            {
                result = new CrossTrustBoundaryRuleNode(node.Text, boolValue) {Scope = Scope};
            }

            return result;
        }
    }
}