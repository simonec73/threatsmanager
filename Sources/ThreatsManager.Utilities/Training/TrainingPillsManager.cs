using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Patterns.Threading;
using Newtonsoft.Json;

namespace ThreatsManager.Utilities.Training
{
    /// <summary>
    /// Training Pills Manager.
    /// </summary>
    /// <remarks>In implements the Singleton pattern.</remarks>
    public sealed class TrainingPillsManager
    {
        private static readonly Lazy<TrainingPillsManager> _instance = 
            new Lazy<TrainingPillsManager>(() => new TrainingPillsManager());

        private List<string> _knownSources = new List<string>();

        private Dictionary<TrainingLevel, List<LessonInfo>> _trainings = 
            new Dictionary<TrainingLevel, List<LessonInfo>>();

        private List<string> _tags = new List<string>();

        /// <summary>
        /// Returns the unique instance of the Training Pills Manager.
        /// </summary>
        public static TrainingPillsManager Instance => _instance.Value;

        private TrainingPillsManager()
        {
        }

        /// <summary>
        /// Add the Training Pills source related to the Assembly.
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed.</param>
        public void Add([NotNull] Assembly assembly)
        {
            var attribs = assembly.GetCustomAttributes<TrainingPillsAttribute>().ToArray();
            if (attribs.Any())
            {
                foreach (var attrib in attribs)
                    Add(attrib.BaseUrl);
            }
        }

        /// <summary>
        /// Add the Training Pills source from the specified base url.
        /// </summary>
        /// <param name="baseUrl"></param>
        public void Add([Required] string baseUrl)
        {
            if (!_knownSources.Contains(baseUrl))
            {
                lock (_instance)
                {
                    if (!_knownSources.Contains(baseUrl))
                    {
                        _knownSources.Add(baseUrl);
                        ProcessTrainingPills(baseUrl);
                    }
                }
            }
        }

        /// <summary>
        /// Get the list of known tags, ordered alphabetically.
        /// </summary>
        public IEnumerable<string> Tags => _tags.OrderBy(x => x).ToArray();

        /// <summary>
        /// Supported levels.
        /// </summary>
        public IEnumerable<TrainingLevel> SupportedLevels => _trainings.Keys;

        /// <summary>
        /// Get the lessons for a specific level.
        /// </summary>
        /// <param name="level">Level of the training.</param>
        /// <param name="requiredTags">Required tags.</param>
        /// <returns>List of Lessons for the given level and with at least one of the searched tags, if specified.</returns>
        public IEnumerable<LessonInfo> GetLessons(TrainingLevel level, IEnumerable<string> requiredTags = null)
        {
            IEnumerable<LessonInfo> result;

            if (!_trainings.TryGetValue(level, out var trainings))
                trainings = null;

            if ((trainings?.Any() ?? false) && requiredTags != null)
            {
                result = GetLessons(trainings, requiredTags);
            }
            else
                result = trainings;

            return result;
        }

        /// <summary>
        /// Get the lessons.
        /// </summary>
        /// <param name="requiredTags">Required tags.</param>
        /// <returns>List of Lessons with at least one of the searched tags, if specified.</returns>
        public IEnumerable<LessonInfo> GetLessons(IEnumerable<string> requiredTags = null)
        {
            List<LessonInfo> trainings = new List<LessonInfo>();
            var intro = GetLessons(TrainingLevel.Introductive, requiredTags);
            if (intro?.Any() ?? false)
            {
                trainings.AddRange(intro);
            }
            var adv = GetLessons(TrainingLevel.Advanced, requiredTags);
            if (adv?.Any() ?? false)
            {
                trainings.AddRange(adv);
            }
            var exp = GetLessons(TrainingLevel.Expert, requiredTags);
            if (exp?.Any() ?? false)
            {
                trainings.AddRange(exp);
            }

            return trainings;
        }

        private IEnumerable<LessonInfo> GetLessons([NotNull] IEnumerable<LessonInfo> trainings, [NotNull] IEnumerable<string> tags)
        {
            List<LessonInfo> result = new List<LessonInfo>();

            foreach (var item in trainings)
            {
                if (item.HasTag(tags))
                    result.Add(item);
            }

            return result;
        }

        [Background]
        private void ProcessTrainingPills([Required] string baseUrl)
        {
            using (var webClient = new System.Net.WebClient())
            {
                Uri uri = new Uri(baseUrl);

                string json = null;
                try
                {
                    json = webClient.DownloadString(new Uri(uri, "trainingpills.json"));
                }
                catch
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var definition = JsonConvert.DeserializeObject<TrainingsDefinition>(json,
                        new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.None
                        });

                    var sections = definition?.Sections?.ToArray();
                    if (sections?.Any() ?? false)
                    {
                        foreach (var section in sections)
                        {
                            var topics = section.Topics?.ToArray();
                            if (topics?.Any() ?? false)
                            {
                                List<LessonInfo> trainings;
                                if (!_trainings.TryGetValue(section.SectionType, out trainings))
                                {
                                    trainings = new List<LessonInfo>();
                                    _trainings.Add(section.SectionType, trainings);
                                }

                                foreach (var topic in topics)
                                {
                                    var lessons = topic.Lessons?.ToArray();
                                    if (lessons?.Any() ?? false)
                                    {
                                        foreach (var lesson in lessons)
                                        {
                                            trainings.Add(new LessonInfo(baseUrl, section.Prefix, topic.Name, lesson));
                                            var tags = lesson.Tags?.ToArray();
                                            if (tags?.Any() ?? false)
                                            {
                                                foreach (var tag in tags)
                                                {
                                                    if (!_tags.Contains(tag))
                                                        _tags.Add(tag);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
