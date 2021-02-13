using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using ThreatsManager.Engine;

namespace ThreatsManager.Controls
{
    public partial class ExtensionConfig : UserControl
    {
        private string _extensionId;
        private string _assemblyTitle;

        public ExtensionConfig()
        {
            InitializeComponent();
        }

        public void ConfigureExtension(string extensionId)
        {
            _extensionId = extensionId;

            var configuration = Manager.Instance.Configuration;
            _componentName.Text = configuration.GetExtensionName(extensionId);
            _componentType.Text = configuration.GetExtensionType(extensionId);
            _enabled.Checked = configuration.IsEnabled(extensionId);
            _assemblyTitle = configuration.GetExtensionAssemblyTitle(extensionId);
            var properties = configuration.GetParameterNames(extensionId);
            if (properties?.Any() ?? false)
            {
                var panel = _properties.PrimaryGrid;
                foreach (var property in properties)
                {
                    var value = configuration.GetParameterValue(extensionId, property);
                    var row = new GridRow(property, value);
                    row.Cells[1].PropertyChanged += OnPropertyChanged;
                    panel.Rows.Add(row);
                }
            }
            else
            {
                _propertiesLayout.Visible = false;
                Height = _extensionNameLayout.Height + _extensionTypeLayout.Height + 12 +
                         _extensionNameLayout.Padding.Top + _extensionNameLayout.Padding.Bottom +
                         _extensionTypeLayout.Padding.Top + _extensionTypeLayout.Padding.Bottom;
            }
        }

        public string ExtensionId => _extensionId;
        public string ExtensionName => _componentName.Text;
        public string ExtensionType => _componentType.Text;
        public bool IsEnabled => _enabled.Checked;
        public string AssemblyTitle => _assemblyTitle;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var row = cell?.GridRow;
            var property = (string) row.Cells[0].Value;
            Manager.Instance.Configuration.SetParameterValue(_extensionId, property, (string) cell.Value);
        }

        private void _enabled_CheckedChanged(object sender, EventArgs e)
        {
            if (_enabled.Checked)
                Manager.Instance.Configuration.Enable(_extensionId);
            else
                Manager.Instance.Configuration.Disable(_extensionId);
        }
    }
}
