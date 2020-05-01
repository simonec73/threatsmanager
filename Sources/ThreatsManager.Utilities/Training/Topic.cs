using Newtonsoft.Json;
using System.Collections.Generic;

namespace ThreatsManager.Utilities.Training
{
    /// <summary>
    /// Training Topic, which is a container of Lessons.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Topic
    {
        /// <summary>
        /// Name of the topic.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// Lessons associated to the topic.
        /// </summary>
        [JsonProperty("lessons")]
        public List<Lesson> Lessons;
    }
}
