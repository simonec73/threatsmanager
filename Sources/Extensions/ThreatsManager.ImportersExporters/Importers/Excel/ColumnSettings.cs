using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class ColumnSettings
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("fieldType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        /// <summary>
        /// Flag indicating if the column indicates a key column for the rules.
        /// </summary>
        /// <remarks>We should have up to 1 specifier columns. If more than one is defined, only the first will be considered.</remarks>
        [JsonProperty("isKey")]
        public bool IsKey { get; set; }

        [JsonProperty("mapping")]
        public Dictionary<string, string> Mapping { get; set; }
    }
}