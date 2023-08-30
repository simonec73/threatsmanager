using System;
using System.Collections.Generic;
using System.IO;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System.Text;
using ThreatsManager.Utilities.Aspects;
using System.Security.Cryptography;
using ThreatsManager.PackageManagers.Packaging;

namespace ThreatsManager.PackageManagers
{
    [Extension("C3E6420A-296D-4859-99BD-6045D45A20E3", "Password Encrypted File Package Manager", 11, ExecutionMode.Business)]
    public class PasswordEncryptedFilePackageManager : BaseFilePackageManager, ISecurePackageManager, IInitializableObject
    {
        private IPasswordProtectionData _protectionData;

        public PasswordEncryptedFilePackageManager() 
        {
            Extension = ".tme";
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

        public ProtectionType RequiredProtection => ProtectionType.Password;

        public bool IsInitialized => _protectionData != null;

        public void SetProtectionData([NotNull] IProtectionData protectionData)
        {
            if (protectionData is IPasswordProtectionData passwordProtectionData)
            {
                _protectionData = passwordProtectionData;
            }
            else
            {
                throw new UnsupportedEncryptionException();
            }
        }

        [InitializationRequired]
        public IThreatModel Load(LocationType locationType, [Required] string location,
            IEnumerable<IExtensionMetadata> extensions, bool strict = true, Guid? newThreatModelId = null)
        {
            IThreatModel result = null;

            if (locationType == LocationType.FileSystem && File.Exists(location))
            {
                if (Package.IsEncrypted(location))
                {
                    var package = new Package(location);

                    var threatModelContent = package.Read(ThreatModelFile, _protectionData.Password);
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
                else
                {
                    throw new FileNotEncryptedException(location);
                }
            }
            else
            {
                throw new FileNotFoundException("Unable to find the specified file.", location);
            }

            return result;
        }

        [InitializationRequired]
        public bool Save([NotNull] IThreatModel model, LocationType locationType, 
            [Required] string location, bool autoAddDateTime, IEnumerable<IExtensionMetadata> extensions, 
            out string newLocation)
        {
            bool result = false;
            newLocation = null;

            if (locationType == LocationType.FileSystem && model is IThreatModel tm)
            {
                var tmSerialized = ThreatModelManager.Serialize(tm);

                var alg = _protectionData.Algorithm ?? "AES256";
                var hmac = _protectionData.HMAC ?? "HMACSHA256";
                byte[] salt = _protectionData.Salt;
                if (!(salt?.Any() ?? false))
                {
                    salt = new byte[128 / 8];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }
                }
                int iterations = 20000;
                if (_protectionData.Iterations > 0)
                    iterations = _protectionData.Iterations;

                newLocation = autoAddDateTime
                    ? Path.Combine(Path.GetDirectoryName(location),
                        $"{StripDateTimeSuffix(Path.GetFileNameWithoutExtension(location))}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(location)}")
                    : location;

                var package = Package.Create(newLocation, alg, hmac, salt, iterations);
                package.Add(ThreatModelFile, tmSerialized, _protectionData.Password);

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
