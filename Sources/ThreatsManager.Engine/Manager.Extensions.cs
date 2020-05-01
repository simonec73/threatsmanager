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

        public IEnumerable<Lazy<T, IExtensionMetadata>> GetExtensionsMetadata<T>() where T : class
        {
            return _extensionsManager.GetExtensions<T>()?
                .Where(x => _configuration?.IsEnabled(x.Metadata.Id) ?? false);
        }

        public T GetExtension<T>([Required] string id) where T : class
        {
            return (_configuration?.IsEnabled(id) ?? false) ?
                _extensionsManager.GetExtension<T>(id) : default(T);
        }

        public IEnumerable<T> GetExtensions<T>() where T : class
        {
            return _extensionsManager.GetExtensions<T>()?
                .Where(x => _configuration?.IsEnabled(x.Metadata.Id) ?? false)
                .Select(x => x.Value);
        }

        public void ApplyExtensionInitializers()
        {
            var initializers = GetExtensionsMetadata<IExtensionInitializer>()?
                .Where(x => _configuration?.IsEnabled(x.Metadata.Id) ?? false).ToArray();

            if (initializers?.Any() ?? false)
            {
                foreach (var lazy in initializers)
                {
                    if (lazy.Value is IExtensionInitializer initializer)
                        initializer.Initialize();
                }
            }
        }
    }
}
