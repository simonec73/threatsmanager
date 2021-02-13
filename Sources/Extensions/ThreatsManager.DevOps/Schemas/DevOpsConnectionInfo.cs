using Newtonsoft.Json;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsConnectionInfo
    {
        [JsonProperty("connector")]
        public string ConnectorId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}