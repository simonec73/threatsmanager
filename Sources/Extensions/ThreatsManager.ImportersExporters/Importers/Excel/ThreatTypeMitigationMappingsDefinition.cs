using Newtonsoft.Json;
using System.Collections.Generic;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ThreatTypeMitigationMappingsDefinition
    {
        [JsonProperty("mappings")]
        public List<ThreatTypeMitigationMapping> Mappings { get; set; }

        [JsonProperty("threatTypeSchemaName")]
        public string ThreatTypeSchemaName { get; set; }

        [JsonProperty("threatTypeSchemaNs")]
        public string ThreatTypeSchemaNamespace { get; set; }

        [JsonProperty("threatTypePropertyName")]
        public string ThreatTypePropertyName { get; set; }

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
