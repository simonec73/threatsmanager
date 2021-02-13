using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
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
#pragma warning restore 649

        public void AddExtensionsAssembly([Required] string path)
        {
            _catalog.Catalogs.Add(new AssemblyCatalog(path));
        }

        public void SetExecutionMode(ExecutionMode executionMode)
        {
            _executionMode = executionMode;
        }

        public void Load(bool loadHelp)
        {
            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(_catalog);

            //Fill the imports of this object  
            try
            {
                _container.ComposeParts(this);
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
            return _extensions?
                    .Where(x => x?.Metadata != null && IsExecutionModeCompliant(x.Metadata.Mode))
                    .OrderBy(x => x.Metadata.Priority)
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

            var extension = _extensions?
                .FirstOrDefault(x => (x.Metadata != null) && string.CompareOrdinal(id, x.Metadata.Id) == 0);

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
            return _extensions?
                .FirstOrDefault(x => (x.Metadata != null) && string.CompareOrdinal(id, x.Metadata.Id) == 0)?
                .Value?
                .GetExtensionAssemblyTitle();
        }

        public IExtensionMetadata GetExtensionMetadata([Required] string id)
        {
            return _extensions?
                .FirstOrDefault(x => (x.Metadata != null) && string.CompareOrdinal(id, x.Metadata.Id) == 0)?
                .Metadata;
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public IEnumerable<KeyValuePair<IExtensionMetadata, T>> GetExtensions<T>() where T : class, IExtension
        {
            return _extensions?
                    .Where(x => x?.Metadata != null && IsExecutionModeCompliant(x.Metadata.Mode) && x.Value is T)
                    .OrderBy(x => x.Metadata.Priority)
                    .Distinct(new ExtensionMetadataEqualityComparer<IExtension>())
                    .Select(x => new KeyValuePair<IExtensionMetadata, T>(x.Metadata, x.Value as T));
        }

        public T GetExtension<T>([Required] string id) where T : class, IExtension
        {
            return _extensions?
                .FirstOrDefault(x => (x?.Metadata != null) && string.CompareOrdinal(id, x.Metadata.Id) == 0)?
                .Value as T;
        }

        public T GetExtensionByLabel<T>([Required] string label) where T : class, IExtension
        {
            return _extensions?
                .FirstOrDefault(x => (x?.Metadata != null) && string.CompareOrdinal(label, x.Metadata.Label) == 0)?
                .Value as T;
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

        [Background]
        private void LoadHelpConfiguration()
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

        // From https://stackoverflow.com/questions/540749/can-a-c-sharp-class-inherit-attributes-from-its-interface.
        private static IEnumerable<T> GetCustomAttributesIncludingBaseInterfaces<T>(Type type)
        {
            var attributeType = typeof(T);
            return type.GetCustomAttributes(attributeType, true).
                Union(type.GetInterfaces().
                    SelectMany(interfaceType => interfaceType.GetCustomAttributes(attributeType, true))).
                Distinct().Cast<T>();
        }
    }
}
