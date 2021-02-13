using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Config;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine
{
    /// <summary>
    /// Configuration manager for the Extensions.
    /// </summary>
    /// <remarks>It protects the secrets at rest, by encrypting them using DPAPI.
    /// This is not overly secure, but the intent is only to provide a limited protection.</remarks>
    public class ExtensionsConfigurationManager
    {
        private ExtensionsManager _extensionsManager;
        private IEnumerable<string> _extensions;
        private readonly List<string> _disabledExtensions = new List<string>();
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        private readonly List<string> _additionalPaths = new List<string>();
        private readonly List<string> _prefixes = new List<string>();
        private readonly List<string> _certificateThumbprints = new List<string>();

        private static ConfigurationUserLevel _configurationUserLevel = ConfigurationUserLevel.PerUserRoamingAndLocal;

        private const string Entropy =
            "3gO2vG/wfDG9YyT3VWx4RWDwpXSOSHWBynsMvrHp31UYpF0mmDbpi8HALtKPP5jCYXVne4XjvvEP95LSvRO3AA==";

        internal ExtensionsConfigurationManager()
        {
        }

        public static void SetConfigurationUserLevel(ConfigurationUserLevel configurationUserLevel)
        {
            _configurationUserLevel = configurationUserLevel;
        }

        internal void Initialize([NotNull] ExtensionsManager extensionsManager)
        {
            _extensionsManager = extensionsManager;
            _extensions = extensionsManager.GetExtensionIds();
            GetExtensionsConfiguration();
        }

        public IEnumerable<string> Extensions => _extensions;
        public IEnumerable<string> EnabledExtensions => _extensions?.Where(x => !_disabledExtensions.Contains(x));

        #region Enable & Disable.
        public void Enable([Required] string id)
        {
            if ((_extensions?.Contains(id) ?? false) && _disabledExtensions.Contains(id))
            {
                _disabledExtensions.Remove(id);

                var configSection = GetConfigurationSection();
                if (configSection?.Extensions != null)
                {
                    var extensionConfigs = configSection.Extensions.OfType<ExtensionConfig>();
                    if (extensionConfigs?.Any(x => string.CompareOrdinal(x.Id, id) == 0) ?? false)
                    {
                        var extensionConfig =
                            extensionConfigs.FirstOrDefault(x => string.CompareOrdinal(x.Id, id) == 0);
                        if (extensionConfig != null)
                            extensionConfig.Enabled = true;
                    }
                    else
                    {
                        var extensionConfig = new ExtensionConfig()
                        {
                            Id = id,
                            Enabled = true
                        };
                        configSection.Extensions.Add(extensionConfig);
                    }

                    configSection.CurrentConfiguration.Save();
                }
            }
        }

        public void Disable([Required] string id)
        {
            if ((_extensions?.Contains(id) ?? false) && !_disabledExtensions.Contains(id))
            {
                _disabledExtensions.Add(id);

                var configSection = GetConfigurationSection();
                if (configSection?.Extensions != null)
                {
                    var extensionConfigs = configSection.Extensions.OfType<ExtensionConfig>();
                    if (extensionConfigs?.Any(x => string.CompareOrdinal(x.Id, id) == 0) ?? false)
                    {
                        var extensionConfig =
                            extensionConfigs.FirstOrDefault(x => string.CompareOrdinal(x.Id, id) == 0);
                        if (extensionConfig != null)
                            extensionConfig.Enabled = false;
                    }
                    else
                    {
                        var extensionConfig = new ExtensionConfig()
                        {
                            Id = id,
                            Enabled = false
                        };
                        configSection.Extensions.Add(extensionConfig);
                    }

                    configSection.CurrentConfiguration.Save();
                }
            }
        }

        public string GetExtensionName([Required] string id)
        {
            return _extensionsManager.GetExtensionName(id);
        }

        public string GetExtensionType([Required] string id)
        {
            return _extensionsManager.GetExtensionType(id);
        }

        public bool IsEnabled([Required] string id)
        {
            return (_extensions?.Contains(id) ?? false) && !_disabledExtensions.Contains(id);
        }

        public string GetExtensionAssemblyTitle([Required] string id)
        {
            return _extensionsManager.GetExtensionAssemblyTitle(id);
        }
        #endregion
        
        #region Extensions configuration.
        public IEnumerable<string> GetParameterNames([Required] string id)
        {
            return _extensionsManager.GetExtensionParameters(id);
        }

        public void SetParameterValue([Required] string id, [Required] string name, string value)
        {
            var parameters = GetParameterNames(id);
            if (parameters?.Contains(name) ?? false)
            {
                var key = $"{id}_{name}";
                if (!_values.ContainsKey(key) || string.CompareOrdinal(_values[key], value) != 0)
                {
                    _values[key] = value;

                    var configSection = GetConfigurationSection();
                    if (configSection?.Extensions != null)
                    {
                        var extensionConfigs = configSection.Extensions.OfType<ExtensionConfig>();
                        var encryptedValue = Encrypt(value);
                        if (extensionConfigs?.Any(x => string.CompareOrdinal(x.Id, id) == 0) ?? false)
                        {
                            var extensionConfig =
                                extensionConfigs.FirstOrDefault(x => string.CompareOrdinal(x.Id, id) == 0);
                            if (extensionConfig?.Parameters.IndexOf(name) >= 0)
                            {
                                var parameter = extensionConfig.Parameters[name];
                                parameter.Value = encryptedValue;
                            }
                            else
                            {
                                extensionConfig?.Parameters.Add(new PropertyConfigElement(name, encryptedValue));
                            }
                        }
                        else
                        {
                            var extensionConfig = new ExtensionConfig()
                            {
                                Id = id,
                                Enabled = true,
                            };
                            extensionConfig.Parameters.Add(new PropertyConfigElement(name, encryptedValue));
                            configSection.Extensions.Add(extensionConfig);
                        }

                        configSection.CurrentConfiguration.Save();
                    }
                }
            }
        }

        public string GetParameterValue([Required] string id, [Required] string name)
        {
            string result = null;

            var parameters = GetParameterNames(id);
            if ((parameters?.Contains(name) ?? false) && _values.ContainsKey($"{id}_{name}"))
            {
                result = _values[$"{id}_{name}"];
            }

            return result;
        }

        public IDictionary<string, string> GetParameters([Required] string id)
        {
            Dictionary<string, string> result = null;

            var parameters = GetParameterNames(id);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var parameterName = $"{id}_{parameter}";
                    if (_values.ContainsKey(parameterName))
                    {
                        if (result == null)
                            result = new Dictionary<string, string>();

                        result.Add(parameter, _values[parameterName]);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Additional extensions folders configuration.
        public IEnumerable<string> Folders
        {
            get
            {
                var config = GetConfigurationSection();
                return config.Folders.OfType<FolderConfig>().Select(x => x.Name);
            }

            set
            {
                var configSection = GetConfigurationSection();
                if (configSection?.Folders != null)
                {
                    configSection.Folders.Clear();

                    if (value?.Any() ?? false)
                    {
                        foreach (var item in value)
                        {
                            var folderConfig = new FolderConfig()
                            {
                                Name = item
                            };
                            configSection.Folders.Add(folderConfig);
                        }
                    }

                    configSection.CurrentConfiguration.Save();
                }
            }
        }

        public IEnumerable<string> Prefixes
        {
            get
            {
                var config = GetConfigurationSection();
                return config.Prefixes.OfType<PrefixConfig>().Select(x => x.Name);
            }

            set
            {
                var configSection = GetConfigurationSection();
                if (configSection?.Prefixes != null)
                {
                    configSection.Prefixes.Clear();

                    if (value?.Any() ?? false)
                    {
                        foreach (var item in value)
                        {
                            var prefixConfig = new PrefixConfig()
                            {
                                Name = item
                            };
                            configSection.Prefixes.Add(prefixConfig);
                        }
                    }

                    configSection.CurrentConfiguration.Save();
                }
            }
        }

        public IEnumerable<CertificateConfig> Certificates
        {
            get
            {
                var config = GetConfigurationSection();
                return config.Certificates.OfType<CertificateConfig>();
            }
 
            set
            {
                var configSection = GetConfigurationSection();
                if (configSection?.Certificates != null)
                {
                    configSection.Certificates.Clear();

                    if (value?.Any() ?? false)
                    {
                        foreach (var item in value)
                        {
                            configSection.Certificates.Add(item);
                        }
                    }

                    configSection.CurrentConfiguration.Save();
                }
            }
        }
        #endregion

        #region Auxiliary functions.
        private void GetExtensionsConfiguration()
        {
            var configSection = GetConfigurationSection();

            var extensionConfigs = configSection?.Extensions?.OfType<ExtensionConfig>();

            if (extensionConfigs != null && extensionConfigs.Any())
            {
                foreach (var extension in extensionConfigs)
                {
                    if (!extension.Enabled)
                        _disabledExtensions.Add(extension.Id);

                    var parameters = extension.Parameters.OfType<PropertyConfigElement>();
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            _values.Add($"{extension.Id}_{parameter.Name}", Decrypt(parameter.Value));
                        }
                    }
                }
            }
        }

        public static ThreatsManagerConfigurationSection GetConfigurationSection()
        {
            Configuration config =
                    ConfigurationManager.OpenExeConfiguration(_configurationUserLevel);

            ThreatsManagerConfigurationSection configSection =
                (ThreatsManagerConfigurationSection)config?.Sections["threatsManager"];

            if (configSection == null)
            {
                configSection = new ThreatsManagerConfigurationSection();
                configSection.SectionInformation.AllowExeDefinition =
                    ConfigurationAllowExeDefinition.MachineToLocalUser;
                config.Sections.Add("threatsManager", configSection);
                config.Save();
            }

            if (!configSection.Updated)
            {
                configSection.Updated = true;

                var oldConfigFile = GetOldConfigFile(config.FilePath);
                if (!string.IsNullOrWhiteSpace(oldConfigFile) && File.Exists(oldConfigFile))
                {
                    using (var stream = File.OpenRead(oldConfigFile))
                    {
                        var serializer = new XmlSerializer(typeof(OldUserConfig));
                        var oldConfig = serializer.Deserialize(stream) as OldUserConfig;

                        configSection.Setup = oldConfig?.ThreatsManagerConfig?.Setup ?? false;
                        configSection.Mode = oldConfig?.ThreatsManagerConfig?.Mode ?? ExecutionMode.Expert;
                        configSection.OverrideThreatEventsInfo =
                            oldConfig?.ThreatsManagerConfig?.OverrideThreatEventsInfo ?? false;
                        configSection.SmartSave = oldConfig?.ThreatsManagerConfig?.SmartSave ?? false;
                        configSection.SmartSaveCount = oldConfig?.ThreatsManagerConfig?.SmartSaveCount ?? 0;
                        configSection.SmartSaveInterval =
                            oldConfig?.ThreatsManagerConfig?.SmartSaveInterval ?? 0;
                        configSection.UserDictionary = oldConfig?.ThreatsManagerConfig?.UserDictionary;
                        configSection.StatusBarItems = oldConfig?.ThreatsManagerConfig?.StatusBar?.TagSplit();
                        configSection.ExtensionsConfigFolder = oldConfig?.ThreatsManagerConfig?.ExtensionsConfigFolder;
                        configSection.DisableHelp = oldConfig?.ThreatsManagerConfig?.DisableHelp ?? false;

                        var docs = oldConfig?.ThreatsManagerConfig?.KnownDocuments;
                        if (docs != null)
                        {
                            foreach (var doc in docs)
                            {
                                configSection.KnownDocuments.Add(new KnownDocumentConfig()
                                {
                                    Path = doc.Path,
                                    LocationType = doc.LocationType,
                                    PackageManager = doc.PackageManager
                                });
                            }
                        }

                        var extensions = oldConfig?.ThreatsManagerConfig?.Extensions;
                        if (extensions != null)
                        {
                            foreach (var extension in extensions)
                            {
                                var newExtension = new ExtensionConfig()
                                {
                                    Id = extension.Id,
                                    Enabled = extension.Enabled
                                };

                                var parameters = extension.Parameters?.ToArray();
                                if (parameters?.Any() ?? false)
                                {
                                    foreach (var param in parameters)
                                    {
                                        newExtension.Parameters.Add(new PropertyConfigElement(param.Name, param.Value));
                                    }
                                }

                                configSection.Extensions.Add(newExtension);
                            }
                        }

                        var folders = oldConfig?.ThreatsManagerConfig?.Folders;
                        if (folders != null)
                        {
                            foreach (var folder in folders)
                            {
                                configSection.Folders.Add(new FolderConfig()
                                {
                                    Name = folder.Name
                                });
                            }
                        }

                        var prefixes = oldConfig?.ThreatsManagerConfig?.Prefixes;
                        if (prefixes != null)
                        {
                            foreach (var prefix in prefixes)
                            {
                                configSection.Prefixes.Add(new PrefixConfig()
                                {
                                    Name = prefix.Name
                                });
                            }
                        }

                        var certificates = oldConfig?.ThreatsManagerConfig?.Certificates;
                        if (certificates != null)
                        {
                            foreach (var certificate in certificates)
                            {
                                configSection.Certificates.Add(new CertificateConfig()
                                {
                                    Subject = certificate.Subject,
                                    Issuer = certificate.Issuer,
                                    Thumbprint = certificate.Thumbprint,
                                    ExpirationDate = certificate.ExpirationDate
                                });
                            }
                        }
                    }

                    config.Save();
                }
            }

            return configSection;
        }

        private static string GetOldConfigFile([Required] string configFilePath)
        {
            string result = null;

            var dirName = Path.GetDirectoryName(configFilePath);
            if (!string.IsNullOrWhiteSpace(dirName))
            {
                var dir = new DirectoryInfo(dirName);
                var root = dir.Parent?.Parent;
                if (root != null && Directory.Exists(root.FullName))
                {
                    var firstLevel =
                        root.EnumerateDirectories()
                            .Where(x => x.Name.Contains(System.Diagnostics.Process.GetCurrentProcess().ProcessName));
                    List<DirectoryInfo> secondLevel = new List<DirectoryInfo>();
                    foreach (var first in firstLevel)
                        secondLevel.AddRange(first.EnumerateDirectories());
                    var latest = secondLevel
                        .Where(x => string.Compare(dir.FullName, x.FullName, StringComparison.OrdinalIgnoreCase) != 0)
                        .OrderByDescending(x => x.CreationTime).FirstOrDefault();

                    var path = latest?.FullName;
                    if (!string.IsNullOrWhiteSpace(path))
                        result = Path.Combine(path, "user.config");
                }
            }

            return result;
        }

        private string Encrypt(string text)
        {
            string result;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var encoded = Encoding.UTF8.GetBytes(text);
                var entropy = Convert.FromBase64String(Entropy);
                var encryptedEncoded = ProtectedData.Protect(encoded, entropy, DataProtectionScope.CurrentUser);

                result = Convert.ToBase64String(encryptedEncoded);
            }
            else
            {
                result = text;
            }

            return result;
        }

        private string Decrypt(string text)
        {
            string result;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var encryptedEncoded = Convert.FromBase64String(text);
                var entropy = Convert.FromBase64String(Entropy);
                var encoded = ProtectedData.Unprotect(encryptedEncoded, entropy, DataProtectionScope.CurrentUser);

                result = Encoding.UTF8.GetString(encoded);
            }
            else
            {
                result = text;
            }

            return result;
        }
        #endregion
    }
}
