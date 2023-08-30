﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.PackageManagers.Packaging;

namespace ThreatsManager.PackageManagers
{
    [Extension("84252804-27F2-46C0-91A7-8EB7BF57EE58", "File Package Manager", 10, ExecutionMode.Business)]
    public class PlainFilePackageManager : BaseFilePackageManager, IPackageManager
    {
        public PlainFilePackageManager() 
        { 
            Extension = ".tm";
            PackageType = "Threat Model";
        }

        public LocationType SupportedLocations => BaseSupportedLocations;

        public void AutoCleanup(LocationType locationType, string location, int maxInstances)
        {
            BaseAutoCleanup(locationType, location, maxInstances);
        }

        public bool CanHandle(LocationType locationType, string location)
        {
            return BaseCanHandle(locationType, location);
        }

        public string GetFilter(LocationType locationType)
        {
            return BaseGetFilter(locationType);
        }

        public string GetLatest(LocationType locationType, string location, out DateTime dateTime)
        {
            return BaseGetLatest(locationType, location, out dateTime);
        }

        public IThreatModel Load(LocationType locationType, [Required] string location, 
            IEnumerable<IExtensionMetadata> extensions, bool strict = true,
            Guid? newThreatModelId = null)
        {
            IThreatModel result = null;

            if (locationType == LocationType.FileSystem && File.Exists(location))
            {
                if (Package.IsEncrypted(location))
                {
                    throw new FileEncryptedException(location);
                }
                else
                {
                    var package = new Package(location);

                    var threatModelContent = package.Read(ThreatModelFile);
                    if (threatModelContent != null)
                    {
                        try
                        {
                            result = ThreatModelManager.Deserialize(threatModelContent, !strict, newThreatModelId);
                        }
                        catch (JsonSerializationException e)
                        {
                            HandleJsonSerializationException(package, extensions, e);
                            throw;
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Unable to find the specified file.", location);
            }

            return result;
        }

        public bool Save([NotNull] IThreatModel model, LocationType locationType, 
            [Required] string location, bool autoAddDateTime, IEnumerable<IExtensionMetadata> extensions, 
            out string newLocation)
        {
            bool result = false;
            newLocation = null;

            if (locationType == LocationType.FileSystem && model is IThreatModel tm)
            {
                var tmSerialized = ThreatModelManager.Serialize(tm);

                newLocation = autoAddDateTime
                    ? Path.Combine(Path.GetDirectoryName(location),
                        $"{StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location))}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(location)}")
                    : location;

                var package = Package.Create(newLocation);
                package.Add(ThreatModelFile, tmSerialized);

                if (extensions?.Any() ?? false)
                {
                    var extList = new ExtensionsList
                    {
                        Extensions =
                            new List<ExtensionInfo>(extensions.Where(x => x != null).Select(x => new ExtensionInfo(x.Id, x.Label)))
                    };
                    var extSerialized = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(extList, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.None,
                    }));

                    package.Add(ExtensionsFile, extSerialized);
                }

                package.Save();

                if (autoAddDateTime)
                    File.Copy(newLocation, Path.Combine(Path.GetDirectoryName(location), $"{StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location))}{Path.GetExtension(location)}"), true);

                result = true;
            }

            return result;
        }
    }
}
