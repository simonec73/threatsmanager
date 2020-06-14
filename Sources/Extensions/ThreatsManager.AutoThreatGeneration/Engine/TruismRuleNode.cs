using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TruismRuleNode : SelectionRuleNode
    {
        public TruismRuleNode()
        {
            Name = "TRUE";
        }

        public override bool Evaluate([NotNull] IIdentity identity)
        {
            return true;
        }

        public override string ToString()
        {
            return $"TRUE";
        }
    }
}
