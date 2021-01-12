using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Engine.Config
{
    [XmlRoot("configuration")]
    public class OldUserConfig
    {
        [XmlElement("threatsManager")]
        public ThreatsManagerConfig ThreatsManagerConfig { get; set; }
    }

    public class ThreatsManagerConfig
    {
        public ThreatsManagerConfig()
        {
            this.InitializePropertyDefaultValues();
        }

        [XmlAttribute("setup")]
        public bool Setup { get; set; }

        [XmlAttribute("mode")]
        public ExecutionMode Mode { get; set; }

        [XmlAttribute("overrideThreatEvents")]
        public bool OverrideThreatEventsInfo { get; set; }

        [XmlArray("knownDocuments")]
        [XmlArrayItem("doc")]
        public List<KnownDocumentInfo> KnownDocuments { get; set; }

        [XmlArray("extensions")]
        [XmlArrayItem("add")]
        public List<ExtensionInfo> Extensions { get; set; }

        [XmlArray("folders")]
        [XmlArrayItem("folder")]
        public List<NameInfo> Folders { get; set; }

        [XmlArray("prefixes")]
        [XmlArrayItem("prefix")]
        public List<NameInfo> Prefixes { get; set; }

        [XmlArray("certificates")]
        [XmlArrayItem("certificate")]
        public List<CertificateInfo> Certificates { get; set; }

        [XmlAttribute("smartSave"), DefaultValue(false)]
        public bool SmartSave { get; set; }

        [XmlAttribute("smartSaveCount"), DefaultValue(0)]
        public int SmartSaveCount { get; set; }

        [XmlAttribute("smartSaveInterval"), DefaultValue(0)]
        public int SmartSaveInterval { get; set; }

        [XmlAttribute("dictionary"), DefaultValue(null)]
        public string UserDictionary { get; set; }

        [XmlAttribute("statusBar"), DefaultValue(null)]
        public string StatusBar { get; set; }

        [XmlAttribute("extensionsConfigFolder"), DefaultValue(null)]
        public string ExtensionsConfigFolder { get; set; }

        [XmlAttribute("disableHelp"), DefaultValue(false)]
        public bool DisableHelp { get; set; }
    }

    public class KnownDocumentInfo
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("locationType")]
        public LocationType LocationType { get; set; }

        [XmlAttribute("packageManager")]
        public string PackageManager { get; set; }
    }

    public class ExtensionInfo
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("add")]
        public List<NameValueInfo> Parameters { get; set; }
    }

    public class NameValueInfo
    { 
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class NameInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public class CertificateInfo
    {
        [XmlAttribute("thumbprint")]
        public string Thumbprint { get; set; }

        [XmlAttribute("subject")]
        public string Subject { get; set; }

        [XmlAttribute("issuer")]
        public string Issuer { get; set; }

        [XmlAttribute("expiration")]
        public string ExpirationDate { get; set; }
    }
}
