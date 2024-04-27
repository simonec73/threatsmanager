﻿using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.Layout;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Quality.Annotations
{
    public partial class AnswerControl : UserControl
    {
        private AnnotationAnswer _answer;
        private RichTextBoxSpellAsYouTypeAdapter _spellText;

        public AnswerControl()
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

            _spellText = _spellAsYouType.AddSpellCheck(_text);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public AnnotationAnswer Answer
        {
            get => _answer;

            set
            {
                _answer = value;
                _text.Text = _answer?.Text;
                _answeredBy.Text = _answer?.AnsweredBy;
                _answeredOn.Value = _answer?.AnsweredOn ?? DateTime.MinValue;
                _answeredVia.Text = _answer?.AnsweredVia;
            }
        }

        private void _text_TextChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.Text = _text.Text;
        }

        private void _answeredBy_TextChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.AnsweredBy = _answeredBy.Text;
        }

        private void _answeredOn_ValueChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.AnsweredOn = _answeredOn.Value;
        }

        private void _answeredVia_TextChanged(object sender, EventArgs e)
        {
            if (_answer != null)
                _answer.AnsweredVia = _answeredVia.Text;
        }

        private void _answeredOn_ButtonCustomClick(object sender, EventArgs e)
        {
            _answeredOn.Value = DateTime.Now;
        }

        private void _answeredVia_ButtonCustomClick(object sender, EventArgs e)
        {
            _answeredVia.Text = "Email";
        }

        private void _answeredVia_ButtonCustom2Click(object sender, EventArgs e)
        {
            _answeredVia.Text = "Call";
        }

        private void layoutControlItem3_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            if (sender is LayoutControlItem layoutControlItem)
            {
                if (layoutControlItem.Control is RichTextBox richTextBox)
                {
                    using (var dialog = new TextEditorDialog
                    {
                        Multiline = true, 
                        Text = richTextBox.Text,
                        ReadOnly = richTextBox.ReadOnly
                    })
                    {
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            richTextBox.Text = dialog.Text;
                    }
                }
            }
        }
    }
}
