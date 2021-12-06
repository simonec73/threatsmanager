using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality
{
    class QualityConfigurationManager
    {
        private readonly IThreatModel _model;
        private readonly string _extensionId;
        private readonly ExtensionConfiguration _configuration;
        private bool _dirty;
        
        public QualityConfigurationManager([NotNull] IThreatModel model, [Required] string extensionId)
        {
            _model = model;
            _extensionId = extensionId;
            _configuration = model.GetExtensionConfiguration(extensionId);
        }

        public void Apply()
        {
            if (_dirty)
                _model.SetExtensionConfiguration(_extensionId, _configuration);

            var schemaManager = new CalculatedSeverityPropertySchemaManager(_model);
            if (EnableCalculatedSeverity)
            {
                schemaManager.AddSupport();
            }
            else
            {
                schemaManager.RemoveSupport();
            }
        }

        public IEnumerable<ConfigurationData> Data => _configuration?.Data;

        public bool EnableCalculatedSeverity
        {
            get => _configuration?.LocalGet<bool>("calculatedSeverity") ?? false;

            set
            {
                _configuration?.LocalSet<bool>("calculatedSeverity", value);
                _dirty = true;
            }
        }
    }
}
