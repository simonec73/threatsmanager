using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Training Topic, which is a container of Pages.
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
        /// Pages associated to the topic.
        /// </summary>
        [JsonProperty("lessons")]
        public List<Page> Lessons;
    }
}
