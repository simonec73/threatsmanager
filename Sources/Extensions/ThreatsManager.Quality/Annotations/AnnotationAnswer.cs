using System;
using Newtonsoft.Json;
using ThreatsManager.Utilities;

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
                ModifiedBy = UserName.GetDisplayName();
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
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("answeredVia")]
        public string AnsweredVia { get; set; }
    }
}