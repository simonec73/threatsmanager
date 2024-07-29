using Newtonsoft.Json;

namespace ThreatsManager.ImportersExporters.Importers
{
    /// <summary>
    /// Parameter definition.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Parameter
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        /// <remarks>It must start with a '@'.</remarks>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Message prompt to be shown if the parameter is not defined.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt { get; set; }
    }
}
