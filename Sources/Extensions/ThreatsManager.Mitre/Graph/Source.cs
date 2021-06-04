using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Source
    {
        public Source()
        {

        }

        public Source([Required] string source, [Required] string version, DateTime timestamp)
        {
            Name = source;
            Version = version;
            Timestamp = timestamp;
        }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("version")]
        public string Version { get; private set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; private set; }
    }
}