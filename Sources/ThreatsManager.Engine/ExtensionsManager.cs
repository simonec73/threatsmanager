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
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Training;

namespace ThreatsManager.Engine
{
#pragma warning disable CS0169
    class ExtensionsManager
    {
        private CompositionContainer _container;
        private readonly AggregateCatalog _catalog = new AggregateCatalog();
        private static ExecutionMode _executionMode; 

#pragma warning disable 649
        [ImportMany(typeof(IPanelFactory))]
        [ExtensionDescription("UI Panel")]
        private IEnumerable<Lazy<IPanelFactory, IExtensionMetadata>> _panelFactories;

        [ImportMany(typeof(IPackageManager))]
        [ExtensionDescription("Package Manager")]
        private IEnumerable<Lazy<IPackageManager, IExtensionMetadata>> _packageManagers;

        [ImportMany(typeof(IContextAwareAction))]
        [ExtensionDescription("Context Aware Action")]
        private IEnumerable<Lazy<IContextAwareAction, IExtensionMetadata>> _contextAwareActions;

        [ImportMany(typeof(IInitializer))]
        [ExtensionDescription("Initializer")]
        private IEnumerable<Lazy<IInitializer, IExtensionMetadata>> _initializers;

        [ImportMany(typeof(IMainRibbonExtension))]
        [ExtensionDescription("Main Ribbon Button")]
        private IEnumerable<Lazy<IMainRibbonExtension, IExtensionMetadata>> _mainRibbonExtensions;

        [ImportMany(typeof(IListProviderExtension))]
        [ExtensionDescription("List Provider")]
        private IEnumerable<Lazy<IListProviderExtension, IExtensionMetadata>> _listProviders;

        [ImportMany(typeof(IStatusInfoProviderExtension))]
        [ExtensionDescription("Status Info Provider")]
        private IEnumerable<Lazy<IStatusInfoProviderExtension, IExtensionMetadata>> _statusInfoProviders;

        [ImportMany(typeof(IExtensionInitializer))]
        [ExtensionDescription("Extension Initializer")]
        private IEnumerable<Lazy<IExtensionInitializer, IExtensionMetadata>> _extensionInitializers;

        [ImportMany(typeof(IQualityAnalyzer))]
        [ExtensionDescription("Quality Analyzer")]
        private IEnumerable<Lazy<IQualityAnalyzer, IExtensionMetadata>> _qualityAnalyzers;

        [ImportMany(typeof(IPropertySchemasExtractor))]
        [ExtensionDescription("Property Schemas Extractor")]
        private IEnumerable<Lazy<IPropertySchemasExtractor, IExtensionMetadata>> _propertySchemasExtractors;

        [ImportMany(typeof(IDevOpsConnector))]
        [ExtensionDescription("DevOps Connector")]
        private IEnumerable<Lazy<IDevOpsConnector, IExtensionMetadata>> _devOpsConnectors;

        [ImportMany(typeof(ISettingsPanelProvider))]
        [ExtensionDescription("Settings Panel Provider")]
        private IEnumerable<Lazy<ISettingsPanelProvider, IExtensionMetadata>> _settingsPanelProviders;

        [ImportMany(typeof(IPostLoadProcessor))]
        [ExtensionDescription("Post-Load Threat Model Processor")]
        private IEnumerable<Lazy<IPostLoadProcessor, IExtensionMetadata>> _postLoadProcessors;

        [ImportMany(typeof(IResidualRiskEstimator))]
        [ExtensionDescription("Residual Risk Estimator")]
        private IEnumerable<Lazy<IResidualRiskEstimator, IExtensionMetadata>> _residualRiskEstimator;

        [ImportMany(typeof(IPropertySchemasUpdater))]
        [ExtensionDescription("Property Schema Updater")]
        private IEnumerable<Lazy<IPropertySchemasUpdater, IExtensionMetadata>> _propertySchemaUpdaters;
#pragma warning restore 649

        public void AddExtensionsAssembly([Required] string path)
        {
            _catalog.Catalogs.Add(new AssemblyCatalog(path));
        }

        public void SetExecutionMode(ExecutionMode executionMode)
        {
            _executionMode = executionMode;
        }

        public void Load()
        {
            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(_catalog);

            //Fill the imports of this object  
            try
            {
                _container.ComposeParts(this);

                var assemblies = _catalog.Catalogs.OfType<AssemblyCatalog>().Select(x => x.Assembly).ToArray();
                if (assemblies.Any())
                {
                    foreach (var assembly in assemblies)
                    {
                        TrainingPillsManager.Instance.Add(assembly);
                    }
                }
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
            IEnumerable<string> result = null;

            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute(typeof(ImportManyAttribute)) != null).ToArray();
            if (fields.Any())
            {
                List<string> list = new List<string>();

                foreach (var field in fields)
                {
                    var variable = field.GetValue(this) as IEnumerable<object>;
                    var ids = variable?
                        .Select(x => ((IExtensionMetadata) x.GetType().GetProperty("Metadata")?.GetValue(x)))
                        .Where(x => x != null)
                        .OrderBy(x => x.Priority)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArray();
                    if (ids?.Any() ?? false)
                    {
                        list.AddRange(ids);
                    }
                }

                result = list;
            }

            return result;
        }

        public IEnumerable<string> GetExtensionParameters([Required] string id)
        {
            return GetExtensionMetadata(id)?.Parameters;
        }

        public string GetExtensionType([Required] string id)
        {
            string result = null;

            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute(typeof(ImportManyAttribute)) != null).ToArray();
            if (fields.Any())
            {
                foreach (var field in fields)
                {
                    var variable = field.GetValue(this) as IEnumerable<object>;
                    if (variable?.Select(x => (IExtensionMetadata) x.GetType().GetProperty("Metadata")?.GetValue(x))
                        .Any(x => string.CompareOrdinal(x.Id, id) == 0) ?? false)
                    {
                        var attribute = field.GetCustomAttribute(typeof(ExtensionDescriptionAttribute));
                        if (attribute is ExtensionDescriptionAttribute extensionDescription)
                        {
                            result = extensionDescription.Text;
                        }
                        break;
                    }
                }
            }

            return result;
        }

        public string GetExtensionAssemblyTitle([Required] string id)
        {
            string result = null;

            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute(typeof(ImportManyAttribute)) != null).ToArray();
            if (fields.Any())
            {
                foreach (var field in fields)
                {
                    var variable = field.GetValue(this) as IEnumerable<object>;
                    var lazy = variable?
                        .FirstOrDefault(x => string.CompareOrdinal((((IExtensionMetadata) x.GetType().GetProperty("Metadata")?.GetValue(x))?.Id ?? null), id) == 0);
                    if (lazy != null)
                    {
                        result = lazy.GetType().GetProperty("Value")?.GetValue(lazy)?.GetExtensionAssemblyTitle();
                        break;
                    }
                }
            }

            return result;
        }

        public IExtensionMetadata GetExtensionMetadata([Required] string id)
        {
            IExtensionMetadata result = null;

            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute(typeof(ImportManyAttribute)) != null).ToArray();
            if (fields.Any())
            {
                foreach (var field in fields)
                {
                    var variable = field.GetValue(this) as IEnumerable<object>;
                    IExtensionMetadata metadata = variable?
                        .Select(x => (IExtensionMetadata) x.GetType().GetProperty("Metadata")?.GetValue(x))
                        .FirstOrDefault(x => string.CompareOrdinal(x.Id, id) == 0);
                    if (metadata != null)
                    {
                        result = metadata;
                        break;
                    }
                }
            }

            return result;
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public IEnumerable<Lazy<T, IExtensionMetadata>> GetExtensions<T>() where T : class
        {
            IEnumerable<Lazy<T, IExtensionMetadata>> result = null;

            var type = GetType();
            var field = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(x => x.GetCustomAttribute(typeof(ImportManyAttribute)) != null &&
                            x.FieldType == typeof(IEnumerable<Lazy<T, IExtensionMetadata>>));
            if (field != null)
            {
                result = field.GetValue(this) as IEnumerable<Lazy<T, IExtensionMetadata>>;
            }

            result = result?.Where(x => IsExecutionModeCompliant(x.Metadata.Mode))
                .OrderBy(x => x.Metadata.Priority)
                .Distinct(new ExtensionMetadataEqualityComparer<T>());

            return result;
        }

        public T GetExtension<T>([Required] string id) where T : class
        {
            T result = default(T);

            var type = GetType();
            var field = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(x => x.GetCustomAttribute(typeof(ImportManyAttribute)) != null &&
                                     x.FieldType == typeof(IEnumerable<Lazy<T, IExtensionMetadata>>));
            if (field != null)
            {
                var list = field.GetValue(this) as IEnumerable<Lazy<T, IExtensionMetadata>>;
                result = list?.FirstOrDefault(x => string.CompareOrdinal(x.Metadata.Id, id) == 0)?.Value ?? default(T);
            }

            return result;
        }

        private bool IsExecutionModeCompliant(ExecutionMode requiredMode)
        {
            bool result;

            switch (requiredMode)
            {
                case ExecutionMode.Expert:
                    result = _executionMode == ExecutionMode.Expert;
                    break;
                case ExecutionMode.Simplified:
                    result = _executionMode == ExecutionMode.Expert ||
                             _executionMode == ExecutionMode.Simplified;
                    break;
                case ExecutionMode.Management:
                    result = _executionMode == ExecutionMode.Expert ||
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
    }
}
