using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TrustBoundaryTemplateRuleNode : SelectionRuleNode
    {
        public TrustBoundaryTemplateRuleNode()
        {
        }

        public TrustBoundaryTemplateRuleNode(ITrustBoundaryTemplate template)
        {
            this.Name = "Flow";
            this.TrustBoundaryTemplate = template?.Id ?? Guid.Empty;
        }

        [JsonProperty("trustBoundaryTemplate")]
        public Guid TrustBoundaryTemplate { get; set; }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IDataFlow flow)
            {
                result = InternalEvaluate(flow);
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] IIdentity identity)
        {
            return TrustBoundaryTemplate != Guid.Empty &&
                   (GetCrossedTrustBoundaries(identity)
                       ?.Any(x => (x.Template?.Id ?? Guid.Empty) == TrustBoundaryTemplate) ?? false);
        }

        public override string ToString()
        {
            return $"Flow crosses a Trust Boundary which is an instance of Trust Boundary Template with ID '{TrustBoundaryTemplate:D}";
        }
    }
}
