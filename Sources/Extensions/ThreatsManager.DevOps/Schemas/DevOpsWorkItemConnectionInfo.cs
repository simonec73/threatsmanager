using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsWorkItemConnectionInfo : DevOpsConnectionInfo
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkItemStatus Status { get; set; }
    }
}