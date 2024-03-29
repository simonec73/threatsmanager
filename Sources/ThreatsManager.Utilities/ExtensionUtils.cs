﻿using System;
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
using ThreatsManager.Interfaces.Extensions.Actions;
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
            return value?.GetType()?.GetExtensionId();
        }

        /// <summary>
        /// Get the Extension Identifier.
        /// </summary>
        /// <param name="type">Extension Type whose identifier is to be returned.</param>
        /// <returns>Identifier of the Extension.</returns>
        /// <remarks>If the Type is not related to an Extension, then the method returns null.</remarks>
        public static string GetExtensionId(this Type type)
        {
            string result = null;

            var attribs = type?.GetCustomAttributes(
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
            return value?.GetType()?.GetExtensionLabel();
        }

        /// <summary>
        /// Get the Extension Label.
        /// </summary>
        /// <param name="type">Extension Type whose Label is to be returned.</param>
        /// <returns>Label of the Extension.</returns>
        /// <remarks>If the Type is not an Extension, then the method returns null.</remarks>
        public static string GetExtensionLabel(this Type type)
        {
            string result = null;

            ExportMetadataAttribute[] attribs = type?.GetCustomAttributes(
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
            return value?.GetType()?.GetExtensionAssemblyTitle();
        }

        /// <summary>
        /// Get the Title of the Assembly containing the Extension.
        /// </summary>
        /// <param name="type">Reference extension Type.</param>
        /// <returns>Title of the containing Assembly.</returns>
        /// <remarks>This method can be used for all classes, not only for Extensions.</remarks>
        public static string GetExtensionAssemblyTitle(this Type type)
        {
            return type?.Assembly
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
            return value?.GetType()?.GetExtensionPriority() ?? 0;
        }

        /// <summary>
        /// Get the Extension priority.
        /// </summary>
        /// <param name="type">Extension Type whose priority is to be returned.</param>
        /// <returns>Priority of the Extension.</returns>
        /// <remarks>If the Type is not an Extension, then the method returns 0.</remarks>
        public static int GetExtensionPriority(this Type type)
        {
            int result = 0;

            var attribs = type?.GetCustomAttributes(
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

        /// <summary>
        /// Get the Universal Identifier of the Extension.
        /// </summary>
        /// <param name="value">Extension whose Universal Identifier is to be returned.</param>
        /// <returns>Universal Identifier of the Extension.</returns>
        /// <remarks>If the object is not an Extension or if the Universal Identifier has not been defined, then the method returns null.</remarks>
        public static string GetExtensionUniversalId(this object value)
        {
            return value?.GetType()?.GetExtensionUniversalId();
        }

        /// <summary>
        /// Get the Universal Identifier of the Extension.
        /// </summary>
        /// <param name="type">Extension Type whose Universal Identifier is to be returned.</param>
        /// <returns>Universal Identifier of the Extension Type.</returns>
        /// <remarks>If the Type is not an Extension or if the Universal Identifier has not been defined, then the method returns null.</remarks>
        public static string GetExtensionUniversalId(this Type type)
        {
            string result = null;

            var attrib = type?.GetCustomAttributes(
                typeof(UniversalIdentifierAttribute), false)?.FirstOrDefault() as UniversalIdentifierAttribute;

            if (attrib != null)
            {
                result = attrib.Name;
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

        /// <summary>
        /// Gets the Extension of a specific type Universal Identifier, loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <param name="universalId">Universal Identifier of the searched Extension.</param>
        /// <returns>Registered Extension.</returns>
        public static T GetExtensionByUniversalId<T>([Required] string universalId) where T : class, IExtension
        {
            T result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null) as IExtensionManager;
                result = instance?.GetExtensionByUniversalId<T>(universalId);
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

            using (var scope = UndoRedoManager.OpenScope("Set Extension Configuration"))
            {
                var property = GetExtensionConfigurationProperty(model, extensionId);
                if (property != null)
                {
                    property.Value = GetConfigurationObject(data?.Where(x => !x.Global));
                    scope?.Complete();
                }
            }

            SaveGlobalConfiguration(extensionId, ExtensionConfigurationFolder, GetConfigurationObject(data?.Where(x => x.Global)));
        }

        private static IPropertyJsonSerializableObject GetExtensionConfigurationProperty([NotNull] IThreatModel model, [Required] string extensionId)
        {
            IPropertyJsonSerializableObject result;

            using (var scope = UndoRedoManager.OpenScope("Get Extension Configuration property"))
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

                result = (model.GetProperty(propertyType) ?? model.AddProperty(propertyType, null)) as IPropertyJsonSerializableObject;
                scope?.Complete();
            }

            return result;
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
                                    TypeNameHandling = TypeNameHandling.Auto,
                                    DefaultValueHandling = DefaultValueHandling.Ignore,
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
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (FileNotFoundException)
            {
                throw new UnauthorizedAccessException($"Folder '{folder}' cannot be created. Access Denied.");
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

                    using (JsonWriter jtw = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            DefaultValueHandling = DefaultValueHandling.Ignore,
                            Formatting = Formatting.Indented
                        };
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

        #region ICommandsBarContextAwareAction accelerator functions.
        /// <summary>
        /// Verifies if a <see cref="ICommandsBarContextAwareAction"/> is visible to a given context.
        /// </summary>
        /// <param name="action"><see cref="ICommandsBarContextAwareAction"/> to be checked.</param>
        /// <param name="referenceContext">Reference context.</param>
        /// <returns>True if the reference context is visible.</returns>
        /// <remarks>If the referenceContext is null or empty, it is visible by default./></remarks>
        public static bool IsVisible(this ICommandsBarContextAwareAction action, string referenceContext)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(referenceContext))
            {
                result = true;
            }
            else
            {
                var supported = action?.SupportedContexts?.ToArray();
                var unsupported = action?.UnsupportedContexts?.ToArray();

                result = (supported?.Contains(referenceContext) ?? true) &&
                    !(unsupported?.Contains(referenceContext) ?? false);
            }

            return result;
        }
        #endregion
    }
}