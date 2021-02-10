namespace ThreatsManager.Utilities.WinForms
{
    partial class ItemEditor
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
            DeregisterCurrentEventHandlers();
            _itemName.TextChanged -= _itemName_TextChanged;
            _superTooltip.MarkupLinkClick -= _superTooltip_MarkupLinkClick;
            _itemPicture.DoubleClick -= _itemPicture_DoubleClick;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._fixedLayout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._itemDescription = new System.Windows.Forms.RichTextBox();
            this._description = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._titlePanel = new System.Windows.Forms.Panel();
            this._itemPicture = new System.Windows.Forms.PictureBox();
            this._itemNameTypeTitlePanel = new System.Windows.Forms.Panel();
            this._itemName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this._itemType = new System.Windows.Forms.Label();
            this._dynamicLayout = new DevComponents.DotNetBar.PanelEx();
            this._spellAsYouType = new Keyoti.RapidSpell.RapidSpellAsYouType(this.components);
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._refresh = new DevComponents.DotNetBar.ButtonX();
            this._fixedLayout.SuspendLayout();
            this._titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._itemPicture)).BeginInit();
            this._itemNameTypeTitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _fixedLayout
            // 
            this._fixedLayout.BackColor = System.Drawing.Color.White;
            this._fixedLayout.Controls.Add(this._itemDescription);
            this._fixedLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this._fixedLayout.ForeColor = System.Drawing.Color.Black;
            this._fixedLayout.Location = new System.Drawing.Point(0, 45);
            this._fixedLayout.Name = "_fixedLayout";
            // 
            // 
            // 
            this._fixedLayout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._description});
            this._fixedLayout.Size = new System.Drawing.Size(323, 102);
            this._fixedLayout.TabIndex = 3;
            // 
            // _itemDescription
            // 
            this._itemDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._itemDescription.Location = new System.Drawing.Point(63, 4);
            this._itemDescription.Margin = new System.Windows.Forms.Padding(0);
            this._itemDescription.Name = "_itemDescription";
            this._itemDescription.Size = new System.Drawing.Size(256, 94);
            this._itemDescription.TabIndex = 0;
            this._itemDescription.Text = "";
            this._itemDescription.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._itemDescription_LinkClicked);
            this._itemDescription.TextChanged += new System.EventHandler(this._itemDescription_TextChanged);
            // 
            // _description
            // 
            this._description.Control = this._itemDescription;
            this._description.Height = 102;
            this._description.MinSize = new System.Drawing.Size(120, 0);
            this._description.Name = "_description";
            this._description.Text = "<a href=\"Description\">Description</a>";
            this._description.Visible = false;
            this._description.Width = 100;
            this._description.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._description.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._description_MarkupLinkClick);
            // 
            // _titlePanel
            // 
            this._titlePanel.BackColor = System.Drawing.Color.White;
            this._titlePanel.Controls.Add(this._itemPicture);
            this._titlePanel.Controls.Add(this._itemNameTypeTitlePanel);
            this._titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._titlePanel.Location = new System.Drawing.Point(0, 0);
            this._titlePanel.Name = "_titlePanel";
            this._titlePanel.Size = new System.Drawing.Size(323, 45);
            this._titlePanel.TabIndex = 0;
            // 
            // _itemPicture
            // 
            this._itemPicture.Location = new System.Drawing.Point(5, 7);
            this._itemPicture.MaximumSize = new System.Drawing.Size(32, 32);
            this._itemPicture.MinimumSize = new System.Drawing.Size(32, 32);
            this._itemPicture.Name = "_itemPicture";
            this._itemPicture.Size = new System.Drawing.Size(32, 32);
            this._itemPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._itemPicture.TabIndex = 0;
            this._itemPicture.TabStop = false;
            this._itemPicture.Visible = false;
            this._itemPicture.DoubleClick += new System.EventHandler(this._itemPicture_DoubleClick);
            // 
            // _itemNameTypeTitlePanel
            // 
            this._itemNameTypeTitlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._itemNameTypeTitlePanel.Controls.Add(this._refresh);
            this._itemNameTypeTitlePanel.Controls.Add(this._itemName);
            this._itemNameTypeTitlePanel.Controls.Add(this._itemType);
            this._itemNameTypeTitlePanel.Location = new System.Drawing.Point(52, 0);
            this._itemNameTypeTitlePanel.Name = "_itemNameTypeTitlePanel";
            this._itemNameTypeTitlePanel.Size = new System.Drawing.Size(271, 45);
            this._itemNameTypeTitlePanel.TabIndex = 1;
            // 
            // _itemName
            // 
            this._itemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._itemName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._itemName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._itemName.DisabledBackColor = System.Drawing.Color.White;
            this._itemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._itemName.ForeColor = System.Drawing.Color.Black;
            this._itemName.Location = new System.Drawing.Point(2, 17);
            this._itemName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._itemName.Name = "_itemName";
            this._itemName.PreventEnterBeep = true;
            this._itemName.Size = new System.Drawing.Size(262, 22);
            this._itemName.TabIndex = 4;
            this._itemName.Text = "Item Name";
            this._itemName.Visible = false;
            this._itemName.WordWrap = false;
            this._itemName.TextChanged += new System.EventHandler(this._itemName_TextChanged);
            // 
            // _itemType
            // 
            this._itemType.AutoSize = true;
            this._itemType.Location = new System.Drawing.Point(0, 4);
            this._itemType.Name = "_itemType";
            this._itemType.Size = new System.Drawing.Size(54, 13);
            this._itemType.TabIndex = 3;
            this._itemType.Text = "Item Type";
            this._itemType.Visible = false;
            // 
            // _dynamicLayout
            // 
            this._dynamicLayout.AutoScroll = true;
            this._dynamicLayout.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this._dynamicLayout.DisabledBackColor = System.Drawing.Color.Empty;
            this._dynamicLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dynamicLayout.Location = new System.Drawing.Point(0, 147);
            this._dynamicLayout.Name = "_dynamicLayout";
            this._dynamicLayout.Padding = new System.Windows.Forms.Padding(5);
            this._dynamicLayout.Size = new System.Drawing.Size(323, 498);
            this._dynamicLayout.TabIndex = 4;
            this._dynamicLayout.Resize += new System.EventHandler(this._dynamicLayout_Resize);
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
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.DelayTooltipHideDuration = 250;
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._superTooltip.MaximumWidth = 400;
            this._superTooltip.MarkupLinkClick += new DevComponents.DotNetBar.MarkupLinkClickEventHandler(this._superTooltip_MarkupLinkClick);
            // 
            // _refresh
            // 
            this._refresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this._refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._refresh.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this._refresh.Location = new System.Drawing.Point(256, 2);
            this._refresh.Name = "_refresh";
            this._refresh.Size = new System.Drawing.Size(12, 12);
            this._refresh.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._refresh.Symbol = "";
            this._refresh.SymbolSize = 8F;
            this._refresh.TabIndex = 5;
            this._refresh.Click += new System.EventHandler(this._refresh_Click);
            // 
            // ItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._dynamicLayout);
            this.Controls.Add(this._fixedLayout);
            this.Controls.Add(this._titlePanel);
            this.Name = "ItemEditor";
            this.Size = new System.Drawing.Size(323, 645);
            this._fixedLayout.ResumeLayout(false);
            this._titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._itemPicture)).EndInit();
            this._itemNameTypeTitlePanel.ResumeLayout(false);
            this._itemNameTypeTitlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl _fixedLayout;
        private System.Windows.Forms.Panel _titlePanel;
        private System.Windows.Forms.PictureBox _itemPicture;
        private System.Windows.Forms.RichTextBox _itemDescription;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _description;
        private System.Windows.Forms.Panel _itemNameTypeTitlePanel;
        private DevComponents.DotNetBar.Controls.TextBoxX _itemName;
        private System.Windows.Forms.Label _itemType;
        private DevComponents.DotNetBar.PanelEx _dynamicLayout;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private DevComponents.DotNetBar.ButtonX _refresh;
    }
}
