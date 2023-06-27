using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
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
    public class Questions : IPostDeserialization
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

        public event Action<Question> QuestionAdded;

        public event Action<Question> QuestionRemoved;
            
        public IEnumerable<Question> Items => _questions?.AsEnumerable();

        public void Add([NotNull] Question question)
        {
            if (_questions == null)
                _questions = new AdvisableCollection<Question>();

            if (!_questions.Contains(question))
            {
                _questions.Add(question);
                UndoRedoManager.Attach(question);
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

                foreach (var question in _legacyQuestions)
                {
                    _questions.Add(question);
                    UndoRedoManager.Detach(question);
                }

                _legacyQuestions.Clear();
            }
        }
    }
}