using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace ThreatsManager.Utilities.Training
{

    /// <summary>
    /// Training Section.
    /// </summary>
    /// <remarks>Describes a Training Level.</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class Section
    {
        /// <summary>
        /// Training Level.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TrainingLevel SectionType;

        /// <summary>
        /// Prefix for the items.
        /// </summary>
        /// <remarks>This is optional.</remarks>
        [JsonProperty("prefix")]
        public string Prefix;

        /// <summary>
        /// List of topics.
        /// </summary>
        /// <remarks>Each topic represents a single lesson.</remarks>
        [JsonProperty("topics")]
        public List<Topic> Topics;
    }
}
