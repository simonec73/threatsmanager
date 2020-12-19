using System;
using Newtonsoft.Json;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AnnotationAnswer : Annotation
    {
        [JsonProperty("answeredOn")]
        private DateTime _answeredOn { get; set; }

        public DateTime AnsweredOn
        {
            get => _answeredOn;
            set
            {
                _answeredOn = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = GetUserName();
            }
        }

        [JsonProperty("answeredBy")]
        private string _answeredBy { get; set; }

        public string AnsweredBy
        {
            get => _answeredBy;
            set
            {
                _answeredBy = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = GetUserName();
            }
        }

        [JsonProperty("answeredVia")]
        public string AnsweredVia { get; set; }
    }
}