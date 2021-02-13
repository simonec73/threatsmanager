namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    partial class ThreatTypeMitigationSelectionDialog
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
            this._threatTypeName = new System.Windows.Forms.Label();
            this._associateExisting = new System.Windows.Forms.RadioButton();
            this._existingMitigation = new System.Windows.Forms.ComboBox();
            this._strengthExisting = new System.Windows.Forms.ComboBox();
            this._createNew = new System.Windows.Forms.RadioButton();
            this._name = new System.Windows.Forms.TextBox();
            this._description = new System.Windows.Forms.RichTextBox();
            this._controlType = new System.Windows.Forms.ComboBox();
            this._strength = new System.Windows.Forms.ComboBox();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutDescription = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem17 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem16 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem19 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem21 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.panel1.Location = new System.Drawing.Point(0, 319);
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
            this._layout.Controls.Add(this._threatTypeName);
            this._layout.Controls.Add(this._associateExisting);
            this._layout.Controls.Add(this._existingMitigation);
            this._layout.Controls.Add(this._strengthExisting);
            this._layout.Controls.Add(this._createNew);
            this._layout.Controls.Add(this._name);
            this._layout.Controls.Add(this._description);
            this._layout.Controls.Add(this._controlType);
            this._layout.Controls.Add(this._strength);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem10,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem9,
            this._layoutDescription,
            this.layoutControlItem12,
            this.layoutControlItem13});
            this._layout.Size = new System.Drawing.Size(440, 319);
            this._layout.TabIndex = 0;
            // 
            // _threatTypeName
            // 
            this._threatTypeName.AutoSize = true;
            this._threatTypeName.Location = new System.Drawing.Point(105, 4);
            this._threatTypeName.Margin = new System.Windows.Forms.Padding(0);
            this._threatTypeName.Name = "_threatTypeName";
            this._threatTypeName.Size = new System.Drawing.Size(331, 13);
            this._threatTypeName.TabIndex = 0;
            this._threatTypeName.Text = "label1";
            // 
            // _associateExisting
            // 
            this._associateExisting.AutoSize = true;
            this._associateExisting.Checked = true;
            this._associateExisting.Location = new System.Drawing.Point(4, 25);
            this._associateExisting.Margin = new System.Windows.Forms.Padding(0);
            this._associateExisting.Name = "_associateExisting";
            this._associateExisting.Size = new System.Drawing.Size(432, 17);
            this._associateExisting.TabIndex = 1;
            this._associateExisting.TabStop = true;
            this._associateExisting.Text = "Associate an Existing Mitigation";
            this._associateExisting.UseVisualStyleBackColor = true;
            this._associateExisting.CheckedChanged += new System.EventHandler(this._associateExisting_CheckedChanged);
            // 
            // _existingMitigation
            // 
            this._existingMitigation.FormattingEnabled = true;
            this._existingMitigation.Location = new System.Drawing.Point(130, 50);
            this._existingMitigation.Margin = new System.Windows.Forms.Padding(0);
            this._existingMitigation.Name = "_existingMitigation";
            this._existingMitigation.Size = new System.Drawing.Size(306, 21);
            this._existingMitigation.TabIndex = 2;
            this._existingMitigation.SelectedIndexChanged += new System.EventHandler(this._existingMitigation_SelectedIndexChanged);
            this._existingMitigation.TextUpdate += new System.EventHandler(this.OnComboBoxTextUpdate);
            this._existingMitigation.KeyDown += new System.Windows.Forms.KeyEventHandler(this._existingMitigation_KeyDown);
            // 
            // _strengthExisting
            // 
            this._strengthExisting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._strengthExisting.FormattingEnabled = true;
            this._strengthExisting.Location = new System.Drawing.Point(130, 79);
            this._strengthExisting.Margin = new System.Windows.Forms.Padding(0);
            this._strengthExisting.Name = "_strengthExisting";
            this._strengthExisting.Size = new System.Drawing.Size(306, 21);
            this._strengthExisting.TabIndex = 3;
            this._strengthExisting.SelectedIndexChanged += new System.EventHandler(this._strengthExisting_SelectedIndexChanged);
            // 
            // _createNew
            // 
            this._createNew.AutoSize = true;
            this._createNew.Location = new System.Drawing.Point(4, 108);
            this._createNew.Margin = new System.Windows.Forms.Padding(0);
            this._createNew.Name = "_createNew";
            this._createNew.Size = new System.Drawing.Size(432, 17);
            this._createNew.TabIndex = 4;
            this._createNew.Text = "Associate a New Mitigation";
            this._createNew.UseVisualStyleBackColor = true;
            this._createNew.CheckedChanged += new System.EventHandler(this._createNew_CheckedChanged);
            // 
            // _name
            // 
            this._name.Enabled = false;
            this._name.Location = new System.Drawing.Point(130, 133);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(306, 20);
            this._name.TabIndex = 5;
            this._name.TextChanged += new System.EventHandler(this._name_TextChanged_1);
            // 
            // _description
            // 
            this._description.AcceptsTab = true;
            this._description.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._description.Enabled = false;
            this._description.Location = new System.Drawing.Point(130, 161);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(306, 94);
            this._description.TabIndex = 6;
            this._description.Text = "";
            // 
            // _controlType
            // 
            this._controlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._controlType.Enabled = false;
            this._controlType.FormattingEnabled = true;
            this._controlType.Location = new System.Drawing.Point(130, 263);
            this._controlType.Margin = new System.Windows.Forms.Padding(0);
            this._controlType.Name = "_controlType";
            this._controlType.Size = new System.Drawing.Size(306, 21);
            this._controlType.TabIndex = 7;
            this._controlType.SelectedIndexChanged += new System.EventHandler(this._controlType_SelectedIndexChanged_1);
            // 
            // _strength
            // 
            this._strength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._strength.Enabled = false;
            this._strength.FormattingEnabled = true;
            this._strength.Location = new System.Drawing.Point(130, 292);
            this._strength.Margin = new System.Windows.Forms.Padding(0);
            this._strength.Name = "_strength";
            this._strength.Size = new System.Drawing.Size(306, 21);
            this._strength.TabIndex = 8;
            this._strength.SelectedIndexChanged += new System.EventHandler(this._strength_SelectedIndexChanged);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._threatTypeName;
            this.layoutControlItem10.Height = 21;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "Threat Type";
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._associateExisting;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._existingMitigation;
            this.layoutControlItem5.Height = 29;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Mitigation";
            this.layoutControlItem5.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._strengthExisting;
            this.layoutControlItem6.Height = 29;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Strength";
            this.layoutControlItem6.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._createNew;
            this.layoutControlItem7.Height = 25;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._name;
            this.layoutControlItem9.Height = 28;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "Name";
            this.layoutControlItem9.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutDescription
            // 
            this._layoutDescription.Control = this._description;
            this._layoutDescription.Height = 99;
            this._layoutDescription.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MinSize = new System.Drawing.Size(120, 0);
            this._layoutDescription.Name = "_layoutDescription";
            this._layoutDescription.Text = "<a href=\"Description\">Description</a>";
            this._layoutDescription.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this._layoutDescription.Width = 100;
            this._layoutDescription.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._layoutDescription_MarkupLinkClick);
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this._controlType;
            this.layoutControlItem12.Height = 29;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "Control Type";
            this.layoutControlItem12.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem12.Width = 100;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this._strength;
            this.layoutControlItem13.Height = 29;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "Strength";
            this.layoutControlItem13.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem13.Width = 100;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Height = 29;
            this.layoutControlItem15.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Text = "Strength";
            this.layoutControlItem15.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem15.Width = 100;
            this.layoutControlItem15.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Height = 84;
            this.layoutControlItem17.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Text = "Description";
            this.layoutControlItem17.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem17.Width = 100;
            this.layoutControlItem17.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Height = 84;
            this.layoutControlItem16.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Text = "Directives";
            this.layoutControlItem16.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem16.Width = 100;
            this.layoutControlItem16.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem19
            // 
            this.layoutControlItem19.Height = 29;
            this.layoutControlItem19.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.Text = "Strength";
            this.layoutControlItem19.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem19.Width = 100;
            this.layoutControlItem19.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._existingMitigation;
            this.layoutControlItem8.Height = 29;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Mitigation";
            this.layoutControlItem8.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem21
            // 
            this.layoutControlItem21.Control = this._strengthExisting;
            this.layoutControlItem21.Height = 29;
            this.layoutControlItem21.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.Text = "Strength";
            this.layoutControlItem21.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem21.Width = 100;
            this.layoutControlItem21.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._name;
            this.layoutControlItem1.Height = 28;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Name";
            this.layoutControlItem1.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._description;
            this.layoutControlItem2.Height = 84;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Description";
            this.layoutControlItem2.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._controlType;
            this.layoutControlItem3.Height = 29;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Control Type";
            this.layoutControlItem3.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem3.Width = 70;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this._strength;
            this.layoutControlItem14.Height = 29;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Text = "Strength";
            this.layoutControlItem14.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem14.Width = 100;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // ThreatTypeMitigationSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(440, 367);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ThreatTypeMitigationSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Associate a Mitigation to the Threat Type";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ThreatTypeMitigationSelectionDialog_FormClosed);
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
        private System.Windows.Forms.Label _threatTypeName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem17;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem16;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem19;
        private System.Windows.Forms.RadioButton _associateExisting;
        private System.Windows.Forms.ComboBox _existingMitigation;
        private System.Windows.Forms.ComboBox _strengthExisting;
        private System.Windows.Forms.RadioButton _createNew;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.RichTextBox _description;
        private System.Windows.Forms.ComboBox _controlType;
        private System.Windows.Forms.ComboBox _strength;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutDescription;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem21;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}