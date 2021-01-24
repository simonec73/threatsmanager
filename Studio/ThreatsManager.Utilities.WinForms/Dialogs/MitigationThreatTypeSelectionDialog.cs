using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class MitigationThreatTypeSelectionDialog : Form
    {
        private IThreatModel _model;
        private IThreatType _threatType;
        private IStrength _strength;

        public MitigationThreatTypeSelectionDialog()
        {
            InitializeComponent();

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

        public IThreatType ThreatType => _threatType;
        public IStrength Strength => _strength;

        public void Initialize([NotNull] IMitigation mitigation)
        {
            _model = mitigation.Model;

            _mitigation.Text = mitigation.Name;

            _severity.Items.Clear();
            var severities = _model.Severities?.Where(x => x.Visible).ToArray();
            if (severities?.Any() ?? false)
            {
                _severity.Items.AddRange(severities);
            }

            _newThreatTypeStrength.Items.Clear();
            _existingThreatTypeStrength.Items.Clear();
            var strengths = _model?.Strengths?.Where(x => x.Visible).ToArray();
            if (strengths?.Any() ?? false)
            {
                _newThreatTypeStrength.Items.AddRange(strengths);
                _existingThreatTypeStrength.Items.AddRange(strengths);
            }

            var threatTypes = _model?.ThreatTypes?
                .Where(x => x.Mitigations?.All(y => y.MitigationId != mitigation.Id) ?? true)
                .OrderBy(x => x.Name)
                .ToArray();

            if (threatTypes?.Any() ?? false)
            {
                _threatTypes.Items.AddRange(threatTypes);
                _threatTypes.Tag = threatTypes;
            }
            else
            {
                _useExisting.Enabled = false;
                _createNew.Checked = true;
                _threatTypes.Enabled = false;
                _existingThreatTypeStrength.Enabled = false;
                _name.Enabled = true;
                _description.Enabled = true;
                _severity.Enabled = true;
                _newThreatTypeStrength.Enabled = true;
            }
        }

        private void _threatTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            _threatType = _threatTypes.SelectedItem as IThreatType;
            _ok.Enabled = CalculateOkEnabled();
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = CalculateOkEnabled();
        }

        private void _useExisting_CheckedChanged(object sender, EventArgs e)
        {
            _ok.Enabled = CalculateOkEnabled();

            _threatTypes.Enabled = true;
            _existingThreatTypeStrength.Enabled = true;
            _strength = _existingThreatTypeStrength.SelectedItem as IStrength;
            _name.Enabled = false;
            _description.Enabled = false;
            _severity.Enabled = false;
            _newThreatTypeStrength.Enabled = false;
        }

        private void _createNew_CheckedChanged(object sender, EventArgs e)
        {
            _ok.Enabled = CalculateOkEnabled();

            _threatTypes.Enabled = false;
            _existingThreatTypeStrength.Enabled = false;
            _name.Enabled = true;
            _description.Enabled = true;
            _newThreatTypeStrength.Enabled = true;
            _severity.Enabled = true;
            _strength = _newThreatTypeStrength.SelectedItem as IStrength;
        }

        private void _newThreatTypeStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            _strength = _newThreatTypeStrength.SelectedItem as IStrength;
            _ok.Enabled = CalculateOkEnabled();
        }

        private void _existingThreatTypeStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            _strength = _existingThreatTypeStrength.SelectedItem as IStrength;
            _ok.Enabled = CalculateOkEnabled();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_createNew.Checked && !string.IsNullOrWhiteSpace(_name.Text) &&
                _severity.SelectedItem is ISeverity severity)
            {
                _threatType = _model.AddThreatType(_name.Text, severity);
                if (_threatType != null)
                    _threatType.Description = _description.Text;
            }
        }

        private bool CalculateOkEnabled()
        {
            return Strength != null &&
                   ((_useExisting.Checked && _threatTypes.SelectedIndex >= 0) ||
                   (_createNew.Checked && !string.IsNullOrWhiteSpace(_name.Text) && 
                    _severity.SelectedItem is ISeverity));
        }

        
        private void OnComboBoxTextUpdate(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                string filter = comboBox.Text;
                comboBox.Items.Clear();

                if (comboBox.Tag is IEnumerable<IThreatType> threatTypes)
                {
                    var selected = GetFilteredItems(filter, threatTypes)?.ToArray();
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

        private void _threatTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(_threatTypes.Text))
                {
                    _threatTypes.DroppedDown = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                    _threatTypes.Text = null;
                    OnComboBoxTextUpdate(_threatTypes, null);
                }
            }
        }

        private void MitigationThreatTypeSelectionDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
