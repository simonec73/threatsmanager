using Newtonsoft.Json;
using System.Collections.Generic;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemsToBeRemovedDefinition
    {
        [JsonProperty("ids")]
        public List<string> Identifiers { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("PropertyName")]
        public string PropertyName { get; set; }

        [JsonProperty("levelSchemaName")]
        public string LevelSchemaName { get; set; }

        [JsonProperty("levelSchemaNs")]
        public string LevelSchemaNamespace { get; set; }

        [JsonProperty("levelPropertyName")]
        public string LevelPropertyName { get; set; }

    }
}
