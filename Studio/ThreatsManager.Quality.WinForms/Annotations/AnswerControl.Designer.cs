
namespace ThreatsManager.Quality.Annotations
{
    partial class AnswerControl
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
            this._text = new System.Windows.Forms.RichTextBox();
            this._answeredBy = new System.Windows.Forms.TextBox();
            this._answeredOn = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this._answeredVia = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._spellAsYouType = new Keyoti.RapidSpell.RapidSpellAsYouType(this.components);
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._answeredOn)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._text);
            this.layoutControl1.Controls.Add(this._answeredBy);
            this.layoutControl1.Controls.Add(this._answeredOn);
            this.layoutControl1.Controls.Add(this._answeredVia);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.layoutControl1.Size = new System.Drawing.Size(522, 320);
            this.layoutControl1.TabIndex = 0;
            // 
            // _text
            // 
            this._text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._text.Location = new System.Drawing.Point(81, 4);
            this._text.Margin = new System.Windows.Forms.Padding(0);
            this._text.Name = "_text";
            this._text.Size = new System.Drawing.Size(437, 253);
            this._text.TabIndex = 0;
            this._text.Text = "";
            this._text.TextChanged += new System.EventHandler(this._text_TextChanged);
            // 
            // _answeredBy
            // 
            this._answeredBy.Location = new System.Drawing.Point(81, 265);
            this._answeredBy.Margin = new System.Windows.Forms.Padding(0);
            this._answeredBy.Name = "_answeredBy";
            this._answeredBy.Size = new System.Drawing.Size(437, 20);
            this._answeredBy.TabIndex = 1;
            this._answeredBy.TextChanged += new System.EventHandler(this._answeredBy_TextChanged);
            // 
            // _answeredOn
            // 
            // 
            // 
            // 
            this._answeredOn.BackgroundStyle.Class = "DateTimeInputBackground";
            this._answeredOn.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._answeredOn.ButtonCustom.Symbol = "";
            this._answeredOn.ButtonCustom.Visible = true;
            this._answeredOn.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this._answeredOn.ButtonDropDown.Visible = true;
            this._answeredOn.IsPopupCalendarOpen = false;
            this._answeredOn.Location = new System.Drawing.Point(81, 293);
            this._answeredOn.Margin = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            // 
            // 
            // 
            this._answeredOn.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._answeredOn.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this._answeredOn.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this._answeredOn.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._answeredOn.MonthCalendar.DisplayMonth = new System.DateTime(2020, 12, 1, 0, 0, 0, 0);
            // 
            // 
            // 
            this._answeredOn.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this._answeredOn.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this._answeredOn.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._answeredOn.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._answeredOn.MonthCalendar.TodayButtonVisible = true;
            this._answeredOn.Name = "_answeredOn";
            this._answeredOn.Size = new System.Drawing.Size(119, 20);
            this._answeredOn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._answeredOn.TabIndex = 2;
            this._answeredOn.ValueChanged += new System.EventHandler(this._answeredOn_ValueChanged);
            this._answeredOn.ButtonCustomClick += new System.EventHandler(this._answeredOn_ButtonCustomClick);
            // 
            // _answeredVia
            // 
            // 
            // 
            // 
            this._answeredVia.Border.Class = "TextBoxBorder";
            this._answeredVia.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._answeredVia.ButtonCustom.Symbol = "";
            this._answeredVia.ButtonCustom.Visible = true;
            this._answeredVia.ButtonCustom2.Symbol = "";
            this._answeredVia.ButtonCustom2.Visible = true;
            this._answeredVia.Location = new System.Drawing.Point(285, 293);
            this._answeredVia.Margin = new System.Windows.Forms.Padding(0);
            this._answeredVia.Name = "_answeredVia";
            this._answeredVia.Size = new System.Drawing.Size(233, 20);
            this._answeredVia.TabIndex = 3;
            this._answeredVia.ButtonCustomClick += new System.EventHandler(this._answeredVia_ButtonCustomClick);
            this._answeredVia.ButtonCustom2Click += new System.EventHandler(this._answeredVia_ButtonCustom2Click);
            this._answeredVia.TextChanged += new System.EventHandler(this._answeredVia_TextChanged);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._text;
            this.layoutControlItem3.Height = 99;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "<a href=\"Text\">Text</a>";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this.layoutControlItem3_MarkupLinkClick);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._answeredBy;
            this.layoutControlItem6.Height = 28;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Answered By";
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._answeredOn;
            this.layoutControlItem7.Height = 28;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Answered On";
            this.layoutControlItem7.Width = 204;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._answeredVia;
            this.layoutControlItem8.Height = 28;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Answered Via";
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._text;
            this.layoutControlItem1.Height = 99;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Text";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._answeredBy;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Answered By";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._answeredOn;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Answered On";
            this.layoutControlItem4.Width = 50;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._answeredVia;
            this.layoutControlItem5.Height = 28;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Answered Via";
            this.layoutControlItem5.Width = 50;
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
            // AnswerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.layoutControl1);
            this.Name = "AnswerControl";
            this.Size = new System.Drawing.Size(522, 320);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._answeredOn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.RichTextBox _text;
        private System.Windows.Forms.TextBox _answeredBy;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput _answeredOn;
        private DevComponents.DotNetBar.Controls.TextBoxX _answeredVia;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}
