using System;
using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions
{
    class ExtensionConfigurationManager
    {
        private readonly IThreatModel _model;
        private readonly string _extensionId;
        private readonly ExtensionConfiguration _configuration;
        private bool _dirty;
        
        public ExtensionConfigurationManager([NotNull] IThreatModel model, [Required] string extensionId)
        {
            _model = model;
            _extensionId = extensionId;
            _configuration = model.GetExtensionConfiguration(extensionId);
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
            get => _configuration?.LocalGet<bool>("effort") ?? false;

            set
            {
                _configuration?.LocalSet<bool>("effort", value);
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
