﻿using System;
using System.Windows.Forms;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class PropertySchemaCreationDialog : Form
    {
        private RichTextBoxSpellAsYouTypeAdapter _spellName;
        private RichTextBoxSpellAsYouTypeAdapter _spellDescription;

        public PropertySchemaCreationDialog()
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

            _spellName = _spellAsYouType.AddSpellCheck(_name);
            _spellDescription = _spellAsYouType.AddSpellCheck(_description);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public string SchemaName => _name.Text;

        public string SchemaNamespace => _namespace.Text;

        public string Description => _description.Text;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_name.Text) &&
                   !string.IsNullOrWhiteSpace(_namespace.Text);
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _namespace_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
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
                    if (dialog.ShowDialog(ActiveForm) == DialogResult.OK)
                        _description.Text = dialog.Text;
                }
            }
            finally
            {
                //_spellAsYouType.CheckAsYouType = true;
            }
        }
    }
}
