using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class DevOpsConnectionDialog : Form
    {
        private IDevOpsConnector _connector;
        private readonly AccessTokenManager _tokenManager;
        private IEnumerable<string> _urls;

        public DevOpsConnectionDialog()
        {
            InitializeComponent();

            var factories = ExtensionUtils.GetExtensions<IDevOpsConnectorFactory>()?.ToArray();
            if (factories?.Any() ?? false)
            {
                _factories.Items.AddRange(factories);
            }

            _tokenManager = new AccessTokenManager();
        }

        public IDevOpsConnector Connector
        {
            get
            {
                return _connector;
            }

            set
            {
                if (value != null && !string.IsNullOrWhiteSpace(value.Url))
                {
                    var factory = _factories.Items
                        .OfType<IDevOpsConnectorFactory>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.GetExtensionId(), value.FactoryId) == 0);

                    _factories.SelectedItem = factory;
                    _serverUrl.Text = value.Url;
                    _accessToken.Text = _tokenManager.GetToken(value.Url);
                    _projectList.Items.Clear();
                    if (!string.IsNullOrWhiteSpace(value.Project))
                        _projectList.Items.Add(value.Project);
                    _projectList.SelectedIndex = 0;
                }
            }
        }

        public string ProjectName => _projectList.SelectedItem as string;

        private void DevOpsConnectionDialog_Load(object sender, EventArgs e)
        {
            _urls = _tokenManager.Urls;
        }

        private async void _loadProjects_Click(object sender, EventArgs e)
        {
            if (LoadProjectsEnabled())
            {
                var url = _serverUrl.Text;
                if (_factories.SelectedItem is IDevOpsConnectorFactory factory && !string.IsNullOrWhiteSpace(url))
                {
                    if (_connector?.IsConnected() ?? false)
                    {
                        _connector.Disconnect();
                    }

                    _connector = factory.Create();
                    _connector.Connect(url, _accessToken.Text);
                    var projects = (await _connector.GetProjectsAsync())?.ToArray();
                    if (projects?.Any() ?? false)
                    {
                        _tokenManager.SetToken(_serverUrl.Text, _accessToken.Text);
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
        }

        private void _projectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = _connector != null && ProjectName != null;
        }

        private void _connectors_SelectedIndexChanged(object sender, EventArgs e)
        {
            _loadProjects.Enabled = LoadProjectsEnabled();
        }

        private void _serverUrl_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_serverUrl.Text) && _urls.ContainsCaseInsensitive(_serverUrl.Text?.Trim('/')))
            {
                var token = _tokenManager.GetToken(_serverUrl.Text);
                if (!string.IsNullOrWhiteSpace(token))
                    _accessToken.Text = token;
            }
            //else
            //    _accessToken.Text = null;

            _loadProjects.Enabled = LoadProjectsEnabled();
        }

        private void _accessToken_TextChanged(object sender, EventArgs e)
        {
            _loadProjects.Enabled = LoadProjectsEnabled();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (ProjectName != null)
                _connector?.OpenProject(ProjectName);
        }

        private bool LoadProjectsEnabled()
        {
            return _factories.SelectedItem != null && !string.IsNullOrWhiteSpace(_serverUrl.Text) &&
                   !string.IsNullOrWhiteSpace(_accessToken.Text);
        }
    }
}
