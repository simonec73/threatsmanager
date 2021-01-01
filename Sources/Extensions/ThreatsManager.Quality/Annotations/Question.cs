using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ThreatsManager.AutoGenRules.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Question
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("rule")]
        public SelectionRule Rule { get; set; }
    }
}
