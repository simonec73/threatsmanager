using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine;
using ThreatsManager.Engine.Config;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Dialogs
{
    public partial class OptionsForm : Form
    {
        private bool _loading;
        private IThreatModel _model;

        public OptionsForm()
        {
            InitializeComponent();

            _loading = true;

            var folders = Manager.Instance.Configuration.Folders?.ToArray();
            if (folders?.Any() ?? false)
                _folders.Items.AddRange(folders);

            var prefixes = Manager.Instance.Configuration.Prefixes?.ToArray();
            if (prefixes?.Any() ?? false)
            {
                _prefixes.Items.AddRange(prefixes);
                _validationPrefix.Checked = true;
            }

            var certificates = Manager.Instance.Configuration.Certificates?.ToArray();
            if (certificates?.Any() ?? false)
            {
                _certificates.Items.AddRange(certificates);
                _validationCertificates.Checked = true;
            }

            var executionModes = EnumExtensions.GetEnumLabels<ExecutionMode>()?.ToArray();
            if (executionModes?.Any() ?? false)
                _executionMode.Items.AddRange(executionModes);

            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            _executionMode.SelectedItem = config.Mode.GetEnumLabel();
            _tooltip.SetToolTip(_executionMode, config.Mode.GetEnumDescription());
            _enableSmartSave.Checked = config.SmartSave;
            _smartSaveInstances.Value = config.SmartSaveCount;
            _smartSaveAutosave.Value = config.SmartSaveInterval;
            _userDictionary.Text = config.UserDictionary;
            if ((Application.UserAppDataRegistry?.GetValue("Consent", false) is string consentString) &&
                    bool.TryParse(consentString, out var consent))
                _disableTelemetry.Checked = !consent;
            else
                _disableTelemetry.Checked = true;
            _disableHelp.Checked = config.DisableHelp;

            _loading = false;
        }

        public OptionsForm([NotNull] IThreatModel model) : this()
        {
            _model = model;
        }
        
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            var factories = ExtensionUtils.GetExtensions<IConfigurationPanelFactory<Form>>()?.ToArray();
            if (factories?.Any() ?? false)
            {
                foreach (var factory in factories)
                {
                    var panel = factory.Create();
                    if (panel is UserControl control)
                    {
                        panel.Initialize(_model);

                        var sideNavPanel = new SideNavPanel
                        {
                            Dock = System.Windows.Forms.DockStyle.Fill,
                            Location = new System.Drawing.Point(118, 31),
                            Name = panel.Label?.Replace(" ", "_"),
                            Size = new System.Drawing.Size(749, 484)
                        };
                        sideNavPanel.Controls.Add(control);
                        _sideNav.Controls.Add(sideNavPanel);

                        var sideNavItem = new SideNavItem
                        {
                            Name = $"_side{sideNavPanel.Name}",
                            Panel = sideNavPanel,
                            Image = panel.Icon,
                            ImageSize = new Size(24, 24),
                            ImageFixedSize = new Size(24, 24),
                            HoverImage = panel.SelectedIcon,
                            PressedImage = panel.SelectedIcon,
                            Text = panel.Label
                        };

                        _sideNav.Items.Add(sideNavItem);
                    }

                }
            }
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            config.Mode = (_executionMode.SelectedItem as string)?.GetEnumValue<ExecutionMode>() ??
                          ExecutionMode.Expert;
            config.SmartSave = _enableSmartSave.Checked;
            config.SmartSaveCount = (int) _smartSaveInstances.Value;
            config.SmartSaveInterval = (int) _smartSaveAutosave.Value;
            config.UserDictionary = _userDictionary.Text;
            config.DisableHelp = _disableHelp.Checked;
            config.CurrentConfiguration.Save();

            Manager.Instance.Configuration.Folders = _folders.Items.OfType<string>();
            Manager.Instance.Configuration.Prefixes = _validationPrefix.Checked ? _prefixes.Items.OfType<string>() : null;
            Manager.Instance.Configuration.Certificates = _validationCertificates.Checked ? 
                _certificates.Items.OfType<CertificateConfig>() : null;

            Application.UserAppDataRegistry?.SetValue("Consent", !_disableTelemetry.Checked);

            foreach (Control sideNavControl in _sideNav.Controls)
            {
                if (sideNavControl is SideNavPanel sideNavPanel)
                {
                    foreach (Control control in sideNavPanel.Controls)
                    {
                        if (control is IConfigurationPanel<Form> panel)
                            panel.Apply();
                    }
                }
            }

            Close();
        }

        private void _addFolder_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select the folder containing the Extensions that you want to add to the list",
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var path = dialog.SelectedPath;
                if (!_folders.Items.Contains(path))
                    _folders.Items.Add(path);
            }
        }

#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
        private void _addStandardFolder_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ThreatsManagerPlatform");
            if (!_folders.Items.Contains(path))
                _folders.Items.Add(path);
        }
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation

        private void _removeFolder_Click(object sender, EventArgs e)
        {
            var selected = _folders.SelectedItems.OfType<string>().ToArray();
            if (selected.Any())
            {
                foreach (var item in selected)
                    _folders.Items.Remove(item);
            }
        }

        private void _validationPrefix_CheckedChanged(object sender, EventArgs e)
        {
            _prefixes.Enabled = _validationPrefix.Checked;
            _addPrefix.Enabled = _validationPrefix.Checked;
            _addStandardPrefix.Enabled = _validationPrefix.Checked;
            _removePrefix.Enabled = _validationPrefix.Checked;
        }

        private void _addPrefix_Click(object sender, EventArgs e)
        {
            using (var dialog = new GenericInfoRequestDialog()
            {
                RequiredInfo = "Prefix"
            })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var prefix = dialog.Info;
                    if (!_prefixes.Items.Contains(prefix))
                        _prefixes.Items.Add(prefix);
                }
            }
        }
        
        private void _addStandardPrefix_Click(object sender, EventArgs e)
        {
            if (!_prefixes.Items.Contains("ThreatsManager"))
                _prefixes.Items.Add("ThreatsManager");
        }

        private void _removePrefix_Click(object sender, EventArgs e)
        {
            var selected = _prefixes.SelectedItems.OfType<string>().ToArray();
            if (selected.Any())
            {
                foreach (var item in selected)
                    _prefixes.Items.Remove(item);
            }
        }

        private void _validationCertificates_CheckedChanged(object sender, EventArgs e)
        {
            _certificates.Enabled = _validationCertificates.Checked;
            _addCertificate.Enabled = _validationCertificates.Checked;
            _addStandardCertificate.Enabled = _validationCertificates.Checked;
            _removeCertificate.Enabled = _validationCertificates.Checked;
        }

        private void _addCertificate_Click(object sender, EventArgs e)
        {
            using (var dialog = new CertificateSelectionDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (!_certificates.Items.Contains(dialog.Certificate))
                        _certificates.Items.Add(dialog.Certificate);
                }
            }
        }

        private void _addStandardCertificate_Click(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
            var certificate = assembly.GetModules().First().GetSignerCertificate();
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
            if (certificate != null)
            {
                var certConfig = new CertificateConfig(certificate);
                if (!_certificates.Items.Contains(certConfig))
                    _certificates.Items.Add(certConfig);
            }
        }

        private void _removeCertificate_Click(object sender, EventArgs e)
        {
            var selected = _certificates.SelectedItems.OfType<CertificateConfig>().ToArray();
            if (selected.Any())
            {
                foreach (var item in selected)
                    _certificates.Items.Remove(item);
            }
        }

        private void _browseUserDictionary_Click(object sender, EventArgs e)
        {
            _userDictFile.FileName = _userDictionary.Text;
            if (_userDictFile.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                _userDictionary.Text = _userDictFile.FileName;
            }
        }

        private void _disableTelemetry_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading && !_disableTelemetry.Checked && 
                (Application.UserAppDataRegistry?.GetValue("Consent", false) is string consentString) &&
                bool.TryParse(consentString, out var consent))
            {
                if (MessageBox.Show(this, Resources.ConsentMessage,
                        Resources.ConsentCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    _disableTelemetry.Checked = true;
                }
            }

        }

        private void _executionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = _executionMode.SelectedItem.ToString().GetEnumValue<ExecutionMode>();
            _tooltip.SetToolTip(_executionMode, mode.GetEnumDescription());

        }
    }
}
