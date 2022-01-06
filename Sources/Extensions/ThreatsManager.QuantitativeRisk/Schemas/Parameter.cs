using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.QuantitativeRisk.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Parameter
    {
        private Parameter()
        {

        }

        public Parameter([Required] string name, string value)
        {
            Name = name;
            Value = value;
        }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }
    }
}
