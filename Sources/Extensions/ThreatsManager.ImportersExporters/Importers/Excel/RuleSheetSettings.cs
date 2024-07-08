using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class RuleSheetSettings
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("objectType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Index of the first row to import.
        /// </summary>
        [JsonProperty("firstRow")]
        public int FirstRow { get; set; }

        /// <summary>
        /// Index of the column to be used as reference.
        /// </summary>
        [JsonProperty("key")]
        public int KeyColumn { get; set; }

        /// <summary>
        /// List of the columns to be imported.
        /// </summary>
        [JsonProperty("columns")]
        public List<RuleColumnSettings> Columns { get; set; }

        [JsonProperty("threatPolicy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ThreatPolicyType ThreatPolicy { get; set; }

        /// <summary>
        /// If true, then the default behavior is to exclude the row, otherwise it is to include it.
        /// Filters will determine if it must be imported or not.
        /// </summary>
        [JsonProperty("defaultExclude")]
        public bool DefaultExclude { get; set; }

        /// <summary>
        /// Filters used to determine which rows must be imported.
        /// </summary>
        [JsonProperty("filters")]
        public List<FilterSettings> Filters { get; set; }
    }
}