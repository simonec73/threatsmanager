using System.Windows.Forms;
using DevComponents.DotNetBar.Layout;

namespace ThreatsManager.DevOps.Panels
{
    partial class KanbanPanel
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
            this._container = new System.Windows.Forms.TableLayoutPanel();
            this._sixth = new DevComponents.DotNetBar.ExpandablePanel();
            this._fifth = new DevComponents.DotNetBar.ExpandablePanel();
            this._fourth = new DevComponents.DotNetBar.ExpandablePanel();
            this._third = new DevComponents.DotNetBar.ExpandablePanel();
            this._second = new DevComponents.DotNetBar.ExpandablePanel();
            this._first = new DevComponents.DotNetBar.ExpandablePanel();
            this._splitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._bottomPanel = new System.Windows.Forms.Panel();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._itemDetails = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._tooltipTimer = new System.Windows.Forms.Timer(this.components);
            this._sixthPalette = new ThreatsManager.DevOps.Panels.KanbanPalette();
            this._fifthPalette = new ThreatsManager.DevOps.Panels.KanbanPalette();
            this._fourthPalette = new ThreatsManager.DevOps.Panels.KanbanPalette();
            this._thirdPalette = new ThreatsManager.DevOps.Panels.KanbanPalette();
            this._secondPalette = new ThreatsManager.DevOps.Panels.KanbanPalette();
            this._firstPalette = new ThreatsManager.DevOps.Panels.KanbanPalette();
            this.expandableSplitter2 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._comments = new ThreatsManager.DevOps.Panels.CommentsPanel();
            this._container.SuspendLayout();
            this._sixth.SuspendLayout();
            this._fifth.SuspendLayout();
            this._fourth.SuspendLayout();
            this._third.SuspendLayout();
            this._second.SuspendLayout();
            this._first.SuspendLayout();
            this._bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.White;
            this._container.ColumnCount = 6;
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this._container.Controls.Add(this._sixth, 5, 0);
            this._container.Controls.Add(this._fifth, 4, 0);
            this._container.Controls.Add(this._fourth, 3, 0);
            this._container.Controls.Add(this._third, 2, 0);
            this._container.Controls.Add(this._second, 1, 0);
            this._container.Controls.Add(this._first, 0, 0);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            this._container.RowCount = 1;
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._container.Size = new System.Drawing.Size(953, 208);
            this._container.TabIndex = 2;
            // 
            // _sixth
            // 
            this._sixth.CanvasColor = System.Drawing.SystemColors.Control;
            this._sixth.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._sixth.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._sixth.Controls.Add(this._sixthPalette);
            this._sixth.DisabledBackColor = System.Drawing.Color.Empty;
            this._sixth.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sixth.HideControlsWhenCollapsed = true;
            this._sixth.Location = new System.Drawing.Point(793, 3);
            this._sixth.Name = "_sixth";
            this._sixth.Size = new System.Drawing.Size(157, 202);
            this._sixth.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._sixth.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._sixth.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._sixth.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._sixth.Style.GradientAngle = 90;
            this._sixth.TabIndex = 5;
            this._sixth.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._sixth.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._sixth.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._sixth.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._sixth.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._sixth.TitleStyle.GradientAngle = 90;
            this._sixth.TitleText = "Sixth";
            this._sixth.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._sixth_ExpandedChanged);
            // 
            // _fifth
            // 
            this._fifth.CanvasColor = System.Drawing.SystemColors.Control;
            this._fifth.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._fifth.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._fifth.Controls.Add(this._fifthPalette);
            this._fifth.DisabledBackColor = System.Drawing.Color.Empty;
            this._fifth.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fifth.HideControlsWhenCollapsed = true;
            this._fifth.Location = new System.Drawing.Point(635, 3);
            this._fifth.Name = "_fifth";
            this._fifth.Size = new System.Drawing.Size(152, 202);
            this._fifth.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._fifth.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._fifth.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._fifth.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._fifth.Style.GradientAngle = 90;
            this._fifth.TabIndex = 4;
            this._fifth.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._fifth.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._fifth.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._fifth.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._fifth.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._fifth.TitleStyle.GradientAngle = 90;
            this._fifth.TitleText = "Fifth";
            this._fifth.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._fifth_ExpandedChanged);
            // 
            // _fourth
            // 
            this._fourth.CanvasColor = System.Drawing.SystemColors.Control;
            this._fourth.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._fourth.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._fourth.Controls.Add(this._fourthPalette);
            this._fourth.DisabledBackColor = System.Drawing.Color.Empty;
            this._fourth.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fourth.HideControlsWhenCollapsed = true;
            this._fourth.Location = new System.Drawing.Point(477, 3);
            this._fourth.Name = "_fourth";
            this._fourth.Size = new System.Drawing.Size(152, 202);
            this._fourth.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._fourth.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._fourth.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._fourth.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._fourth.Style.GradientAngle = 90;
            this._fourth.TabIndex = 3;
            this._fourth.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._fourth.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._fourth.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._fourth.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._fourth.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._fourth.TitleStyle.GradientAngle = 90;
            this._fourth.TitleText = "Fourth";
            this._fourth.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._fourth_ExpandedChanged);
            // 
            // _third
            // 
            this._third.CanvasColor = System.Drawing.SystemColors.Control;
            this._third.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._third.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._third.Controls.Add(this._thirdPalette);
            this._third.DisabledBackColor = System.Drawing.Color.Empty;
            this._third.Dock = System.Windows.Forms.DockStyle.Fill;
            this._third.HideControlsWhenCollapsed = true;
            this._third.Location = new System.Drawing.Point(319, 3);
            this._third.Name = "_third";
            this._third.Size = new System.Drawing.Size(152, 202);
            this._third.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._third.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._third.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._third.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._third.Style.GradientAngle = 90;
            this._third.TabIndex = 2;
            this._third.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._third.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._third.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._third.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._third.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._third.TitleStyle.GradientAngle = 90;
            this._third.TitleText = "Third";
            this._third.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._third_ExpandedChanged);
            // 
            // _second
            // 
            this._second.CanvasColor = System.Drawing.SystemColors.Control;
            this._second.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._second.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._second.Controls.Add(this._secondPalette);
            this._second.DisabledBackColor = System.Drawing.Color.Empty;
            this._second.Dock = System.Windows.Forms.DockStyle.Fill;
            this._second.HideControlsWhenCollapsed = true;
            this._second.Location = new System.Drawing.Point(161, 3);
            this._second.Name = "_second";
            this._second.Size = new System.Drawing.Size(152, 202);
            this._second.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._second.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._second.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._second.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._second.Style.GradientAngle = 90;
            this._second.TabIndex = 1;
            this._second.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._second.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._second.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._second.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._second.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._second.TitleStyle.GradientAngle = 90;
            this._second.TitleText = "Second";
            this._second.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._second_ExpandedChanged);
            // 
            // _first
            // 
            this._first.CanvasColor = System.Drawing.SystemColors.Control;
            this._first.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this._first.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._first.Controls.Add(this._firstPalette);
            this._first.DisabledBackColor = System.Drawing.Color.Empty;
            this._first.Dock = System.Windows.Forms.DockStyle.Fill;
            this._first.HideControlsWhenCollapsed = true;
            this._first.Location = new System.Drawing.Point(3, 3);
            this._first.Name = "_first";
            this._first.Size = new System.Drawing.Size(152, 202);
            this._first.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._first.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._first.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._first.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._first.Style.GradientAngle = 90;
            this._first.TabIndex = 0;
            this._first.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this._first.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._first.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this._first.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._first.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._first.TitleStyle.GradientAngle = 90;
            this._first.TitleText = "First";
            this._first.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this._first_ExpandedChanged);
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
            this._splitter.Location = new System.Drawing.Point(0, 208);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(953, 6);
            this._splitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._splitter.TabIndex = 1;
            this._splitter.TabStop = false;
            // 
            // _bottomPanel
            // 
            this._bottomPanel.BackColor = System.Drawing.Color.White;
            this._bottomPanel.Controls.Add(this._grid);
            this._bottomPanel.Controls.Add(this.expandableSplitter2);
            this._bottomPanel.Controls.Add(this._comments);
            this._bottomPanel.Controls.Add(this.expandableSplitter1);
            this._bottomPanel.Controls.Add(this._itemDetails);
            this._bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomPanel.Location = new System.Drawing.Point(0, 214);
            this._bottomPanel.Margin = new System.Windows.Forms.Padding(0);
            this._bottomPanel.Name = "_bottomPanel";
            this._bottomPanel.Size = new System.Drawing.Size(953, 380);
            this._bottomPanel.TabIndex = 0;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.ExpandableControl = this._itemDetails;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
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
            this.expandableSplitter1.Location = new System.Drawing.Point(438, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(6, 380);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 1;
            this.expandableSplitter1.TabStop = false;
            // 
            // _itemDetails
            // 
            this._itemDetails.BackColor = System.Drawing.Color.White;
            this._itemDetails.Dock = System.Windows.Forms.DockStyle.Left;
            this._itemDetails.Item = null;
            this._itemDetails.Location = new System.Drawing.Point(0, 0);
            this._itemDetails.Name = "_itemDetails";
            this._itemDetails.ReadOnly = false;
            this._itemDetails.Size = new System.Drawing.Size(438, 380);
            this._itemDetails.TabIndex = 0;
            // 
            // _grid
            // 
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(444, 0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(37, 380);
            this._grid.TabIndex = 2;
            this._grid.Text = "superGridControl1";
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.DelayTooltipHideDuration = 250;
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            // 
            // _tooltipTimer
            // 
            this._tooltipTimer.Interval = 1000;
            // 
            // _sixthPalette
            // 
            this._sixthPalette.AllowDelete = false;
            this._sixthPalette.AllowEdit = false;
            this._sixthPalette.AllowInsert = false;
            this._sixthPalette.AllowLink = false;
            this._sixthPalette.AllowMove = false;
            this._sixthPalette.AllowReshape = false;
            this._sixthPalette.AllowResize = false;
            this._sixthPalette.ArrowMoveLarge = 10F;
            this._sixthPalette.ArrowMoveSmall = 1F;
            this._sixthPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._sixthPalette.BackColor = System.Drawing.Color.White;
            this._sixthPalette.ContainerId = 5;
            this._sixthPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sixthPalette.DragsRealtime = true;
            this._sixthPalette.GridCellSizeHeight = 58F;
            this._sixthPalette.GridCellSizeWidth = 52F;
            this._sixthPalette.GridOriginX = 20F;
            this._sixthPalette.GridOriginY = 5F;
            this._sixthPalette.HidesSelection = true;
            this._sixthPalette.Location = new System.Drawing.Point(0, 26);
            this._sixthPalette.Name = "_sixthPalette";
            this._sixthPalette.ShowsNegativeCoordinates = false;
            this._sixthPalette.Size = new System.Drawing.Size(157, 176);
            this._sixthPalette.TabIndex = 4;
            this._sixthPalette.Text = "roadmapPalette5";
            this._sixthPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _fifthPalette
            // 
            this._fifthPalette.AllowDelete = false;
            this._fifthPalette.AllowEdit = false;
            this._fifthPalette.AllowInsert = false;
            this._fifthPalette.AllowLink = false;
            this._fifthPalette.AllowMove = false;
            this._fifthPalette.AllowReshape = false;
            this._fifthPalette.AllowResize = false;
            this._fifthPalette.ArrowMoveLarge = 10F;
            this._fifthPalette.ArrowMoveSmall = 1F;
            this._fifthPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._fifthPalette.BackColor = System.Drawing.Color.White;
            this._fifthPalette.ContainerId = 4;
            this._fifthPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fifthPalette.DragsRealtime = true;
            this._fifthPalette.GridCellSizeHeight = 58F;
            this._fifthPalette.GridCellSizeWidth = 52F;
            this._fifthPalette.GridOriginX = 20F;
            this._fifthPalette.GridOriginY = 5F;
            this._fifthPalette.HidesSelection = true;
            this._fifthPalette.Location = new System.Drawing.Point(0, 26);
            this._fifthPalette.Name = "_fifthPalette";
            this._fifthPalette.ShowsNegativeCoordinates = false;
            this._fifthPalette.Size = new System.Drawing.Size(152, 176);
            this._fifthPalette.TabIndex = 4;
            this._fifthPalette.Text = "roadmapPalette5";
            this._fifthPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _fourthPalette
            // 
            this._fourthPalette.AllowDelete = false;
            this._fourthPalette.AllowEdit = false;
            this._fourthPalette.AllowInsert = false;
            this._fourthPalette.AllowLink = false;
            this._fourthPalette.AllowMove = false;
            this._fourthPalette.AllowReshape = false;
            this._fourthPalette.AllowResize = false;
            this._fourthPalette.ArrowMoveLarge = 10F;
            this._fourthPalette.ArrowMoveSmall = 1F;
            this._fourthPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._fourthPalette.BackColor = System.Drawing.Color.White;
            this._fourthPalette.ContainerId = 3;
            this._fourthPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fourthPalette.DragsRealtime = true;
            this._fourthPalette.GridCellSizeHeight = 58F;
            this._fourthPalette.GridCellSizeWidth = 52F;
            this._fourthPalette.GridOriginX = 20F;
            this._fourthPalette.GridOriginY = 5F;
            this._fourthPalette.HidesSelection = true;
            this._fourthPalette.Location = new System.Drawing.Point(0, 26);
            this._fourthPalette.Name = "_fourthPalette";
            this._fourthPalette.ShowsNegativeCoordinates = false;
            this._fourthPalette.Size = new System.Drawing.Size(152, 176);
            this._fourthPalette.TabIndex = 4;
            this._fourthPalette.Text = "roadmapPalette4";
            this._fourthPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _thirdPalette
            // 
            this._thirdPalette.AllowDelete = false;
            this._thirdPalette.AllowEdit = false;
            this._thirdPalette.AllowInsert = false;
            this._thirdPalette.AllowLink = false;
            this._thirdPalette.AllowMove = false;
            this._thirdPalette.AllowReshape = false;
            this._thirdPalette.AllowResize = false;
            this._thirdPalette.ArrowMoveLarge = 10F;
            this._thirdPalette.ArrowMoveSmall = 1F;
            this._thirdPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._thirdPalette.BackColor = System.Drawing.Color.White;
            this._thirdPalette.ContainerId = 2;
            this._thirdPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._thirdPalette.DragsRealtime = true;
            this._thirdPalette.GridCellSizeHeight = 58F;
            this._thirdPalette.GridCellSizeWidth = 52F;
            this._thirdPalette.GridOriginX = 20F;
            this._thirdPalette.GridOriginY = 5F;
            this._thirdPalette.HidesSelection = true;
            this._thirdPalette.Location = new System.Drawing.Point(0, 26);
            this._thirdPalette.Name = "_thirdPalette";
            this._thirdPalette.ShowsNegativeCoordinates = false;
            this._thirdPalette.Size = new System.Drawing.Size(152, 176);
            this._thirdPalette.TabIndex = 4;
            this._thirdPalette.Text = "roadmapPalette3";
            this._thirdPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _secondPalette
            // 
            this._secondPalette.AllowDelete = false;
            this._secondPalette.AllowEdit = false;
            this._secondPalette.AllowInsert = false;
            this._secondPalette.AllowLink = false;
            this._secondPalette.AllowMove = false;
            this._secondPalette.AllowReshape = false;
            this._secondPalette.AllowResize = false;
            this._secondPalette.ArrowMoveLarge = 10F;
            this._secondPalette.ArrowMoveSmall = 1F;
            this._secondPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._secondPalette.BackColor = System.Drawing.Color.White;
            this._secondPalette.ContainerId = 1;
            this._secondPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._secondPalette.DragsRealtime = true;
            this._secondPalette.GridCellSizeHeight = 58F;
            this._secondPalette.GridCellSizeWidth = 52F;
            this._secondPalette.GridOriginX = 20F;
            this._secondPalette.GridOriginY = 5F;
            this._secondPalette.HidesSelection = true;
            this._secondPalette.Location = new System.Drawing.Point(0, 26);
            this._secondPalette.Name = "_secondPalette";
            this._secondPalette.ShowsNegativeCoordinates = false;
            this._secondPalette.Size = new System.Drawing.Size(152, 176);
            this._secondPalette.TabIndex = 4;
            this._secondPalette.Text = "roadmapPalette2";
            this._secondPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // _firstPalette
            // 
            this._firstPalette.AllowDelete = false;
            this._firstPalette.AllowEdit = false;
            this._firstPalette.AllowInsert = false;
            this._firstPalette.AllowLink = false;
            this._firstPalette.AllowMove = false;
            this._firstPalette.AllowReshape = false;
            this._firstPalette.AllowResize = false;
            this._firstPalette.ArrowMoveLarge = 10F;
            this._firstPalette.ArrowMoveSmall = 1F;
            this._firstPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._firstPalette.BackColor = System.Drawing.Color.White;
            this._firstPalette.ContainerId = 0;
            this._firstPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._firstPalette.DragsRealtime = true;
            this._firstPalette.GridCellSizeHeight = 58F;
            this._firstPalette.GridCellSizeWidth = 52F;
            this._firstPalette.GridOriginX = 20F;
            this._firstPalette.GridOriginY = 5F;
            this._firstPalette.HidesSelection = true;
            this._firstPalette.Location = new System.Drawing.Point(0, 26);
            this._firstPalette.Name = "_firstPalette";
            this._firstPalette.ShowsNegativeCoordinates = false;
            this._firstPalette.Size = new System.Drawing.Size(152, 176);
            this._firstPalette.TabIndex = 4;
            this._firstPalette.Text = "roadmapPalette1";
            this._firstPalette.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.ObjectSingleClicked);
            // 
            // expandableSplitter2
            // 
            this.expandableSplitter2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandableSplitter2.ExpandableControl = this._comments;
            this.expandableSplitter2.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter2.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter2.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter2.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter2.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter2.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter2.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter2.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter2.Location = new System.Drawing.Point(481, 0);
            this.expandableSplitter2.Name = "expandableSplitter2";
            this.expandableSplitter2.Size = new System.Drawing.Size(6, 380);
            this.expandableSplitter2.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter2.TabIndex = 4;
            this.expandableSplitter2.TabStop = false;
            // 
            // _comments
            // 
            this._comments.BackColor = System.Drawing.Color.White;
            this._comments.Dock = System.Windows.Forms.DockStyle.Right;
            this._comments.Location = new System.Drawing.Point(487, 0);
            this._comments.Name = "_comments";
            this._comments.Size = new System.Drawing.Size(466, 380);
            this._comments.TabIndex = 3;
            // 
            // KanbanPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._container);
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._bottomPanel);
            this.Name = "KanbanPanel";
            this.Size = new System.Drawing.Size(953, 594);
            this._container.ResumeLayout(false);
            this._sixth.ResumeLayout(false);
            this._fifth.ResumeLayout(false);
            this._fourth.ResumeLayout(false);
            this._third.ResumeLayout(false);
            this._second.ResumeLayout(false);
            this._first.ResumeLayout(false);
            this._bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ExpandableSplitter _splitter;
        private System.Windows.Forms.TableLayoutPanel _container;
        private DevComponents.DotNetBar.ExpandablePanel _sixth;
        private DevComponents.DotNetBar.ExpandablePanel _fifth;
        private DevComponents.DotNetBar.ExpandablePanel _fourth;
        private DevComponents.DotNetBar.ExpandablePanel _third;
        private DevComponents.DotNetBar.ExpandablePanel _second;
        private DevComponents.DotNetBar.ExpandablePanel _first;
        private KanbanPalette _sixthPalette;
        private KanbanPalette _fifthPalette;
        private KanbanPalette _fourthPalette;
        private KanbanPalette _thirdPalette;
        private KanbanPalette _secondPalette;
        private KanbanPalette _firstPalette;
        private Panel _bottomPanel;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter2;
        private CommentsPanel _comments;
        private Utilities.WinForms.ItemEditor _itemDetails;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private Timer _tooltipTimer;
    }
}
