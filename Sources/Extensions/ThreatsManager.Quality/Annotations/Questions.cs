using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Quality.Annotations
{
    /// <summary>
    /// Questions object.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Questions
    {
        /// <summary>
        /// Enumeration of the Questions.
        /// </summary>
        [JsonProperty("questions")] 
        private List<Question> _questions { get; set; }

        public event Action<Question> QuestionAdded;

        public event Action<Question> QuestionRemoved;
            
        public IEnumerable<Question> Items => _questions?.AsReadOnly();

        public void Add([NotNull] Question question)
        {
            if (_questions == null)
                _questions = new List<Question>();

            if (!_questions.Contains(question))
            {
                _questions.Add(question);
                QuestionAdded?.Invoke(question);
            }
        }

        public void Remove([NotNull] Question question)
        {
            if (_questions?.Contains(question) ?? false)
            {
                _questions.Remove(question);
                QuestionRemoved?.Invoke(question);
            }
        }
    }
}