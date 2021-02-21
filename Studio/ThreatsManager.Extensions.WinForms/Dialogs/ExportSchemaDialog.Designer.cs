namespace ThreatsManager.Extensions.Dialogs
{
    partial class ExportSchemaDialog
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
            this._name = new System.Windows.Forms.TextBox();
            this._schemas = new System.Windows.Forms.CheckedListBox();
            this._browse = new System.Windows.Forms.Button();
            this._fileName = new System.Windows.Forms.TextBox();
            this._uncheckAll = new System.Windows.Forms.Button();
            this._checkAll = new System.Windows.Forms.Button();
            this._description = new System.Windows.Forms.RichTextBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutDescription = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._saveFile = new System.Windows.Forms.SaveFileDialog();
            this.layoutGroup2 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.panel1.Location = new System.Drawing.Point(0, 304);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(274, 13);
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
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(193, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._name);
            this._layout.Controls.Add(this._schemas);
            this._layout.Controls.Add(this._browse);
            this._layout.Controls.Add(this._fileName);
            this._layout.Controls.Add(this._uncheckAll);
            this._layout.Controls.Add(this._checkAll);
            this._layout.Controls.Add(this._description);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this._layoutDescription,
            this.layoutControlItem3,
            this.layoutGroup1});
            this._layout.Size = new System.Drawing.Size(543, 304);
            this._layout.TabIndex = 0;
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(77, 35);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(462, 20);
            this._name.TabIndex = 2;
            this._name.TextChanged += new System.EventHandler(this._name_TextChanged);
            // 
            // _schemas
            // 
            this._schemas.FormattingEnabled = true;
            this._schemas.Location = new System.Drawing.Point(77, 157);
            this._schemas.Margin = new System.Windows.Forms.Padding(0);
            this._schemas.Name = "_schemas";
            this._schemas.Size = new System.Drawing.Size(379, 139);
            this._schemas.TabIndex = 4;
            this._schemas.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._schemas_ItemCheck);
            // 
            // _browse
            // 
            this._browse.Location = new System.Drawing.Point(464, 4);
            this._browse.Margin = new System.Windows.Forms.Padding(0);
            this._browse.Name = "_browse";
            this._browse.Size = new System.Drawing.Size(75, 23);
            this._browse.TabIndex = 1;
            this._browse.Text = "Browse...";
            this._browse.UseVisualStyleBackColor = true;
            this._browse.Click += new System.EventHandler(this._browse_Click);
            // 
            // _fileName
            // 
            this._fileName.Location = new System.Drawing.Point(77, 4);
            this._fileName.Margin = new System.Windows.Forms.Padding(0);
            this._fileName.Name = "_fileName";
            this._fileName.ReadOnly = true;
            this._fileName.Size = new System.Drawing.Size(379, 20);
            this._fileName.TabIndex = 0;
            // 
            // _uncheckAll
            // 
            this._uncheckAll.Location = new System.Drawing.Point(464, 188);
            this._uncheckAll.Margin = new System.Windows.Forms.Padding(0);
            this._uncheckAll.Name = "_uncheckAll";
            this._uncheckAll.Size = new System.Drawing.Size(75, 23);
            this._uncheckAll.TabIndex = 6;
            this._uncheckAll.Text = "Uncheck All";
            this._uncheckAll.UseVisualStyleBackColor = true;
            this._uncheckAll.Click += new System.EventHandler(this._uncheckAll_Click);
            // 
            // _checkAll
            // 
            this._checkAll.Location = new System.Drawing.Point(464, 157);
            this._checkAll.Margin = new System.Windows.Forms.Padding(0);
            this._checkAll.Name = "_checkAll";
            this._checkAll.Size = new System.Drawing.Size(75, 23);
            this._checkAll.TabIndex = 5;
            this._checkAll.Text = "Check All";
            this._checkAll.UseVisualStyleBackColor = true;
            this._checkAll.Click += new System.EventHandler(this._checkAll_Click);
            // 
            // _description
            // 
            this._description.AcceptsTab = true;
            this._description.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._description.Location = new System.Drawing.Point(77, 63);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(462, 86);
            this._description.TabIndex = 3;
            this._description.Text = "";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._fileName;
            this.layoutControlItem1.Height = 28;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "File Name";
            this.layoutControlItem1.Width = 99;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._browse;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 83;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._name;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Export Name";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutDescription
            // 
            this._layoutDescription.Control = this._description;
            this._layoutDescription.Height = 94;
            this._layoutDescription.MinSize = new System.Drawing.Size(120, 0);
            this._layoutDescription.Name = "_layoutDescription";
            this._layoutDescription.Text = "<a href=\"Description\">Description</a>";
            this._layoutDescription.Width = 100;
            this._layoutDescription.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._layoutDescription_MarkupLinkClick);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._schemas;
            this.layoutControlItem3.Height = 99;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Schemas";
            this.layoutControlItem3.Width = 99;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 100;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem8,
            this.layoutControlItem7});
            this.layoutGroup1.MinSize = new System.Drawing.Size(80, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Padding = new System.Windows.Forms.Padding(0);
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 83;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._checkAll;
            this.layoutControlItem8.Height = 31;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 83;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._uncheckAll;
            this.layoutControlItem7.Height = 31;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 83;
            // 
            // _saveFile
            // 
            this._saveFile.DefaultExt = "tmt";
            this._saveFile.Filter = "Threat Model Template (*.tmt)|*.tmt";
            this._saveFile.Title = "Select destination file";
            this._saveFile.RestoreDirectory = true;
            // 
            // layoutGroup2
            // 
            this.layoutGroup2.Height = 100;
            this.layoutGroup2.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutGroup2.MinSize = new System.Drawing.Size(83, 32);
            this.layoutGroup2.Name = "layoutGroup2";
            this.layoutGroup2.Padding = new System.Windows.Forms.Padding(0);
            this.layoutGroup2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup2.Width = 83;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._checkAll;
            this.layoutControlItem5.Height = 31;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 83;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._uncheckAll;
            this.layoutControlItem6.Height = 31;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 83;
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
            this._spellAsYouType.UnderlineStyle = Keyoti.RapidSpell.UnderlineStyle.Wavy;
            this._spellAsYouType.UpdateAllTextBoxes = true;
            this._spellAsYouType.UserDictionaryFile = null;
            this._spellAsYouType.V2Parser = true;
            this._spellAsYouType.WarnDuplicates = true;
            // 
            // ExportSchemaDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(543, 352);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ExportSchemaDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Schema Export";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExportSchemaDialog_FormClosed);
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.Button _browse;
        private System.Windows.Forms.TextBox _fileName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.CheckedListBox _schemas;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.SaveFileDialog _saveFile;
        private System.Windows.Forms.TextBox _name;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Button _uncheckAll;
        private System.Windows.Forms.Button _checkAll;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
        private System.Windows.Forms.RichTextBox _description;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutDescription;
    }
}