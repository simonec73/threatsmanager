using System;
using Newtonsoft.Json;

namespace ThreatsManager.Quality.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FalsePositiveInfo
    {
        [JsonProperty("id")]
        public string QualityInitializerId { get; set; } 

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}