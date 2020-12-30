using Newtonsoft.Json;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class UnaryRuleNode : SelectionRuleNode
    {
        [JsonProperty("child")]
        public SelectionRuleNode Child { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
