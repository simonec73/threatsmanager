using Newtonsoft.Json;

namespace ThreatsManager.ImportersExporters.Importers
{
    /// <summary>
    /// Value of a parameter.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ParameterValue
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
