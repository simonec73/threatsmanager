namespace ThreatsManager.Extensions.Panels.Excel
{
    partial class ExcelReportingPanel
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
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._fieldsMitigations = new System.Windows.Forms.CheckedListBox();
            this._fieldsThreatEvents = new System.Windows.Forms.CheckedListBox();
            this._fieldsDataStores = new System.Windows.Forms.CheckedListBox();
            this._fieldsProcesses = new System.Windows.Forms.CheckedListBox();
            this._fieldsExternalInteractors = new System.Windows.Forms.CheckedListBox();
            this._processes = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._includeExternalInteractors = new System.Windows.Forms.CheckBox();
            this._includeProcesses = new System.Windows.Forms.CheckBox();
            this._externalInteractors = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._includeDataStores = new System.Windows.Forms.CheckBox();
            this._dataStores = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._includeThreatEvents = new System.Windows.Forms.CheckBox();
            this._threatEvents = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._includeMitigations = new System.Windows.Forms.CheckBox();
            this._mitigations = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._includeDataFlows = new System.Windows.Forms.CheckBox();
            this._dataFlows = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._fieldsDataFlows = new System.Windows.Forms.CheckedListBox();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem18 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem20 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem22 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem16 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem17 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._saveFile = new System.Windows.Forms.SaveFileDialog();
            this.layoutControlItem19 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem21 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._fieldsMitigations);
            this._layout.Controls.Add(this._fieldsThreatEvents);
            this._layout.Controls.Add(this._fieldsDataStores);
            this._layout.Controls.Add(this._fieldsProcesses);
            this._layout.Controls.Add(this._fieldsExternalInteractors);
            this._layout.Controls.Add(this._processes);
            this._layout.Controls.Add(this._includeExternalInteractors);
            this._layout.Controls.Add(this._includeProcesses);
            this._layout.Controls.Add(this._externalInteractors);
            this._layout.Controls.Add(this._includeDataStores);
            this._layout.Controls.Add(this._dataStores);
            this._layout.Controls.Add(this._includeThreatEvents);
            this._layout.Controls.Add(this._threatEvents);
            this._layout.Controls.Add(this._includeMitigations);
            this._layout.Controls.Add(this._mitigations);
            this._layout.Controls.Add(this._includeDataFlows);
            this._layout.Controls.Add(this._dataFlows);
            this._layout.Controls.Add(this._fieldsDataFlows);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem14,
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem13,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem15,
            this.layoutControlItem18,
            this.layoutControlItem20,
            this.layoutControlItem22,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem16,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem17});
            this._layout.Size = new System.Drawing.Size(765, 686);
            this._layout.TabIndex = 0;
            // 
            // _fieldsMitigations
            // 
            this._fieldsMitigations.FormattingEnabled = true;
            this._fieldsMitigations.Location = new System.Drawing.Point(657, 563);
            this._fieldsMitigations.Margin = new System.Windows.Forms.Padding(0);
            this._fieldsMitigations.Name = "_fieldsMitigations";
            this._fieldsMitigations.Size = new System.Drawing.Size(87, 94);
            this._fieldsMitigations.TabIndex = 17;
            // 
            // _fieldsThreatEvents
            // 
            this._fieldsThreatEvents.FormattingEnabled = true;
            this._fieldsThreatEvents.Location = new System.Drawing.Point(657, 436);
            this._fieldsThreatEvents.Margin = new System.Windows.Forms.Padding(0);
            this._fieldsThreatEvents.Name = "_fieldsThreatEvents";
            this._fieldsThreatEvents.Size = new System.Drawing.Size(87, 94);
            this._fieldsThreatEvents.TabIndex = 14;
            // 
            // _fieldsDataStores
            // 
            this._fieldsDataStores.FormattingEnabled = true;
            this._fieldsDataStores.Location = new System.Drawing.Point(657, 182);
            this._fieldsDataStores.Margin = new System.Windows.Forms.Padding(0);
            this._fieldsDataStores.Name = "_fieldsDataStores";
            this._fieldsDataStores.Size = new System.Drawing.Size(87, 94);
            this._fieldsDataStores.TabIndex = 8;
            // 
            // _fieldsProcesses
            // 
            this._fieldsProcesses.FormattingEnabled = true;
            this._fieldsProcesses.Location = new System.Drawing.Point(657, 55);
            this._fieldsProcesses.Margin = new System.Windows.Forms.Padding(0);
            this._fieldsProcesses.Name = "_fieldsProcesses";
            this._fieldsProcesses.Size = new System.Drawing.Size(87, 94);
            this._fieldsProcesses.TabIndex = 5;
            // 
            // _fieldsExternalInteractors
            // 
            this._fieldsExternalInteractors.FormattingEnabled = true;
            this._fieldsExternalInteractors.Location = new System.Drawing.Point(657, -72);
            this._fieldsExternalInteractors.Margin = new System.Windows.Forms.Padding(0);
            this._fieldsExternalInteractors.Name = "_fieldsExternalInteractors";
            this._fieldsExternalInteractors.Size = new System.Drawing.Size(87, 94);
            this._fieldsExternalInteractors.TabIndex = 2;
            // 
            // _processes
            // 
            this._processes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._processes.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._processes.ForeColor = System.Drawing.Color.Black;
            this._processes.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._processes.Location = new System.Drawing.Point(159, 55);
            this._processes.Margin = new System.Windows.Forms.Padding(0);
            this._processes.Name = "_processes";
            this._processes.Size = new System.Drawing.Size(360, 94);
            this._processes.TabIndex = 4;
            this._processes.Text = "superGridControl1";
            // 
            // _includeExternalInteractors
            // 
            this._includeExternalInteractors.AutoSize = true;
            this._includeExternalInteractors.Checked = true;
            this._includeExternalInteractors.CheckState = System.Windows.Forms.CheckState.Checked;
            this._includeExternalInteractors.Location = new System.Drawing.Point(4, -97);
            this._includeExternalInteractors.Margin = new System.Windows.Forms.Padding(0);
            this._includeExternalInteractors.Name = "_includeExternalInteractors";
            this._includeExternalInteractors.Size = new System.Drawing.Size(740, 17);
            this._includeExternalInteractors.TabIndex = 0;
            this._includeExternalInteractors.Text = "Include External Interactors page";
            this._includeExternalInteractors.UseVisualStyleBackColor = true;
            this._includeExternalInteractors.CheckedChanged += new System.EventHandler(this._includeExternalInteractors_CheckedChanged);
            // 
            // _includeProcesses
            // 
            this._includeProcesses.AutoSize = true;
            this._includeProcesses.Checked = true;
            this._includeProcesses.CheckState = System.Windows.Forms.CheckState.Checked;
            this._includeProcesses.Location = new System.Drawing.Point(4, 30);
            this._includeProcesses.Margin = new System.Windows.Forms.Padding(0);
            this._includeProcesses.Name = "_includeProcesses";
            this._includeProcesses.Size = new System.Drawing.Size(740, 17);
            this._includeProcesses.TabIndex = 3;
            this._includeProcesses.Text = "Include Processes page";
            this._includeProcesses.UseVisualStyleBackColor = true;
            this._includeProcesses.CheckedChanged += new System.EventHandler(this._includeProcesses_CheckedChanged);
            // 
            // _externalInteractors
            // 
            this._externalInteractors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._externalInteractors.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._externalInteractors.ForeColor = System.Drawing.Color.Black;
            this._externalInteractors.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._externalInteractors.Location = new System.Drawing.Point(159, -72);
            this._externalInteractors.Margin = new System.Windows.Forms.Padding(0);
            this._externalInteractors.Name = "_externalInteractors";
            this._externalInteractors.Size = new System.Drawing.Size(360, 94);
            this._externalInteractors.TabIndex = 1;
            this._externalInteractors.Text = "superGridControl1";
            // 
            // _includeDataStores
            // 
            this._includeDataStores.AutoSize = true;
            this._includeDataStores.Checked = true;
            this._includeDataStores.CheckState = System.Windows.Forms.CheckState.Checked;
            this._includeDataStores.Location = new System.Drawing.Point(4, 157);
            this._includeDataStores.Margin = new System.Windows.Forms.Padding(0);
            this._includeDataStores.Name = "_includeDataStores";
            this._includeDataStores.Size = new System.Drawing.Size(740, 17);
            this._includeDataStores.TabIndex = 6;
            this._includeDataStores.Text = "Include Data Stores page";
            this._includeDataStores.UseVisualStyleBackColor = true;
            this._includeDataStores.CheckedChanged += new System.EventHandler(this._includeDataStores_CheckedChanged);
            // 
            // _dataStores
            // 
            this._dataStores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._dataStores.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._dataStores.ForeColor = System.Drawing.Color.Black;
            this._dataStores.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._dataStores.Location = new System.Drawing.Point(159, 182);
            this._dataStores.Margin = new System.Windows.Forms.Padding(0);
            this._dataStores.Name = "_dataStores";
            this._dataStores.Size = new System.Drawing.Size(360, 94);
            this._dataStores.TabIndex = 7;
            this._dataStores.Text = "superGridControl1";
            // 
            // _includeThreatEvents
            // 
            this._includeThreatEvents.AutoSize = true;
            this._includeThreatEvents.Checked = true;
            this._includeThreatEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this._includeThreatEvents.Location = new System.Drawing.Point(4, 411);
            this._includeThreatEvents.Margin = new System.Windows.Forms.Padding(0);
            this._includeThreatEvents.Name = "_includeThreatEvents";
            this._includeThreatEvents.Size = new System.Drawing.Size(740, 17);
            this._includeThreatEvents.TabIndex = 12;
            this._includeThreatEvents.Text = "Include Threat Events page";
            this._includeThreatEvents.UseVisualStyleBackColor = true;
            this._includeThreatEvents.CheckedChanged += new System.EventHandler(this._includeThreatEvents_CheckedChanged);
            // 
            // _threatEvents
            // 
            this._threatEvents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._threatEvents.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._threatEvents.ForeColor = System.Drawing.Color.Black;
            this._threatEvents.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._threatEvents.Location = new System.Drawing.Point(159, 436);
            this._threatEvents.Margin = new System.Windows.Forms.Padding(0);
            this._threatEvents.Name = "_threatEvents";
            this._threatEvents.Size = new System.Drawing.Size(360, 94);
            this._threatEvents.TabIndex = 13;
            this._threatEvents.Text = "superGridControl2";
            // 
            // _includeMitigations
            // 
            this._includeMitigations.AutoSize = true;
            this._includeMitigations.Checked = true;
            this._includeMitigations.CheckState = System.Windows.Forms.CheckState.Checked;
            this._includeMitigations.Location = new System.Drawing.Point(4, 538);
            this._includeMitigations.Margin = new System.Windows.Forms.Padding(0);
            this._includeMitigations.Name = "_includeMitigations";
            this._includeMitigations.Size = new System.Drawing.Size(740, 17);
            this._includeMitigations.TabIndex = 15;
            this._includeMitigations.Text = "Include Mitigations page";
            this._includeMitigations.UseVisualStyleBackColor = true;
            this._includeMitigations.CheckedChanged += new System.EventHandler(this._includeMitigations_CheckedChanged);
            // 
            // _mitigations
            // 
            this._mitigations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._mitigations.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._mitigations.ForeColor = System.Drawing.Color.Black;
            this._mitigations.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._mitigations.Location = new System.Drawing.Point(159, 563);
            this._mitigations.Margin = new System.Windows.Forms.Padding(0);
            this._mitigations.Name = "_mitigations";
            this._mitigations.Size = new System.Drawing.Size(360, 94);
            this._mitigations.TabIndex = 16;
            this._mitigations.Text = "superGridControl3";
            // 
            // _includeDataFlows
            // 
            this._includeDataFlows.AutoSize = true;
            this._includeDataFlows.Checked = true;
            this._includeDataFlows.CheckState = System.Windows.Forms.CheckState.Checked;
            this._includeDataFlows.Location = new System.Drawing.Point(4, 284);
            this._includeDataFlows.Margin = new System.Windows.Forms.Padding(0);
            this._includeDataFlows.Name = "_includeDataFlows";
            this._includeDataFlows.Size = new System.Drawing.Size(740, 17);
            this._includeDataFlows.TabIndex = 9;
            this._includeDataFlows.Text = "Include Flows page";
            this._includeDataFlows.UseVisualStyleBackColor = true;
            this._includeDataFlows.CheckedChanged += new System.EventHandler(this._includeDataFlows_CheckedChanged);
            // 
            // _dataFlows
            // 
            this._dataFlows.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._dataFlows.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._dataFlows.ForeColor = System.Drawing.Color.Black;
            this._dataFlows.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._dataFlows.Location = new System.Drawing.Point(159, 309);
            this._dataFlows.Margin = new System.Windows.Forms.Padding(0);
            this._dataFlows.Name = "_dataFlows";
            this._dataFlows.Size = new System.Drawing.Size(360, 94);
            this._dataFlows.TabIndex = 10;
            this._dataFlows.Text = "superGridControl2";
            // 
            // _fieldsDataFlows
            // 
            this._fieldsDataFlows.FormattingEnabled = true;
            this._fieldsDataFlows.Location = new System.Drawing.Point(657, 309);
            this._fieldsDataFlows.Margin = new System.Windows.Forms.Padding(0);
            this._fieldsDataFlows.Name = "_fieldsDataFlows";
            this._fieldsDataFlows.Size = new System.Drawing.Size(87, 94);
            this._fieldsDataFlows.TabIndex = 11;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._includeExternalInteractors;
            this.layoutControlItem2.Height = 25;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._externalInteractors;
            this.layoutControlItem4.Height = 20;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "External Interactors";
            this.layoutControlItem4.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem4.Width = 70;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this._fieldsExternalInteractors;
            this.layoutControlItem14.Height = 102;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Text = "Additional Fields";
            this.layoutControlItem14.Width = 30;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._includeProcesses;
            this.layoutControlItem1.Height = 25;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._processes;
            this.layoutControlItem3.Height = 20;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Processes";
            this.layoutControlItem3.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem3.Width = 70;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this._fieldsProcesses;
            this.layoutControlItem13.Height = 102;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "Additional Fields";
            this.layoutControlItem13.Width = 30;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._includeDataStores;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._dataStores;
            this.layoutControlItem7.Height = 20;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Data Stores";
            this.layoutControlItem7.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem7.Width = 70;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this._fieldsDataStores;
            this.layoutControlItem15.Height = 102;
            this.layoutControlItem15.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Text = "Additional Fields";
            this.layoutControlItem15.Width = 30;
            this.layoutControlItem15.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem18
            // 
            this.layoutControlItem18.Control = this._includeDataFlows;
            this.layoutControlItem18.Height = 25;
            this.layoutControlItem18.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem18.Name = "layoutControlItem18";
            this.layoutControlItem18.Width = 100;
            this.layoutControlItem18.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem20
            // 
            this.layoutControlItem20.Control = this._dataFlows;
            this.layoutControlItem20.Height = 102;
            this.layoutControlItem20.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem20.Name = "layoutControlItem20";
            this.layoutControlItem20.Text = "Flows";
            this.layoutControlItem20.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem20.Width = 70;
            this.layoutControlItem20.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem22
            // 
            this.layoutControlItem22.Control = this._fieldsDataFlows;
            this.layoutControlItem22.Height = 102;
            this.layoutControlItem22.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem22.Name = "layoutControlItem22";
            this.layoutControlItem22.Text = "Additional Fields";
            this.layoutControlItem22.Width = 30;
            this.layoutControlItem22.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._includeThreatEvents;
            this.layoutControlItem8.Height = 25;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._threatEvents;
            this.layoutControlItem9.Height = 20;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "Threat Events";
            this.layoutControlItem9.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem9.Width = 70;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Control = this._fieldsThreatEvents;
            this.layoutControlItem16.Height = 102;
            this.layoutControlItem16.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Text = "Additional Fields";
            this.layoutControlItem16.Width = 30;
            this.layoutControlItem16.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._includeMitigations;
            this.layoutControlItem10.Height = 25;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._mitigations;
            this.layoutControlItem11.Height = 20;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Mitigations";
            this.layoutControlItem11.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem11.Width = 70;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this._fieldsMitigations;
            this.layoutControlItem17.Height = 102;
            this.layoutControlItem17.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Text = "Additional Fields";
            this.layoutControlItem17.Width = 30;
            this.layoutControlItem17.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._dataStores;
            this.layoutControlItem6.Height = 150;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Processes";
            this.layoutControlItem6.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _saveFile
            // 
            this._saveFile.DefaultExt = "xlsx";
            this._saveFile.Filter = "Excel 2016 files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            this._saveFile.Title = "Save Excel Report";
            this._saveFile.RestoreDirectory = true;
            // 
            // layoutControlItem19
            // 
            this.layoutControlItem19.Control = this._dataFlows;
            this.layoutControlItem19.Height = 20;
            this.layoutControlItem19.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem19.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.Text = "Threat Events";
            this.layoutControlItem19.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem19.Width = 70;
            this.layoutControlItem19.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem21
            // 
            this.layoutControlItem21.Height = 102;
            this.layoutControlItem21.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.Text = "Additional Fields";
            this.layoutControlItem21.Width = 30;
            this.layoutControlItem21.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ExcelReportingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._layout);
            this.Name = "ExcelReportingPanel";
            this.Size = new System.Drawing.Size(765, 686);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _processes;
        private System.Windows.Forms.CheckBox _includeExternalInteractors;
        private System.Windows.Forms.CheckBox _includeProcesses;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _externalInteractors;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _dataStores;
        private System.Windows.Forms.CheckBox _includeDataStores;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.CheckBox _includeThreatEvents;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _threatEvents;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.CheckBox _includeMitigations;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _mitigations;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.SaveFileDialog _saveFile;
        private System.Windows.Forms.CheckedListBox _fieldsMitigations;
        private System.Windows.Forms.CheckedListBox _fieldsThreatEvents;
        private System.Windows.Forms.CheckedListBox _fieldsDataStores;
        private System.Windows.Forms.CheckedListBox _fieldsProcesses;
        private System.Windows.Forms.CheckedListBox _fieldsExternalInteractors;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem16;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem17;
        private System.Windows.Forms.CheckBox _includeDataFlows;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _dataFlows;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem18;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem20;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem19;
        private System.Windows.Forms.CheckedListBox _fieldsDataFlows;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem22;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem21;
    }
}
