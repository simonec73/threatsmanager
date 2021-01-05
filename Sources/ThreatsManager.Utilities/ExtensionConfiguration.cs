using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Extension Configuration.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtensionConfiguration
    {
        /// <summary>
        /// Properties of the Extension Configuration.
        /// </summary>
        [JsonProperty("properties")]
        public Dictionary<string, object> Properties { get; set; }
    }
}