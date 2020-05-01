using System.Configuration;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Engine.Config
{
    public class KnownDocumentConfig : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true, IsKey = true)]
        public string Path
        {
            get => (string) this["path"];
            set => this["path"] = value;
        }

        [ConfigurationProperty("locationType", IsRequired = true, IsKey = false)]
        public LocationType LocationType
        {
            get => (LocationType) this["locationType"];
            set => this["locationType"] = value;
        }

        [ConfigurationProperty("packageManager", IsRequired = true, IsKey = false)]
        public string PackageManager
        {
            get => (string) this["packageManager"];
            set => this["packageManager"] = value;
        }
    }

}
