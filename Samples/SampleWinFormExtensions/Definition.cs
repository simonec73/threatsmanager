using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.SampleWinFormExtensions
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class Definition
    {
        public Definition([NotNull] string name, [NotNull] string value)
        {
            Name = name;
            Value = value;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
