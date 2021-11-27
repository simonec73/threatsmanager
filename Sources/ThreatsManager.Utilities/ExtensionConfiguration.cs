using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Extension Configuration.
    /// </summary>
    public class ExtensionConfiguration
    {
        private readonly List<ConfigurationData> _configData = new List<ConfigurationData>();

        internal ExtensionConfiguration(IEnumerable<ConfigurationData> configData)
        {
            var items = configData?.ToArray();
            if (items?.Any() ?? false)
            {
                _configData.AddRange(items);
            }
        }

        /// <summary>
        /// Configuration Properties for the Extension.
        /// </summary>
        public IEnumerable<ConfigurationData> Data => _configData.AsReadOnly();

        /// <summary>
        /// Get an Extension Configuration Property from the Global configuration.
        /// </summary>
        /// <typeparam name="T">Type of the Property.</typeparam>
        /// <param name="propertyName">Name of the Property.</param>
        /// <param name="defaultValue">[Optional] Default value of the Property, to be used if the property has not been configured yet.</param>
        /// <returns>Value of the property.</returns>
        public T GlobalGet<T>(string propertyName, T defaultValue = default(T))
        {
            return Get<T>(propertyName, true, defaultValue);
        }

        /// <summary>
        /// Get an Extension Configuration Property from the Local configuration.
        /// </summary>
        /// <typeparam name="T">Type of the Property.</typeparam>
        /// <param name="propertyName">Name of the Property.</param>
        /// <param name="defaultValue">[Optional] Default value of the Property, to be used if the property has not been configured yet.</param>
        /// <returns>Value of the property.</returns>
        public T LocalGet<T>(string propertyName, T defaultValue = default(T))
        {
            return Get<T>(propertyName, false, defaultValue);
        }

        /// <summary>
        /// Set an Extension Configuration Property into the Global configuration.
        /// </summary>
        /// <typeparam name="T">Type of the Property.</typeparam>
        /// <param name="propertyName">Name of the Property.</param>
        /// <param name="value">Value of the Property.</param>
        public void GlobalSet<T>(string propertyName, T value)
        {
            Set<T>(propertyName, true, value);
        }

        /// <summary>
        /// Set an Extension Configuration Property into the Local configuration.
        /// </summary>
        /// <typeparam name="T">Type of the Property.</typeparam>
        /// <param name="propertyName">Name of the Property.</param>
        /// <param name="value">Value of the Property.</param>
        public void LocalSet<T>(string propertyName, T value)
        {
            Set<T>(propertyName, false, value);
        }

        private T Get<T>(string propertyName, bool global, T defaultValue)
        {
            T result = defaultValue;

            var cd = _configData
                .FirstOrDefault(x => string.CompareOrdinal(x.Name, propertyName) == 0);
            if (cd == null)
            {
                var c = new ConfigurationData<T>(propertyName, defaultValue, global);
                _configData.Add(c);
            } 
            else if (cd is ConfigurationData<T> configValue)
            {
                result = configValue.TypedValue;
            }
            else
            {
                result = (T) Convert.ChangeType(cd.Value, typeof(T));
            }

            return result;
        }

        private void Set<T>(string propertyName, bool global, T value)
        {
            var cd = _configData
                .FirstOrDefault(x => string.CompareOrdinal(x.Name, propertyName) == 0);
            if (cd == null)
            {
                var c = new ConfigurationData<T>(propertyName, value, global);
                _configData.Add(c);
            } 
            else
            {
                cd.Value = value;
            }
        }

    }
}
