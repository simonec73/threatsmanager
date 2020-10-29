using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.Utilities.Help
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
        public LearningLevel SectionType;

        /// <summary>
        /// List of topics.
        /// </summary>
        /// <remarks>Each topic represents a single lesson.</remarks>
        [JsonProperty("topics")]
        public List<Topic> Topics;
    }
}
