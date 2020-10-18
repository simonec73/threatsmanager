using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.PackageManagers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtensionInfo
    {
        public ExtensionInfo()
        {

        }

        public ExtensionInfo([Required] string id, [Required] string label)
        {
            Id = id;
            Label = label;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}