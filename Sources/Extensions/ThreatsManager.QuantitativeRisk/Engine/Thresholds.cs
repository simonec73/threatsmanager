using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Thresholds
    {
        [JsonProperty("thresholds")]
        public List<Threshold> Items { get; set; }
    }
}