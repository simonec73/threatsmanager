using Newtonsoft.Json;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class FilterSettings
    {
        /// <summary>
        /// Index of the column. Starts from 1.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

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

        /// <summary>
        /// Flag specifying if in case of a match we shall stop processing the following filters.
        /// </summary>
        /// <remarks>In case of cascade, the last filter which stops processing determines the decision.</remarks>
        [JsonProperty("stopIfMatch")]
        public bool StopIfMatch { get; set; }

        /// <summary>
        /// Flag specifying if in case of a not match we shall stop processing the following filters.
        /// </summary>
        /// <remarks>In case of cascade, the last filter which stops processing determines the decision.</remarks>
        [JsonProperty("stopIfNotMatch")]
        public bool StopIfNotMatch { get; set; }
    }
}