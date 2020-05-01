using Newtonsoft.Json;
using System.Collections.Generic;

namespace ThreatsManager.Utilities.Training
{
    /// <summary>
    /// Training Lesson.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Lesson
    {
        /// <summary>
        /// Identifier of the lesson.
        /// </summary>
        /// <remarks>It must be unique for the Section.</remarks>
        [JsonProperty("id")]
        public int Id;

        /// <summary>
        /// Name of the topic.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// Description of the topic.
        /// </summary>
        [JsonProperty("description")]
        public string Description;

        /// <summary>
        /// Tags associated to the topic.
        /// </summary>
        [JsonProperty("tags")]
        public List<string> Tags;
    }
}
