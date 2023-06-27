using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class TruismRuleNode : SelectionRuleNode
    {
        public TruismRuleNode()
        {
            Name = "TRUE";
        }

        public override bool Evaluate([NotNull] object context)
        {
            return true;
        }

        public override string ToString()
        {
            return $"TRUE";
        }
    }
}
