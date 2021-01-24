using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class ExportSchemaDialog : Form, IInitializableObject
    {
        private IThreatModel _model;

        public ExportSchemaDialog()
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

        public ExportSchemaDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;

            var schemas = _model.Schemas?
                .Where(x => !x.NotExportable).ToArray();
            if (schemas?.Any() ?? false)
            {
                _schemas.Items.AddRange(schemas);
            }
        }

        public bool IsInitialized => _model != null;

        public bool IsValid()
        {
            return IsInitialized && !string.IsNullOrWhiteSpace(_fileName.Text) && 
                   !string.IsNullOrWhiteSpace(_name.Text) && _schemas.CheckedItems.Count > 0;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var list = _schemas.CheckedItems.OfType<IPropertySchema>().Select(x => x.Id).ToArray();
            if (list.Any())
            {
                var def = new DuplicationDefinition()
                {
                    PropertySchemas = list
                };

                _model.SaveTemplate(def, _name.Text, _description.Text, _fileName.Text);
            }
        }

        private void _browse_Click(object sender, EventArgs e)
        {
            if (_saveFile.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                _fileName.Text = _saveFile.FileName;
                _ok.Enabled = IsValid();
            }
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _schemas_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var count = _schemas.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked)
                count++;
            else
                count--;

            _ok.Enabled = IsInitialized && !string.IsNullOrWhiteSpace(_fileName.Text) &&
                          !string.IsNullOrWhiteSpace(_name.Text) && count > 0;
        }

        private void _checkAll_Click(object sender, EventArgs e)
        {
            var count = _schemas.Items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    _schemas.SetItemChecked(i, true);
            }
        }

        private void _uncheckAll_Click(object sender, EventArgs e)
        {
            var count = _schemas.Items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    _schemas.SetItemChecked(i, false);
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

        private void ExportSchemaDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
