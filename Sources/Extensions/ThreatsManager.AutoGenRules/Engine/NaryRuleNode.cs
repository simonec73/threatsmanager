using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class NaryRuleNode : SelectionRuleNode
    {
        [JsonProperty("children")]
        private readonly List<SelectionRuleNode> _children = new List<SelectionRuleNode>();

        public List<SelectionRuleNode> Children => _children;

        public override string ToString()
        {
            return Name;
        }
    }
}
