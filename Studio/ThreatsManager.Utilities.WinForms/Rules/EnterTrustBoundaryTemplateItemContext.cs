using System.Linq;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.WinForms.Rules
{
    internal class EnterTrustBoundaryTemplateItemContext : ListItemContext
    {
        private readonly IThreatModel _model;

        public EnterTrustBoundaryTemplateItemContext(IThreatModel model) : this(model, Scope.Object)
        {
        }

        public EnterTrustBoundaryTemplateItemContext(IThreatModel model, Scope scope) : 
            base(model?.TrustBoundaryTemplates?.Select(x => x.Name).OrderBy(x => x), scope)
        {
            _model = model;
        }

        public override SelectionRuleNode CreateNode([NotNull] Node node, params object[] parameters)
        {
            SelectionRuleNode result = null;

            if (parameters != null && parameters.Length == 1 && parameters[0] is string trustBoundaryTemplateName)
            {
                var trustBoundaryTemplate = _model?.TrustBoundaryTemplates?
                    .FirstOrDefault(x => string.CompareOrdinal(x.Name, trustBoundaryTemplateName) == 0);

                if (trustBoundaryTemplate != null)
                {
                    result = new EnterTrustBoundaryTemplateRuleNode(node.Text, trustBoundaryTemplate) {Scope = Scope};
                }
            }

            return result;
        }
    }
}