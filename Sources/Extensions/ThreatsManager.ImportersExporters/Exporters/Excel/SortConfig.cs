using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SortConfig
    {
        [JsonProperty("fieldType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        [JsonProperty("desc")]
        public bool Descending { get; set; }
    }
}