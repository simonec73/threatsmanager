using System;
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
    public partial class MitigationCreationDialog : Form, IInitializableObject
    {
        private IThreatModel _threatModel;

        private IMitigation _mitigation;

        public MitigationCreationDialog()
        {
            InitializeComponent();

            _controlType.Items.AddRange(EnumExtensions.GetEnumLabels<SecurityControlType>().ToArray());

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

        public MitigationCreationDialog([NotNull] IThreatModel model) : this()
        {
            _threatModel = model;
        }

        public bool IsInitialized => _threatModel != null;

        public IMitigation Mitigation => _mitigation;

        private void _controlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private bool IsValid()
        {
            return IsInitialized && !string.IsNullOrWhiteSpace(_name.Text) && _controlType.SelectedIndex >= 0;
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse<SecurityControlType>((string) _controlType.SelectedItem, out var controlType))
            {
                _mitigation = _threatModel.AddMitigation(_name.Text);
                _mitigation.Description = _description.Text;
                _mitigation.ControlType = controlType;
            }
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

        private void MitigationCreationDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
