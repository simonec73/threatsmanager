using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ItemDetails
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("itemType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType ItemType { get; set; }

        [JsonProperty("ignoreThreats")]
        public List<string> IgnoreThreatTypes { get; set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("controls")]
        public List<Control> AdditionalControls { get; set; }
    }
}
