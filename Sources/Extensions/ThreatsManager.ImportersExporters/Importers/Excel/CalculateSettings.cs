using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class CalculateSettings
    {
        /// <summary>
        /// Source type for the calculated column.
        /// </summary>
        [JsonProperty("source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SourceType Source { get; set; }

        /// <summary>
        /// Value of the calculated column.
        /// </summary>
        /// <remarks>It is used if the source type is 'Parameter'.</remarks>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Location of the Excel file.
        /// </summary>
        /// <remarks>It is used if the source type is 'ExcelFile'.</remarks>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// Name of the Excel sheet containing the information to be imported. It is a Regular Expression.
        /// </summary>
        /// <remarks>It is used if the source type is 'ExcelFile'.</remarks>
        [JsonProperty("sheet")]
        public string Sheet { get; set; }

        /// <summary>
        /// First row to be analyzed for importing.
        /// </summary>
        /// <remarks>It is used if the source type is 'ExcelFile'.</remarks>
        [JsonProperty("firstRow")]
        public int FirstRow { get; set; }

        /// <summary>
        /// Index of the column in the Excel sheet that must be used as a reference to identify the correct row.
        /// </summary>
        /// <remarks>It is used if the source type is 'ExcelFile'.</remarks>
        [JsonProperty("key")]
        public int Key { get; set; }

        /// <summary>
        /// Index of the column containing the value that must be imported.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("schemaName")]
        public string SchemaName { get; set; }

        [JsonProperty("schemaNs")]
        public string SchemaNamespace { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        /// <summary>
        /// Flag indicating if the column indicates a specifier that can be used to generate specific names.
        /// </summary>
        /// <remarks>We should have up to 1 specifier columns. If more than one is defined, only the first will be considered.</remarks>
        [JsonProperty("isSpecifier")]
        public bool Specifier { get; set; }

        /// <summary>
        /// Allows to automatically create an artifact from the Column value.
        /// </summary>
        /// <remarks>Each unique value of the column will create a single artifact.</remarks>
        [JsonProperty("artifact")]
        public ArtifactSettings Artifact { get; set; }
    }
}
