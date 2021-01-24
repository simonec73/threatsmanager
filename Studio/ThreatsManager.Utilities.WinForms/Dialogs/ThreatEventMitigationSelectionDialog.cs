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
    public partial class ThreatEventMitigationSelectionDialog : Form, IInitializableObject
    {
        private IThreatEvent _threatEvent;
        private IThreatEventMitigation _mitigation;

        public ThreatEventMitigationSelectionDialog()
        {
            InitializeComponent();

            _controlType.Items.AddRange(Enum.GetNames(typeof(SecurityControlType)));
            _status.Items.AddRange(Enum.GetNames(typeof(MitigationStatus)));
            _status.SelectedIndex = 0;

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
            AddSpellCheck(_directives);
            _spellAsYouType.SetRepaintTimer(500);
       }

        public ThreatEventMitigationSelectionDialog([NotNull] IThreatEvent threatEvent) : this()
        {
            _threatEvent = threatEvent;

            _threatTypeName.Text = threatEvent.ThreatType?.Name;
            _threatEventName.Text = threatEvent.Name;
            _associatedTo.Text = threatEvent.Parent?.Name;
            _associatedTo.Image = threatEvent.Parent.GetImage(ImageSize.Small);
            _superTooltip.SetSuperTooltip(_associatedTo, _threatEvent.Model?.GetSuperTooltipInfo(threatEvent.Parent, false));

            var strengths = threatEvent.Model?.Strengths.Where(x => x.Visible).ToArray();
            if (strengths?.Any() ?? false)
            {
                _strength.Items.AddRange(strengths);
            }

            var alreadyIncludedMitigations = threatEvent.Mitigations?.ToArray();
            var standardMitigations = threatEvent.ThreatType?.Mitigations?
                .Where(x => !(alreadyIncludedMitigations?.Any(y => y.MitigationId == x.MitigationId) ?? false))
                .OrderBy(x => x.Mitigation.Name)
                .ToArray();
            if (standardMitigations?.Any() ?? false)
            {
                _standardMitigation.Items.AddRange(standardMitigations);
                _standardMitigation.Tag = standardMitigations;
            }
            else
            {
                _associateNonstandard.Checked = true;
                _associateStandard.Enabled = false;
            }

            var mitigations = threatEvent.Model?.Mitigations?
                .Where(x => !(alreadyIncludedMitigations?.Any(y => y.MitigationId == x.Id) ?? false) &&
                            !(standardMitigations?.Any(y => y.MitigationId == x.Id) ?? false))
                .OrderBy(x => x.Name)
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                _nonStandardMitigation.Items.AddRange(mitigations);
                _nonStandardMitigation.Tag = mitigations;
            }
            else
            {
                _associateNonstandard.Enabled = false;

                if (!(standardMitigations?.Any() ?? false))
                    _createNew.Checked = true;
            }

            EnableControls();
        }

        public bool IsInitialized => _threatEvent != null;

        public IThreatEventMitigation Mitigation => _mitigation;

        private void _controlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _associateStandard_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void _associateNonstandard_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void _createNew_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            _standardMitigation.Enabled = _associateStandard.Checked;
            _associateToStandard.Enabled = _associateNonstandard.Checked;
            _nonStandardMitigation.Enabled = _associateNonstandard.Checked;
            _newToStandard.Enabled = _createNew.Checked;
            _name.Enabled = _createNew.Checked;
            _description.Enabled = _createNew.Checked;
            _controlType.Enabled = _createNew.Checked;
            _ok.Enabled = IsValid();
        }

        private bool IsValid()
        {
            return IsInitialized && _strength.SelectedItem != null &&
                   ((_associateStandard.Checked && _standardMitigation.SelectedItem != null) ||
                   (_associateNonstandard.Checked && _nonStandardMitigation.SelectedItem != null) ||
                   (_createNew.Checked && !string.IsNullOrWhiteSpace(_name.Text) && _controlType.SelectedItem != null));
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_strength.SelectedItem is IStrength strength &&
                Enum.TryParse<MitigationStatus>(_status.SelectedItem.ToString(), out var status))
            {
                if (_associateStandard.Checked)
                {
                    if (_standardMitigation.SelectedItem is IThreatTypeMitigation threatTypeMitigation)
                    {
                        _mitigation = _threatEvent.AddMitigation(threatTypeMitigation.Mitigation,
                            strength, status, _directives.Text);
                    }
                }
                else if (_associateNonstandard.Checked)
                {
                    if (_nonStandardMitigation.SelectedItem is IMitigation mitigation)
                    {
                        if (_associateToStandard.Checked)
                        {
                            _threatEvent.ThreatType.AddMitigation(mitigation, strength);
                        }

                        _mitigation = _threatEvent.AddMitigation(mitigation, strength,
                            status, _directives.Text);
                    }
                }
                else if (_createNew.Checked)
                {
                    if (Enum.TryParse<SecurityControlType>((string) _controlType.SelectedItem, out var controlType))
                    {
                        var newMitigation = _threatEvent.Model.AddMitigation(_name.Text);
                        newMitigation.Description = _description.Text;
                        newMitigation.ControlType = controlType;

                        if (_newToStandard.Checked)
                        {
                            _threatEvent.ThreatType.AddMitigation(newMitigation, strength);
                        }

                        _mitigation = _threatEvent.AddMitigation(newMitigation, strength,
                            status, _directives.Text);
                    }
                }
            }
        }

        private void _standardMitigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_standardMitigation.SelectedItem is IThreatTypeMitigation mitigation)
            {
                _strength.SelectedItem = mitigation.Strength;
            }
            _ok.Enabled = IsValid();
        }

        private void _strength_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _nonStandardMitigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _strengthNonStandard_SelectedIndexChanged(object sender, EventArgs e)
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
                } else if (comboBox.Tag is IEnumerable<IThreatTypeMitigation> standardMitigations)
                {
                    var selectedSm = GetFilteredItems(filter, standardMitigations)?.ToArray();
                    if (selectedSm?.Any() ?? false)
                    {
                        comboBox.Items.AddRange(selectedSm);
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
 
        private IEnumerable<IThreatTypeMitigation> GetFilteredItems(string filter, IEnumerable<IThreatTypeMitigation> mitigations)
        {
            IEnumerable<IThreatTypeMitigation> result = null;

            if (string.IsNullOrEmpty(filter))
            {
                result = mitigations;
            }
            else
            {
                var lcFilter = filter?.ToLowerInvariant();
                result = mitigations?.Where(x => (x.Mitigation?.Name?.ToLowerInvariant().Contains(lcFilter) ?? false) ||
                                                 (x.Mitigation?.Description?.ToLowerInvariant().Contains(lcFilter) ?? false));
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

        private void _layoutDirectives_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            try
            {
                //_spellAsYouType.CheckAsYouType = false;

                using (var dialog = new TextEditorDialog
                {
                    Text = _directives.Text, 
                    Multiline = true, 
                    ReadOnly = _directives.ReadOnly
                })
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        _directives.Text = dialog.Text;
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

        private void _standardMitigation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(_standardMitigation.Text))
                {
                    _standardMitigation.DroppedDown = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                    _standardMitigation.Text = null;
                    OnComboBoxTextUpdate(_standardMitigation, null);
                }
            }
        }

        private void _nonStandardMitigation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(_nonStandardMitigation.Text))
                {
                    _nonStandardMitigation.DroppedDown = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                    _nonStandardMitigation.Text = null;
                    OnComboBoxTextUpdate(_nonStandardMitigation, null);
                }
            }
        }

        private void ThreatEventMitigationSelectionDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
