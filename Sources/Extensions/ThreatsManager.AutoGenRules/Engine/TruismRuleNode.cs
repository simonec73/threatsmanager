using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
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
