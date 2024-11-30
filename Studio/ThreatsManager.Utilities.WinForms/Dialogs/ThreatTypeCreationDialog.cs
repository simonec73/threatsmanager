﻿using System;
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
    public partial class ThreatTypeCreationDialog : Form, IInitializableObject
    {
        private IThreatModel _model;
        private IThreatType _threatType;
        private RichTextBoxSpellAsYouTypeAdapter _spellName;
        private RichTextBoxSpellAsYouTypeAdapter _spellDescription;

        public ThreatTypeCreationDialog()
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

        public ThreatTypeCreationDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;
            var severities = model.Severities?.Where(x => x.Visible).ToArray();
            if (severities?.Any() ?? false)
                _severity.Items.AddRange(severities);
        }

        public bool IsInitialized => _model != null;

        private bool IsValid()
        {
            return IsInitialized && !string.IsNullOrWhiteSpace(_name.Text) &&
                _severity.SelectedItem is ISeverity;
        }

        public IThreatType ThreatType => _threatType;

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                using (var scope = UndoRedoManager.OpenScope("Add Threat Type"))
                {
                    _threatType = _model.AddThreatType(_name.Text, _severity.SelectedItem as ISeverity);
                    if (!string.IsNullOrWhiteSpace(_description.Text) && _threatType != null)
                        _threatType.Description = _description.Text;
                    scope?.Complete();
                }
            }
        }

        private void _severity_SelectedIndexChanged(object sender, EventArgs e)
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
