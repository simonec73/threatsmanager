using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Estimation : Frequency
    {
        [JsonProperty("mmin")]
        public double MagnitudeMin { get; set; }

        [JsonProperty("mml")]
        public double MagnitudeMostLikely { get; set; }

        [JsonProperty("mmax")]
        public double MagnitudeMax { get; set; }

        [JsonProperty("mconf")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Confidence MagnitudeConfidence { get; set; }

        public override bool IsValid => base.IsValid && (MagnitudeMin <= MagnitudeMostLikely)
                               && (MagnitudeMostLikely <= MagnitudeMax)
                               && (MagnitudeMin < MagnitudeMax);
    }
}