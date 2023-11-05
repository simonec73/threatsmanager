using System.Collections.Generic;
using System.Linq;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Configuration
{
    class ExtensionConfigurationManager
    {
        private readonly IThreatModel _model;
        private readonly string _extensionId;
        private readonly ExtensionConfiguration _configuration;
        private bool _dirty;
        
        public ExtensionConfigurationManager([NotNull] IThreatModel model)
        {
            _model = model;
            _extensionId = typeof(ConfigurationPanelFactory).GetExtensionId();
            _configuration = model.GetExtensionConfiguration(_extensionId);
        }

        public void Apply()
        {
            if (_dirty)
                _model.SetExtensionConfiguration(_extensionId, _configuration);

            var schemaManager = new EffortPropertySchemaManager(_model);
            if (EnableEffort)
            {
                schemaManager.AddSupport();
            }
            else
            {
                schemaManager.RemoveSupport();
            }
        }

        public IEnumerable<ConfigurationData> Data => _configuration?.Data;

        public bool EnableEffort
        {
            get => _configuration?.GlobalGet<bool>("effort") ?? _configuration?.LocalGet<bool>("effort") ?? false;

            set
            {
                _configuration?.GlobalSet<bool>("effort", value);
                _dirty = true;
            }
        }

        public int NormalizationReference
        {
            get => _configuration?.GlobalGet<int>("normalization") ?? 0;

            set
            {
                _configuration?.GlobalSet<int>("normalization", value);
                _dirty = true;
            }
        }
    }
}
