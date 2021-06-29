using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Annotations
{
    public class TopicToBeClarified : Annotation
    {
        public TopicToBeClarified()
        {
            Printable = false;
        }

        [JsonProperty("askedOn")]
        private DateTime _askedOn { get; set; }

        public DateTime AskedOn
        {
            get => _askedOn;
            set
            {
                _askedOn = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("askedBy")]
        private string _askedBy { get; set; }

        public string AskedBy
        {
            get => _askedBy;
            set
            {
                _askedBy = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("askedVia")]
        public string AskedVia { get; set; }

        [JsonProperty("answered")]
        private bool _answered { get; set; }

        public bool Answered
        {
            get => _answered;
            set
            {
                _answered = value;
                ModifiedOn = DateTime.Now;
                ModifiedBy = UserName.GetDisplayName();
            }
        }

        [JsonProperty("answers")]
        private List<AnnotationAnswer> _answers { get; set; }

        public IEnumerable<AnnotationAnswer> Answers => _answers?.AsReadOnly();

        public AnnotationAnswer AddAnswer()
        {
            if (_answers == null)
                _answers = new List<AnnotationAnswer>();

            var result = new AnnotationAnswer();
            _answers.Add(result);

            return result;
        }

        public void RemoveAnswer(AnnotationAnswer answer)
        {
            if (_answers?.Contains(answer) ?? false)
                _answers.Remove(answer);
        }
    }
}
