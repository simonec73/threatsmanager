using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class ExtensionConfig : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public string Id
        {
            get => (string)this["id"];
            set => this["id"] = value;
        }

        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
        public bool Enabled
        {
            get => (bool)this["enabled"];
            set => this["enabled"] = value;
        }

        [ConfigurationProperty("properties", IsDefaultCollection = false)]
        public PropertyConfigurationCollection Parameters => (PropertyConfigurationCollection) base["properties"];
    }
}
