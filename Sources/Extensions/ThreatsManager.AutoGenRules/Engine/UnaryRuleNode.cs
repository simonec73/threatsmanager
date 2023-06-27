using Newtonsoft.Json;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public abstract class UnaryRuleNode : SelectionRuleNode
    {
        [JsonProperty("child", TypeNameHandling = TypeNameHandling.Objects)]
        [Child]
        public SelectionRuleNode Child { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
