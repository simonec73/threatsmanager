using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.PackageManagers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtensionsList
    {
        [JsonProperty("extensions")]
        public List<ExtensionInfo> Extensions { get; set; }
    }
}