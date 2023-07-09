using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class TopicToBeClarified : Annotation, IPostDeserialization, IThreatModelAware
    {
        public TopicToBeClarified()
        {
            Printable = false;
        }

        [JsonProperty("modelId")]
        private Guid _modelId { get; set; }

        [JsonProperty("askedOn")]
        private DateTime _askedOn { get; set; }

        public Guid ModelId
        {
            get => _modelId;

            set
            {
                if (_modelId != value)
                    _modelId = value;
            }
        }

        [property:NotRecorded]
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

        [property:NotRecorded]
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

        [property:NotRecorded]
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
        [Reference]
        [field:NotRecorded]
        private List<AnnotationAnswer> _legacyAnswers { get; set; }

        [JsonProperty("items")]
        [Child]
        private AdvisableCollection<AnnotationAnswer> _answers { get; set; }

        [property:NotRecorded]
        public IEnumerable<AnnotationAnswer> Answers => _answers?.AsEnumerable();

        public AnnotationAnswer AddAnswer()
        {
            if (_answers == null)
                _answers = new AdvisableCollection<AnnotationAnswer>();

            var result = new AnnotationAnswer();
            _answers.Add(result);
            UndoRedoManager.Attach(result, ThreatModelManager.Get(_modelId));

            return result;
        }

        public void RemoveAnswer(AnnotationAnswer answer)
        {
            if (_answers?.Contains(answer) ?? false)
            {
                UndoRedoManager.Detach(answer);
                _answers.Remove(answer);
            }
        }

        public void ExecutePostDeserialization()
        {
            if (_legacyAnswers?.Any() ?? false)
            {
                if (_answers == null)
                    _answers = new AdvisableCollection<AnnotationAnswer>();

                var model = ThreatModelManager.Get(_modelId);
                foreach (var answer in _legacyAnswers)
                {
                    UndoRedoManager.Attach(answer, model);
                    _answers.Add(answer);
                }

                _legacyAnswers.Clear();
            }
        }
    }
}
