namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    partial class TextEditorDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                _spellAsYouType.RemoveAllTextComponents();
            }
            catch
            {
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._zoom = new DevComponents.DotNetBar.Controls.Slider();
            this._text = new System.Windows.Forms.RichTextBox();
            this._textLayoutControlItem = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._spellAsYouType = new Keyoti.RapidSpell.RapidSpellAsYouType(this.components);
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 313);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(245, 13);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(164, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._zoom);
            this._layout.Controls.Add(this._text);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._textLayoutControlItem,
            this.layoutControlItem2});
            this._layout.Size = new System.Drawing.Size(484, 313);
            this._layout.TabIndex = 0;
            // 
            // _zoom
            // 
            // 
            // 
            // 
            this._zoom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._zoom.LabelPosition = DevComponents.DotNetBar.eSliderLabelPosition.Top;
            this._zoom.Location = new System.Drawing.Point(40, 264);
            this._zoom.Margin = new System.Windows.Forms.Padding(0);
            this._zoom.Maximum = 400;
            this._zoom.Minimum = 100;
            this._zoom.Name = "_zoom";
            this._zoom.Size = new System.Drawing.Size(440, 42);
            this._zoom.Step = 10;
            this._zoom.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._zoom.TabIndex = 1;
            this._zoom.Text = "100%";
            this._zoom.TrackMarker = false;
            this._zoom.Value = 100;
            this._zoom.ValueChanged += new System.EventHandler(this._zoom_ValueChanged);
            // 
            // _text
            // 
            this._text.AcceptsTab = true;
            this._text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._text.Location = new System.Drawing.Point(4, 4);
            this._text.Margin = new System.Windows.Forms.Padding(0);
            this._text.Name = "_text";
            this._text.Size = new System.Drawing.Size(476, 252);
            this._text.TabIndex = 0;
            this._text.Text = "";
            this._text.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._text_LinkClicked);
            this._text.MultilineChanged += new System.EventHandler(this._text_MultilineChanged);
            // 
            // _textLayoutControlItem
            // 
            this._textLayoutControlItem.Control = this._text;
            this._textLayoutControlItem.Height = 99;
            this._textLayoutControlItem.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._textLayoutControlItem.MinSize = new System.Drawing.Size(120, 0);
            this._textLayoutControlItem.Name = "_textLayoutControlItem";
            this._textLayoutControlItem.Text = "Text";
            this._textLayoutControlItem.TextVisible = false;
            this._textLayoutControlItem.Width = 100;
            this._textLayoutControlItem.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._zoom;
            this.layoutControlItem2.Height = 50;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Zoom";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _spellAsYouType
            // 
            this._spellAsYouType.AddMenuText = "Add";
            this._spellAsYouType.AllowAnyCase = false;
            this._spellAsYouType.AllowMixedCase = true;
            this._spellAsYouType.AutoCorrectEnabled = true;
            this._spellAsYouType.CheckAsYouType = true;
            this._spellAsYouType.CheckCompoundWords = false;
            this._spellAsYouType.CheckDisabledTextBoxes = false;
            this._spellAsYouType.CheckReadOnlyTextBoxes = false;
            this._spellAsYouType.ConsiderationRange = 300;
            this._spellAsYouType.ContextMenuStripEnabled = true;
            this._spellAsYouType.DictFilePath = null;
            this._spellAsYouType.FindCapitalizedSuggestions = true;
            this._spellAsYouType.GUILanguage = Keyoti.RapidSpell.LanguageType.ENGLISH;
            this._spellAsYouType.IgnoreAllMenuText = "Ignore All";
            this._spellAsYouType.IgnoreCapitalizedWords = false;
            this._spellAsYouType.IgnoreIncorrectSentenceCapitalization = false;
            this._spellAsYouType.IgnoreInEnglishLowerCaseI = false;
            this._spellAsYouType.IgnoreMenuText = "Ignore";
            this._spellAsYouType.IgnoreURLsAndEmailAddresses = true;
            this._spellAsYouType.IgnoreWordsWithDigits = true;
            this._spellAsYouType.IgnoreXML = false;
            this._spellAsYouType.IncludeUserDictionaryInSuggestions = false;
            this._spellAsYouType.LanguageParser = Keyoti.RapidSpell.LanguageType.ENGLISH;
            this._spellAsYouType.LookIntoHyphenatedText = true;
            this._spellAsYouType.OptionsEnabled = true;
            this._spellAsYouType.OptionsFileName = "ThreatsManagerPlatform_Spell.xml";
            this._spellAsYouType.OptionsStorageLocation = Keyoti.RapidSpell.Options.UserOptions.StorageType.IsolatedStorage;
            this._spellAsYouType.RemoveDuplicateWordText = "Remove duplicate word";
            this._spellAsYouType.SeparateHyphenWords = false;
            this._spellAsYouType.ShowAddMenuOption = true;
            this._spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase = true;
            this._spellAsYouType.ShowSuggestionsContextMenu = true;
            this._spellAsYouType.ShowSuggestionsWhenTextIsSelected = false;
            this._spellAsYouType.SuggestionsMethod = Keyoti.RapidSpell.SuggestionsMethodType.HashingSuggestions;
            this._spellAsYouType.SuggestSplitWords = true;
            this._spellAsYouType.TextBoxBase = null;
            this._spellAsYouType.TextComponent = null;
            this._spellAsYouType.UnderlineColor = System.Drawing.Color.Red;
            this._spellAsYouType.UnderlineStyle = Keyoti.RapidSpell.UnderlineStyle.Wavy2;
            this._spellAsYouType.UpdateAllTextBoxes = true;
            this._spellAsYouType.UserDictionaryFile = null;
            this._spellAsYouType.V2Parser = true;
            this._spellAsYouType.WarnDuplicates = true;
            // 
            // TextEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "TextEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Text Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TextEditorDialog_FormClosed);
            this.Load += new System.EventHandler(this.TextEditorDialog_Load);
            this.LocationChanged += new System.EventHandler(this.TextEditorDialog_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.TextEditorDialog_SizeChanged);
            this.Resize += new System.EventHandler(this.TextEditorDialog_Resize);
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.RichTextBox _text;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _textLayoutControlItem;
        private DevComponents.DotNetBar.Controls.Slider _zoom;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}