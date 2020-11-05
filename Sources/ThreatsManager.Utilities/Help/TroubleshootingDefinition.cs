using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Definition of Learnings.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TroubleshootingDefinition
    {
        /// <summary>
        /// Pages defined.
        /// </summary>
        [JsonProperty("pages")]
        public List<Page> Pages;
    }
}