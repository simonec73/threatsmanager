namespace ThreatsManager.Extensions.Panels.Overview
{
    partial class OverviewPanel
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
            _itemEditor.Item = null;

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
            this._itemEditor = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._roadmapCharts = new ThreatsManager.Extensions.Panels.RoadmapThreatTypesCharts();
            this._roadmapDetails = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.gridColumn7 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this._mitigationsByStatus = new ThreatsManager.Extensions.Panels.MitigationStatusChart();
            this._lowestRisks = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn3 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this._threatTypesBySeverity = new ThreatsManager.Extensions.Panels.ThreatTypesChart();
            this._roadmap = new ThreatsManager.Extensions.Panels.RoadmapChart();
            this._topRisks = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.gridColumn4 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn5 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn6 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // _itemEditor
            // 
            this._itemEditor.BackColor = System.Drawing.Color.White;
            this._itemEditor.Dock = System.Windows.Forms.DockStyle.Right;
            this._itemEditor.Item = null;
            this._itemEditor.Location = new System.Drawing.Point(625, 0);
            this._itemEditor.Name = "_itemEditor";
            this._itemEditor.ReadOnly = false;
            this._itemEditor.Size = new System.Drawing.Size(268, 488);
            this._itemEditor.TabIndex = 0;
            this._itemEditor.Visible = false;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandableSplitter1.ExpandableControl = this._itemEditor;
            this.expandableSplitter1.Expanded = false;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.ForeColor = System.Drawing.Color.Black;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(887, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(6, 488);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 1;
            this.expandableSplitter1.TabStop = false;
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._container.Controls.Add(this._roadmapCharts);
            this._container.Controls.Add(this._roadmapDetails);
            this._container.Controls.Add(this._mitigationsByStatus);
            this._container.Controls.Add(this._lowestRisks);
            this._container.Controls.Add(this._threatTypesBySeverity);
            this._container.Controls.Add(this._roadmap);
            this._container.Controls.Add(this._topRisks);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.ForeColor = System.Drawing.Color.Black;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutGroup1});
            this._container.Size = new System.Drawing.Size(887, 488);
            this._container.TabIndex = 2;
            // 
            // _roadmapCharts
            // 
            this._roadmapCharts.BackColor = System.Drawing.Color.White;
            this._roadmapCharts.Location = new System.Drawing.Point(594, 25);
            this._roadmapCharts.Margin = new System.Windows.Forms.Padding(0);
            this._roadmapCharts.Name = "_roadmapCharts";
            this._roadmapCharts.Size = new System.Drawing.Size(285, 215);
            this._roadmapCharts.TabIndex = 10000;
            // 
            // _roadmapDetails
            // 
            this._roadmapDetails.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._roadmapDetails.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._roadmapDetails.Location = new System.Drawing.Point(297, 265);
            this._roadmapDetails.Margin = new System.Windows.Forms.Padding(0);
            this._roadmapDetails.Name = "_roadmapDetails";
            // 
            // 
            // 
            this._roadmapDetails.PrimaryGrid.AllowEdit = false;
            this._roadmapDetails.PrimaryGrid.Columns.Add(this.gridColumn7);
            this._roadmapDetails.PrimaryGrid.InitialActiveRow = DevComponents.DotNetBar.SuperGrid.RelativeRow.None;
            this._roadmapDetails.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.None;
            this._roadmapDetails.PrimaryGrid.MultiSelect = false;
            this._roadmapDetails.PrimaryGrid.ShowRowDirtyMarker = false;
            this._roadmapDetails.PrimaryGrid.ShowRowHeaders = false;
            this._roadmapDetails.PrimaryGrid.ShowTreeButtons = true;
            this._roadmapDetails.PrimaryGrid.ShowTreeLines = true;
            this._roadmapDetails.Size = new System.Drawing.Size(285, 219);
            this._roadmapDetails.TabIndex = 4;
            this._roadmapDetails.Text = "superGridControl1";
            this._roadmapDetails.CellActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellActivatedEventArgs>(this.OnCellActivated);
            this._roadmapDetails.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.OnRowActivated);
            // 
            // gridColumn7
            // 
            this.gridColumn7.AllowEdit = false;
            this.gridColumn7.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.gridColumn7.HeaderText = "Phase";
            this.gridColumn7.Name = "Phase";
            // 
            // _mitigationsByStatus
            // 
            this._mitigationsByStatus.Location = new System.Drawing.Point(4, 265);
            this._mitigationsByStatus.Margin = new System.Windows.Forms.Padding(0);
            this._mitigationsByStatus.Name = "_mitigationsByStatus";
            this._mitigationsByStatus.Size = new System.Drawing.Size(285, 219);
            this._mitigationsByStatus.TabIndex = 3;
            // 
            // _lowestRisks
            // 
            this._lowestRisks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._lowestRisks.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._lowestRisks.ForeColor = System.Drawing.Color.Black;
            this._lowestRisks.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._lowestRisks.Location = new System.Drawing.Point(594, 385);
            this._lowestRisks.Margin = new System.Windows.Forms.Padding(0);
            this._lowestRisks.Name = "_lowestRisks";
            // 
            // 
            // 
            this._lowestRisks.PrimaryGrid.AllowEdit = false;
            this._lowestRisks.PrimaryGrid.Columns.Add(this.gridColumn1);
            this._lowestRisks.PrimaryGrid.Columns.Add(this.gridColumn2);
            this._lowestRisks.PrimaryGrid.Columns.Add(this.gridColumn3);
            this._lowestRisks.PrimaryGrid.InitialActiveRow = DevComponents.DotNetBar.SuperGrid.RelativeRow.None;
            this._lowestRisks.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.None;
            this._lowestRisks.PrimaryGrid.MultiSelect = false;
            this._lowestRisks.PrimaryGrid.ShowCheckBox = false;
            this._lowestRisks.PrimaryGrid.ShowRowDirtyMarker = false;
            this._lowestRisks.PrimaryGrid.ShowRowHeaders = false;
            this._lowestRisks.Size = new System.Drawing.Size(285, 95);
            this._lowestRisks.TabIndex = 10002;
            this._lowestRisks.Text = "superGridControl1";
            this._lowestRisks.CellActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellActivatedEventArgs>(this.OnCellActivated);
            this._lowestRisks.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.OnRowActivated);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AllowEdit = false;
            this.gridColumn1.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.gridColumn1.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            this.gridColumn1.HeaderText = "Threat Type Name";
            this.gridColumn1.Name = "Column1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.AllowEdit = false;
            this.gridColumn2.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            this.gridColumn2.HeaderText = "Max Severity";
            this.gridColumn2.Name = "Column2";
            this.gridColumn2.Width = 80;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AllowEdit = false;
            this.gridColumn3.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            this.gridColumn3.HeaderText = "Weight";
            this.gridColumn3.Name = "Column3";
            this.gridColumn3.ToolTip = "Estimation of the weight of the Threat Type, by adding up the Severities of the r" +
    "elated Threat Events.";
            this.gridColumn3.Width = 60;
            // 
            // _threatTypesBySeverity
            // 
            this._threatTypesBySeverity.Location = new System.Drawing.Point(4, 21);
            this._threatTypesBySeverity.Margin = new System.Windows.Forms.Padding(0);
            this._threatTypesBySeverity.Name = "_threatTypesBySeverity";
            this._threatTypesBySeverity.Size = new System.Drawing.Size(285, 219);
            this._threatTypesBySeverity.TabIndex = 1;
            // 
            // _roadmap
            // 
            this._roadmap.AcceptableRisk = 0F;
            this._roadmap.Current = 0F;
            this._roadmap.Location = new System.Drawing.Point(297, 21);
            this._roadmap.LongTerm = 0F;
            this._roadmap.Margin = new System.Windows.Forms.Padding(0);
            this._roadmap.MidTerm = 0F;
            this._roadmap.Name = "_roadmap";
            this._roadmap.ShortTerm = 0F;
            this._roadmap.Size = new System.Drawing.Size(285, 219);
            this._roadmap.TabIndex = 2;
            // 
            // _topRisks
            // 
            this._topRisks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._topRisks.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._topRisks.ForeColor = System.Drawing.Color.Black;
            this._topRisks.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._topRisks.Location = new System.Drawing.Point(594, 265);
            this._topRisks.Margin = new System.Windows.Forms.Padding(0);
            this._topRisks.Name = "_topRisks";
            // 
            // 
            // 
            this._topRisks.PrimaryGrid.AllowEdit = false;
            this._topRisks.PrimaryGrid.Columns.Add(this.gridColumn4);
            this._topRisks.PrimaryGrid.Columns.Add(this.gridColumn5);
            this._topRisks.PrimaryGrid.Columns.Add(this.gridColumn6);
            this._topRisks.PrimaryGrid.InitialActiveRow = DevComponents.DotNetBar.SuperGrid.RelativeRow.None;
            this._topRisks.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.None;
            this._topRisks.PrimaryGrid.MultiSelect = false;
            this._topRisks.PrimaryGrid.ShowCheckBox = false;
            this._topRisks.PrimaryGrid.ShowRowDirtyMarker = false;
            this._topRisks.PrimaryGrid.ShowRowHeaders = false;
            this._topRisks.Size = new System.Drawing.Size(285, 95);
            this._topRisks.TabIndex = 10001;
            this._topRisks.Text = "superGridControl1";
            this._topRisks.CellActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellActivatedEventArgs>(this.OnCellActivated);
            this._topRisks.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this.OnRowActivated);
            // 
            // gridColumn4
            // 
            this.gridColumn4.AllowEdit = false;
            this.gridColumn4.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.gridColumn4.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            this.gridColumn4.HeaderText = "Threat Type Name";
            this.gridColumn4.Name = "Column1";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AllowEdit = false;
            this.gridColumn5.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            this.gridColumn5.HeaderText = "Max Severity";
            this.gridColumn5.Name = "Column2";
            this.gridColumn5.Width = 80;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AllowEdit = false;
            this.gridColumn6.ColumnSortMode = DevComponents.DotNetBar.SuperGrid.ColumnSortMode.None;
            this.gridColumn6.HeaderText = "Weight";
            this.gridColumn6.Name = "Column3";
            this.gridColumn6.ToolTip = "Estimation of the weight of the Threat Type, by adding up the Severities of the r" +
    "elated Threat Events.";
            this.gridColumn6.Width = 60;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._threatTypesBySeverity;
            this.layoutControlItem2.Height = 50;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Threat Types by Severity";
            this.layoutControlItem2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem2.Width = 50;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._roadmap;
            this.layoutControlItem1.Height = 50;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Roadmap";
            this.layoutControlItem1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem1.Width = 50;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._mitigationsByStatus;
            this.layoutControlItem5.Height = 50;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Mitigations by Status";
            this.layoutControlItem5.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem5.Width = 50;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._roadmapDetails;
            this.layoutControlItem6.Height = 208;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Roadmap Details";
            this.layoutControlItem6.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem6.Width = 50;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 100;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem7,
            this.layoutControlItem4,
            this.layoutControlItem3});
            this.layoutGroup1.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 34;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._roadmapCharts;
            this.layoutControlItem7.Height = 50;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Roadmap Threat Types By Severity";
            this.layoutControlItem7.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._topRisks;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Highest Risks";
            this.layoutControlItem4.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._lowestRisks;
            this.layoutControlItem3.Height = 25;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Lowest Risks";
            this.layoutControlItem3.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // OverviewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._container);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this._itemEditor);
            this.Name = "OverviewPanel";
            this.Size = new System.Drawing.Size(893, 488);
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Utilities.WinForms.ItemEditor _itemEditor;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private RoadmapChart _roadmap;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private ThreatTypesChart _threatTypesBySeverity;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _lowestRisks;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private MitigationStatusChart _mitigationsByStatus;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn3;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _topRisks;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn4;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn5;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _roadmapDetails;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private RoadmapThreatTypesCharts _roadmapCharts;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
    }
}
