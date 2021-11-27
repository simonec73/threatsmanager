using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Frequency : FactBased
    {
        [JsonProperty("fmin")]
        public double FrequencyMin { get; set; }

        [JsonProperty("fml")]
        public double FrequencyMostLikely { get; set; }

        [JsonProperty("fmax")]
        public double FrequencyMax { get; set; }

        [JsonProperty("fconf")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Confidence FrequencyConfidence { get; set; }

        public virtual bool IsValid => (FrequencyMin <= FrequencyMostLikely)
                               && (FrequencyMostLikely <= FrequencyMax)
                               && (FrequencyMin < FrequencyMax);
    }
}