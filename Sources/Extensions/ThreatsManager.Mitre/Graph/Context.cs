using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Context
    {
        public Context()
        {

        }

        public Context(ContextType contextType, string className, string name)
        {
            ContextType = contextType;
            Class = className;
            Name = name;
        }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContextType ContextType { get; private set; }

        [JsonProperty("class")]
        public string Class { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }
    }
}