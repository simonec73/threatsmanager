﻿using System;
using System.IO;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class GenericIdentityCreationDialog : Form
    {
        private RichTextBoxSpellAsYouTypeAdapter _spellName;
        private RichTextBoxSpellAsYouTypeAdapter _spellDescription;

        public GenericIdentityCreationDialog()
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

        public string IdentityTypeName
        {
            set => Text = $"{value} creation";
        }

        public string IdentityName
        {
            get => _name.Text;
            set => _name.Text = value;
        }

        public string IdentityDescription
        {
            get => _description.Text;
            set => _description.Text = value;
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = !string.IsNullOrWhiteSpace(_name.Text);
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
    }
}
