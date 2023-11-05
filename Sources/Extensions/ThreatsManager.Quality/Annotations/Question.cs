using Newtonsoft.Json;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Question
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("rule")]
        [Child]
        public SelectionRule Rule { get; set; }
    }
}
