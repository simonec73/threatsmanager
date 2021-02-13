using DevComponents.AdvTree;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class BooleanItemContext : ButtonItemContext
    {
        public BooleanItemContext(Scope scope) : base(scope)
        {

        }

        public override SelectionRuleNode CreateNode(Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is bool boolValue)
            {
                result = new BooleanRuleNode(node.Text, null, null, boolValue) { Scope = Scope };
            }

            return result;
        }
    }
}