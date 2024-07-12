using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlTypeAssociationsDefinition
    {
        [JsonProperty("associations")]
        public List<ControlTypeAssociation> Associations { get; set; }

        /// <summary>
        /// Control Type for the Mitigation.
        /// </summary>
        [JsonProperty("default")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SecurityControlType Default { get; set; }

        [JsonProperty("mitigationSchemaName")]
        public string MitigationSchemaName { get; set; }

        [JsonProperty("mitigationSchemaNs")]
        public string MitigationSchemaNamespace { get; set; }

        [JsonProperty("mitigationPropertyName")]
        public string MitigationPropertyName { get; set; }

        [JsonProperty("levelSchemaName")]
        public string LevelSchemaName { get; set; }

        [JsonProperty("levelSchemaNs")]
        public string LevelSchemaNamespace { get; set; }

        [JsonProperty("levelPropertyName")]
        public string LevelPropertyName { get; set; }

    }
}
