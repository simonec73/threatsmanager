using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class TruismItemContext : ButtonItemContext
    {
        public TruismItemContext() : base(Scope.Object)
        {
        }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            return new TruismRuleNode();
        }
    }
}