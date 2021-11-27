using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Loss : Magnitude
    {
        [JsonProperty("form")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LossForm Form { get; set; }
    }
}