using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using ThreatsManager.Interfaces;
using ThreatsManager.Packaging;
using ThreatsManager.Interfaces.Exceptions;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.PackageManagers
{
    [Extension("C3E6420A-296D-4859-99BD-6045D45A20E3", "Encrypted File Package Manager", 11, ExecutionMode.Simplified)]
    public class EncryptedFilePackageManager : ISecurePackageManager
    {
        private const string ThreatModelFile = "threatmodel.json";

        private IProtectionData _protectionData;

        public LocationType SupportedLocations => LocationType.FileSystem;

        public ProtectionType RequiredProtection => ProtectionType.Password;

        public string GetFilter(LocationType locationType)
        {
            return "Secured Threat Model (*.tmx)|*.tmx";
        }

        public bool CanHandle(LocationType locationType, string location)
        {
            return SupportedLocations.HasFlag(locationType) &&
                    string.Compare(Path.GetExtension(location), ".tmx", StringComparison.OrdinalIgnoreCase) == 0;
        }

        public IThreatModel Load(LocationType locationType, string location)
        {
            throw new EncryptionRequiredException(RequiredProtection);
        }

        public IThreatModel Load(LocationType locationType, string location, SecureString password)
        {
            IThreatModel result = null;

            if (File.Exists(location))
            {
                var package = new Package(location);

                var threatModelContent = package.Read(ThreatModelFile, password);
                if (threatModelContent != null)
                {
                    result = ThreatModelManager.Deserialize(threatModelContent);
                }
            }
            else
            {
                throw new FileNotFoundException();
            }

            return result;
        }

        public IThreatModel Load(LocationType locationType, string location, X509Certificate2 certificate)
        {
            throw new UnsupportedEncryptionException();
        }

        public bool Save(IThreatModel model, LocationType locationType, string location)
        {
            throw new EncryptionRequiredException(RequiredProtection);
        }

        public bool Save(IThreatModel model, LocationType locationType, string location, SecureString password)
        {
            bool result = false;

            if (model is IThreatModel tm)
            {
                var tmSerialized = ThreatModelManager.Serialize(tm);

                var package = Package.Create(location);
                package.Add(ThreatModelFile, tmSerialized, password);
                package.Save();

                result = true;
            }

            return result;
        }

        public bool Save(IThreatModel model, LocationType locationType, string location, IEnumerable<X509Certificate2> certificates)
        {
            throw new UnsupportedEncryptionException();
        }
    }
}
