using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
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
        /// Gets all the Extensions of a specific type loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <returns>List of registered Extensions.</returns>
        public static IEnumerable<T> GetExtensions<T>() where T : class
        {
            IEnumerable<T> result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null);
                result = type.GetMethod("GetExtensions")?.MakeGenericMethod(typeof(T))
                    .Invoke(instance, BindingFlags.Instance | BindingFlags.Public, null, new object[] { }, CultureInfo.InvariantCulture) as IEnumerable<T>;
            }

            return result;
        }

        /// <summary>
        /// Gets the Extension of a specific type and ID, loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <returns>Registered Extension.</returns>
        public static T GetExtension<T>([Required] string extensionId) where T : class
        {
            T result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null);
                result = type.GetMethod("GetExtension")?.MakeGenericMethod(typeof(T))
                    .Invoke(instance, BindingFlags.Instance | BindingFlags.Public, null, new [] { (object)extensionId }, CultureInfo.InvariantCulture) as T;
            }

            return result;
        }

        /// <summary>
        /// Gets the Extension of a specific type and Label, loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <param name="label">Label of the Extension.</param>
        /// <returns>Registered Extension.</returns>
        public static T GetExtensionByLabel<T>([Required] string label) where T : class
        {
            T result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null);
                result = type.GetMethod("GetExtensionByLabel")?.MakeGenericMethod(typeof(T))
                    .Invoke(instance, BindingFlags.Instance | BindingFlags.Public, null, new [] { (object)label }, CultureInfo.InvariantCulture) as T;
            }

            return result;
        }

        #region Extensions Configuration.
        /// <summary>
        /// Get the specific configuration for an Extension.
        /// </summary>
        /// <param name="model">Current Threat Model.</param>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <param name="folder">Folder containing the Global configuration of the Extension.</param>
        /// <returns>Configuration of the Extension.</returns>
        public static IEnumerable<ConfigurationData> GetExtensionConfiguration(this IThreatModel model, 
            [Required] string extensionId, [Required] string folder)
        {
            List<ConfigurationData> result = null;

            var property = GetExtensionConfigurationProperty(model, extensionId);
            if (property != null && property.Value is ExtensionConfiguration extensionConfig)
            {
                var configurationData = GetConfigurationArray(extensionConfig, false)?.ToArray();
                if (configurationData?.Any() ?? false)
                {
                    result = new List<ConfigurationData>(configurationData);
                }
            }

            var config = GetGlobalConfiguration(extensionId, folder);
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

            return result;
        }

        /// <summary>
        /// Set the specific configuration for an Extension.
        /// </summary>
        /// <param name="model">Current Threat Model.</param>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <param name="folder">Folder containing the Global configuration of the Extension.</param>
        /// <param name="configuration">Configuration of the Extension.</param>
        public static void SetExtensionConfiguration(this IThreatModel model, 
            [Required] string extensionId, [Required] string folder, IEnumerable<ConfigurationData> configuration)
        {
            var property = GetExtensionConfigurationProperty(model, extensionId);
            if (property != null)
            {
                property.Value = GetConfigurationObject(configuration?.Where(x => !x.Global));
            }

            SaveGlobalConfiguration(extensionId, folder, GetConfigurationObject(configuration?.Where(x => x.Global)));
        }

        private static IPropertyJsonSerializableObject GetExtensionConfigurationProperty([NotNull] IThreatModel model, [Required] string extensionId)
        {
            var propertySchema =
                model.GetSchema("ExtensionsConfiguration", "https://github.com/simonec73/ThreatsManager");
            if (propertySchema == null)
            {
                propertySchema =
                    model.AddSchema("ExtensionsConfiguration", "https://github.com/simonec73/ThreatsManager");
                propertySchema.AppliesTo = Scope.ThreatModel;
                propertySchema.AutoApply = false;
                propertySchema.NotExportable = true;
                propertySchema.Priority = 100;
                propertySchema.RequiredExecutionMode = ExecutionMode.Business;
                propertySchema.System = true;
                propertySchema.Visible = false;
            }

            var propertyType = propertySchema.GetPropertyType(extensionId);
            if (propertyType == null)
            {
                propertyType = propertySchema.AddPropertyType(extensionId, PropertyValueType.JsonSerializableObject);
                propertyType.Visible = false;
                propertyType.DoNotPrint = true;
            }

            return (model.GetProperty(propertyType) ?? model.AddProperty(propertyType, null)) as IPropertyJsonSerializableObject;
        }

        private static ExtensionConfiguration GetConfigurationObject(IEnumerable<ConfigurationData> configuration)
        {
            ExtensionConfiguration result = null;

            var items = configuration?.ToArray();
            if (items?.Any() ?? false)
            {
                result = new ExtensionConfiguration()
                {
                    Properties = items.ToDictionary(x => x.Name, y => y.Value)
                };
            }

            return result;
        }

        private static IEnumerable<ConfigurationData> GetConfigurationArray(ExtensionConfiguration configuration, bool global)
        {
            IEnumerable<ConfigurationData> result = null;

            var items = configuration?.Properties;
            if (items?.Any() ?? false)
            {
                result = items.Select(x => new ConfigurationData(x.Key, x.Value, global));
            }

            return result;
        }

        private static ExtensionConfiguration GetGlobalConfiguration([Required] string extensionId, [Required] string folder)
        {
            ExtensionConfiguration result = null;

            var pathName = Path.Combine(folder, $"{extensionId}.tmg");

            if (File.Exists(pathName))
            {
                using (var file = File.OpenRead(pathName))
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var jsonText = Encoding.Unicode.GetString(ms.ToArray());
                        result = JsonConvert.DeserializeObject<ExtensionConfiguration>(jsonText,
                            new JsonSerializerSettings()
                            {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                                TypeNameHandling = TypeNameHandling.All,
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
                                SerializationBinder = new KnownTypesBinder(),
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            });
                    }
                }
            }

            return result;
        }

        private static void SaveGlobalConfiguration([Required] string extensionId, [Required] string folder, ExtensionConfiguration config)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var pathName = Path.Combine(folder, $"{extensionId}.tmg");

            using (var file = File.OpenWrite(pathName))
            {
                using (var writer = new BinaryWriter(file))
                {
                    var serialization = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(config, 
                        Formatting.Indented, new JsonSerializerSettings()
                        {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                            TypeNameHandling = TypeNameHandling.All
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
                        }));

                    writer.Write(serialization);
                }
            }

        }
        #endregion
    }
}