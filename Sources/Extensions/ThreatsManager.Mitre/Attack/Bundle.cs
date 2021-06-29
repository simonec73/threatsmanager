using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Attack
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Bundle
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("spec_version")]
        public string SpecVersion;

        [JsonProperty("objects")]
        public IEnumerable<Object> Objects;
    }
}
