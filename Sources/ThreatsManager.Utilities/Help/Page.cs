using Newtonsoft.Json;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Help page.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Page
    {
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
        /// Url of the lesson.
        /// </summary>
        [JsonProperty("url")]
        public string Url;
    }
}
