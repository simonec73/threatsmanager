using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.AutoThreatGeneration.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MitigationSelectionRule : SelectionRule
    {
        public MitigationSelectionRule()
        {
        }

        public MitigationSelectionRule([NotNull] SelectionRule rule)
        {
            Root = rule.Root;
        }

        [JsonProperty("severity")]
        public int? SeverityId { get; set; }

        [JsonProperty("strength")]
        public int? StrengthId { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MitigationStatus? Status { get; set; }
    }
}