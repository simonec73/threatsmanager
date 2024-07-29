using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Threat
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("strength")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DefaultStrength Strength { get; set; }

        [JsonProperty("top")]
        public bool Top { get; set; }
    }
}
