using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Help;
using PostSharp.Patterns.Threading;
using ThreatsManager.Engine.Policies;
using ThreatsManager.Utilities.Policies;

namespace ThreatsManager.Engine
{
#pragma warning disable CS0169
    class ExtensionsManager
    {
        private CompositionContainer _container;
        private readonly AggregateCatalog _catalog = new AggregateCatalog();
        private static ExecutionMode _executionMode; 

#pragma warning disable 649
        [ImportMany(typeof(IExtension))] 
        private IEnumerable<Lazy<IExtension, IExtensionMetadata>> _extensions;

        private IDictionary<string, string> _extensionsByUniversalId;
#pragma warning restore 649

        public void AddExtensionsAssembly([Required] string path)
        {
            _catalog.Catalogs.Add(new AssemblyCatalog(path));
        }

        public void SetExecutionMode(ExecutionMode executionMode)
        {
            var policy = new MaxExecutionModePolicy();
            var maxExecutionMode = policy.MaxExecutionMode ?? ExecutionMode.Pioneer;

            if (executionMode > maxExecutionMode)
                _executionMode = executionMode;
            else
                _executionMode = maxExecutionMode;
        }

        public void Load(bool loadHelp)
        {
            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(_catalog);

            //Fill the imports of this object  
            try
            {
                _container.ComposeParts(this);

                LoadUniversalIDs();

                if (loadHelp)
                    LoadHelpConfiguration();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (!string.IsNullOrEmpty(exFileNotFound?.FusionLog))
                    {
                        sb.AppendLine("Fusion Log:");
                        sb.AppendLine(exFileNotFound.FusionLog);
                    }
                    sb.AppendLine();
                }
                throw new FileNotFoundException(sb.ToString(), ex);
            }
        }

        public string GetExtensionName([Required] string id)
        {
            return GetExtensionMetadata(id)?.Label;
        }

        public IEnumerable<string> GetExtensionIds()
        {
            return GetExtensions()?
                    .Select(x => x.Metadata.Id)
                    .Distinct()
                    .ToArray();
        }

        public IEnumerable<string> GetExtensionParameters([Required] string id)
        {
            return GetExtensionMetadata(id)?.Parameters;
        }

        public string GetExtensionType([Required] string id)
        {
            string result = null;

            var extension = GetExtension(id);

            if (extension != null)
            {
                var attribute =
                    GetCustomAttributesIncludingBaseInterfaces<ExtensionDescriptionAttribute>(extension.Value.GetType())?
                        .FirstOrDefault();
                if (attribute != null)
                {
                    result = attribute.Text;
                }
            }

            return result;
        }

        public string GetExtensionAssemblyTitle([Required] string id)
        {
            return GetExtension(id)?
                .Value?
                .GetExtensionAssemblyTitle();
        }

        public IExtensionMetadata GetExtensionMetadata([Required] string id)
        {
            return GetExtension(id)?.Metadata;
        }

        public IEnumerable<KeyValuePair<IExtensionMetadata, T>> GetExtensions<T>() where T : class, IExtension
        {
            return GetExtensions()?
                    .Distinct(new ExtensionMetadataEqualityComparer<IExtension>())
                    .Select(x => new KeyValuePair<IExtensionMetadata, T>(x.Metadata, x.Value as T))
                    .Where(x => x.Value != null);
        }

        public T GetExtension<T>([Required] string id) where T : class, IExtension
        {
            return GetExtension(id)?.Value as T;
        }

        public T GetExtensionByLabel<T>([Required] string label) where T : class, IExtension
        {
            return GetExtensions()?
                .FirstOrDefault(x => string.CompareOrdinal(label, x.Metadata.Label) == 0)?
                .Value as T;
        }

        public T GetExtensionByUniversalId<T>([Required] string universalId) where T : class, IExtension
        {
            T result = default(T);

            var id = _extensionsByUniversalId?
                .Where(x => string.CompareOrdinal(x.Key, universalId) == 0)?
                .Select(x => x.Value)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(id))
                result = GetExtension<T>(id);

            return result;
        }

        #region Private member functions.
        private void LoadUniversalIDs()
        {
            var extensions = _extensions?.Select(x => x.Value)?.ToArray();
            if (extensions?.Any() ?? false)
            {
                var dict = new Dictionary<string, string>();
                foreach (var extension in extensions)
                {
                    var universalId = extension.GetExtensionUniversalId();
                    if (!string.IsNullOrWhiteSpace(universalId))
                    {
                        dict.Add(universalId, extension.GetExtensionId());
                    }
                }

                if (dict.Any())
                {
                    _extensionsByUniversalId = dict;
                }
            }
        }

        [Background]
        private void LoadHelpConfiguration()
        {
            var policy = new HelpTroubleshootPolicy();
            if (policy.HelpTroubleshoot ?? true)
            {
                var assemblies = _catalog.Catalogs.OfType<AssemblyCatalog>().Select(x => x.Assembly).ToArray();
                if (assemblies.Any())
                {
                    foreach (var assembly in assemblies)
                    {
                        LearningManager.Instance.Add(assembly);
                        TroubleshootingManager.Instance.Add(assembly);
                    }
                    LearningManager.Instance.AnalyzeSources();
                    TroubleshootingManager.Instance.AnalyzeSources();
                }
            }
        }

        private IEnumerable<Lazy<IExtension, IExtensionMetadata>> GetExtensions()
        {
            var policy = new DisabledExtensionsPolicy();
            var disabled = policy.DisabledExtensions?.ToArray();

            return _extensions?
                .Where(x => x?.Metadata != null &&
                    IsExecutionModeCompliant(x.Metadata.Mode) &&
                    !(disabled?.Any(y => string.CompareOrdinal(y, x.Metadata.Label) == 0) ?? false))
                .OrderBy(x => x.Metadata.Priority);
        }

        private Lazy<IExtension, IExtensionMetadata> GetExtension([Required] string id)
        {
            return GetExtensions()?
                .FirstOrDefault(x => string.CompareOrdinal(id, x.Metadata.Id) == 0);
        }

        private bool IsExecutionModeCompliant(ExecutionMode requiredMode)
        {
            bool result;

            switch (requiredMode)
            {
                case ExecutionMode.Pioneer:
                    result = _executionMode == ExecutionMode.Pioneer;
                    break;
                case ExecutionMode.Expert:
                    result = _executionMode == ExecutionMode.Pioneer ||
                             _executionMode == ExecutionMode.Expert;
                    break;
                case ExecutionMode.Simplified:
                    result = _executionMode == ExecutionMode.Pioneer ||
                             _executionMode == ExecutionMode.Expert ||
                             _executionMode == ExecutionMode.Simplified;
                    break;
                case ExecutionMode.Management:
                    result = _executionMode == ExecutionMode.Pioneer ||
                             _executionMode == ExecutionMode.Expert ||
                             _executionMode == ExecutionMode.Simplified || 
                             _executionMode == ExecutionMode.Management;
                    break;
                case ExecutionMode.Business:
                    result = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requiredMode), requiredMode, null);
            }

            return result;
        }

        // From https://stackoverflow.com/questions/540749/can-a-c-sharp-class-inherit-attributes-from-its-interface.
        private static IEnumerable<T> GetCustomAttributesIncludingBaseInterfaces<T>(Type type)
        {
            var attributeType = typeof(T);
            return type.GetCustomAttributes(attributeType, true).
                Union(type.GetInterfaces().
                    SelectMany(interfaceType => interfaceType.GetCustomAttributes(attributeType, true))).
                Distinct().Cast<T>();
        }
        #endregion
    }
}
