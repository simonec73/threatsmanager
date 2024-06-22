﻿namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    partial class ThreatEventScenarioCreationDialog
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

            if (_spellName != null)
            {
                _spellName.Dispose();
            }

            if (_spellDescription != null)
            {
                _spellDescription.Dispose();
            }

            if (_spellMotivation != null)
            {
                _spellMotivation.Dispose();
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
            this._motivation = new System.Windows.Forms.RichTextBox();
            this._actor = new System.Windows.Forms.ComboBox();
            this._severity = new System.Windows.Forms.ComboBox();
            this._description = new System.Windows.Forms.RichTextBox();
            this._name = new System.Windows.Forms.TextBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutDescription = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutMotivation = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.panel1.Location = new System.Drawing.Point(0, 276);
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
            this._layout.Controls.Add(this._motivation);
            this._layout.Controls.Add(this._actor);
            this._layout.Controls.Add(this._severity);
            this._layout.Controls.Add(this._description);
            this._layout.Controls.Add(this._name);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this._layoutDescription,
            this.layoutControlItem4,
            this._layoutMotivation,
            this.layoutControlItem3});
            this._layout.Size = new System.Drawing.Size(440, 276);
            this._layout.TabIndex = 0;
            // 
            // _motivation
            // 
            this._motivation.AcceptsTab = true;
            this._motivation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._motivation.Location = new System.Drawing.Point(63, 165);
            this._motivation.Margin = new System.Windows.Forms.Padding(0);
            this._motivation.Name = "_motivation";
            this._motivation.Size = new System.Drawing.Size(373, 76);
            this._motivation.TabIndex = 3;
            this._motivation.Text = "";
            // 
            // _actor
            // 
            this._actor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._actor.FormattingEnabled = true;
            this._actor.Location = new System.Drawing.Point(63, 136);
            this._actor.Margin = new System.Windows.Forms.Padding(0);
            this._actor.Name = "_actor";
            this._actor.Size = new System.Drawing.Size(373, 21);
            this._actor.TabIndex = 2;
            this._actor.SelectedIndexChanged += new System.EventHandler(this._actor_SelectedIndexChanged);
            // 
            // _severity
            // 
            this._severity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._severity.FormattingEnabled = true;
            this._severity.Location = new System.Drawing.Point(63, 249);
            this._severity.Margin = new System.Windows.Forms.Padding(0);
            this._severity.Name = "_severity";
            this._severity.Size = new System.Drawing.Size(373, 21);
            this._severity.TabIndex = 4;
            this._severity.SelectedIndexChanged += new System.EventHandler(this._severity_SelectedIndexChanged);
            // 
            // _description
            // 
            this._description.AcceptsTab = true;
            this._description.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._description.Location = new System.Drawing.Point(63, 32);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(373, 96);
            this._description.TabIndex = 1;
            this._description.Text = "";
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(63, 4);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(373, 20);
            this._name.TabIndex = 0;
            this._name.TextChanged += new System.EventHandler(this._threatName_TextChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._name;
            this.layoutControlItem1.Height = 28;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Name";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutDescription
            // 
            this._layoutDescription.Control = this._description;
            this._layoutDescription.Height = 99;
            this._layoutDescription.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MinSize = new System.Drawing.Size(120, 0);
            this._layoutDescription.Name = "_layoutDescription";
            this._layoutDescription.Text = "<a href=\"Description\">Description</a>";
            this._layoutDescription.Width = 100;
            this._layoutDescription.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDescription.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._layoutDescription_MarkupLinkClick);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._actor;
            this.layoutControlItem4.Height = 29;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Actor";
            this.layoutControlItem4.Width = 95;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutMotivation
            // 
            this._layoutMotivation.Control = this._motivation;
            this._layoutMotivation.Height = 84;
            this._layoutMotivation.MinSize = new System.Drawing.Size(120, 0);
            this._layoutMotivation.Name = "_layoutMotivation";
            this._layoutMotivation.Text = "<a href=\"Motivation\">Motivation</a>";
            this._layoutMotivation.Width = 100;
            this._layoutMotivation.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutMotivation.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._layoutMotivation_MarkupLinkClick);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._severity;
            this.layoutControlItem3.Height = 29;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Severity";
            this.layoutControlItem3.Width = 70;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // ThreatEventScenarioCreationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(440, 324);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ThreatEventScenarioCreationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Threat Event Scenario creation";
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
        private System.Windows.Forms.TextBox _name;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.RichTextBox _description;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutDescription;
        private System.Windows.Forms.ComboBox _actor;
        private System.Windows.Forms.ComboBox _severity;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.RichTextBox _motivation;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutMotivation;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
    }
}