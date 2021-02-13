namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    partial class MitigationThreatTypeSelectionDialog
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
            this._mitigation = new System.Windows.Forms.Label();
            this._newThreatTypeStrength = new System.Windows.Forms.ComboBox();
            this._createNew = new System.Windows.Forms.RadioButton();
            this._useExisting = new System.Windows.Forms.RadioButton();
            this._threatTypes = new System.Windows.Forms.ComboBox();
            this._name = new System.Windows.Forms.TextBox();
            this._description = new System.Windows.Forms.RichTextBox();
            this._existingThreatTypeStrength = new System.Windows.Forms.ComboBox();
            this._severity = new System.Windows.Forms.ComboBox();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutDescription = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.panel1.Location = new System.Drawing.Point(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(223, 13);
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
            this._ok.Location = new System.Drawing.Point(142, 13);
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
            this._layout.Controls.Add(this._mitigation);
            this._layout.Controls.Add(this._newThreatTypeStrength);
            this._layout.Controls.Add(this._createNew);
            this._layout.Controls.Add(this._useExisting);
            this._layout.Controls.Add(this._threatTypes);
            this._layout.Controls.Add(this._name);
            this._layout.Controls.Add(this._description);
            this._layout.Controls.Add(this._existingThreatTypeStrength);
            this._layout.Controls.Add(this._severity);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem11,
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.layoutControlItem9,
            this.layoutControlItem3,
            this.layoutControlItem6,
            this._layoutDescription,
            this.layoutControlItem10,
            this.layoutControlItem8});
            this._layout.Size = new System.Drawing.Size(440, 301);
            this._layout.TabIndex = 0;
            // 
            // _mitigation
            // 
            this._mitigation.AutoSize = true;
            this._mitigation.Location = new System.Drawing.Point(72, 4);
            this._mitigation.Margin = new System.Windows.Forms.Padding(0);
            this._mitigation.Name = "_mitigation";
            this._mitigation.Size = new System.Drawing.Size(364, 13);
            this._mitigation.TabIndex = 0;
            this._mitigation.Text = "label1";
            // 
            // _newThreatTypeStrength
            // 
            this._newThreatTypeStrength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._newThreatTypeStrength.Enabled = false;
            this._newThreatTypeStrength.FormattingEnabled = true;
            this._newThreatTypeStrength.Location = new System.Drawing.Point(93, 275);
            this._newThreatTypeStrength.Margin = new System.Windows.Forms.Padding(0);
            this._newThreatTypeStrength.Name = "_newThreatTypeStrength";
            this._newThreatTypeStrength.Size = new System.Drawing.Size(343, 21);
            this._newThreatTypeStrength.TabIndex = 8;
            this._newThreatTypeStrength.SelectedIndexChanged += new System.EventHandler(this._newThreatTypeStrength_SelectedIndexChanged);
            // 
            // _createNew
            // 
            this._createNew.AutoSize = true;
            this._createNew.Location = new System.Drawing.Point(4, 108);
            this._createNew.Margin = new System.Windows.Forms.Padding(0);
            this._createNew.Name = "_createNew";
            this._createNew.Size = new System.Drawing.Size(432, 17);
            this._createNew.TabIndex = 4;
            this._createNew.Text = "Assign a new Threat Type";
            this._createNew.UseVisualStyleBackColor = true;
            this._createNew.CheckedChanged += new System.EventHandler(this._createNew_CheckedChanged);
            // 
            // _useExisting
            // 
            this._useExisting.AutoSize = true;
            this._useExisting.Checked = true;
            this._useExisting.Location = new System.Drawing.Point(4, 25);
            this._useExisting.Margin = new System.Windows.Forms.Padding(0);
            this._useExisting.Name = "_useExisting";
            this._useExisting.Size = new System.Drawing.Size(432, 17);
            this._useExisting.TabIndex = 1;
            this._useExisting.TabStop = true;
            this._useExisting.Text = "Use Existing Threat Type";
            this._useExisting.UseVisualStyleBackColor = true;
            this._useExisting.CheckedChanged += new System.EventHandler(this._useExisting_CheckedChanged);
            // 
            // _threatTypes
            // 
            this._threatTypes.FormattingEnabled = true;
            this._threatTypes.Location = new System.Drawing.Point(93, 50);
            this._threatTypes.Margin = new System.Windows.Forms.Padding(0);
            this._threatTypes.Name = "_threatTypes";
            this._threatTypes.Size = new System.Drawing.Size(343, 21);
            this._threatTypes.TabIndex = 2;
            this._threatTypes.SelectedIndexChanged += new System.EventHandler(this._threatTypes_SelectedIndexChanged);
            this._threatTypes.TextUpdate += new System.EventHandler(this.OnComboBoxTextUpdate);
            this._threatTypes.KeyDown += new System.Windows.Forms.KeyEventHandler(this._threatTypes_KeyDown);
            // 
            // _name
            // 
            this._name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._name.Enabled = false;
            this._name.Location = new System.Drawing.Point(93, 133);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(343, 20);
            this._name.TabIndex = 5;
            this._name.TextChanged += new System.EventHandler(this._name_TextChanged);
            // 
            // _description
            // 
            this._description.AcceptsTab = true;
            this._description.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._description.Enabled = false;
            this._description.Location = new System.Drawing.Point(93, 161);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(343, 77);
            this._description.TabIndex = 6;
            this._description.Text = "";
            // 
            // _existingThreatTypeStrength
            // 
            this._existingThreatTypeStrength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._existingThreatTypeStrength.FormattingEnabled = true;
            this._existingThreatTypeStrength.Location = new System.Drawing.Point(93, 79);
            this._existingThreatTypeStrength.Margin = new System.Windows.Forms.Padding(0);
            this._existingThreatTypeStrength.Name = "_existingThreatTypeStrength";
            this._existingThreatTypeStrength.Size = new System.Drawing.Size(343, 21);
            this._existingThreatTypeStrength.TabIndex = 3;
            this._existingThreatTypeStrength.SelectedIndexChanged += new System.EventHandler(this._existingThreatTypeStrength_SelectedIndexChanged);
            // 
            // _severity
            // 
            this._severity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._severity.Enabled = false;
            this._severity.FormattingEnabled = true;
            this._severity.Location = new System.Drawing.Point(93, 246);
            this._severity.Margin = new System.Windows.Forms.Padding(0);
            this._severity.Name = "_severity";
            this._severity.Size = new System.Drawing.Size(343, 21);
            this._severity.TabIndex = 7;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._mitigation;
            this.layoutControlItem11.Height = 21;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Mitigation";
            this.layoutControlItem11.Width = 100;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._useExisting;
            this.layoutControlItem2.Height = 25;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._threatTypes;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem1.Text = "Threat Type";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._existingThreatTypeStrength;
            this.layoutControlItem9.Height = 29;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem9.Text = "Strength";
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._createNew;
            this.layoutControlItem3.Height = 25;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._name;
            this.layoutControlItem6.Height = 28;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem6.Text = "Name";
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutDescription
            // 
            this._layoutDescription.Control = this._description;
            this._layoutDescription.Height = 99;
            this._layoutDescription.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MinSize = new System.Drawing.Size(120, 0);
            this._layoutDescription.Name = "_layoutDescription";
            this._layoutDescription.Padding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this._layoutDescription.Text = "<a href=\"Description\">Description</a>";
            this._layoutDescription.Width = 100;
            this._layoutDescription.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._layoutDescription_MarkupLinkClick);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._severity;
            this.layoutControlItem10.Height = 29;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Padding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem10.Text = "Severity";
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._newThreatTypeStrength;
            this.layoutControlItem8.Height = 29;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem8.Text = "Strength";
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._name;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Name";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._description;
            this.layoutControlItem5.Height = 84;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Description";
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // MitigationThreatTypeSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(440, 349);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "MitigationThreatTypeSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Threat Type";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MitigationThreatTypeSelectionDialog_FormClosed);
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutDescription;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.ComboBox _threatTypes;
        private System.Windows.Forms.RadioButton _createNew;
        private System.Windows.Forms.RadioButton _useExisting;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.ComboBox _newThreatTypeStrength;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.ComboBox _existingThreatTypeStrength;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.ComboBox _severity;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private System.Windows.Forms.Label _mitigation;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.RichTextBox _description;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}