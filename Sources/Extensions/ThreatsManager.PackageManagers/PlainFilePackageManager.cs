using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ThreatsManager.Packaging;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.PackageManagers
{
    [Extension("84252804-27F2-46C0-91A7-8EB7BF57EE58", "File Package Manager", 10, ExecutionMode.Business)]
    public class PlainFilePackageManager : IPackageManager 
    {
        private const string ThreatModelFile = "threatmodel.json";
        private const string ExtensionsFile = "extensions.json";

        public LocationType SupportedLocations => LocationType.FileSystem;

        public bool CanHandle(LocationType locationType, [Required] string location)
        {
            return SupportedLocations.HasFlag(locationType) &&
                   !location.StartsWith(@"\\") &&
                   string.Compare(Path.GetExtension(location), ".tm", StringComparison.OrdinalIgnoreCase) == 0;
        }

        public string GetFilter(LocationType locationType)
        {
            return "Threat Model (*.tm)|*.tm";
        }

        public string GetLatest(LocationType locationType, [Required] string location, out DateTime dateTime)
        {
            string result = null;
            dateTime = DateTime.MinValue;

            if (CanHandle(locationType, location))
            {
                var directory = Path.GetDirectoryName(location);
                var filter = string.Concat(Path.GetFileNameWithoutExtension(location), "_??????????????", ".tm");

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


        public IThreatModel Load(LocationType locationType, [Required] string location, 
            IEnumerable<IExtensionMetadata> extensions, bool strict = true)
        {
            IThreatModel result = null;

            if (File.Exists(location))
            {
                var package = new Package(location);

                var threatModelContent = package.Read(ThreatModelFile);
                if (threatModelContent != null)
                {
                    try
                    {
                        result = ThreatModelManager.Deserialize(threatModelContent, !strict);
                    }
                    catch (JsonSerializationException e)
                    {
                        HandleJsonSerializationException(package, extensions, e);
                        throw;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Unable to find the specified file.", location);
            }

            return result;
        }

        public bool Save([NotNull] IThreatModel model, LocationType locationType, [Required] string location, 
            bool autoAddDateTime, IEnumerable<IExtensionMetadata> extensions, out string newLocation)
        {
            bool result = false;
            newLocation = null;

            if (model is IThreatModel tm)
            {
                var tmSerialized = ThreatModelManager.Serialize(tm);
                var extList = new ExtensionsList
                {
                    Extensions =
                        new List<ExtensionInfo>(extensions.Where(x => x != null).Select(x => new ExtensionInfo(x.Id, x.Label)))
                };
                var extSerialized = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(extList, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.None,
                }));

                newLocation = autoAddDateTime
                    ? Path.Combine(Path.GetDirectoryName(location),
                        $"{StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location))}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(location)}")
                    : location;

                var package = Package.Create(newLocation);
                package.Add(ThreatModelFile, tmSerialized);
                package.Add(ExtensionsFile, extSerialized);
                package.Save();

                if (autoAddDateTime)
                    File.Copy(newLocation, Path.Combine(Path.GetDirectoryName(location), $"{StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location))}{Path.GetExtension(location)}"), true);

                result = true;
            }

            return result;
        }

        public void AutoCleanup(LocationType locationType, [Required] string location, [StrictlyPositive] int maxInstances)
        {
            if (CanHandle(locationType, location))
            {
                var directory = Path.GetDirectoryName(location);
                var filter = string.Concat(StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location)), "_??????????????", ".tm");

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

        private void HandleJsonSerializationException([NotNull] Package package, 
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

        private string StripDateTimeSuffix([Required] string text)
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

        private DateTime GetDateTime([Required] string text)
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
    }
}
