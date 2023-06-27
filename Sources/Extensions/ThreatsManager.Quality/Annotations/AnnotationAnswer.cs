using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class AnnotationAnswer : Annotation
    {
        [JsonProperty("answeredOn")]
        private DateTime _answeredOn { get; set; }

        [property:NotRecorded]
        public DateTime AnsweredOn
        {
            get => _answeredOn;
            set
            {
                _answeredOn = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("answeredBy")]
        private string _answeredBy { get; set; }

        [property:NotRecorded]
        public string AnsweredBy
        {
            get => _answeredBy;
            set
            {
                _answeredBy = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("answeredVia")]
        public string AnsweredVia { get; set; }
    }
}