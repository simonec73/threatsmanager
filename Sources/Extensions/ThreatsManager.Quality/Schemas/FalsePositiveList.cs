using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Quality.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FalsePositiveList
    {
        [JsonProperty("falsePositives")]
        public List<FalsePositiveInfo> FalsePositives { get; set; }
    }
}
