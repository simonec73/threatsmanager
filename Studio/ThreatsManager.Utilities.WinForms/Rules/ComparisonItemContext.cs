using DevComponents.AdvTree;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class ComparisonItemContext : ButtonItemContext
    {
        public ComparisonItemContext(Scope scope) : base(scope)
        {

        }

        public override SelectionRuleNode CreateNode(Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 2 && 
                parameters[0] is ComparisonOperator op &&
                parameters[1] is string value)
            {
                result = new ComparisonRuleNode(node.Text, null, null, op, value) { Scope = Scope };
            }

            return result;
        }
    }
}