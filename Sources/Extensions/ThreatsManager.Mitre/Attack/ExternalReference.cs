using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Attack
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExternalReference
    {
        [JsonProperty("description")]
        public string Description;

        [JsonProperty("url")]
        public string Url;

        [JsonProperty("external_id")]
        public string ExternalId;

        [JsonProperty("source_name")]
        public string Source;
    }
}