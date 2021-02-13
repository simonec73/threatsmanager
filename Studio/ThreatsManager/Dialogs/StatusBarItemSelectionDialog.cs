using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class StatusBarItemSelectionDialog : Form
    {
        public StatusBarItemSelectionDialog()
        {
            InitializeComponent();
        }

        public StatusBarItemSelectionDialog([NotNull] IEnumerable<IStatusInfoProviderExtension> providers) : this()
        {
            _providers.Items.AddRange(providers.ToArray());
        }

        private void _providers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_providers.SelectedItem is IStatusInfoProviderExtension provider)
            {
                _description.Text = provider.Description;
                _ok.Enabled = true;
            }
            else
                _ok.Enabled = false;
        }

        public IStatusInfoProviderExtension Provider
        {
            get => _providers.SelectedItem as IStatusInfoProviderExtension;
            set
            {
                _providers.SelectedItem = value;
                _remove.Enabled = value != null;
            }
        }
    }
}
