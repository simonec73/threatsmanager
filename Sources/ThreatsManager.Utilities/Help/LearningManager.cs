using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Reflection;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Learning Manager.
    /// </summary>
    /// <remarks>Implements the Singleton pattern.</remarks>
    public sealed class LearningManager
    {
        private static readonly Lazy<LearningManager> _instance = 
            new Lazy<LearningManager>(() => new LearningManager());

        private readonly Dictionary<Priority, List<string>> _knownSources = new Dictionary<Priority, List<string>>();

        private readonly Dictionary<LearningLevel, List<TopicInfo>> _learning = 
            new Dictionary<LearningLevel, List<TopicInfo>>();

        /// <summary>
        /// Returns the unique instance of the Learning Manager.
        /// </summary>
        public static LearningManager Instance => _instance.Value;

        private LearningManager()
        {
        }

        /// <summary>
        /// Add the Learning source related to the Assembly.
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed.</param>
        /// <remarks>It should be used only by class ExtensionsManager from ThreatsManager.Engine.</remarks>
        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public void Add([PostSharp.Patterns.Contracts.NotNull] Assembly assembly)
        {
            var attrib = assembly.GetCustomAttributes<LearningSourceAttribute>().FirstOrDefault();
            if (attrib != null)
            {
                var priority = attrib.Priority;
                List<string> sources;
                if (_knownSources.ContainsKey(priority))
                {
                    sources = _knownSources[priority];
                }
                else
                {
                    sources = new List<string>();
                    _knownSources.Add(priority, sources);
                }

                sources.Add(attrib.Url);
            }
        }

        /// <summary>
        /// Analyze all the sources.
        /// </summary>
        /// <remarks>It should be used only by class ExtensionsManager from ThreatsManager.Engine.</remarks>
        public void AnalyzeSources()
        {
            if (_knownSources.Any())
            {
                var sourcesList = _knownSources
                    .OrderByDescending(x => x.Key)
                    .Select(x => x.Value)
                    .ToArray();

                foreach (var sources in sourcesList)
                {
                    if (sources.Any())
                    {
                        foreach (var source in sources)
                        {
                            ProcessSource(source);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Supported levels.
        /// </summary>
        public IEnumerable<LearningLevel> SupportedLevels => _learning.Keys;

        /// <summary>
        /// Get the Topics for a specific level.
        /// </summary>
        /// <param name="level">Learning Level.</param>
        /// <returns>List of Topics for the given level.</returns>
        public IEnumerable<TopicInfo> GetTopics(LearningLevel level)
        {
            IEnumerable<TopicInfo> result;
            if (_learning.TryGetValue(level, out var topics))
            {
                result = topics;
            }
            else
            {
                result = null;
            }

            return result;
        }

        private void ProcessSource([Required] string url)
        {
            using (var webClient = new System.Net.WebClient())
            {
                string json;
                try
                {
                    webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    json = webClient.DownloadString(new Uri(url));
                }
                catch
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(json))
                {
                    LearningDefinition definition = null;

                    try
                    {
                        using (var textReader = new StringReader(json))
                        using (var reader = new JsonTextReader(textReader))
                        {
                            var serializer = new JsonSerializer
                            {
                                TypeNameHandling = TypeNameHandling.None
                            };
                            definition = serializer.Deserialize<LearningDefinition>(reader);
                        }
                    }
                    catch (JsonReaderException)
                    {
                        // The file cannot be parsed. Let's simply ignore it.
                        return;
                    }

                    var sections = definition?.Sections?.ToArray();
                    if (sections?.Any() ?? false)
                    {
                        foreach (var section in sections)
                        {
                            var topics = section.Topics?.ToArray();
                            if (topics?.Any() ?? false)
                            {
                                if (!_learning.TryGetValue(section.SectionType, out var topicInfos))
                                {
                                    topicInfos = new List<TopicInfo>();
                                    _learning.Add(section.SectionType, topicInfos);
                                }

                                foreach (var topic in topics)
                                {
                                    var topicInfo = topicInfos.FirstOrDefault(x =>
                                        string.CompareOrdinal(topic.Name, x.Name) == 0);
                                    if (topicInfo == null)
                                    {
                                        topicInfo = new TopicInfo(topic.Name);
                                        topicInfos.Add(topicInfo);
                                    }

                                    var lessons = topic.Lessons?.ToArray();
                                    if (lessons?.Any() ?? false)
                                    {
                                        foreach (var lesson in lessons)
                                        {
                                            topicInfo.AddLesson(lesson);
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
