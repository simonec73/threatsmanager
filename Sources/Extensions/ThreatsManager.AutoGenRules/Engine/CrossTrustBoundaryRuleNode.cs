using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CrossTrustBoundaryRuleNode : SelectionRuleNode
    {
        public CrossTrustBoundaryRuleNode()
        {

        }

        public CrossTrustBoundaryRuleNode([Required] string name, bool value)
        {
            Name = name;
            Value = value;
        }

        [JsonProperty("value")]
        public bool Value { get; set; }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IDataFlow dataFlow)
            {
                result = (Value && dataFlow.Source.Parent != dataFlow.Target.Parent) ||
                    (!Value && dataFlow.Source.Parent == dataFlow.Target.Parent);
            }

            return result;
        }

        public override string ToString()
        {
            return $"FLOW CROSSES ANY TRUST BOUNDARY";
        }
    }
}
