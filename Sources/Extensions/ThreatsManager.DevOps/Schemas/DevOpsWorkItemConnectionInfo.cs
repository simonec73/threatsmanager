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

        [JsonProperty("itemUrl")]
        public string ItemUrl { get; set; }

        [JsonProperty("assignedTo")]
        public string AssignedTo { get; set; }
    }
}