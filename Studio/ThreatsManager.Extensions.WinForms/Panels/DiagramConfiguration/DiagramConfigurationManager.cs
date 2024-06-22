using System.Collections.Generic;
using System.Linq;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.DiagramConfiguration
{
    class DiagramConfigurationManager
    {
        private readonly IThreatModel _model;
        private readonly string _extensionId;
        private readonly ExtensionConfiguration _configuration;
        private bool _dirty;
        
        public DiagramConfigurationManager([NotNull] IThreatModel model)
        {
            _model = model;
            _extensionId = typeof(DiagramConfigurationPanelFactory).GetExtensionId();
            _configuration = model.GetExtensionConfiguration(_extensionId);
        }

        public void Apply()
        {
            if (_dirty)
                _model.SetExtensionConfiguration(_extensionId, _configuration);
        }

        public IEnumerable<ConfigurationData> Data => _configuration?.Data;

        public int DiagramIconSize
        {
            get
            {
                int result;

                var size = _configuration?.GlobalGet<int>("iconSize");
                if ((size ?? 0) == 0)
                    result = 32;
                else
                    result = size.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("iconSize", value);
                _dirty = true;
            }
        }

        public int DiagramIconCenterSize
        {
            get
            {
                int result;

                var size = _configuration?.GlobalGet<int>("iconCenterSize");
                if ((size ?? 0) == 0)
                    result = DiagramIconSize / 2;
                else
                    result = size.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("iconCenterSize", value);
                _dirty = true;
            }
        }

        public int DiagramMarkerSize
        {
            get
            {
                int result;

                var size = _configuration?.GlobalGet<int>("markerSize");
                if ((size ?? 0) == 0)
                    result = DiagramIconSize / 2;
                else
                    result = size.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("markerSize", value);
                _dirty = true;
            }
        }

        public int DiagramZoomFactor
        {
            get
            {
                int result;

                var zoom = _configuration?.GlobalGet<int>("zoomFactor");
                if ((zoom ?? 0) == 0)
                    result = 100;
                else
                    result = zoom.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("zoomFactor", value);
                _dirty = true;
            }
        }

        public int DiagramHorizontalSpacing
        {
            get
            {
                int result;

                var value = _configuration?.GlobalGet<int>("hSpacing");
                if ((value ?? 0) == 0)
                    result = 50;
                else
                    result = value.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("hSpacing", value);
                _dirty = true;
            }
        }

        public int DiagramVerticalSpacing
        {
            get
            {
                int result;

                var value = _configuration?.GlobalGet<int>("vSpacing");
                if ((value ?? 0) == 0)
                    result = 50;
                else
                    result = value.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("vSpacing", value);
                _dirty = true;
            }
        }

        public string MarkerExtension
        {
            get
            {
                string result;

                var value = _configuration?.GlobalGet<string>("marker");
                if (value != null && ExtensionUtils.GetExtension<IMarkerProviderFactory>(value) != null)
                {
                    result = value;
                }
                else
                {
                    result = ExtensionUtils.GetExtensions<IMarkerProviderFactory>()?.FirstOrDefault()?.GetExtensionId();
                }

                return result;
            }

            set
            {
                _configuration?.GlobalSet<string>("marker", value);
                _dirty = true;
            }
        }
        
        public int FlowWrappingWidth
        {
            get
            {
                int result;

                var value = _configuration?.GlobalGet<int>("fWrap");
                if ((value ?? 0) == 0)
                    result = 150;
                else
                    result = value.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("fWrap", value);
                _dirty = true;
            }
        }

        public int EntityWrappingWidth
        {
            get
            {
                int result;

                var value = _configuration?.GlobalGet<int>("eWrap");
                if ((value ?? 0) == 0)
                    result = 150;
                else
                    result = value.Value;

                return result;
            }

            set
            {
                _configuration?.GlobalSet<int>("eWrap", value);
                _dirty = true;
            }
        }
    }
}
