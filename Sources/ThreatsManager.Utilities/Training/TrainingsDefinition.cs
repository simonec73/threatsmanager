using Newtonsoft.Json;
using System.Collections.Generic;

namespace ThreatsManager.Utilities.Training
{
    /// <summary>
    /// Definition of trainings.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TrainingsDefinition
    {
        /// <summary>
        /// Sections defined. Each section describes a training level.
        /// </summary>
        [JsonProperty("sections")]
        public List<Section> Sections;
    }
}
