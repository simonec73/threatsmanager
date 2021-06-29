using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Properties;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SelectionRule
    {
        [JsonProperty("node")]
        public SelectionRuleNode Root { get; set; }

        public override string ToString()
        {
            string result = Root?.ToString();

            if (result == null)
                result = Resources.LabelSelectionRule;

            return result;
        }

        public bool Evaluate([NotNull] object context)
        {
            return Root?.Evaluate(context) ?? false;
        }
    }
}
