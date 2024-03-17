using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.DiagramConfiguration
{
    public partial class DiagramConfigurationPanel : UserControl, IConfigurationPanel<Form>, IDesktopAlertAwareExtension
    {
        private DiagramConfigurationManager _configuration;

        public DiagramConfigurationPanel()
        {
            InitializeComponent();
        }

        private readonly Guid _id = Guid.NewGuid();

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void Initialize([NotNull] IThreatModel model)
        {
            _configuration = new DiagramConfigurationManager(model);

            switch (_configuration.DiagramIconSize)
            {
                case 32:
                    _iconSize.Value = (int)IconSize.Size32;
                    break;
                case 64:
                    _iconSize.Value = (int)IconSize.Size64;
                    break;
                case 96:
                    _iconSize.Value = (int)IconSize.Size96;
                    break;
                case 128:
                    _iconSize.Value = (int)IconSize.Size128;
                    break;
                case 256:
                    _iconSize.Value = (int)IconSize.Size256;
                    break;
                default:
                    if (Dpi.Factor.Height >= 8)
                    {
                        _configuration.DiagramIconSize = 256;
                        _iconSize.Value = (int)IconSize.Size256;
                    }
                    else if (Dpi.Factor.Height >= 4)
                    {
                        _configuration.DiagramIconSize = 128;
                        _iconSize.Value = (int)IconSize.Size128;
                    }
                    else if (Dpi.Factor.Height >= 3)
                    {
                        _configuration.DiagramIconSize = 96;
                        _iconSize.Value = (int)IconSize.Size96;
                    }
                    else if (Dpi.Factor.Height >= 2)
                    {
                        _configuration.DiagramIconSize = 64;
                        _iconSize.Value = (int)IconSize.Size64;
                    }
                    else
                    {
                        _configuration.DiagramIconSize = 32;
                        _iconSize.Value = (int)IconSize.Size32;
                    }
                    _configuration.Apply();
                    break;
            }

            _iconCenterSize.Minimum = _configuration.DiagramIconSize / 4;
            _iconCenterSize.Maximum = _configuration.DiagramIconSize - 8;
            if (_configuration.DiagramIconCenterSize < _iconCenterSize.Minimum)
            {
                _iconCenterSize.Value = _iconCenterSize.Minimum;
                _configuration.DiagramIconCenterSize = _iconCenterSize.Value;
                _configuration.Apply();
            }
            else if (_configuration.DiagramIconCenterSize > _iconCenterSize.Maximum)
            {
                _iconCenterSize.Value = _iconCenterSize.Maximum;
                _configuration.DiagramIconCenterSize = _iconCenterSize.Value;
                _configuration.Apply();
            }
            else
                _iconCenterSize.Value = _configuration.DiagramIconCenterSize;
            _markerSize.Maximum = _configuration.DiagramIconSize / 2;
            if (_configuration.DiagramMarkerSize > _markerSize.Maximum)
            {
                _markerSize.Value = _markerSize.Maximum;
                _configuration.DiagramMarkerSize = _markerSize.Value;
                _configuration.Apply();
            }
            else
                _markerSize.Value = _configuration.DiagramMarkerSize;

            var zoomFactor = _configuration.DiagramZoomFactor;
            Zoom zoom;
            switch (zoomFactor)
            {
                case 25:
                    zoom = Zoom.Zoom25;
                    break;
                case 50:
                    zoom = Zoom.Zoom50;
                    break;
                case 75:
                    zoom = Zoom.Zoom75;
                    break;
                case 125:
                    zoom = Zoom.Zoom125;
                    break;
                case 150:
                    zoom = Zoom.Zoom150;
                    break;
                case 200:
                    zoom = Zoom.Zoom200;
                    break;
                case 300:
                    zoom = Zoom.Zoom300;
                    break;
                case 400:
                    zoom = Zoom.Zoom400;
                    break;
                case 500:
                    zoom = Zoom.Zoom500;
                    break;
                case 100:
                default:
                    zoom = Zoom.Zoom100;
                    break;
            }
            _defaultZoom.Value = (int) zoom;

            var extensions = ExtensionUtils.GetExtensions<IMarkerProviderFactory>()?.ToArray();
            if (extensions?.Any() ?? false)
            {
                _marker.Items.AddRange(extensions);
                var selected = _configuration.MarkerExtension;
                _marker.SelectedItem = extensions.FirstOrDefault(x => string.CompareOrdinal(x.GetExtensionId(), selected) == 0);
            }
        }

        public void Apply()
        {
            try
            {
                _configuration.Apply();
            }
            catch (Exception ex)
            {
                ShowWarning?.Invoke(ex.Message);
            }
        }

        public string Label => "Diagrams";
        public Bitmap Icon => Properties.Resources.diagram_white;
        public Bitmap SelectedIcon => Properties.Resources.diagram_black;

        public IEnumerable<ConfigurationData> Configuration => _configuration?.Data;

        private void _iconSize_ValueChanged(object sender, EventArgs e)
        {
            int size = 0;

            var iconSize = (IconSize) _iconSize.Value;
            switch (iconSize)
            {
                case IconSize.Size32:
                    size = 32;
                    break;
                case IconSize.Size64:
                    size = 64;
                    break;
                case IconSize.Size96:
                    size = 96;
                    break;
                case IconSize.Size128:
                    size = 128;
                    break;
                case IconSize.Size256:
                    size = 256;
                    break;
            }

            if (iconSize > 0)
            {
                _iconSizeText.Text = $"{size} pixels";
                _configuration.DiagramIconSize = size;

                var value = size - 8;
                if (_iconCenterSize.Value > value)
                {
                    _iconCenterSize.Value = value;
                    _configuration.DiagramIconCenterSize = value;

                }
                _iconCenterSize.Maximum = value;

                value = size / 2;
                if (_markerSize.Value > value)
                {
                    _markerSize.Value = value;
                    _configuration.DiagramMarkerSize = value;

                }
                _markerSize.Maximum = size / 2;
            }
        }

        private void _iconCenterSize_ValueChanged(object sender, EventArgs e)
        {
            var size = _iconCenterSize.Value;

            if (size > 0)
            {
                _iconCenterSizeText.Text = $"{size} pixels";
                _configuration.DiagramIconCenterSize = size;
            }
        }

        private void _markerSize_ValueChanged(object sender, EventArgs e)
        {
            var size = _markerSize.Value;

            if (size > 0)
            {
                _markerSizeText.Text = $"{size} pixels";
                _configuration.DiagramMarkerSize = size;
            }
        }

        private void _defaultZoom_ValueChanged(object sender, EventArgs e)
        {
            int zoomFactor = 0;

            var zoom = (Zoom)_defaultZoom.Value;
            switch (zoom)
            {
                case Zoom.Zoom25:
                    zoomFactor = 25;
                    break;
                case Zoom.Zoom50:
                    zoomFactor = 50;
                    break;
                case Zoom.Zoom75:
                    zoomFactor = 75;
                    break;
                case Zoom.Zoom100:
                    zoomFactor = 100;
                    break;
                case Zoom.Zoom125:
                    zoomFactor = 125;
                    break;
                case Zoom.Zoom150:
                    zoomFactor = 150;
                    break;
                case Zoom.Zoom200:
                    zoomFactor = 200;
                    break;
                case Zoom.Zoom300:
                    zoomFactor = 300;
                    break;
                case Zoom.Zoom400:
                    zoomFactor = 400;
                    break;
                case Zoom.Zoom500:
                    zoomFactor = 500;
                    break;
            }

            if (zoomFactor > 0)
            {
                _defaultZoomText.Text = $"{zoomFactor}%";
                _configuration.DiagramZoomFactor = zoomFactor;
            }
        }

        private void _hSpacing_ValueChanged(object sender, EventArgs e)
        {
            var size = _hSpacing.Value;

            if (size > 0)
            {
                _hSpacingText.Text = $"{size} pixels";
                _configuration.DiagramHorizontalSpacing = size;
            }
        }

        private void _vSpacing_ValueChanged(object sender, EventArgs e)
        {
            var size = _vSpacing.Value;

            if (size > 0)
            {
                _vSpacingText.Text = $"{size} pixels";
                _configuration.DiagramVerticalSpacing = size;
            }
        }

        private void _marker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_marker.SelectedItem is IMarkerProviderFactory factory)
            {
                _configuration.MarkerExtension = factory.GetExtensionId();
            }
        }
    }
}
