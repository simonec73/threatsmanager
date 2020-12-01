using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsWorkItemStateMapping
    {
        public DevOpsWorkItemStateMapping([Required] string field, WorkItemStatus status)
        {
            Field = field;
            Status = status;
        }

        [JsonProperty("field")]
        public string Field { get; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WorkItemStatus Status { get; }
    }
}