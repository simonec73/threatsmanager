using System;
using Newtonsoft.Json;

namespace ThreatsManager.DevOps
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Iteration
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("start")]
        public DateTime? Start { get; set; }

        [JsonProperty("end")]
        public DateTime? End { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}