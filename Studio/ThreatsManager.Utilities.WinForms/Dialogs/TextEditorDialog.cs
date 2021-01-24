using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Layout;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class TextEditorDialog : Form
    {
        private bool _loading;
        private static int _persistentZoom = 100;
        private static FormWindowState _windowState = FormWindowState.Normal;
        private static Size _persistentSize;
        private static Point _persistentLocation;

        public TextEditorDialog()
        {
            _loading = true;

            InitializeComponent();
            
            if (_persistentSize.IsEmpty)
            {
                _persistentSize = this.Size;
            }
            if (_persistentLocation.IsEmpty)
            {
                _persistentLocation = new Point((int)(100 * Dpi.Factor.Width), (int)(100 * Dpi.Factor.Height));
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
            }

            try
            {
                _spellAsYouType.UserDictionaryFile = SpellCheckConfig.UserDictionary;
            }
            catch
            {
                // User Dictionary File is optional. If it is not possible to create it, then let's simply block it.
                _spellAsYouType.UserDictionaryFile = null;
            }

            AddSpellCheck(_text);
            _spellAsYouType.SetRepaintTimer(500);

            _loading = false;
        }

        private void TextEditorDialog_Load(object sender, EventArgs e)
        {
            _zoom.Value = _persistentZoom;

            this.SuspendLayout();
            if (_windowState == FormWindowState.Maximized)
            {
                Location = _persistentLocation;
                WindowState = _windowState;
            }
            else
            {
                Size = _persistentSize;
                Location = _persistentLocation;
            }
            _spellAsYouType.ForceCheckAll();
            this.ResumeLayout();
        }

        public override string Text
        {
            get => _text?.Text;
            set
            {
                if (_text != null)
                    _text.Text = value;
            }
        }

        public bool Multiline
        {
            get => _text.Multiline;
            set => _text.Multiline = value;
        }

        public bool ReadOnly
        {
            get => _text.ReadOnly;
            set => _text.ReadOnly = value;
        }

        private void _zoom_ValueChanged(object sender, EventArgs e)
        {
            _zoom.Text = $"{_zoom.Value}%";
            _text.ZoomFactor = ((float) _zoom.Value) / 100.0f;
            _persistentZoom = _zoom.Value;
            SetSingleLineHeight();
        }

        private void _text_MultilineChanged(object sender, EventArgs e)
        {
            if (_text.Multiline)
            {
                _layout.SuspendLayout();
                _textLayoutControlItem.HeightType = eLayoutSizeType.Percent;
                _textLayoutControlItem.Height = 99;
                _layout.ResumeLayout();
            }
            else
            {
                SetSingleLineHeight();
            }
        }
        
        private void SetSingleLineHeight()
        {
            if (!_text.Multiline)
            {
                _layout.SuspendLayout();
                _textLayoutControlItem.HeightType = eLayoutSizeType.Absolute;
                _textLayoutControlItem.Height = Convert.ToInt32(28.0 * Dpi.Factor.Height * _zoom.Value / 100.0f);
                _layout.ResumeLayout();
            }
        }

        private void TextEditorDialog_Resize(object sender, EventArgs e)
        { 
            if (!_loading)
            {
                _windowState = WindowState;
            }
        }

        private void TextEditorDialog_LocationChanged(object sender, EventArgs e)
        {
            if (!_loading && WindowState != FormWindowState.Maximized)
                _persistentLocation = Location;
        }

        private void TextEditorDialog_SizeChanged(object sender, EventArgs e)
        {
            if (!_loading && WindowState != FormWindowState.Maximized)
                _persistentSize = Size;
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

        private void _text_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(e.LinkText,
                    @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]",
                    RegexOptions.IgnoreCase))
                {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    Process.Start(e.LinkText);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                }
            }
            catch
            {
                // Ignore the error because the link is simply not trusted.
            }
        }

        private void TextEditorDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
