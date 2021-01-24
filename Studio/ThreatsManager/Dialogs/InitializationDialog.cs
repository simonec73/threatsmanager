using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.Dialogs
{
    public partial class InitializationDialog : Form
    {
        public InitializationDialog()
        {
            InitializeComponent();

            var modes = EnumExtensions.GetEnumLabels<ExecutionMode>()?.ToArray();
            if (modes?.Any() ?? false)
            {
                _executionModes.Items.AddRange(modes);
                _executionModes.SelectedItem = ExecutionMode.Simplified.GetEnumLabel();
                _tooltip.SetToolTip(_executionModes, ExecutionMode.Simplified.GetEnumDescription());
            }
        }

        public bool AutomaticExtensionsConfig => _automaticConfiguration.Checked;

        public ExecutionMode Mode => (_executionModes.SelectedItem as string)?.GetEnumValue<ExecutionMode>() ??
                                     ExecutionMode.Simplified;

        public bool SmartSave => !_smartSaveOff.Checked;

        public int SmartSaveCount
        {
            get
            {
                int result = 0;

                if (_smartSaveLow.Checked)
                    result = 3;
                else if (_smartSaveHigh.Checked)
                    result = 5;

                return result;
            }
        }

        public int SmartSaveInterval
        {
            get
            {
                int result = 0;

                if (_smartSaveLow.Checked)
                    result = 10;
                else if (_smartSaveHigh.Checked)
                    result = 3;

                return result;
            }
        }

        private void _initWizard_FinishButtonClick(object sender, CancelEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void _executionModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mode = _executionModes.SelectedItem.ToString().GetEnumValue<ExecutionMode>();
            _tooltip.SetToolTip(_executionModes, mode.GetEnumDescription());
        }
    }
}
