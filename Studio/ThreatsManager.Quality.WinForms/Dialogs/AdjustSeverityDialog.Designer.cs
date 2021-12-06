
namespace ThreatsManager.Quality.Dialogs
{
    partial class AdjustSeverityDialog
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
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._points = new DevComponents.Editors.IntegerInput();
            this._reason = new System.Windows.Forms.RichTextBox();
            this._adjustment = new System.Windows.Forms.ComboBox();
            this._threatEventName = new System.Windows.Forms.Label();
            this._associatedTo = new System.Windows.Forms.Label();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._reasonContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._spellAsYouType = new Keyoti.RapidSpell.RapidSpellAsYouType(this.components);
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._points)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 206);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(541, 46);
            this.panel1.TabIndex = 3;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(273, 11);
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
            this._ok.Location = new System.Drawing.Point(192, 11);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._points);
            this.layoutControl1.Controls.Add(this._reason);
            this.layoutControl1.Controls.Add(this._adjustment);
            this.layoutControl1.Controls.Add(this._threatEventName);
            this.layoutControl1.Controls.Add(this._associatedTo);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this._reasonContainer});
            this.layoutControl1.Size = new System.Drawing.Size(541, 206);
            this.layoutControl1.TabIndex = 4;
            // 
            // _points
            // 
            // 
            // 
            // 
            this._points.BackgroundStyle.Class = "DateTimeInputBackground";
            this._points.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._points.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this._points.Location = new System.Drawing.Point(445, 56);
            this._points.Margin = new System.Windows.Forms.Padding(0);
            this._points.MaxValue = 1000;
            this._points.MinValue = -1000;
            this._points.Name = "_points";
            this._points.ShowUpDown = true;
            this._points.Size = new System.Drawing.Size(92, 20);
            this._points.TabIndex = 3;
            this._points.ValueChanged += new System.EventHandler(this._points_ValueChanged);
            // 
            // _reason
            // 
            this._reason.Location = new System.Drawing.Point(83, 85);
            this._reason.Margin = new System.Windows.Forms.Padding(0);
            this._reason.Name = "_reason";
            this._reason.Size = new System.Drawing.Size(454, 117);
            this._reason.TabIndex = 4;
            this._reason.Text = "";
            this._reason.TextChanged += new System.EventHandler(this._reason_TextChanged);
            // 
            // _adjustment
            // 
            this._adjustment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._adjustment.FormattingEnabled = true;
            this._adjustment.Location = new System.Drawing.Point(83, 56);
            this._adjustment.Margin = new System.Windows.Forms.Padding(0);
            this._adjustment.Name = "_adjustment";
            this._adjustment.Size = new System.Drawing.Size(275, 21);
            this._adjustment.TabIndex = 2;
            this._adjustment.SelectedIndexChanged += new System.EventHandler(this._adjustment_SelectedIndexChanged);
            // 
            // _threatEventName
            // 
            this._threatEventName.AutoSize = true;
            this._threatEventName.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._threatEventName.Location = new System.Drawing.Point(83, 4);
            this._threatEventName.Margin = new System.Windows.Forms.Padding(0);
            this._threatEventName.Name = "_threatEventName";
            this._threatEventName.Size = new System.Drawing.Size(454, 18);
            this._threatEventName.TabIndex = 0;
            this._threatEventName.Text = "label1";
            this._threatEventName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _associatedTo
            // 
            this._associatedTo.AutoSize = true;
            this._associatedTo.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._associatedTo.Location = new System.Drawing.Point(83, 30);
            this._associatedTo.Margin = new System.Windows.Forms.Padding(0);
            this._associatedTo.Name = "_associatedTo";
            this._associatedTo.Size = new System.Drawing.Size(454, 18);
            this._associatedTo.TabIndex = 1;
            this._associatedTo.Text = "label1";
            this._associatedTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._threatEventName;
            this.layoutControlItem3.Height = 26;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Threat Event";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._associatedTo;
            this.layoutControlItem4.Height = 26;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Associated To";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._adjustment;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Adjustment";
            this.layoutControlItem1.Width = 67;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._points;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Points";
            this.layoutControlItem2.Width = 33;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _reasonContainer
            // 
            this._reasonContainer.Control = this._reason;
            this._reasonContainer.Height = 100;
            this._reasonContainer.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._reasonContainer.MinSize = new System.Drawing.Size(120, 0);
            this._reasonContainer.Name = "_reasonContainer";
            this._reasonContainer.Text = "<a href=\"Reason\">Reason</a>";
            this._reasonContainer.Width = 100;
            this._reasonContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._reasonContainer.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._reasonContainer_MarkupLinkClick);
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
            // AdjustSeverityDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(541, 252);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdjustSeverityDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adjust Severity";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._points)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.RichTextBox _reason;
        private System.Windows.Forms.ComboBox _adjustment;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _reasonContainer;
        private System.Windows.Forms.Label _threatEventName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Label _associatedTo;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.Editors.IntegerInput _points;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}