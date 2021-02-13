using System;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class DevOpsConnectionDialog : Form
    {
        private IDevOpsConnector _connector;

        public DevOpsConnectionDialog()
        {
            InitializeComponent();

            var factories = ExtensionUtils.GetExtensions<IDevOpsConnectorFactory>()?.ToArray();
            if (factories?.Any() ?? false)
            {
                _factories.Items.AddRange(factories);
            }
        }

        public IDevOpsConnector Connector
        {
            get
            {
                return _connector;
            }

            set
            {
                if (value != null)
                {
                    var factory = _factories.Items
                        .OfType<IDevOpsConnectorFactory>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.GetExtensionId(), value.FactoryId) == 0);

                    _factories.SelectedItem = factory;
                    _serverUrl.Text = value.Url;
                    _projectList.Items.Clear();
                    _projectList.Items.Add(value.Project);
                    _projectList.SelectedIndex = 0;
                }
            }
        }

        public string ProjectName => _projectList.SelectedItem as string;

        private void _loadProjects_Click(object sender, EventArgs e)
        {
            var url = _serverUrl.Text;
            if (_factories.SelectedItem is IDevOpsConnectorFactory factory && !string.IsNullOrWhiteSpace(url))
            {
                if (_connector?.IsConnected() ?? false)
                {
                    _connector.Disconnect();
                }

                _connector = factory.Create();
                var projects = _connector.Connect(url)?.ToArray();
                if (projects?.Any() ?? false)
                {
                    _projectList.Items.Clear();
                    _projectList.Items.AddRange(projects);
                }
                else
                {
                    _connector.Disconnect();
                    MessageBox.Show(this, "No Project has been found.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void _projectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = _connector != null && ProjectName != null;
        }

        private void _connectors_SelectedIndexChanged(object sender, EventArgs e)
        {
            _loadProjects.Enabled = _factories.SelectedItem != null && !string.IsNullOrWhiteSpace(_serverUrl.Text);
        }

        private void _serverUrl_TextChanged(object sender, EventArgs e)
        {
            _loadProjects.Enabled = _factories.SelectedItem != null && !string.IsNullOrWhiteSpace(_serverUrl.Text);
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (ProjectName != null)
                _connector?.OpenProject(ProjectName);
        }
    }
}
