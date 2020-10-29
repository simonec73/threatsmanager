using System.Collections.Generic;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Information about a Topic.
    /// </summary>
    public class TopicInfo
    {
        private readonly List<Page> _lessons = new List<Page>();

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="name">Name of the Topic.</param>
        public TopicInfo([Required] string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the Topic.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Lessons associated to the Topic.
        /// </summary>
        public IEnumerable<Page> Lessons => _lessons;

        /// <summary>
        /// Add a lesson to the Topic.
        /// </summary>
        /// <param name="lesson">Lesson to be added.</param>
        public void AddLesson(Page lesson)
        {
            if (!_lessons.Contains(lesson))
                _lessons.Add(lesson);
        }
    }
}