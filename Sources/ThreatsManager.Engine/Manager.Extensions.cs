using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Engine
{
    public partial class Manager
    {
        private readonly ExtensionsManager _extensionsManager = new ExtensionsManager();
        private ExtensionsConfigurationManager _configuration;

        public IExtensionMetadata GetExtensionMetadata([Required] string id)
        {
            return (_configuration?.IsEnabled(id) ?? false) ? 
                _extensionsManager.GetExtensionMetadata(id) : null;
        }

        public IEnumerable<KeyValuePair<IExtensionMetadata, T>> GetExtensionsMetadata<T>() where T : class, IExtension
        {
            return _extensionsManager.GetExtensions<T>()?
                .Where(x => _configuration?.IsEnabled(x.Key.Id) ?? false);
        }

        public T GetExtension<T>([Required] string id) where T : class, IExtension
        {
            return (_configuration?.IsEnabled(id) ?? false) ?
                _extensionsManager.GetExtension<T>(id) : default(T);
        }

        public T GetExtensionByLabel<T>([Required] string label) where T : class, IExtension
        {
            T result = default(T);

            var extension = _extensionsManager.GetExtensions<T>()?
                .FirstOrDefault(x => string.CompareOrdinal(x.Key.Label, label) == 0);
            if (extension.HasValue && extension.Value.Key != null && (_configuration?.IsEnabled(extension.Value.Key.Id) ?? false))
            {
                result = extension.Value.Value;
            }

            return result;
        }

        public IEnumerable<T> GetExtensions<T>() where T : class, IExtension
        {
            return _extensionsManager.GetExtensions<T>()?
                .Where(x => _configuration?.IsEnabled(x.Key.Id) ?? false)
                .Select(x => x.Value);
        }

        public void ApplyExtensionInitializers()
        {
            var initializers = GetExtensionsMetadata<IExtensionInitializer>()?
                .Where(x => _configuration?.IsEnabled(x.Key.Id) ?? false).ToArray();

            if (initializers?.Any() ?? false)
            {
                foreach (var lazy in initializers)
                {
                    lazy.Value?.Initialize();
                }
            }
        }
    }
}
