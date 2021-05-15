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
    /// Troubleshooting Manager.
    /// </summary>
    /// <remarks>Implements the Singleton pattern.</remarks>
    public sealed class TroubleshootingManager
    {
        private static readonly Lazy<TroubleshootingManager> _instance = 
            new Lazy<TroubleshootingManager>(() => new TroubleshootingManager());

        private readonly Dictionary<Priority, List<string>> _knownSources = new Dictionary<Priority, List<string>>();

        private readonly List<Page> _pages = new List<Page>();

        /// <summary>
        /// Returns the unique instance of the Troubleshooting Manager.
        /// </summary>
        public static TroubleshootingManager Instance => _instance.Value;

        private TroubleshootingManager()
        {
        }

        /// <summary>
        /// Enumeration of the Troubleshooting pages.
        /// </summary>
        public IEnumerable<Page> Pages => _pages;

        /// <summary>
        /// Add the Troubleshooting source related to the Assembly.
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed.</param>
        /// <remarks>It should be used only by class ExtensionsManager from ThreatsManager.Engine.</remarks>
        [SuppressMessage("ReSharper", "IdentifierTypo")]
        public void Add([PostSharp.Patterns.Contracts.NotNull] Assembly assembly)
        {
            var attrib = assembly.GetCustomAttributes<TroubleshootingSourceAttribute>().FirstOrDefault();
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
                    TroubleshootingDefinition definition = null;

                    try
                    {
                        using (var textReader = new StringReader(json))
                        using (var reader = new JsonTextReader(textReader))
                        {
                            var serializer = new JsonSerializer
                            {
                                TypeNameHandling = TypeNameHandling.None
                            };
                            definition = serializer.Deserialize<TroubleshootingDefinition>(reader);
                        }
                    }
                    catch (JsonReaderException)
                    {
                        // The file cannot be parsed. Let's simply ignore it.
                        return;
                    }

                    var pages = definition?.Pages?.ToArray();
                    if (pages?.Any() ?? false)
                    {
                        _pages.AddRange(pages);
                    }
                }
            }
        }
    }
}
