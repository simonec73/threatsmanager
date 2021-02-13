namespace ThreatsManager.Extensions.Panels
{
    partial class RoadmapChart
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
            _chart.PreRenderSeriesBar -= _chart_PreRenderSeriesBar;

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
            DevComponents.DotNetBar.Charts.ChartXy chartXy1 = new DevComponents.DotNetBar.Charts.ChartXy();
            DevComponents.DotNetBar.Charts.ReferenceLine referenceLine1 = new DevComponents.DotNetBar.Charts.ReferenceLine();
            DevComponents.DotNetBar.Charts.Style.Background background1 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.ChartSeries chartSeries1 = new DevComponents.DotNetBar.Charts.ChartSeries();
            DevComponents.DotNetBar.Charts.SeriesPoint seriesPoint1 = new DevComponents.DotNetBar.Charts.SeriesPoint();
            DevComponents.DotNetBar.Charts.SeriesPoint seriesPoint2 = new DevComponents.DotNetBar.Charts.SeriesPoint();
            DevComponents.DotNetBar.Charts.SeriesPoint seriesPoint3 = new DevComponents.DotNetBar.Charts.SeriesPoint();
            DevComponents.DotNetBar.Charts.SeriesPoint seriesPoint4 = new DevComponents.DotNetBar.Charts.SeriesPoint();
            DevComponents.DotNetBar.Charts.Style.Background background2 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background3 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor1 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Background background4 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor2 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Padding padding1 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background5 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor3 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Thickness thickness1 = new DevComponents.DotNetBar.Charts.Style.Thickness();
            DevComponents.DotNetBar.Charts.Style.Padding padding2 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Padding padding3 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background6 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background7 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background8 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background9 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background10 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background11 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background12 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background13 = new DevComponents.DotNetBar.Charts.Style.Background();
            this._chart = new DevComponents.DotNetBar.Charts.ChartControl();
            this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._snapshot = new System.Windows.Forms.ToolStripMenuItem();
            this._contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _chart
            // 
            chartXy1.AxisX.AutoCalcBarMargins = false;
            chartXy1.AxisX.MajorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.Gainsboro;
            chartXy1.AxisX.MajorGridLines.Visible = false;
            chartXy1.AxisX.MajorTickmarks.AutoTickmarkLayout = false;
            chartXy1.AxisX.MinorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.WhiteSmoke;
            chartXy1.AxisX.MinorGridLines.Visible = false;
            chartXy1.AxisX.MinorTickmarks.Visible = false;
            chartXy1.AxisX.Title.Name = null;
            chartXy1.AxisY.AxisAlignment = DevComponents.DotNetBar.Charts.AxisAlignment.Far;
            chartXy1.AxisY.ChartAxisVisualStyle.AxisColor = System.Drawing.Color.Transparent;
            chartXy1.AxisY.MajorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.Gainsboro;
            chartXy1.AxisY.MajorGridLines.Visible = false;
            chartXy1.AxisY.MajorTickmarks.ShowLabels = false;
            chartXy1.AxisY.MajorTickmarks.Visible = false;
            chartXy1.AxisY.MaxValue = 100;
            chartXy1.AxisY.MinorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.WhiteSmoke;
            chartXy1.AxisY.MinorGridLines.Visible = false;
            chartXy1.AxisY.MinorTickmarks.Visible = false;
            chartXy1.AxisY.MinValue = 0;
            referenceLine1.AxisValue = 0F;
            referenceLine1.Name = "AcceptableRisk";
            referenceLine1.ReferenceLineVisualStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            referenceLine1.ReferenceLineVisualStyle.LineColor = System.Drawing.Color.Gray;
            referenceLine1.ReferenceLineVisualStyle.LinePattern = DevComponents.DotNetBar.Charts.Style.LinePattern.Dash;
            referenceLine1.ReferenceLineVisualStyle.TextAlignment = DevComponents.DotNetBar.Charts.Style.Alignment.TopRight;
            referenceLine1.ReferenceLineVisualStyle.TextColor = System.Drawing.Color.Gray;
            referenceLine1.Text = "Acceptable Risk";
            chartXy1.AxisY.ReferenceLines.Add(referenceLine1);
            chartXy1.AxisY.Title.Name = null;
            chartXy1.BarShadingEnabled = DevComponents.DotNetBar.Charts.Style.Tbool.False;
            chartXy1.ChartCrosshair.CrosshairLabelMode = DevComponents.DotNetBar.Charts.CrosshairLabelMode.NearestSeries;
            background1.Color1 = System.Drawing.Color.White;
            chartXy1.ChartCrosshair.CrosshairVisualStyle.Background = background1;
            chartXy1.ChartCrosshair.HighlightPoints = true;
            chartSeries1.CrosshairHighlightPoints = DevComponents.DotNetBar.Charts.Style.Tbool.False;
            chartSeries1.EmptyValues = null;
            chartSeries1.Name = "Risk";
            seriesPoint1.ValueX = "Current";
            seriesPoint1.ValueY = new object[] {
        ((object)(0F))};
            seriesPoint2.ValueX = "Short Term";
            seriesPoint2.ValueY = new object[] {
        ((object)(0F))};
            seriesPoint3.ValueX = "Mid Term";
            seriesPoint3.ValueY = new object[] {
        ((object)(0F))};
            seriesPoint4.ValueX = "Long Term";
            seriesPoint4.ValueY = new object[] {
        ((object)(0F))};
            chartSeries1.SeriesPoints.Add(seriesPoint1);
            chartSeries1.SeriesPoints.Add(seriesPoint2);
            chartSeries1.SeriesPoints.Add(seriesPoint3);
            chartSeries1.SeriesPoints.Add(seriesPoint4);
            chartSeries1.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.VerticalBar;
            chartXy1.ChartSeries.Add(chartSeries1);
            background2.Color1 = System.Drawing.Color.Yellow;
            chartXy1.ChartSeriesVisualStyle.MarkerHighlightVisualStyle.Background = background2;
            chartXy1.ChartSeriesVisualStyle.MarkerHighlightVisualStyle.Size = new System.Drawing.Size(15, 15);
            chartXy1.ChartSeriesVisualStyle.MarkerHighlightVisualStyle.Type = DevComponents.DotNetBar.Charts.PointMarkerType.Ellipse;
            background3.Color1 = System.Drawing.Color.White;
            chartXy1.ChartVisualStyle.Background = background3;
            borderColor1.Bottom = System.Drawing.Color.Black;
            borderColor1.Left = System.Drawing.Color.Black;
            borderColor1.Right = System.Drawing.Color.Black;
            borderColor1.Top = System.Drawing.Color.Black;
            chartXy1.ChartVisualStyle.BorderColor = borderColor1;
            background4.Color1 = System.Drawing.Color.White;
            chartXy1.ContainerVisualStyles.Default.Background = background4;
            borderColor2.Bottom = System.Drawing.Color.DimGray;
            borderColor2.Left = System.Drawing.Color.DimGray;
            borderColor2.Right = System.Drawing.Color.DimGray;
            borderColor2.Top = System.Drawing.Color.DimGray;
            chartXy1.ContainerVisualStyles.Default.BorderColor = borderColor2;
            chartXy1.ContainerVisualStyles.Default.DropShadow.Enabled = DevComponents.DotNetBar.Charts.Style.Tbool.False;
            padding1.Bottom = 6;
            padding1.Left = 6;
            padding1.Right = 6;
            padding1.Top = 6;
            chartXy1.ContainerVisualStyles.Default.Padding = padding1;
            chartXy1.CustomPalette = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))))};
            chartXy1.Legend.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.TopLeft;
            chartXy1.Legend.AlignVerticalItems = true;
            background5.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartXy1.Legend.ChartLegendVisualStyles.Default.Background = background5;
            borderColor3.Bottom = System.Drawing.Color.Black;
            borderColor3.Left = System.Drawing.Color.Black;
            borderColor3.Right = System.Drawing.Color.Black;
            borderColor3.Top = System.Drawing.Color.Black;
            chartXy1.Legend.ChartLegendVisualStyles.Default.BorderColor = borderColor3;
            thickness1.Bottom = 1;
            thickness1.Left = 1;
            thickness1.Right = 1;
            thickness1.Top = 1;
            chartXy1.Legend.ChartLegendVisualStyles.Default.BorderThickness = thickness1;
            padding2.Bottom = 8;
            padding2.Left = 8;
            padding2.Right = 8;
            padding2.Top = 8;
            chartXy1.Legend.ChartLegendVisualStyles.Default.Margin = padding2;
            padding3.Bottom = 4;
            padding3.Left = 4;
            padding3.Right = 4;
            padding3.Top = 4;
            chartXy1.Legend.ChartLegendVisualStyles.Default.Padding = padding3;
            chartXy1.Legend.Direction = DevComponents.DotNetBar.Charts.Direction.LeftToRight;
            chartXy1.Legend.MaxHorizontalPct = 75D;
            chartXy1.Legend.Placement = DevComponents.DotNetBar.Charts.Placement.Inside;
            chartXy1.Legend.Visible = false;
            chartXy1.Name = "Roadmap";
            chartXy1.PaletteGroup = DevComponents.DotNetBar.Charts.PaletteGroup.Custom;
            this._chart.ChartPanel.ChartContainers.Add(chartXy1);
            this._chart.ChartPanel.Legend.Visible = false;
            this._chart.ChartPanel.Name = "PrimaryPanel";
            this._chart.ContextMenuStrip = this._contextMenu;
            background6.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ArrowBackground = background6;
            background7.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ThumbBackground = background7;
            background8.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background8;
            background9.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background9;
            background10.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ArrowBackground = background10;
            background11.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ThumbBackground = background11;
            background12.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background12;
            background13.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background13;
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chart.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._chart.Location = new System.Drawing.Point(0, 0);
            this._chart.Margin = new System.Windows.Forms.Padding(0);
            this._chart.Name = "_chart";
            this._chart.Size = new System.Drawing.Size(448, 296);
            this._chart.TabIndex = 0;
            this._chart.Text = "chartControl1";
            this._chart.PreRenderSeriesBar += new System.EventHandler<DevComponents.DotNetBar.Charts.PreRenderSeriesBarEventArgs>(this._chart_PreRenderSeriesBar);
            // 
            // _contextMenu
            // 
            this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._snapshot});
            this._contextMenu.Name = "_contextMenu";
            this._contextMenu.Size = new System.Drawing.Size(212, 26);
            // 
            // _snapshot
            // 
            this._snapshot.Name = "_snapshot";
            this._snapshot.Size = new System.Drawing.Size(211, 22);
            this._snapshot.Text = "Get snapshot in Clipboard";
            this._snapshot.Click += new System.EventHandler(this._snapshot_Click);
            // 
            // RoadmapChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._chart);
            this.Name = "RoadmapChart";
            this.Size = new System.Drawing.Size(448, 296);
            this._contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Charts.ChartControl _chart;
        private System.Windows.Forms.ContextMenuStrip _contextMenu;
        private System.Windows.Forms.ToolStripMenuItem _snapshot;
    }
}
