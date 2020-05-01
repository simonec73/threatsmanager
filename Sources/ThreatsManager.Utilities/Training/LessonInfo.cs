using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreatsManager.Utilities.Training
{
    /// <summary>
    /// Information about a Lesson.
    /// </summary>
    public class LessonInfo
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="jsonUrl">Base Url of the training.</param>
        /// <param name="prefix">Prefix to be adopted to perform the search.</param>
        /// <param name="topic">Topic containing the lesson.</param>
        /// <param name="lesson">Lesson definition.</param>
        public LessonInfo([Required] string jsonUrl, string prefix, 
            [Required] string topic, [NotNull] Lesson lesson)
        {
            Uri uri = new Uri(jsonUrl);
            Url = new Uri(uri, $"{prefix}{lesson.Id}/{prefix}{lesson.Id}.html");
            Topic = topic;
            Name = lesson.Name;
            Description = lesson.Description;
            Tags = lesson.Tags?.ToArray();
        }

        /// <summary>
        /// Url of the entry point.
        /// </summary>
        public Uri Url {get; private set;}

        /// <summary>
        /// Topic for the lesson.
        /// </summary>
        public string Topic {get; private set;}

        /// <summary>
        /// Name of the training.
        /// </summary>
        public string Name {get; private set;}

        /// <summary>
        /// Description of the training.
        /// </summary>
        public string Description {get; private set;}

        /// <summary>
        /// Tags associated to the Training.
        /// </summary>
        public IEnumerable<string> Tags {get; private set;}

        /// <summary>
        /// Checks if the Training Info has at least a Tag among the specified list.
        /// </summary>
        /// <param name="tags">List of desired tags.</param>
        /// <returns>True if at least a tag is present, false otherwise.</returns>
        public bool HasTag([NotNull] IEnumerable<string> tags)
        {
            return tags.Any(x => Tags?.Contains(x) ?? false);
        }
    }
}
