using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Dialogs;
using ThreatsManager.Engine;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace ThreatsManager
{
    public partial class MainForm
    {
        private IEnumerable<IStatusInfoProviderExtension> _providers;

        private void LoadStatusInfoProviders()
        {
           _providers = Manager.Instance.GetExtensions<IStatusInfoProviderExtension>()?.ToArray();

            if (_providers?.Any() ?? false)
            {
                _addStatusInfoProvider.Enabled = true;

                var config = ExtensionsConfigurationManager.GetConfigurationSection();
                var bars = config.StatusBarItems?.ToArray();
                if (bars?.Any() ?? false)
                {
                    foreach (var bar in bars)
                    {
                        var provider =
                            _providers.FirstOrDefault(x => string.CompareOrdinal(bar, x.GetExtensionId()) == 0);
                        if (provider != null)
                        {
                            AddProviderLabel(provider);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(config.UserDictionary))
                {
                    config.UserDictionary = Path.Combine(Program.Folder, "ThreatsManagerPlatform_userdict.txt");
                    config.CurrentConfiguration.Save();
                }
            }
        }

        private void _addStatusInfoProvider_Click(object sender, EventArgs e)
        {
            var existing = GetLoadedStatusInfoProviders()?.ToArray();
            var providers = _providers?
                .Where(x => !(existing?.Contains(x.GetExtensionId()) ?? false))
                .ToArray();

            if (providers?.Any() ?? false)
            {
                using (var dialog = new StatusBarItemSelectionDialog(providers))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var provider = dialog.Provider;
                        if (provider != null)
                        {
                            AddProviderLabel(provider);
                            UpdateProviderConfig();
                        }
                    }
                }
            }
        }

        private void AddProviderLabel([NotNull] IStatusInfoProviderExtension provider)
        {
            var id = provider.GetExtensionId();
            provider.Initialize(_model);
            var label = new LabelItem()
            {
                Tag = id,
                PaddingRight = 20
            };
            _statusBar.Items.Insert(_statusBar.Items.Count - 1, label);
            label.Text = provider.CurrentStatus;
            provider.UpdateInfo += UpdateStatusInfo;
            label.Click += ClickOnStatusInfo;
        }

        private void UpdateProviderConfig()
        {
            var config = ExtensionsConfigurationManager.GetConfigurationSection();
            var providers = GetLoadedStatusInfoProviders()?.ToArray();
            if (providers?.Any() ?? false)
            {
                config.StatusBarItems = providers;
                config.CurrentConfiguration.Save();
            }
        }

        private IEnumerable<string> GetLoadedStatusInfoProviders()
        {
            return _statusBar.Items.OfType<LabelItem>().Select(x => x.Tag as string)
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private void ClickOnStatusInfo(object sender, EventArgs e)
        {
            if (sender is LabelItem label && label.Tag is string id)
            {
                var existing = GetLoadedStatusInfoProviders()?.ToArray();
                var providers = _providers?
                    .Where(x => (string.CompareOrdinal(id, x.GetExtensionId()) == 0) ||
                                !(existing?.Contains(x.GetExtensionId()) ?? false))
                    .ToArray();

                if (providers?.Any() ?? false)
                {
                    using (var dialog = new StatusBarItemSelectionDialog(providers)
                    {
                        Provider = _providers.FirstOrDefault(x =>
                            string.CompareOrdinal(id, x.GetExtensionId()) == 0)
                    })
                    {
                        var result = dialog.ShowDialog(this);
                        if (result == DialogResult.OK)
                        {
                            var provider = dialog.Provider;
                            if (provider != null)
                            {
                                var newId = provider.GetExtensionId();
                                if (string.CompareOrdinal(id, newId) != 0)
                                {
                                    provider.Initialize(_model);
                                    label.Text = provider.CurrentStatus;
                                    label.Tag = newId;
                                    provider.UpdateInfo += UpdateStatusInfo;
                                    UpdateProviderConfig();
                                    _statusBar.Refresh();
                                }
                            }
                        }
                        else if (result == DialogResult.Abort)
                        {
                            dialog.Provider?.Dispose();
                            _statusBar.Items.Remove(label);
                            UpdateProviderConfig();
                            _statusBar.Refresh();
                        }
                    }
                }
            }
        }

        [Dispatched]
        private void UpdateStatusInfo(string id, string status)
        {
            var label = _statusBar.Items.OfType<LabelItem>()
                .FirstOrDefault(x => string.CompareOrdinal(id, x.Tag as string) == 0);
            if (label != null)
                label.Text = status;

            _statusBar.Refresh();
        }

        private void UpdateStatusInfoProviders()
        {
            var labels = _statusBar.Items.OfType<LabelItem>().ToArray();
            if (labels.Any() && (_providers?.Any() ?? false))
            {
                foreach (var label in labels)
                {
                    if (label.Tag is string id && !string.IsNullOrWhiteSpace(id))
                    {
                        var provider = _providers
                            .FirstOrDefault(x => string.CompareOrdinal(x.GetExtensionId(), id) == 0);

                        if (provider != null)
                        {
                            provider.Initialize(_model);
                            label.Text = provider.CurrentStatus;
                        }
                    }
                }
            }
        }
    }
}