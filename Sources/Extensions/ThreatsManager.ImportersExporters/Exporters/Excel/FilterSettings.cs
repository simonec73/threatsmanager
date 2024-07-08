using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FilterSettings
    {
        /// <summary>
        /// Field Type.
        /// </summary>
        [JsonProperty("fieldType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Indicates if the item to be considered is the current one, or something else.
        /// </summary>
        [JsonProperty("referencedItem")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReferencedItemType ReferencedItem { get; set; } 

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        /// <summary>
        /// Value which must be compared. This is a regular expression.
        /// </summary>
        [JsonProperty("regex")]
        public string Regex { get; set; }

        /// <summary>
        /// If true, then the filter is matched if the text is null, empty or whitespaces.
        /// </summary>
        [JsonProperty("isEmpty")]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Specifies if the check is case sensitive.
        /// </summary>
        [JsonProperty("caseSensitive")]
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Flag specifying if in case of a match we have to include the row. If false, then in case of match we will exclude the row.
        /// </summary>
        [JsonProperty("include")]
        public bool Include { get; set; }
    }
}