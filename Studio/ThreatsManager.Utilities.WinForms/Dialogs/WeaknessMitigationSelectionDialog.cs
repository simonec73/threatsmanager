using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class WeaknessMitigationSelectionDialog : Form, IInitializableObject
    {
        private IWeakness _weakness;
        private IWeaknessMitigation _mitigation;

        public WeaknessMitigationSelectionDialog()
        {
            InitializeComponent();

            _controlType.Items.AddRange(Enum.GetNames(typeof(SecurityControlType)));

            try
            {
                _spellAsYouType.UserDictionaryFile = SpellCheckConfig.UserDictionary;
            }
            catch
            {
                // User Dictionary File is optional. If it is not possible to create it, then let's simply block it.
                _spellAsYouType.UserDictionaryFile = null;
            }

            AddSpellCheck(_name);
            AddSpellCheck(_description);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public WeaknessMitigationSelectionDialog([NotNull] IWeakness weakness) : this()
        {
            _weakness = weakness;
            _weaknessName.Text = weakness.Name;

            var strengths = _weakness.Model?.Strengths?.ToArray();
            if (strengths?.Any() ?? false)
            {
                _strength.Items.AddRange(strengths);
                _strengthExisting.Items.AddRange(strengths);
            }

            var alreadyIncludedMitigations = weakness.Mitigations?.ToArray();

            var mitigations = weakness.Model?.Mitigations?
                .Where(x => !(alreadyIncludedMitigations?.Any(y => y.MitigationId == x.Id) ?? false))
                .OrderBy(x => x.Name)
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                _existingMitigation.Items.AddRange(mitigations);
                _existingMitigation.Tag = mitigations;
            }
            else
            {
                _createNew.Checked = true;
                _associateExisting.Enabled = false;
                EnableControls();
            }
        }

        public bool IsInitialized => _weakness != null;

        public IWeaknessMitigation Mitigation => _mitigation;

        private void _associateExisting_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void _createNew_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            _existingMitigation.Enabled = _associateExisting.Checked;
            _strengthExisting.Enabled = _associateExisting.Checked;
            _name.Enabled = _createNew.Checked;
            _description.Enabled = _createNew.Checked;
            _controlType.Enabled = _createNew.Checked;
            _strength.Enabled = _createNew.Checked;
            _ok.Enabled = IsValid();
        }

        private bool IsValid()
        {
            return IsInitialized &&
                   (_associateExisting.Checked && _existingMitigation.SelectedItem != null &&
                    _strengthExisting.SelectedItem != null) ||
                   (_createNew.Checked && !string.IsNullOrWhiteSpace(_name.Text) && _controlType.SelectedItem != null &&
                    _strength.SelectedItem != null);
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_associateExisting.Checked)
            {
                if (_existingMitigation.SelectedItem is IMitigation mitigation &&
                    _strengthExisting.SelectedItem != null)
                {
                    _mitigation = _weakness.AddMitigation(mitigation, _strengthExisting.SelectedItem as IStrength);
                }
            }
            else if (_createNew.Checked)
            {
                if (Enum.TryParse<SecurityControlType>((string)_controlType.SelectedItem, out var controlType) &&
                    _strength.SelectedItem != null)
                {
                    var newMitigation = _weakness.Model.AddMitigation(_name.Text);
                    newMitigation.Description = _description.Text;
                    newMitigation.ControlType = controlType;
                    _mitigation = _weakness.AddMitigation(newMitigation, _strength.SelectedItem as IStrength);
                }
            }
        }

        private void _existingMitigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _strengthExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _name_TextChanged_1(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _controlType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _strength_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void OnComboBoxTextUpdate(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string filter = comboBox.Text;
                comboBox.Items.Clear();

                if (comboBox.Tag is IEnumerable<IMitigation> mitigations)
                {
                    var selected = GetFilteredItems(filter, mitigations)?.ToArray();
                    if (selected?.Any() ?? false)
                    {
                        comboBox.Items.AddRange(selected);
                    }
                }

                comboBox.DroppedDown = true;
                comboBox.IntegralHeight = true;
                comboBox.SelectedIndex = -1;
                comboBox.Text = filter;
                comboBox.SelectionStart = filter.Length;
                comboBox.SelectionLength = 0;
            }
        }

        private IEnumerable<IIdentity> GetFilteredItems(string filter, IEnumerable<IIdentity> identities)
        {
            IEnumerable<IIdentity> result = null;

            if (string.IsNullOrEmpty(filter))
            {
                result = identities;
            }
            else
            {
                var lcFilter = filter?.ToLowerInvariant();
                result = identities?.Where(x => (x.Name?.ToLowerInvariant().Contains(lcFilter) ?? false) ||
                                                (x.Description?.ToLowerInvariant().Contains(lcFilter) ?? false));
            }

            return result;
        }

        private void _layoutDescription_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            try
            {
                //_spellAsYouType.CheckAsYouType = false;

                using (var dialog = new TextEditorDialog
                {
                    Text = _description.Text, 
                    Multiline = true, 
                    ReadOnly = _description.ReadOnly
                })
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        _description.Text = dialog.Text;
                }
            }
            finally
            {
                //_spellAsYouType.CheckAsYouType = true;
            }
        }

        private void AddSpellCheck([NotNull] TextBoxBase control)
        {
            try
            {
                if (control is RichTextBox richTextBox)
                {
                    _spellAsYouType.AddTextComponent(new RichTextBoxSpellAsYouTypeAdapter(richTextBox, 
                        _spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase));
                }
                else
                {
                    _spellAsYouType.AddTextBoxBase(control);
                }
            }
            catch
            {
            }
        }

        private void _existingMitigation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(_existingMitigation.Text))
                {
                    _existingMitigation.DroppedDown = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                    _existingMitigation.Text = null;
                    OnComboBoxTextUpdate(_existingMitigation, null);
                }
            }
        }

        private void ThreatTypeMitigationSelectionDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
