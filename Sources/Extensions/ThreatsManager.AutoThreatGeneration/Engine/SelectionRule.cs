using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.AutoThreatGeneration.Engine
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

        public bool Evaluate([NotNull] IIdentity identity)
        {
            return Root?.Evaluate(identity) ?? false;
        }
    }
}
