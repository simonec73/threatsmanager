using System.Collections.Generic;
using System.Configuration;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Config
{
    public class ThreatsManagerConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("updated", DefaultValue = false, IsRequired = false)]
        public bool Updated
        {
            get => (bool)this["updated"];
            set => this["updated"] = value;
        }

        [ConfigurationProperty("setup", DefaultValue = false, IsRequired = false)]
        public bool Setup
        {
            get => (bool)this["setup"];
            set => this["setup"] = value;
        }

        [ConfigurationProperty("mode", DefaultValue = ExecutionMode.Expert, IsRequired = false)]
        public ExecutionMode Mode
        {
            get => (ExecutionMode) this["mode"];
            set => this["mode"] = value;
        }

        [ConfigurationProperty("overrideThreatEvents", DefaultValue = false, IsRequired = false)]
        public bool OverrideThreatEventsInfo
        {
            get => (bool) this["overrideThreatEvents"];
            set => this["overrideThreatEvents"] = value;
        }

        [ConfigurationProperty("knownDocuments", IsDefaultCollection = false)]
        public KnownDocumentConfigCollection KnownDocuments
        {
            get
            {
                KnownDocumentConfigCollection collection =
                    (KnownDocumentConfigCollection) base["knownDocuments"];
                return collection;
            }
        }

        [ConfigurationProperty("extensions", IsDefaultCollection = false)]
        public ExtensionConfigCollection Extensions
        {
            get
            {
                ExtensionConfigCollection collection =
                    (ExtensionConfigCollection) base["extensions"];
                return collection;
            }
        }

        [ConfigurationProperty("folders", IsDefaultCollection = false)]
        public FolderConfigCollection Folders
        {
            get
            {
                FolderConfigCollection collection =
                    (FolderConfigCollection) base["folders"];
                return collection;
            }
        }

        [ConfigurationProperty("prefixes", IsDefaultCollection = false)]
        public PrefixConfigCollection Prefixes
        {
            get
            {
                PrefixConfigCollection collection =
                    (PrefixConfigCollection) base["prefixes"];
                return collection;
            }
        }

        [ConfigurationProperty("certificates", IsDefaultCollection = false)]
        public CertificateConfigCollection Certificates
        {
            get
            {
                CertificateConfigCollection collection =
                    (CertificateConfigCollection) base["certificates"];
                return collection;
            }
        }

        [ConfigurationProperty("smartSave")]
        public bool SmartSave
        {
            get => (bool) base["smartSave"];
            set => base["smartSave"] = value;
        }

        [ConfigurationProperty("smartSaveCount")]
        public int SmartSaveCount
        {
            get => (int) base["smartSaveCount"];
            set => base["smartSaveCount"] = value;
        }

        [ConfigurationProperty("smartSaveInterval")]
        public int SmartSaveInterval
        {
            get => (int) base["smartSaveInterval"];
            set => base["smartSaveInterval"] = value;
        }

        [ConfigurationProperty("dictionary")]
        public string UserDictionary
        {
            get => (string) base["dictionary"];
            set => base["dictionary"] = value;
        }

        [ConfigurationProperty("statusBar")]
        private string StatusBar
        {
            get => (string)base["statusBar"];
            set => base["statusBar"] = value;
        }

        public IEnumerable<string> StatusBarItems
        {
            get => StatusBar?.TagSplit();
            set => StatusBar = value?.TagConcat();
        }

        [ConfigurationProperty("extensionsConfigFolder")]
        public string ExtensionsConfigFolder
        {
            get => (string) base["extensionsConfigFolder"];
            set => base["extensionsConfigFolder"] = value;
        }

        [ConfigurationProperty("disableHelp")]
        public bool DisableHelp
        {
            get => (bool) base["disableHelp"];
            set => base["disableHelp"] = value;
        }
    }
}
