using System;
using System.IO;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class StrengthCreationDialog : Form, IInitializableObject
    {
        private IThreatModel _threatModel;
        private IStrength _strength;

        public StrengthCreationDialog()
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

        public StrengthCreationDialog([NotNull] IThreatModel threatModel) : this()
        {
            _threatModel = threatModel;
        }

        public bool IsInitialized => _threatModel != null;

        private bool IsValid()
        {
            return IsInitialized && !string.IsNullOrWhiteSpace(_name.Text);
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            int id = GetFreeIdUp(_id.Value);
            if (id == -1)
                id = GetFreeIdDown(_id.Value);
            if (id > 0)
            {
                _strength = _threatModel.AddStrength(id, _name.Text);
                _strength.Description = _description.Text;
            }
        }

        private int GetFreeIdUp(int id)
        {
            int result = id;

            if (_threatModel.GetStrength(id) != null)
            {
                if (id == (int) DefaultStrength.Maximum)
                    result = -1;
                else
                {
                    result = GetFreeIdUp(id + 1);
                }
            }

            return result;
        }

        private int GetFreeIdDown(int id)
        {
            int result = id;

            if (_threatModel.GetStrength(id) != null)
            {
                if (id > (int) DefaultStrength.Negligible)
                {
                    result = GetFreeIdDown(id - 1);
                }
                else
                    result = -1;
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

        private void StrengthCreationDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
