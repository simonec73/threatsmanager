using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.PackageManagers
{
    [Extension("FA6F6023-8369-4D2F-97C1-1EB5ED83DA21", "Json File Package Manager", 15, ExecutionMode.Business)]
    public class JsonFilePackageManager : IPackageManager 
    { 
        public LocationType SupportedLocations => LocationType.FileSystem;

        public bool CanHandle(LocationType locationType, [Required] string location)
        {
            return SupportedLocations.HasFlag(locationType) &&
                   !location.StartsWith(@"\\") &&
                   string.Compare(Path.GetExtension(location), ".tmj", StringComparison.OrdinalIgnoreCase) == 0;
        }

        public string GetFilter(LocationType locationType)
        {
            return "Json Threat Model (*.tmj)|*.tmj";
        }

        public string GetLatest(LocationType locationType, [Required] string location, out DateTime dateTime)
        {
            string result = null;
            dateTime = DateTime.MinValue;

            if (CanHandle(locationType, location))
            {
                var directory = Path.GetDirectoryName(location);
                var filter = string.Concat(Path.GetFileNameWithoutExtension(location), "_??????????????", ".tmj");

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
                var threatModelContent = File.ReadAllBytes(location);
                if (threatModelContent != null)
                {
                    try
                    {
                        result = ThreatModelManager.Deserialize(threatModelContent, !strict);
                    }
                    catch (JsonSerializationException e)
                    {
                        throw new ThreatModelOpeningFailureException("A serialization issue has occurred.", e);
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

                newLocation = autoAddDateTime
                    ? Path.Combine(Path.GetDirectoryName(location),
                        $"{StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location))}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(location)}")
                    : location;

                File.WriteAllBytes(newLocation, tmSerialized);

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
