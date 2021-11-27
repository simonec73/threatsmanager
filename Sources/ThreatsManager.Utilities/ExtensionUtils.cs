using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Utilities to be used to simplify working with Extensions.
    /// </summary>
    public static class ExtensionUtils
    {
        #region Extension Properties.
        /// <summary>
        /// Get the Extension Identifier.
        /// </summary>
        /// <param name="value">Extension whose identifier is to be returned.</param>
        /// <returns>Identifier of the Extension.</returns>
        /// <remarks>If the object is not an Extension, then the method returns null.</remarks>
        public static string GetExtensionId(this object value)
        {
            string result = null;

            Type type = value.GetType();

            var attribs = type.GetCustomAttributes(
                typeof(ExportMetadataAttribute), false) as ExportMetadataAttribute[];

            var attrib = attribs?.FirstOrDefault(x => string.CompareOrdinal(x.Name, "Id") == 0);
            if (attrib != null && attrib.Value is string attribValue)
            {
                result = attribValue;
            }
            else
            {
                var extensionAttribute = type.GetCustomAttributes(typeof(ExtensionAttribute), false)
                    .OfType<ExtensionAttribute>().FirstOrDefault();
                if (extensionAttribute != null)
                    result = extensionAttribute.Id;
            }

            return result;
        }

        /// <summary>
        /// Get the Extension Label.
        /// </summary>
        /// <param name="value">Extension whose Label is to be returned.</param>
        /// <returns>Label of the Extension.</returns>
        /// <remarks>If the object is not an Extension, then the method returns null.</remarks>
        public static string GetExtensionLabel(this object value)
        {
            string result = null;

            Type type = value.GetType();

            ExportMetadataAttribute[] attribs = type.GetCustomAttributes(
                typeof(ExportMetadataAttribute), false) as ExportMetadataAttribute[];

            var attrib = attribs?.FirstOrDefault(x => string.CompareOrdinal(x.Name, "Label") == 0);
            if (attrib != null && attrib.Value is string attribValue)
            {
                result = attribValue;
            }
            else
            {
                var extensionAttribute = type.GetCustomAttributes(typeof(ExtensionAttribute), false)
                    .OfType<ExtensionAttribute>().FirstOrDefault();
                if (extensionAttribute != null)
                    result = extensionAttribute.Label;
            }

            return result;
        }

        /// <summary>
        /// Get the Title of the Assembly containing the Extension.
        /// </summary>
        /// <param name="value">Reference extension.</param>
        /// <returns>Title of the containing Assembly.</returns>
        /// <remarks>This method can be used for all classes, not only for Extensions.</remarks>
        public static string GetExtensionAssemblyTitle(this object value)
        {
            return value.GetType().Assembly
                .GetCustomAttributes(typeof(AssemblyTitleAttribute), false)
                .OfType<AssemblyTitleAttribute>()
                .Select(x => x.Title)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get the Extension priority.
        /// </summary>
        /// <param name="value">Extension whose priority is to be returned.</param>
        /// <returns>Priority of the Extension.</returns>
        /// <remarks>If the object is not an Extension, then the method returns 0.</remarks>
        public static int GetExtensionPriority(this object value)
        {
            int result = 0;

            Type type = value.GetType();

            var attribs = type.GetCustomAttributes(
                typeof(ExportMetadataAttribute), false) as ExportMetadataAttribute[];

            var attrib = attribs?.FirstOrDefault(x => string.CompareOrdinal(x.Name, "Priority") == 0);
            if (attrib != null && attrib.Value is int attribValue)
            {
                result = attribValue;
            }
            else
            {
                var extensionAttribute = type.GetCustomAttributes(typeof(ExtensionAttribute), false)
                    .OfType<ExtensionAttribute>().FirstOrDefault();
                if (extensionAttribute != null)
                    result = extensionAttribute.Priority;
            }

            return result;
        }
        #endregion

        #region Extension Enumeration.
        /// <summary>
        /// Gets all the Extensions of a specific type loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <returns>List of registered Extensions.</returns>
        public static IEnumerable<T> GetExtensions<T>() where T : class, IExtension
        {
            IEnumerable<T> result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null) as IExtensionManager;
                result = instance?.GetExtensions<T>();
            }

            return result;
        }

        /// <summary>
        /// Gets the Extension of a specific type and ID, loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <returns>Registered Extension.</returns>
        public static T GetExtension<T>([Required] string extensionId) where T : class, IExtension
        {
            T result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null) as IExtensionManager;
                result = instance?.GetExtension<T>(extensionId);
            }

            return result;
        }

        /// <summary>
        /// Gets the Extension of a specific type and Label, loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <param name="label">Label of the Extension.</param>
        /// <returns>Registered Extension.</returns>
        public static T GetExtensionByLabel<T>([Required] string label) where T : class, IExtension
        {
            T result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null) as IExtensionManager;
                result = instance?.GetExtensionByLabel<T>(label);
            }

            return result;
        }
        #endregion

        #region Extensions Configuration.
        /// <summary>
        /// Folder where the Extension Configuration files are stored.
        /// </summary>
        public static string ExtensionConfigurationFolder;

        /// <summary>
        /// Get the specific configuration for an Extension.
        /// </summary>
        /// <param name="model">Current Threat Model.</param>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <returns>Configuration of the Extension.</returns>
        public static ExtensionConfiguration GetExtensionConfiguration(this IThreatModel model, 
            [Required] string extensionId)
        {
            List<ConfigurationData> result = null;

            var property = GetExtensionConfigurationProperty(model, extensionId);
            if (property != null && property.Value is ExtensionConfigurationData extensionConfig)
            {
                var configurationData = GetConfigurationArray(extensionConfig, false)?.ToArray();
                if (configurationData?.Any() ?? false)
                {
                    result = new List<ConfigurationData>(configurationData);
                }
            }

            var config = GetGlobalConfiguration(extensionId, ExtensionConfigurationFolder);
            if (config?.Properties?.Any() ?? false)
            {
                var configurationData = GetConfigurationArray(config, true)?.ToArray();
                if (configurationData?.Any() ?? false)
                {
                    if (result == null)
                        result = new List<ConfigurationData>();
                    
                    result.AddRange(configurationData);
                }
            }

            return new ExtensionConfiguration(result);
        }

        /// <summary>
        /// Set the specific configuration for an Extension.
        /// </summary>
        /// <param name="model">Current Threat Model.</param>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <param name="configuration">Configuration of the Extension.</param>
        public static void SetExtensionConfiguration(this IThreatModel model, 
            [Required] string extensionId, ExtensionConfiguration configuration)
        {
            var data = configuration?.Data?.ToArray();

            var property = GetExtensionConfigurationProperty(model, extensionId);
            if (property != null)
            {
                property.Value = GetConfigurationObject(data?.Where(x => !x.Global));
            }

            SaveGlobalConfiguration(extensionId, ExtensionConfigurationFolder, GetConfigurationObject(data?.Where(x => x.Global)));
        }

        private static IPropertyJsonSerializableObject GetExtensionConfigurationProperty([NotNull] IThreatModel model, [Required] string extensionId)
        {
            var propertySchema =
                model.GetSchema("ExtensionsConfiguration", "https://github.com/simonec73/ThreatsManager") ?? model.AddSchema("ExtensionsConfiguration", "https://github.com/simonec73/ThreatsManager");
            propertySchema.AppliesTo = Scope.ThreatModel;
            propertySchema.AutoApply = false;
            propertySchema.NotExportable = true;
            propertySchema.Priority = 100;
            propertySchema.RequiredExecutionMode = ExecutionMode.Business;
            propertySchema.System = true;
            propertySchema.Visible = false;

            var propertyType = propertySchema.GetPropertyType(extensionId) ?? propertySchema.AddPropertyType(extensionId, PropertyValueType.JsonSerializableObject);
            propertyType.Visible = false;
            propertyType.DoNotPrint = true;

            return (model.GetProperty(propertyType) ?? model.AddProperty(propertyType, null)) as IPropertyJsonSerializableObject;
        }

        private static ExtensionConfigurationData GetConfigurationObject(IEnumerable<ConfigurationData> configuration)
        {
            ExtensionConfigurationData result = null;

            var items = configuration?.ToArray();
            if (items?.Any() ?? false)
            {
                result = new ExtensionConfigurationData()
                {
                    Properties = items.ToDictionary(x => x.Name, y => y.Value)
                };
            }

            return result;
        }

        private static IEnumerable<ConfigurationData> GetConfigurationArray(ExtensionConfigurationData configuration, bool global)
        {
            IEnumerable<ConfigurationData> result = null;

            var items = configuration?.Properties;
            if (items?.Any() ?? false)
            {
                result = items.Select(x => new ConfigurationData(x.Key, x.Value, global));
            }

            return result;
        }

        private static ExtensionConfigurationData GetGlobalConfiguration([Required] string extensionId, [Required] string folder)
        {
            ExtensionConfigurationData result = null;

            var pathName = Path.Combine(folder, $"{extensionId}.tmg");

            if (File.Exists(pathName))
            {
                using (var file = File.OpenRead(pathName))
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);

                        string jsonText;
                        var json = ms.ToArray();
                        if (json.Length > 0)
                        {
                            if (json[0] == 0xFF)
                                jsonText = Encoding.Unicode.GetString(json, 2, json.Length - 2);
                            else
                                jsonText = Encoding.Unicode.GetString(json);

                            using (var textReader = new StringReader(jsonText))
                            using (var reader = new JsonTextReader(textReader))
                            {
                                var serializer = new JsonSerializer
                                {
                                    TypeNameHandling = TypeNameHandling.All,
                                    SerializationBinder = new KnownTypesBinder(),
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };
                                result = serializer.Deserialize<ExtensionConfigurationData>(reader);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static void SaveGlobalConfiguration([Required] string extensionId, [Required] string folder, ExtensionConfigurationData config)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var pathName = Path.Combine(folder, $"{extensionId}.tmg");

            if (File.Exists(pathName))
                File.Delete(pathName);

            using (var file = File.OpenWrite(pathName))
            {
                using (var writer = new BinaryWriter(file))
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);

                    using(JsonWriter jtw = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented};
                        serializer.Serialize(jtw, config);
                    }

                    var serialization = Encoding.Unicode.GetBytes(sb.ToString());
                    
                    writer.Write((byte)0xFF);
                    writer.Write((byte)0xFE);
                    writer.Write(serialization);
                }
            }

        }
        #endregion
    }
}