using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ColumnSettings
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("width")]
        public float Width { get; set; }

        [JsonProperty("fieldType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }
    }
}