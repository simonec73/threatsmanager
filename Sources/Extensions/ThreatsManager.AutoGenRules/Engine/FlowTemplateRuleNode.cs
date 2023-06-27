using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class FlowTemplateRuleNode : SelectionRuleNode
    {
        public FlowTemplateRuleNode()
        {
        }

        public FlowTemplateRuleNode(IFlowTemplate flowTemplate)
        {
            this.Name = "Flow Template";
            this.FlowTemplate = flowTemplate?.Id ?? Guid.Empty;
        }

        [JsonProperty("flowTemplate")]
        public Guid FlowTemplate { get; set; }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IDataFlow flow)
            {
                result = InternalEvaluate(flow);
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] IDataFlow flow)
        {
            return FlowTemplate != Guid.Empty && flow.Template?.Id == FlowTemplate;
        }

        public override string ToString()
        {
            return $"{Scope}:Is an instance of Flow Template with ID '{FlowTemplate:D}";
        }
    }
}
