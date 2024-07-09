using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class ImportSettings
    {
        /// <summary>
        /// Direction of the operation.
        /// </summary>
        /// <remarks>For import must be Import.</remarks>
        [JsonProperty("direction")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Direction Direction { get; set; }

        /// <summary>
        /// Version of the format.
        /// </summary>
        /// <remarks>It is checked by the importer. If is not correct, the import fails.</remarks>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Level of the imported information. 
        /// </summary>
        /// <remarks>It is used for versioning and filtering within the rules file.</remarks>
        [JsonProperty("level")]
        public string Level { get; set; }

        /// <summary>
        /// Strict validation checks that all sheets are configured as expected before executing the import.
        /// If there is any discrepancy, then import is aborted.
        /// If it is false, then no pre-validation is performed and partial import would be possible.
        /// In fact, in that case only the mismatching sheets are skipped.
        /// </summary>
        [JsonProperty("strictValidation")]
        public bool StrictValidation { get; set; }

        /// <summary>
        /// Automatically remove Threat Types with no associated Mitigation, and Mitigations with no associated Threat.
        /// </summary>
        [JsonProperty("autoCleanup")]
        public bool AutoCleanup { get; set; }

        /// <summary>
        /// List of parameters.
        /// </summary>
        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Sheets to be imported.
        /// </summary>
        [JsonProperty("sheets")]
        public List<SheetSettings> Sheets { get; set; }

        /// <summary>
        /// Json file containing the additional details required to import the Item KnowledgeBases.
        /// </summary>
        /// <remarks>It is a path relative to the location of the import settings file.</remarks>
        [JsonProperty("ruleItems")]
        public string ItemTemplateRules { get; set; }
    }
}
