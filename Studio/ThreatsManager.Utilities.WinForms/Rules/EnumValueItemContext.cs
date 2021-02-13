using System.Collections.Generic;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class EnumValueItemContext : ListItemContext
    {
        public EnumValueItemContext([NotNull] IEnumerable<string> values, Scope scope) : base(values, scope)
        {
        }

        public override SelectionRuleNode CreateNode(Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 2 && 
                parameters[0] is IEnumerable<string> values &&
                parameters[1] is string value)
            {
                result = new EnumValueRuleNode(node.Text, null, null, values, value) { Scope = Scope };
            }

            return result;
        }
    }
}