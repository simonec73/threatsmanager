using System.Windows.Forms;
using DevComponents.DotNetBar.Layout;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    partial class RoadmapPanel
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
            _grid.CellMouseDown -= _grid_CellMouseDown;
            _grid.CellMouseLeave -= _grid_CellMouseLeave;
            _grid.CellMouseMove -= _grid_CellMouseMove;
            _notAssessedPalette.MitigationDropped -= SetNotAssessed;
            _shortTermPalette.MitigationDropped -= SetShortTerm;
            _midTermPalette.MitigationDropped -= SetMidTerm;
            _longTermPalette.MitigationDropped -= SetLongTerm;
            _noActionRequiredPalette.MitigationDropped -= SetNoActionRequired;

            _itemDetails.Item = null;

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
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._splitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this._bottomCenterSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._gridItemDetails = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this._bottomRightSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._chart = new ThreatsManager.Extensions.Panels.RoadmapChart();
            this._bottomLeftSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._itemDetails = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this._container = new System.Windows.Forms.TableLayoutPanel();
            this._noActionRequired = new DevComponents.DotNetBar.ExpandablePanel();
            this._noActionRequiredPalette = new ThreatsManager.Extensions.Panels.Roadmap.RoadmapPalette();
            this._longTerm = new DevComponents.DotNetBar.ExpandablePanel();
            this._longTermPalette = new ThreatsManager.Extensions.Panels.Roadmap.RoadmapPalette();
            this._midTerm = new DevComponents.DotNetBar.ExpandablePanel();
            this._midTermPalette = new ThreatsManager.Extensions.Panels.Roadmap.RoadmapPalette();
            this._shortTerm = new DevComponents.DotNetBar.ExpandablePanel();
            this._shortTermPalette = new ThreatsManager.Extensions.Panels.Roadmap.RoadmapPalette();
            this._notAssessed = new DevComponents.DotNetBar.ExpandablePanel();
            this._notAssessedPalette = new ThreatsManager.Extensions.Panels.Roadmap.RoadmapPalette();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._tooltipTimer = new System.Windows.Forms.Timer(this.components);
            this._bottomPanel.SuspendLayout();
            this._container.SuspendLayout();
            this._noActionRequired.SuspendLayout();
            this._longTerm.SuspendLayout();
            this._midTerm.SuspendLayout();
            this._shortTerm.SuspendLayout();
            this._notAssessed.SuspendLayout();
            this.SuspendLayout();
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(456, 0);
            this._grid.Margin = new System.Windows.Forms.Padding(0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(60, 98);
            this._grid.TabIndex = 0;
            this._grid.Text = "superGridControl1";
            this._grid.CellClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellClickEventArgs>(this._grid_CellClick);
            this._grid.CellMouseDown += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this._grid_CellMouseDown);
            this._grid.CellMouseLeave += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellEventArgs>(this._grid_CellMouseLeave);
            this._grid.CellMouseMove += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this._grid_CellMouseMove);
            // 
            // _splitter
            // 
            this._splitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._splitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._splitter.ExpandableControl = this._bottomPanel;
            this._splitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._splitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._splitter.ForeColor = System.Drawing.Color.Black;
            this._splitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._splitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._splitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._splitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._splitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._splitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._splitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._splitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._splitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._splitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._splitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._splitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._splitter.Location = new System.Drawing.Point(0, 137);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(953, 6);
            this._splitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._splitter.TabIndex = 1;
            this._splitter.TabStop = false;
            // 
            // _bottomPanel
            // 
            this._bottomPanel.Controls.Add(this._grid);
            this._bottomPanel.Controls.Add(this._bottomCenterSplitter);
            this._bottomPanel.Controls.Add(this._gridItemDetails);
            this._bottomPanel.Controls.Add(this._bottomRightSplitter);
            this._bottomPanel.Controls.Add(this._bottomLeftSplitter);
            this._bottomPanel.Controls.Add(this._chart);
            this._bottomPanel.Controls.Add(this._itemDetails);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 143);
            this._bottomPanel.Margin = new System.Windows.Forms.Padding(0);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(953, 451);
            this._bottomPanel.TabIndex = 0;
            // 
            // _bottomCenterSplitter
            // 
            this._bottomCenterSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomCenterSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomCenterSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._bottomCenterSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomCenterSplitter.ExpandableControl = this._gridItemDetails;
            this._bottomCenterSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomCenterSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomCenterSplitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomCenterSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomCenterSplitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomCenterSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomCenterSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._bottomCenterSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._bottomCenterSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._bottomCenterSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._bottomCenterSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._bottomCenterSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._bottomCenterSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomCenterSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomCenterSplitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomCenterSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomCenterSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomCenterSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomCenterSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._bottomCenterSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._bottomCenterSplitter.Location = new System.Drawing.Point(456, 98);
            this._bottomCenterSplitter.Name = "_bottomCenterSplitter";
            this._bottomCenterSplitter.Size = new System.Drawing.Size(60, 6);
            this._bottomCenterSplitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._bottomCenterSplitter.TabIndex = 4;
            this._bottomCenterSplitter.TabStop = false;
            // 
            // _gridItemDetails
            // 
            this._gridItemDetails.BackColor = System.Drawing.Color.White;
            this._gridItemDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._gridItemDetails.Item = null;
            this._gridItemDetails.Location = new System.Drawing.Point(456, 104);
            this._gridItemDetails.Name = "_gridItemDetails";
            this._gridItemDetails.ReadOnly = false;
            this._gridItemDetails.Size = new System.Drawing.Size(60, 347);
            this._gridItemDetails.TabIndex = 3;
            // 
            // _bottomRightSplitter
            // 
            this._bottomRightSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomRightSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomRightSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._bottomRightSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this._bottomRightSplitter.ExpandableControl = this._chart;
            this._bottomRightSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomRightSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomRightSplitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomRightSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomRightSplitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomRightSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomRightSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._bottomRightSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._bottomRightSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._bottomRightSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._bottomRightSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._bottomRightSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._bottomRightSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomRightSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomRightSplitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomRightSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomRightSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomRightSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomRightSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._bottomRightSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._bottomRightSplitter.Location = new System.Drawing.Point(516, 0);
            this._bottomRightSplitter.Name = "_bottomRightSplitter";
            this._bottomRightSplitter.Size = new System.Drawing.Size(6, 451);
            this._bottomRightSplitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._bottomRightSplitter.TabIndex = 2;
            this._bottomRightSplitter.TabStop = false;
            // 
            // _chart
            // 
            this._chart.AcceptableRisk = 0F;
            this._chart.Current = 0F;
            this._chart.Dock = System.Windows.Forms.DockStyle.Right;
            this._chart.Location = new System.Drawing.Point(522, 0);
            this._chart.LongTerm = 0F;
            this._chart.Margin = new System.Windows.Forms.Padding(0);
            this._chart.MidTerm = 0F;
            this._chart.Name = "_chart";
            this._chart.ShortTerm = 0F;
            this._chart.Size = new System.Drawing.Size(431, 451);
            this._chart.TabIndex = 0;
            // 
            // _bottomLeftSplitter
            // 
            this._bottomLeftSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomLeftSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomLeftSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._bottomLeftSplitter.ExpandableControl = this._itemDetails;
            this._bottomLeftSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomLeftSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomLeftSplitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomLeftSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomLeftSplitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomLeftSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomLeftSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._bottomLeftSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._bottomLeftSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._bottomLeftSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._bottomLeftSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._bottomLeftSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._bottomLeftSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomLeftSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomLeftSplitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._bottomLeftSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._bottomLeftSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._bottomLeftSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._bottomLeftSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._bottomLeftSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._bottomLeftSplitter.Location = new System.Drawing.Point(450, 0);
            this._bottomLeftSplitter.Name = "_bottomLeftSplitter";
            this._bottomLeftSplitter.Size = new System.Drawing.Size(6, 451);
            this._bottomLeftSplitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._bottomLeftSplitter.TabIndex = 1;
            this._bottomLeftSplitter.TabStop = false;
            // 
            // _itemDetails
            // 
            this._itemDetails.BackColor = System.Drawing.Color.White;
            this._itemDetails.Dock = System.Windows.Forms.DockStyle.Left;
            this._itemDetails.Item = null;
            this._itemDetails.Location = new System.Drawing.Point(0, 0);
            this._itemDetails.Margin = new System.Windows.Forms.Padding(0);
            this._itemDetails.Name = "_itemDetails";
            this._itemDetails.ReadOnly = false;
            this._itemDetails.Size = new System.Drawing.Size(450, 451);
            this._itemDetails.TabIndex = 1;
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.White;
            this._container.ColumnCount = 5;
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._container.Controls.Add(this._noActionRequired, 4, 0);
            this._container.Controls.Add(this._longTerm, 3, 0);
            this._container.Controls.Add(this._midTerm, 2, 0);
            this._container.Controls.Add(this._shortTerm, 1, 0);
            this._container.Controls.Add(this._notAssessed, 0, 0);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            this._container.RowCount = 1;
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._container.Size = new System.Drawing.Size(953, 137);
            this._container.TabIndex = 2;
            // 
            // _noActionRequired
            // 
            this._noActionRequired.CanvasColor = System.Drawing.SystemColors.Control;
            this._noActionRequired.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._noActionRequired.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._noActionRequired.Controls.Add(this._noActionRequiredPalette);
            this._noActionRequired.DisabledBackColor = System.Drawing.Color.Empty;
            this._noActionRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            this._noActionRequired.HideControlsWhenCollapsed = true;
            this._noActionRequired.Location = new System.Drawing.Point(763, 3);
            this._noActionRequired.Name = "_noActionRequired";
            this._noActionRequired.Size = new System.Drawing.Size(187, 131);
            this._noActionRequired.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._noActionRequired.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._noActionRequired.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._noActionRequired.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._noActionRequired.Style.GradientAngle = 90;
            this._noActionRequired.TabIndex = 4;
            this._noActionRequired.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._noActionRequired.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._noActionRequired.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._noActionRequired.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._noActionRequired.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._noActionRequired.TitleStyle.GradientAngle = 90;
            this._noActionRequired.TitleText = "No Action Required";
            this._noActionRequired.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._noActionRequired_ExpandedChanged);
            // 
            // _noActionRequiredPalette
            // 
            this._noActionRequiredPalette.AllowDelete = false;
            this._noActionRequiredPalette.AllowEdit = false;
            this._noActionRequiredPalette.AllowInsert = false;
            this._noActionRequiredPalette.AllowLink = false;
            this._noActionRequiredPalette.AllowMove = false;
            this._noActionRequiredPalette.AllowReshape = false;
            this._noActionRequiredPalette.AllowResize = false;
            this._noActionRequiredPalette.ArrowMoveLarge = 10F;
            this._noActionRequiredPalette.ArrowMoveSmall = 1F;
            this._noActionRequiredPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._noActionRequiredPalette.BackColor = System.Drawing.Color.White;
            this._noActionRequiredPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._noActionRequiredPalette.DragsRealtime = true;
            this._noActionRequiredPalette.GridCellSizeHeight = 58F;
            this._noActionRequiredPalette.GridCellSizeWidth = 52F;
            this._noActionRequiredPalette.GridOriginX = 20F;
            this._noActionRequiredPalette.GridOriginY = 5F;
            this._noActionRequiredPalette.HidesSelection = true;
            this._noActionRequiredPalette.Location = new System.Drawing.Point(0, 26);
            this._noActionRequiredPalette.Name = "_noActionRequiredPalette";
            this._noActionRequiredPalette.ShowsNegativeCoordinates = false;
            this._noActionRequiredPalette.Size = new System.Drawing.Size(187, 105);
            this._noActionRequiredPalette.TabIndex = 4;
            this._noActionRequiredPalette.Text = "roadmapPalette5";
            this._noActionRequiredPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _longTerm
            // 
            this._longTerm.CanvasColor = System.Drawing.SystemColors.Control;
            this._longTerm.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._longTerm.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._longTerm.Controls.Add(this._longTermPalette);
            this._longTerm.DisabledBackColor = System.Drawing.Color.Empty;
            this._longTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this._longTerm.HideControlsWhenCollapsed = true;
            this._longTerm.Location = new System.Drawing.Point(573, 3);
            this._longTerm.Name = "_longTerm";
            this._longTerm.Size = new System.Drawing.Size(184, 131);
            this._longTerm.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._longTerm.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._longTerm.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._longTerm.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._longTerm.Style.GradientAngle = 90;
            this._longTerm.TabIndex = 3;
            this._longTerm.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._longTerm.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._longTerm.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._longTerm.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._longTerm.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._longTerm.TitleStyle.GradientAngle = 90;
            this._longTerm.TitleText = "Long Term";
            this._longTerm.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._longTerm_ExpandedChanged);
            // 
            // _longTermPalette
            // 
            this._longTermPalette.AllowDelete = false;
            this._longTermPalette.AllowEdit = false;
            this._longTermPalette.AllowInsert = false;
            this._longTermPalette.AllowLink = false;
            this._longTermPalette.AllowMove = false;
            this._longTermPalette.AllowReshape = false;
            this._longTermPalette.AllowResize = false;
            this._longTermPalette.ArrowMoveLarge = 10F;
            this._longTermPalette.ArrowMoveSmall = 1F;
            this._longTermPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._longTermPalette.BackColor = System.Drawing.Color.White;
            this._longTermPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._longTermPalette.DragsRealtime = true;
            this._longTermPalette.GridCellSizeHeight = 58F;
            this._longTermPalette.GridCellSizeWidth = 52F;
            this._longTermPalette.GridOriginX = 20F;
            this._longTermPalette.GridOriginY = 5F;
            this._longTermPalette.HidesSelection = true;
            this._longTermPalette.Location = new System.Drawing.Point(0, 26);
            this._longTermPalette.Name = "_longTermPalette";
            this._longTermPalette.ShowsNegativeCoordinates = false;
            this._longTermPalette.Size = new System.Drawing.Size(184, 105);
            this._longTermPalette.TabIndex = 4;
            this._longTermPalette.Text = "roadmapPalette4";
            this._longTermPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _midTerm
            // 
            this._midTerm.CanvasColor = System.Drawing.SystemColors.Control;
            this._midTerm.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._midTerm.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._midTerm.Controls.Add(this._midTermPalette);
            this._midTerm.DisabledBackColor = System.Drawing.Color.Empty;
            this._midTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this._midTerm.HideControlsWhenCollapsed = true;
            this._midTerm.Location = new System.Drawing.Point(383, 3);
            this._midTerm.Name = "_midTerm";
            this._midTerm.Size = new System.Drawing.Size(184, 131);
            this._midTerm.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._midTerm.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._midTerm.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._midTerm.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._midTerm.Style.GradientAngle = 90;
            this._midTerm.TabIndex = 2;
            this._midTerm.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._midTerm.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._midTerm.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._midTerm.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._midTerm.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._midTerm.TitleStyle.GradientAngle = 90;
            this._midTerm.TitleText = "Mid Term";
            this._midTerm.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._midTerm_ExpandedChanged);
            // 
            // _midTermPalette
            // 
            this._midTermPalette.AllowDelete = false;
            this._midTermPalette.AllowEdit = false;
            this._midTermPalette.AllowInsert = false;
            this._midTermPalette.AllowLink = false;
            this._midTermPalette.AllowMove = false;
            this._midTermPalette.AllowReshape = false;
            this._midTermPalette.AllowResize = false;
            this._midTermPalette.ArrowMoveLarge = 10F;
            this._midTermPalette.ArrowMoveSmall = 1F;
            this._midTermPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._midTermPalette.BackColor = System.Drawing.Color.White;
            this._midTermPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._midTermPalette.DragsRealtime = true;
            this._midTermPalette.GridCellSizeHeight = 58F;
            this._midTermPalette.GridCellSizeWidth = 52F;
            this._midTermPalette.GridOriginX = 20F;
            this._midTermPalette.GridOriginY = 5F;
            this._midTermPalette.HidesSelection = true;
            this._midTermPalette.Location = new System.Drawing.Point(0, 26);
            this._midTermPalette.Name = "_midTermPalette";
            this._midTermPalette.ShowsNegativeCoordinates = false;
            this._midTermPalette.Size = new System.Drawing.Size(184, 105);
            this._midTermPalette.TabIndex = 4;
            this._midTermPalette.Text = "roadmapPalette3";
            this._midTermPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _shortTerm
            // 
            this._shortTerm.CanvasColor = System.Drawing.SystemColors.Control;
            this._shortTerm.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._shortTerm.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._shortTerm.Controls.Add(this._shortTermPalette);
            this._shortTerm.DisabledBackColor = System.Drawing.Color.Empty;
            this._shortTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shortTerm.HideControlsWhenCollapsed = true;
            this._shortTerm.Location = new System.Drawing.Point(193, 3);
            this._shortTerm.Name = "_shortTerm";
            this._shortTerm.Size = new System.Drawing.Size(184, 131);
            this._shortTerm.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._shortTerm.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._shortTerm.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._shortTerm.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._shortTerm.Style.GradientAngle = 90;
            this._shortTerm.TabIndex = 1;
            this._shortTerm.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._shortTerm.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._shortTerm.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._shortTerm.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._shortTerm.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._shortTerm.TitleStyle.GradientAngle = 90;
            this._shortTerm.TitleText = "Short Term";
            this._shortTerm.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._shortTerm_ExpandedChanged);
            // 
            // _shortTermPalette
            // 
            this._shortTermPalette.AllowDelete = false;
            this._shortTermPalette.AllowEdit = false;
            this._shortTermPalette.AllowInsert = false;
            this._shortTermPalette.AllowLink = false;
            this._shortTermPalette.AllowMove = false;
            this._shortTermPalette.AllowReshape = false;
            this._shortTermPalette.AllowResize = false;
            this._shortTermPalette.ArrowMoveLarge = 10F;
            this._shortTermPalette.ArrowMoveSmall = 1F;
            this._shortTermPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._shortTermPalette.BackColor = System.Drawing.Color.White;
            this._shortTermPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shortTermPalette.DragsRealtime = true;
            this._shortTermPalette.GridCellSizeHeight = 58F;
            this._shortTermPalette.GridCellSizeWidth = 52F;
            this._shortTermPalette.GridOriginX = 20F;
            this._shortTermPalette.GridOriginY = 5F;
            this._shortTermPalette.HidesSelection = true;
            this._shortTermPalette.Location = new System.Drawing.Point(0, 26);
            this._shortTermPalette.Name = "_shortTermPalette";
            this._shortTermPalette.ShowsNegativeCoordinates = false;
            this._shortTermPalette.Size = new System.Drawing.Size(184, 105);
            this._shortTermPalette.TabIndex = 4;
            this._shortTermPalette.Text = "roadmapPalette2";
            this._shortTermPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _notAssessed
            // 
            this._notAssessed.CanvasColor = System.Drawing.SystemColors.Control;
            this._notAssessed.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._notAssessed.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._notAssessed.Controls.Add(this._notAssessedPalette);
            this._notAssessed.DisabledBackColor = System.Drawing.Color.Empty;
            this._notAssessed.Dock = System.Windows.Forms.DockStyle.Fill;
            this._notAssessed.HideControlsWhenCollapsed = true;
            this._notAssessed.Location = new System.Drawing.Point(3, 3);
            this._notAssessed.Name = "_notAssessed";
            this._notAssessed.Size = new System.Drawing.Size(184, 131);
            this._notAssessed.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._notAssessed.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._notAssessed.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._notAssessed.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._notAssessed.Style.GradientAngle = 90;
            this._notAssessed.TabIndex = 0;
            this._notAssessed.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._notAssessed.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._notAssessed.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._notAssessed.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._notAssessed.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._notAssessed.TitleStyle.GradientAngle = 90;
            this._notAssessed.TitleText = "Not Assessed";
            this._notAssessed.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._notAssessed_ExpandedChanged);
            // 
            // _notAssessedPalette
            // 
            this._notAssessedPalette.AllowDelete = false;
            this._notAssessedPalette.AllowEdit = false;
            this._notAssessedPalette.AllowInsert = false;
            this._notAssessedPalette.AllowLink = false;
            this._notAssessedPalette.AllowMove = false;
            this._notAssessedPalette.AllowReshape = false;
            this._notAssessedPalette.AllowResize = false;
            this._notAssessedPalette.ArrowMoveLarge = 10F;
            this._notAssessedPalette.ArrowMoveSmall = 1F;
            this._notAssessedPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._notAssessedPalette.BackColor = System.Drawing.Color.White;
            this._notAssessedPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._notAssessedPalette.DragsRealtime = true;
            this._notAssessedPalette.GridCellSizeHeight = 58F;
            this._notAssessedPalette.GridCellSizeWidth = 52F;
            this._notAssessedPalette.GridOriginX = 20F;
            this._notAssessedPalette.GridOriginY = 5F;
            this._notAssessedPalette.HidesSelection = true;
            this._notAssessedPalette.Location = new System.Drawing.Point(0, 26);
            this._notAssessedPalette.Name = "_notAssessedPalette";
            this._notAssessedPalette.ShowsNegativeCoordinates = false;
            this._notAssessedPalette.Size = new System.Drawing.Size(184, 105);
            this._notAssessedPalette.TabIndex = 4;
            this._notAssessedPalette.Text = "roadmapPalette1";
            this._notAssessedPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.DelayTooltipHideDuration = 250;
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._superTooltip.MarkupLinkClick += new DevComponents.DotNetBar.MarkupLinkClickEventHandler(this._superTooltip_MarkupLinkClick);
            // 
            // _tooltipTimer
            // 
            this._tooltipTimer.Interval = 1000;
            this._tooltipTimer.Tick += new System.EventHandler(this._tooltipTimer_Tick);
            // 
            // RoadmapPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._container);
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._bottomPanel);
            this.Name = "RoadmapPanel";
            this.Size = new System.Drawing.Size(953, 594);
            this._bottomPanel.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this._noActionRequired.ResumeLayout(false);
            this._longTerm.ResumeLayout(false);
            this._midTerm.ResumeLayout(false);
            this._shortTerm.ResumeLayout(false);
            this._notAssessed.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ExpandableSplitter _splitter;
        private System.Windows.Forms.TableLayoutPanel _container;
        private DevComponents.DotNetBar.ExpandablePanel _noActionRequired;
        private DevComponents.DotNetBar.ExpandablePanel _longTerm;
        private DevComponents.DotNetBar.ExpandablePanel _midTerm;
        private DevComponents.DotNetBar.ExpandablePanel _shortTerm;
        private DevComponents.DotNetBar.ExpandablePanel _notAssessed;
        private RoadmapPalette _noActionRequiredPalette;
        private RoadmapPalette _longTermPalette;
        private RoadmapPalette _midTermPalette;
        private RoadmapPalette _shortTermPalette;
        private RoadmapPalette _notAssessedPalette;
        private Utilities.WinForms.ItemEditor _itemDetails;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private System.Windows.Forms.Timer _tooltipTimer;
        private RoadmapChart _chart;
        private System.Windows.Forms.Panel _bottomPanel;
        private DevComponents.DotNetBar.ExpandableSplitter _bottomRightSplitter;
        private DevComponents.DotNetBar.ExpandableSplitter _bottomLeftSplitter;
        private Utilities.WinForms.ItemEditor _gridItemDetails;
        private DevComponents.DotNetBar.ExpandableSplitter _bottomCenterSplitter;
    }
}
