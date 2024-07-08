using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Control
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("controlType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SecurityControlType ControlType { get; private set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MitigationStatus? Status { get; private set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("threats")]
        public List<Threat> Threats { get; set; }
    }
}
