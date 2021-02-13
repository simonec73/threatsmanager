using DevComponents.DotNetBar;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    partial class ThreatEventMitigationSelectionDialog
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
            _superTooltip.SetSuperTooltip(_associatedTo, null);

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
            this._strength = new System.Windows.Forms.ComboBox();
            this._associatedTo = new DevComponents.DotNetBar.LabelX();
            this._threatEventName = new System.Windows.Forms.Label();
            this._threatTypeName = new System.Windows.Forms.Label();
            this._associateToStandard = new System.Windows.Forms.CheckBox();
            this._nonStandardMitigation = new System.Windows.Forms.ComboBox();
            this._standardMitigation = new System.Windows.Forms.ComboBox();
            this._createNew = new System.Windows.Forms.RadioButton();
            this._associateNonstandard = new System.Windows.Forms.RadioButton();
            this._associateStandard = new System.Windows.Forms.RadioButton();
            this._controlType = new System.Windows.Forms.ComboBox();
            this._description = new System.Windows.Forms.RichTextBox();
            this._name = new System.Windows.Forms.TextBox();
            this._newToStandard = new System.Windows.Forms.CheckBox();
            this._directives = new System.Windows.Forms.RichTextBox();
            this._status = new System.Windows.Forms.ComboBox();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._sxGroup = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutDescription = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._dxGroup = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem20 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutDirectives = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem17 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem16 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem19 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._spellAsYouType = new Keyoti.RapidSpell.RapidSpellAsYouType(this.components);
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 349);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(868, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(437, 13);
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
            this._ok.Location = new System.Drawing.Point(356, 13);
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
            this._layout.Controls.Add(this._strength);
            this._layout.Controls.Add(this._associatedTo);
            this._layout.Controls.Add(this._threatEventName);
            this._layout.Controls.Add(this._threatTypeName);
            this._layout.Controls.Add(this._associateToStandard);
            this._layout.Controls.Add(this._nonStandardMitigation);
            this._layout.Controls.Add(this._standardMitigation);
            this._layout.Controls.Add(this._createNew);
            this._layout.Controls.Add(this._associateNonstandard);
            this._layout.Controls.Add(this._associateStandard);
            this._layout.Controls.Add(this._controlType);
            this._layout.Controls.Add(this._description);
            this._layout.Controls.Add(this._name);
            this._layout.Controls.Add(this._newToStandard);
            this._layout.Controls.Add(this._directives);
            this._layout.Controls.Add(this._status);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem11,
            this.layoutControlItem10,
            this.layoutControlItem13,
            this._sxGroup,
            this._dxGroup});
            this._layout.Size = new System.Drawing.Size(868, 349);
            this._layout.TabIndex = 0;
            // 
            // _strength
            // 
            this._strength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._strength.FormattingEnabled = true;
            this._strength.Location = new System.Drawing.Point(533, 75);
            this._strength.Margin = new System.Windows.Forms.Padding(0);
            this._strength.Name = "_strength";
            this._strength.Size = new System.Drawing.Size(327, 21);
            this._strength.TabIndex = 14;
            this._strength.SelectedIndexChanged += new System.EventHandler(this._strength_SelectedIndexChanged);
            // 
            // _associatedTo
            // 
            this._associatedTo.AutoSize = true;
            // 
            // 
            // 
            this._associatedTo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._associatedTo.Location = new System.Drawing.Point(83, 46);
            this._associatedTo.Margin = new System.Windows.Forms.Padding(0);
            this._associatedTo.Name = "_associatedTo";
            this._associatedTo.Size = new System.Drawing.Size(32, 15);
            this._associatedTo.TabIndex = 2;
            this._associatedTo.Text = "label1";
            // 
            // _threatEventName
            // 
            this._threatEventName.AutoSize = true;
            this._threatEventName.Location = new System.Drawing.Point(83, 4);
            this._threatEventName.Margin = new System.Windows.Forms.Padding(0);
            this._threatEventName.Name = "_threatEventName";
            this._threatEventName.Size = new System.Drawing.Size(781, 13);
            this._threatEventName.TabIndex = 0;
            this._threatEventName.Text = "label2";
            // 
            // _threatTypeName
            // 
            this._threatTypeName.AutoSize = true;
            this._threatTypeName.Location = new System.Drawing.Point(83, 25);
            this._threatTypeName.Margin = new System.Windows.Forms.Padding(0);
            this._threatTypeName.Name = "_threatTypeName";
            this._threatTypeName.Size = new System.Drawing.Size(781, 13);
            this._threatTypeName.TabIndex = 1;
            this._threatTypeName.Text = "label1";
            // 
            // _associateToStandard
            // 
            this._associateToStandard.AutoSize = true;
            this._associateToStandard.Checked = true;
            this._associateToStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            this._associateToStandard.Enabled = false;
            this._associateToStandard.Location = new System.Drawing.Point(314, 129);
            this._associateToStandard.Margin = new System.Windows.Forms.Padding(0);
            this._associateToStandard.Name = "_associateToStandard";
            this._associateToStandard.Size = new System.Drawing.Size(112, 17);
            this._associateToStandard.TabIndex = 6;
            this._associateToStandard.Text = "Set as Standard";
            this._associateToStandard.UseVisualStyleBackColor = true;
            // 
            // _nonStandardMitigation
            // 
            this._nonStandardMitigation.Enabled = false;
            this._nonStandardMitigation.FormattingEnabled = true;
            this._nonStandardMitigation.Location = new System.Drawing.Point(167, 154);
            this._nonStandardMitigation.Margin = new System.Windows.Forms.Padding(0);
            this._nonStandardMitigation.Name = "_nonStandardMitigation";
            this._nonStandardMitigation.Size = new System.Drawing.Size(259, 21);
            this._nonStandardMitigation.TabIndex = 7;
            this._nonStandardMitigation.SelectedIndexChanged += new System.EventHandler(this._nonStandardMitigation_SelectedIndexChanged);
            this._nonStandardMitigation.TextUpdate += new System.EventHandler(this.OnComboBoxTextUpdate);
            this._nonStandardMitigation.KeyDown += new System.Windows.Forms.KeyEventHandler(this._nonStandardMitigation_KeyDown);
            // 
            // _standardMitigation
            // 
            this._standardMitigation.FormattingEnabled = true;
            this._standardMitigation.Location = new System.Drawing.Point(167, 100);
            this._standardMitigation.Margin = new System.Windows.Forms.Padding(0);
            this._standardMitigation.Name = "_standardMitigation";
            this._standardMitigation.Size = new System.Drawing.Size(259, 21);
            this._standardMitigation.TabIndex = 4;
            this._standardMitigation.SelectedIndexChanged += new System.EventHandler(this._standardMitigation_SelectedIndexChanged);
            this._standardMitigation.TextUpdate += new System.EventHandler(this.OnComboBoxTextUpdate);
            this._standardMitigation.KeyDown += new System.Windows.Forms.KeyEventHandler(this._standardMitigation_KeyDown);
            // 
            // _createNew
            // 
            this._createNew.AutoSize = true;
            this._createNew.Location = new System.Drawing.Point(8, 183);
            this._createNew.Margin = new System.Windows.Forms.Padding(0);
            this._createNew.Name = "_createNew";
            this._createNew.Size = new System.Drawing.Size(298, 17);
            this._createNew.TabIndex = 8;
            this._createNew.Text = "Associate a New Mitigation";
            this._createNew.UseVisualStyleBackColor = true;
            this._createNew.CheckedChanged += new System.EventHandler(this._createNew_CheckedChanged);
            // 
            // _associateNonstandard
            // 
            this._associateNonstandard.AutoSize = true;
            this._associateNonstandard.Location = new System.Drawing.Point(8, 129);
            this._associateNonstandard.Margin = new System.Windows.Forms.Padding(0);
            this._associateNonstandard.Name = "_associateNonstandard";
            this._associateNonstandard.Size = new System.Drawing.Size(298, 17);
            this._associateNonstandard.TabIndex = 5;
            this._associateNonstandard.Text = "Associate a non-Standard Mitigation";
            this._associateNonstandard.UseVisualStyleBackColor = true;
            this._associateNonstandard.CheckedChanged += new System.EventHandler(this._associateNonstandard_CheckedChanged);
            // 
            // _associateStandard
            // 
            this._associateStandard.AutoSize = true;
            this._associateStandard.Checked = true;
            this._associateStandard.Location = new System.Drawing.Point(8, 75);
            this._associateStandard.Margin = new System.Windows.Forms.Padding(0);
            this._associateStandard.Name = "_associateStandard";
            this._associateStandard.Size = new System.Drawing.Size(418, 17);
            this._associateStandard.TabIndex = 3;
            this._associateStandard.TabStop = true;
            this._associateStandard.Text = "Associate a Standard Mitigation";
            this._associateStandard.UseVisualStyleBackColor = true;
            this._associateStandard.CheckedChanged += new System.EventHandler(this._associateStandard_CheckedChanged);
            // 
            // _controlType
            // 
            this._controlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._controlType.Enabled = false;
            this._controlType.FormattingEnabled = true;
            this._controlType.Location = new System.Drawing.Point(167, 316);
            this._controlType.Margin = new System.Windows.Forms.Padding(0);
            this._controlType.Name = "_controlType";
            this._controlType.Size = new System.Drawing.Size(259, 21);
            this._controlType.TabIndex = 12;
            this._controlType.SelectedIndexChanged += new System.EventHandler(this._controlType_SelectedIndexChanged);
            // 
            // _description
            // 
            this._description.AcceptsTab = true;
            this._description.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._description.Enabled = false;
            this._description.Location = new System.Drawing.Point(167, 236);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(259, 72);
            this._description.TabIndex = 11;
            this._description.Text = "";
            // 
            // _name
            // 
            this._name.Enabled = false;
            this._name.Location = new System.Drawing.Point(167, 208);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(259, 20);
            this._name.TabIndex = 10;
            this._name.TextChanged += new System.EventHandler(this._name_TextChanged);
            // 
            // _newToStandard
            // 
            this._newToStandard.AutoSize = true;
            this._newToStandard.Checked = true;
            this._newToStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            this._newToStandard.Enabled = false;
            this._newToStandard.Location = new System.Drawing.Point(314, 183);
            this._newToStandard.Margin = new System.Windows.Forms.Padding(0);
            this._newToStandard.Name = "_newToStandard";
            this._newToStandard.Size = new System.Drawing.Size(112, 17);
            this._newToStandard.TabIndex = 9;
            this._newToStandard.Text = "Set as Standard";
            this._newToStandard.UseVisualStyleBackColor = true;
            // 
            // _directives
            // 
            this._directives.AcceptsTab = true;
            this._directives.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._directives.Location = new System.Drawing.Point(533, 133);
            this._directives.Margin = new System.Windows.Forms.Padding(0);
            this._directives.Name = "_directives";
            this._directives.Size = new System.Drawing.Size(327, 205);
            this._directives.TabIndex = 16;
            this._directives.Text = "";
            // 
            // _status
            // 
            this._status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._status.FormattingEnabled = true;
            this._status.Location = new System.Drawing.Point(533, 104);
            this._status.Margin = new System.Windows.Forms.Padding(0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(327, 21);
            this._status.TabIndex = 15;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._threatEventName;
            this.layoutControlItem11.Height = 21;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Threat Event";
            this.layoutControlItem11.Width = 100;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this._associatedTo;
            this.layoutControlItem13.Height = 25;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "Associated To";
            this.layoutControlItem13.Width = 100;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _sxGroup
            // 
            this._sxGroup.Height = 99;
            this._sxGroup.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._sxGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem4,
            this.layoutControlItem7,
            this.layoutControlItem5,
            this.layoutControlItem9,
            this.layoutControlItem8,
            this.layoutControlItem6,
            this.layoutControlItem12,
            this.layoutControlItem1,
            this._layoutDescription,
            this.layoutControlItem3});
            this._sxGroup.MinSize = new System.Drawing.Size(120, 32);
            this._sxGroup.Name = "_sxGroup";
            this._sxGroup.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._sxGroup.Width = 50;
            this._sxGroup.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._associateStandard;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._standardMitigation;
            this.layoutControlItem7.Height = 29;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Standard Mitigation";
            this.layoutControlItem7.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._associateNonstandard;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 99;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._associateToStandard;
            this.layoutControlItem9.Height = 25;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 120;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._nonStandardMitigation;
            this.layoutControlItem8.Height = 29;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Mitigation";
            this.layoutControlItem8.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._createNew;
            this.layoutControlItem6.Height = 25;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 99;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this._newToStandard;
            this.layoutControlItem12.Height = 25;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Width = 120;
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
            // _dxGroup
            // 
            this._dxGroup.Height = 90;
            this._dxGroup.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._dxGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem14,
            this.layoutControlItem20,
            this._layoutDirectives});
            this._dxGroup.MinSize = new System.Drawing.Size(120, 32);
            this._dxGroup.Name = "_dxGroup";
            this._dxGroup.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._dxGroup.Width = 50;
            this._dxGroup.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this._strength;
            this.layoutControlItem14.Height = 29;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Text = "Strength";
            this.layoutControlItem14.Width = 100;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem20
            // 
            this.layoutControlItem20.Control = this._status;
            this.layoutControlItem20.Height = 29;
            this.layoutControlItem20.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem20.Name = "layoutControlItem20";
            this.layoutControlItem20.Text = "Mitigation Status";
            this.layoutControlItem20.Width = 100;
            this.layoutControlItem20.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutDirectives
            // 
            this._layoutDirectives.Control = this._directives;
            this._layoutDirectives.Height = 100;
            this._layoutDirectives.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDirectives.MinSize = new System.Drawing.Size(120, 0);
            this._layoutDirectives.Name = "_layoutDirectives";
            this._layoutDirectives.Text = "<a href=\"Directives\">Directives</a>";
            this._layoutDirectives.Width = 100;
            this._layoutDirectives.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._layoutDirectives.MarkupLinkClick += new DevComponents.DotNetBar.Layout.MarkupLinkClickEventHandler(this._layoutDirectives_MarkupLinkClick);
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
            this.layoutControlItem17.Control = this._directives;
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
            // 
            // ThreatEventMitigationSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(868, 397);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ThreatEventMitigationSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Associate a Mitigation to the Threat Event";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ThreatEventMitigationSelectionDialog_FormClosed);
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
        private System.Windows.Forms.ComboBox _controlType;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.CheckBox _associateToStandard;
        private System.Windows.Forms.ComboBox _nonStandardMitigation;
        private System.Windows.Forms.ComboBox _standardMitigation;
        private System.Windows.Forms.RadioButton _createNew;
        private System.Windows.Forms.RadioButton _associateNonstandard;
        private System.Windows.Forms.RadioButton _associateStandard;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.Label _threatEventName;
        private System.Windows.Forms.Label _threatTypeName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.CheckBox _newToStandard;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private LabelX _associatedTo;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private System.Windows.Forms.ComboBox _strength;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private System.Windows.Forms.RichTextBox _directives;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutDirectives;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem17;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem16;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem19;
        private DevComponents.DotNetBar.Layout.LayoutGroup _sxGroup;
        private DevComponents.DotNetBar.Layout.LayoutGroup _dxGroup;
        private System.Windows.Forms.ComboBox _status;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem20;
        private Keyoti.RapidSpell.RapidSpellAsYouType _spellAsYouType;
        private SuperTooltip _superTooltip;
    }
}