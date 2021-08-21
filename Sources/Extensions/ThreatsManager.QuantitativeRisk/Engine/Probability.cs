using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Probability : FactBased
    {
        [JsonProperty("min")]
        public double Minimum { get; set; }

        [JsonProperty("ml")]
        public double MostLikely { get; set; }

        [JsonProperty("max")]
        public double Maximum { get; set; }

        [JsonProperty("conf")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Confidence Confidence { get; set; }

        public virtual bool IsValid => (Minimum <= MostLikely)
                                       && (MostLikely <= Maximum)
                                       && (Minimum < Maximum);
    }
}