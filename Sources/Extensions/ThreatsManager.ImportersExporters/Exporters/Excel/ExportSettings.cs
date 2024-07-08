using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExportSettings
    {
        /// <summary>
        /// Direction of the operation.
        /// </summary>
        /// <remarks>For export must be Export.</remarks>
        [JsonProperty("direction")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Direction Direction { get; set; }

        /// <summary>
        /// Version of the format.
        /// </summary>
        /// <remarks>It is checked by the exporter. If is not correct, the export fails.</remarks>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Configuration of the sheets that must be exported.
        /// </summary>
        [JsonProperty("sheets")]
        public List<SheetSettings> Sheets { get; set; }
    }
}
