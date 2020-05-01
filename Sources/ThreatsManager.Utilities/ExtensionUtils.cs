using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

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
        /// Gets all the Extensions of a specific type loaded by the Platform. 
        /// </summary>
        /// <typeparam name="T">Extension type.</typeparam>
        /// <param name="extensionId">Identifier of the Extension.</param>
        /// <returns>List of registered Extensions.</returns>
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
    }
}