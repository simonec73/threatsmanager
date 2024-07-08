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

        /// <summary>
        /// Flag indicating if the column indicates a specifier that can be used to generate specific names.
        /// </summary>
        /// <remarks>We should have up to 1 specifier columns. If more than one is defined, only the first will be considered.</remarks>
        [JsonProperty("isSpecifier")]
        public bool IsSpecifier { get; set; }

        [JsonProperty("mapping")]
        public Dictionary<string, string> Mapping { get; set; }

        /// <summary>
        /// Allows to automatically create an artifact from the Column value.
        /// </summary>
        /// <remarks>Each unique value of the column will create a single artifact.</remarks>
        [JsonProperty("artifact")]
        public ArtifactSettings Artifact { get; set; }
    }
}