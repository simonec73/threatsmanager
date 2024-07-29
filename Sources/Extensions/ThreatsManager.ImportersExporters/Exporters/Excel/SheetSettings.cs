using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SheetSettings
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("objectType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// List of the columns to be imported.
        /// </summary>
        [JsonProperty("columns")]
        public List<ColumnSettings> Columns { get; set; }

        [JsonProperty("sortRule")]
        public IEnumerable<SortConfig> SortRule { get; set; }

        [JsonProperty("filter")]
        public FilterSettings Filter { get; set; }
    }
}