using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.PackageManagers.Packaging;

namespace ThreatsManager.PackageManagers
{
    public abstract class BaseFilePackageManager : IPackageManager
    {
        protected const string ExtensionsFile = "extensions.json";
        protected string ThreatModelFile = "threatmodel.json";
        protected string Extension = ".undefined";
        protected string PackageType = "Undefined";

        public LocationType SupportedLocations => LocationType.FileSystem;

        public bool CanHandle(LocationType locationType, [Required] string location)
        {
            return SupportedLocations.HasFlag(locationType) &&
                   !location.StartsWith(@"\\") &&
                   string.Compare(Path.GetExtension(location), Extension, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public string GetFilter(LocationType locationType)
        {
            string result = null;

            if (locationType == LocationType.FileSystem)
                result = $"{PackageType} ({Extension})|{Extension}";

            return result;
        }

        public string GetLatest(LocationType locationType, [Required] string location, out DateTime dateTime)
        {
            string result = null;
            dateTime = DateTime.MinValue;

            if (CanHandle(locationType, location))
            {
                var directory = Path.GetDirectoryName(location);
                var filter = string.Concat(Path.GetFileNameWithoutExtension(location), "_??????????????", Extension);

                if (!string.IsNullOrWhiteSpace(directory))
                {
                    result = Directory
                        .GetFiles(directory, filter, SearchOption.TopDirectoryOnly)
                        .OrderByDescending(x => x)
                        .FirstOrDefault();

                    if (result != null)
                        dateTime = GetDateTime(result);
                }
            }

            return result;
        }

        public void AutoCleanup(LocationType locationType, [Required] string location, [StrictlyPositive] int maxInstances)
        {
            if (CanHandle(locationType, location))
            {
                var directory = Path.GetDirectoryName(location);
                var filter = string.Concat(StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location)), "_??????????????", Extension);

                if (!string.IsNullOrWhiteSpace(directory))
                {
                    var files = Directory.GetFiles(directory, filter, SearchOption.TopDirectoryOnly);
                    var orderedFiles = files.OrderByDescending(x => x).Skip(maxInstances);

                    foreach (var file in orderedFiles)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public abstract IThreatModel Load(LocationType locationType, string location, 
            IEnumerable<IExtensionMetadata> extensions, bool strict = true, Guid? newThreatModelId = null);

        public abstract bool Save(IThreatModel model, LocationType locationType, string location, 
            bool autoAddDateTime, IEnumerable<IExtensionMetadata> extensions, out string newLocation);

        #region Protected member functions.
        protected string StripDateTimeSuffix([Required] string text)
        {
            string result = text;

            Regex regex = new Regex("(_[0-9]{14})");
            var match = regex.Match(text);
            if (match.Success)
            {
                var capture = match.Captures.OfType<Capture>().FirstOrDefault();
                if (capture != null)
                {
                    result = text.Replace(capture.Value, "");
                }
            }

            return result;
        }

        protected DateTime GetDateTime([Required] string text)
        {
            DateTime result = DateTime.MinValue;

            Regex regex = new Regex("(_[0-9]{14})");
            var match = regex.Match(text);
            if (match.Success)
            {
                var capture = match.Captures.OfType<Capture>().FirstOrDefault();
                if (capture != null)
                {
                    result = DateTime.ParseExact(capture.Value.Substring(1), "yyyyMMddHHmmss",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            return result;
        }

        protected void HandleJsonSerializationException([NotNull] Package package,
            IEnumerable<IExtensionMetadata> extensions,
            [NotNull] JsonSerializationException e)
        {
            byte[] extensionsContent = null;

            try
            {
                extensionsContent = package.Read(ExtensionsFile);

            }
            catch
            {
            }

            if (extensionsContent?.Any() ?? false)
            {
                var storedExtensions = JsonConvert.DeserializeObject<ExtensionsList>(
                    Encoding.Unicode.GetString(extensionsContent), new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.None,
                    });

                var currentExtensions = extensions?.Select(x => new ExtensionInfo(x.Id, x.Label)).ToArray();

                if (currentExtensions?.Any() ?? false)
                {
                    var missingExtensions =
                        storedExtensions.Extensions.Except(currentExtensions, new ExtensionInfoComparer())
                            .ToArray();

                    if (missingExtensions.Any())
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine(
                            "A serialization issue has occurred, probably due to one or more of those missing Extensions:");
                        foreach (var ext in missingExtensions)
                        {
                            builder.AppendLine($"- {ext.Label}");
                        }

                        throw new ThreatModelOpeningFailureException(builder.ToString(), e);
                    }

                    throw new ThreatModelOpeningFailureException("A serialization issue has occurred.", e);
                }
            }
        }
        #endregion
    }
}
