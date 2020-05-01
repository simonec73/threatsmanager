using System.Configuration;

namespace ThreatsManager.Engine.Config
{
    public class PropertyConfigElement : ConfigurationElement
    {
        public PropertyConfigElement()
        {
        }

        public PropertyConfigElement(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true, DefaultValue = null)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}