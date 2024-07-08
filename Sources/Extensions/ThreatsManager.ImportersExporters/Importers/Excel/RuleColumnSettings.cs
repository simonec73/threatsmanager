using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class RuleColumnSettings
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("fieldType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RuleFieldType FieldType { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }
    }
}