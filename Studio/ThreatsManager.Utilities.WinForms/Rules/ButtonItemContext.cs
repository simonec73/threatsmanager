using DevComponents.AdvTree;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal abstract class ButtonItemContext
    {
        public ButtonItemContext(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; private set; }

        public abstract SelectionRuleNode CreateNode(Node node, params object[] parameters);
    }
}