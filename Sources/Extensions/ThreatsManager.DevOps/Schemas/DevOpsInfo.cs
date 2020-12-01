using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsInfo
    {
        [JsonProperty("infos")]
        public List<DevOpsConnectionInfo> Infos { get; set; }
    }
}