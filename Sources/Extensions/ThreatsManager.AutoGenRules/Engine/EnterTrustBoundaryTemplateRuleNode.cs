using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class EnterTrustBoundaryTemplateRuleNode : SelectionRuleNode
    {
        public EnterTrustBoundaryTemplateRuleNode()
        {
        }

        public EnterTrustBoundaryTemplateRuleNode([Required] string name, ITrustBoundaryTemplate template)
        {
            Name = name;
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
                   (GetEnteringTrustBoundaries(identity)
                       ?.Any(x => (x.Template?.Id ?? Guid.Empty) == TrustBoundaryTemplate) ?? false);
        }

        public override string ToString()
        {
            return $"Flow enters a Trust Boundary which is an instance of Trust Boundary Template with ID '{TrustBoundaryTemplate:D}";
        }
    }
}
