using System.Linq;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class FlowTemplateItemContext : ListItemContext
    {
        private readonly IThreatModel _model;

        public FlowTemplateItemContext([NotNull] IThreatModel model, Scope scope) : 
            base(model?.FlowTemplates?.Select(x => x.Name).OrderBy(x => x), scope)
        {
            _model = model;
        }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is string flowTemplateName)
            {
                var flowTemplate = _model?.FlowTemplates?
                    .FirstOrDefault(x => string.CompareOrdinal(x.Name, flowTemplateName) == 0);

                if (flowTemplate != null)
                {
                    result = new FlowTemplateRuleNode(flowTemplate) {Scope = Scope};
                }
            }

            return result;
        }
    }
}