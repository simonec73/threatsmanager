using System;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    /// <summary>
    /// Item Context used for Unary and Nary rule nodes.
    /// </summary>
    internal class NaryItemContext : ButtonItemContext
    {
        public NaryItemContext([NotNull] Type type) : base(Scope.Object)
        {
            BooleanType = type;
        }

        public Type BooleanType { get; set; }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            var result = (SelectionRuleNode)Activator.CreateInstance(BooleanType);
            result.Name = node.Text;
            result.Scope = Scope;

            return result;
        }
    }
}