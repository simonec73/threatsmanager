
namespace ThreatsManager.Quality.Annotations
{
    partial class AnnotationControl
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

            if (_spellText != null)
            {
                _spellText.Dispose();
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
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._objectName = new System.Windows.Forms.Label();
            this._askedBy = new DevComponents.DotNetBar.Controls.TextBoxX();
            this._answered = new System.Windows.Forms.CheckBox();
            this._askedOn = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this._askedVia = new DevComponents.DotNetBar.Controls.TextBoxX();
            this._modifiedOn = new System.Windows.Forms.Label();
            this._modifiedBy = new System.Windows.Forms.Label();
            this._createdOn = new System.Windows.Forms.Label();
            this._createdBy = new System.Windows.Forms.Label();
            this._text = new System.Windows.Forms.RichTextBox();
            this._answers = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this._addAnswer = new DevComponents.DotNetBar.ButtonItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this._objectContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._textContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._askedByContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._askedOnContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._askedViaContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._answersContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._answeredContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._spellAsYouType = new Keyoti.RapidSpell.RapidSpellAsYouType(this.components);
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._askedOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._answers)).BeginInit();
            this._answers.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._objectName);
            this.layoutControl1.Controls.Add(this._askedBy);
            this.layoutControl1.Controls.Add(this._answered);
            this.layoutControl1.Controls.Add(this._askedOn);
            this.layoutControl1.Controls.Add(this._askedVia);
            this.layoutControl1.Controls.Add(this._modifiedOn);
            this.layoutControl1.Controls.Add(this._modifiedBy);
            this.layoutControl1.Controls.Add(this._createdOn);
            this.layoutControl1.Controls.Add(this._createdBy);
            this.layoutControl1.Controls.Add(this._text);
            this.layoutControl1.Controls.Add(this._answers);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(6);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutGroup1,
            this._textContainer,
            this._askedByContainer,
            this._askedOnContainer,
            this._askedViaContainer,
            this._answersContainer,
            this._answeredContainer,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControl1.Size = new System.Drawing.Size(1474, 923);
            this.layoutControl1.TabIndex = 0;
            // 
            // _objectName
            // 
            this._objectName.AutoSize = true;
            this._objectName.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._objectName.Location = new System.Drawing.Point(78, 7);
            this._objectName.Margin = new System.Windows.Forms.Padding(0);
            this._objectName.Name = "_objectName";
            this._objectName.Size = new System.Drawing.Size(1388, 36);
            this._objectName.TabIndex = 0;
            this._objectName.Text = "label1";
            this._objectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _askedBy
            // 
            this._askedBy.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._askedBy.Border.Class = "TextBoxBorder";
            this._askedBy.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedBy.ButtonCustom.Symbol = "";
            this._askedBy.ButtonCustom.Visible = true;
            this._askedBy.DisabledBackColor = System.Drawing.Color.White;
            this._askedBy.ForeColor = System.Drawing.Color.Black;
            this._askedBy.Location = new System.Drawing.Point(135, 238);
            this._askedBy.Margin = new System.Windows.Forms.Padding(0);
            this._askedBy.Name = "_askedBy";
            this._askedBy.PreventEnterBeep = true;
            this._askedBy.Size = new System.Drawing.Size(446, 31);
            this._askedBy.TabIndex = 3;
            this._askedBy.ButtonCustomClick += new System.EventHandler(this._askedBy_ButtonCustomClick);
            this._askedBy.TextChanged += new System.EventHandler(this._askedBy_TextChanged);
            // 
            // _answered
            // 
            this._answered.AutoSize = true;
            this._answered.Location = new System.Drawing.Point(8, 802);
            this._answered.Margin = new System.Windows.Forms.Padding(0);
            this._answered.Name = "_answered";
            this._answered.Size = new System.Drawing.Size(1458, 34);
            this._answered.TabIndex = 7;
            this._answered.Text = "Has been fully answered";
            this._answered.UseVisualStyleBackColor = true;
            this._answered.CheckedChanged += new System.EventHandler(this._answered_CheckedChanged);
            // 
            // _askedOn
            // 
            // 
            // 
            // 
            this._askedOn.BackgroundStyle.Class = "DateTimeInputBackground";
            this._askedOn.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.ButtonCustom.Symbol = "";
            this._askedOn.ButtonCustom.Visible = true;
            this._askedOn.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this._askedOn.ButtonDropDown.Visible = true;
            this._askedOn.IsPopupCalendarOpen = false;
            this._askedOn.Location = new System.Drawing.Point(724, 238);
            this._askedOn.Margin = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            // 
            // 
            // 
            this._askedOn.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this._askedOn.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.MonthCalendar.DisplayMonth = new System.DateTime(2020, 12, 1, 0, 0, 0, 0);
            // 
            // 
            // 
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.MonthCalendar.TodayButtonVisible = true;
            this._askedOn.Name = "_askedOn";
            this._askedOn.Size = new System.Drawing.Size(299, 31);
            this._askedOn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._askedOn.TabIndex = 4;
            this._askedOn.ValueChanged += new System.EventHandler(this._askedOn_ValueChanged);
            this._askedOn.ButtonCustomClick += new System.EventHandler(this._askedOn_ButtonCustomClick);
            // 
            // _askedVia
            // 
            this._askedVia.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._askedVia.Border.Class = "TextBoxBorder";
            this._askedVia.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedVia.ButtonCustom.Symbol = "";
            this._askedVia.ButtonCustom.Visible = true;
            this._askedVia.ButtonCustom2.Symbol = "";
            this._askedVia.ButtonCustom2.Visible = true;
            this._askedVia.DisabledBackColor = System.Drawing.Color.White;
            this._askedVia.ForeColor = System.Drawing.Color.Black;
            this._askedVia.Location = new System.Drawing.Point(1166, 238);
            this._askedVia.Margin = new System.Windows.Forms.Padding(0);
            this._askedVia.Name = "_askedVia";
            this._askedVia.Size = new System.Drawing.Size(300, 31);
            this._askedVia.TabIndex = 5;
            this._askedVia.ButtonCustomClick += new System.EventHandler(this._askedVia_ButtonCustomClick);
            this._askedVia.ButtonCustom2Click += new System.EventHandler(this._askedVia_ButtonCustom2Click);
            this._askedVia.TextChanged += new System.EventHandler(this._askedVia_TextChanged);
            // 
            // _modifiedOn
            // 
            this._modifiedOn.AutoSize = true;
            this._modifiedOn.Location = new System.Drawing.Point(872, 890);
            this._modifiedOn.Margin = new System.Windows.Forms.Padding(0);
            this._modifiedOn.Name = "_modifiedOn";
            this._modifiedOn.Size = new System.Drawing.Size(594, 26);
            this._modifiedOn.TabIndex = 11;
            this._modifiedOn.Text = "label4";
            // 
            // _modifiedBy
            // 
            this._modifiedBy.AutoSize = true;
            this._modifiedBy.Location = new System.Drawing.Point(135, 890);
            this._modifiedBy.Margin = new System.Windows.Forms.Padding(0);
            this._modifiedBy.Name = "_modifiedBy";
            this._modifiedBy.Size = new System.Drawing.Size(594, 26);
            this._modifiedBy.TabIndex = 10;
            this._modifiedBy.Text = "label3";
            // 
            // _createdOn
            // 
            this._createdOn.AutoSize = true;
            this._createdOn.Location = new System.Drawing.Point(872, 850);
            this._createdOn.Margin = new System.Windows.Forms.Padding(0);
            this._createdOn.Name = "_createdOn";
            this._createdOn.Size = new System.Drawing.Size(594, 26);
            this._createdOn.TabIndex = 9;
            this._createdOn.Text = "label2";
            // 
            // _createdBy
            // 
            this._createdBy.AutoSize = true;
            this._createdBy.Location = new System.Drawing.Point(135, 850);
            this._createdBy.Margin = new System.Windows.Forms.Padding(0);
            this._createdBy.Name = "_createdBy";
            this._createdBy.Size = new System.Drawing.Size(594, 26);
            this._createdBy.TabIndex = 8;
            this._createdBy.Text = "label1";
            // 
            // _text
            // 
            this._text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._text.Location = new System.Drawing.Point(135, 68);
            this._text.Margin = new System.Windows.Forms.Padding(0);
            this._text.Name = "_text";
            this._text.Size = new System.Drawing.Size(1331, 156);
            this._text.TabIndex = 2;
            this._text.Text = "";
            this._text.TextChanged += new System.EventHandler(this._text_TextChanged);
            // 
            // _answers
            // 
            this._answers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._answers.CloseButtonOnTabsVisible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this._answers.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this._answers.ControlBox.MenuBox.Name = "";
            this._answers.ControlBox.Name = "";
            this._answers.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._answers.ControlBox.MenuBox,
            this._answers.ControlBox.CloseBox});
            this._answers.Controls.Add(this.superTabControlPanel1);
            this._answers.ForeColor = System.Drawing.Color.Black;
            this._answers.Location = new System.Drawing.Point(135, 292);
            this._answers.Margin = new System.Windows.Forms.Padding(0);
            this._answers.Name = "_answers";
            this._answers.ReorderTabsEnabled = false;
            this._answers.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._answers.SelectedTabIndex = 0;
            this._answers.Size = new System.Drawing.Size(1331, 496);
            this._answers.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._answers.TabIndex = 6;
            this._answers.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._addAnswer});
            this._answers.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.Office2010BackstageBlue;
            this._answers.TabItemClose += new System.EventHandler<DevComponents.DotNetBar.SuperTabStripTabItemCloseEventArgs>(this._tabContainer_TabItemClose);
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(1331, 496);
            this.superTabControlPanel1.TabIndex = 1;
            // 
            // _addAnswer
            // 
            this._addAnswer.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._addAnswer.Name = "_addAnswer";
            this._addAnswer.ShowSubItems = false;
            this._addAnswer.Symbol = "";
            this._addAnswer.SymbolColor = System.Drawing.Color.White;
            this._addAnswer.SymbolSize = 12F;
            this._addAnswer.Text = "buttonItem1";
            this._addAnswer.Click += new System.EventHandler(this._addAnswer_Click);
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 61;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._objectContainer});
            this.layoutGroup1.MinSize = new System.Drawing.Size(240, 61);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Padding = new System.Windows.Forms.Padding(0);
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 100;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _objectContainer
            // 
            this._objectContainer.Control = this._objectName;
            this._objectContainer.Height = 50;
            this._objectContainer.MinSize = new System.Drawing.Size(128, 34);
            this._objectContainer.Name = "_objectContainer";
            this._objectContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._objectContainer.Text = "Label:";
            this._objectContainer.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this._objectContainer.Width = 100;
            this._objectContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _textContainer
            // 
            this._textContainer.Control = this._text;
            this._textContainer.Height = 25;
            this._textContainer.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._textContainer.MinSize = new System.Drawing.Size(240, 0);
            this._textContainer.Name = "_textContainer";
            this._textContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._textContainer.Text = "<a href=\"Text\">Text</a>";
            this._textContainer.Width = 100;
            this._textContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._textContainer.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._textContainer_MarkupLinkClick);
            // 
            // _askedByContainer
            // 
            this._askedByContainer.Control = this._askedBy;
            this._askedByContainer.Height = 54;
            this._askedByContainer.MinSize = new System.Drawing.Size(240, 0);
            this._askedByContainer.Name = "_askedByContainer";
            this._askedByContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._askedByContainer.Text = "Asked By";
            this._askedByContainer.Width = 40;
            this._askedByContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _askedOnContainer
            // 
            this._askedOnContainer.Control = this._askedOn;
            this._askedOnContainer.Height = 54;
            this._askedOnContainer.MinSize = new System.Drawing.Size(128, 34);
            this._askedOnContainer.Name = "_askedOnContainer";
            this._askedOnContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._askedOnContainer.Text = "Asked On";
            this._askedOnContainer.Width = 30;
            this._askedOnContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _askedViaContainer
            // 
            this._askedViaContainer.Control = this._askedVia;
            this._askedViaContainer.Height = 54;
            this._askedViaContainer.MinSize = new System.Drawing.Size(240, 0);
            this._askedViaContainer.Name = "_askedViaContainer";
            this._askedViaContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._askedViaContainer.Text = "Asked Via";
            this._askedViaContainer.Width = 30;
            this._askedViaContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _answersContainer
            // 
            this._answersContainer.Control = this._answers;
            this._answersContainer.Height = 75;
            this._answersContainer.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._answersContainer.MinSize = new System.Drawing.Size(128, 34);
            this._answersContainer.Name = "_answersContainer";
            this._answersContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._answersContainer.Text = "Answers";
            this._answersContainer.Width = 100;
            this._answersContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _answeredContainer
            // 
            this._answeredContainer.Control = this._answered;
            this._answeredContainer.Height = 48;
            this._answeredContainer.MinSize = new System.Drawing.Size(64, 38);
            this._answeredContainer.Name = "_answeredContainer";
            this._answeredContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._answeredContainer.Width = 100;
            this._answeredContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._createdBy;
            this.layoutControlItem4.Height = 40;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem4.Text = "Created By";
            this.layoutControlItem4.Width = 50;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._createdOn;
            this.layoutControlItem5.Height = 40;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem5.Text = "Created On";
            this.layoutControlItem5.Width = 50;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._modifiedBy;
            this.layoutControlItem6.Height = 40;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem6.Text = "Modified By";
            this.layoutControlItem6.Width = 50;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._modifiedOn;
            this.layoutControlItem7.Height = 40;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem7.Text = "Modified On";
            this.layoutControlItem7.Width = 50;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // AnnotationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AnnotationControl";
            this.Size = new System.Drawing.Size(1474, 923);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._askedOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._answers)).EndInit();
            this._answers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.RichTextBox _text;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _textContainer;
        private DevComponents.DotNetBar.SuperTabControl _answers;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _answersContainer;
        private System.Windows.Forms.Label _createdBy;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label _modifiedOn;
        private System.Windows.Forms.Label _modifiedBy;
        private System.Windows.Forms.Label _createdOn;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.ButtonItem _addAnswer;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput _askedOn;
        private DevComponents.DotNetBar.Controls.TextBoxX _askedVia;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _askedOnContainer;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _askedViaContainer;
        private System.Windows.Forms.CheckBox _answered;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _answeredContainer;
        private DevComponents.DotNetBar.Controls.TextBoxX _askedBy;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _askedByContainer;
        private System.Windows.Forms.Label _objectName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _objectContainer;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}
