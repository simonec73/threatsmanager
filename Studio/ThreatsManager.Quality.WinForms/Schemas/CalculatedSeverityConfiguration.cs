using System;
using Newtonsoft.Json;

namespace ThreatsManager.Quality.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CalculatedSeverityConfiguration
    {
        [JsonProperty("delta")]
        public int Delta { get; set; }

        [JsonProperty("reason")]
        public string DeltaReason { get; set; }

        [JsonProperty("setBy")]
        public string DeltaSetBy { get; set; }

        [JsonProperty("setOn")]
        public DateTime DeltaSetOn { get; set; }
    }
}
