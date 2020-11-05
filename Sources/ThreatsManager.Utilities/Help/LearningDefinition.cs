using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Definition of Learnings.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class LearningDefinition
    {
        /// <summary>
        /// Sections defined. Each section describes a training level.
        /// </summary>
        [JsonProperty("sections")]
        public List<Section> Sections;
    }
}
