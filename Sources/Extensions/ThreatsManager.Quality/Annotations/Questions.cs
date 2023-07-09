using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Quality.Annotations
{
    /// <summary>
    /// Questions object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Questions : IPostDeserialization, IThreatModelAware
    {
        /// <summary>
        /// Enumeration of the Questions.
        /// </summary>
        [JsonProperty("questions")]
        [Reference]
        [field:NotRecorded]
        private List<Question> _legacyQuestions { get; set; }

        [JsonProperty("items")]
        [Child]
        private AdvisableCollection<Question> _questions { get; set; }

        [JsonProperty("modelId")]
        private Guid _modelId { get; set; }

        public event Action<Question> QuestionAdded;

        public event Action<Question> QuestionRemoved;
            
        public IEnumerable<Question> Items => _questions?.AsEnumerable();

        public Guid ModelId
        {
            get => _modelId;

            set
            {
                if (_modelId != value)
                    _modelId = value;
                if (_questions?.Any() ?? false)
                {
                    foreach (var question in _questions)
                    {
                        if (question.Rule != null)
                            question.Rule.ModelId = value;
                    }
                }
            }
        }

        public void Add([NotNull] Question question)
        {
            if (_questions == null)
                _questions = new AdvisableCollection<Question>();

            if (!_questions.Contains(question))
            {
                _questions.Add(question);
                UndoRedoManager.Attach(question, ThreatModelManager.Get(_modelId));
                QuestionAdded?.Invoke(question);
            }
        }

        public void Remove([NotNull] Question question)
        {
            if (_questions?.Contains(question) ?? false)
            {
                UndoRedoManager.Detach(question);
                _questions.Remove(question);
                QuestionRemoved?.Invoke(question);
            }
        }

        public void ExecutePostDeserialization()
        {
            if (_legacyQuestions?.Any() ?? false)
            {
                if (_questions == null)
                    _questions = new AdvisableCollection<Question>();

                var model = ThreatModelManager.Get(_modelId);
                foreach (var question in _legacyQuestions)
                {
                    UndoRedoManager.Attach(question, model);
                    _questions.Add(question);
                }

                _legacyQuestions.Clear();
            }
        }
    }
}