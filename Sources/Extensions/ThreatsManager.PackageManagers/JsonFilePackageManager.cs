using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.PackageManagers
{
    [Extension("FA6F6023-8369-4D2F-97C1-1EB5ED83DA21", "Json File Package Manager", 15, ExecutionMode.Business)]
    public class JsonFilePackageManager : BaseFilePackageManager
    { 
        public JsonFilePackageManager() 
        {
            Extension = ".tmj";
            PackageType = "Json Threat Model";
        }

        public override IThreatModel Load(LocationType locationType, [Required] string location, 
            IEnumerable<IExtensionMetadata> extensions, bool strict = true,
            Guid? newThreatModelId = null)
        {
            IThreatModel result = null;

            if (File.Exists(location))
            {
                var threatModelContent = File.ReadAllBytes(location);
                if (threatModelContent != null)
                {
                    try
                    {
                        result = ThreatModelManager.Deserialize(threatModelContent, !strict, newThreatModelId);
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

        public override bool Save([NotNull] IThreatModel model, LocationType locationType, [Required] string location, 
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
    }
}
