using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class FlowsItemContext : ListItemContext
    {
        public FlowsItemContext(bool incoming, Scope scope) : base(EnumExtensions.GetEnumLabels<EntityType>(), scope)
        {
            Incoming = incoming;
        }

        public bool Incoming { get; private set; }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is string entityTypeString)
            {
                var entityType = entityTypeString.GetEnumValue<EntityType>();

                result = Incoming ?
                    (SelectionRuleNode)new HasIncomingRuleNode(entityType) { Scope = Scope } :
                    new HasOutgoingRuleNode(entityType) { Scope = Scope };
            }

            return result;
        }
    }
}