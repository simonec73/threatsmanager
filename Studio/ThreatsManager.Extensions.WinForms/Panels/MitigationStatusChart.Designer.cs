namespace ThreatsManager.Extensions.Panels
{
    partial class MitigationStatusChart
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
            DevComponents.DotNetBar.Charts.PieChart pieChart1 = new DevComponents.DotNetBar.Charts.PieChart();
            DevComponents.DotNetBar.Charts.PieSeries pieSeries1 = new DevComponents.DotNetBar.Charts.PieSeries();
            DevComponents.DotNetBar.Charts.Style.Background background1 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor1 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Padding padding1 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background2 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor2 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Padding padding2 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background3 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.BorderColor borderColor3 = new DevComponents.DotNetBar.Charts.Style.BorderColor();
            DevComponents.DotNetBar.Charts.Style.Padding padding3 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Padding padding4 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.ChartTitle chartTitle1 = new DevComponents.DotNetBar.Charts.ChartTitle();
            DevComponents.DotNetBar.Charts.Style.Padding padding5 = new DevComponents.DotNetBar.Charts.Style.Padding();
            DevComponents.DotNetBar.Charts.Style.Background background4 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background5 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background6 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background7 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background8 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background9 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background10 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.Style.Background background11 = new DevComponents.DotNetBar.Charts.Style.Background();
            this._chart = new DevComponents.DotNetBar.Charts.ChartControl();
            this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._snapshot = new System.Windows.Forms.ToolStripMenuItem();
            this._contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _chart
            // 
            this._chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            pieChart1.CenterFirstSlice = DevComponents.DotNetBar.Charts.Style.Tbool.False;
            pieSeries1.Name = "Series1";
            pieSeries1.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.Pie;
            pieChart1.ChartSeries.Add(pieSeries1);
            background1.Color1 = System.Drawing.Color.White;
            pieChart1.ChartVisualStyle.Background = background1;
            borderColor1.Bottom = System.Drawing.Color.Black;
            borderColor1.Left = System.Drawing.Color.Black;
            borderColor1.Right = System.Drawing.Color.Black;
            borderColor1.Top = System.Drawing.Color.Black;
            pieChart1.ChartVisualStyle.BorderColor = borderColor1;
            padding1.Bottom = 6;
            padding1.Left = 6;
            padding1.Right = 6;
            padding1.Top = 6;
            pieChart1.ChartVisualStyle.Padding = padding1;
            background2.Color1 = System.Drawing.Color.White;
            pieChart1.ContainerVisualStyles.Default.Background = background2;
            borderColor2.Bottom = System.Drawing.Color.DimGray;
            borderColor2.Left = System.Drawing.Color.DimGray;
            borderColor2.Right = System.Drawing.Color.DimGray;
            borderColor2.Top = System.Drawing.Color.DimGray;
            pieChart1.ContainerVisualStyles.Default.BorderColor = borderColor2;
            pieChart1.ContainerVisualStyles.Default.DropShadow.Enabled = DevComponents.DotNetBar.Charts.Style.Tbool.False;
            padding2.Bottom = 6;
            padding2.Left = 6;
            padding2.Right = 6;
            padding2.Top = 6;
            pieChart1.ContainerVisualStyles.Default.Padding = padding2;
            pieChart1.DetachedOffset = 0D;
            pieChart1.EnableDragDetach = DevComponents.DotNetBar.Charts.Style.Tbool.False;
            pieChart1.ExplodedMargin = 0D;
            pieChart1.Legend.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.BottomCenter;
            pieChart1.Legend.AlignVerticalItems = true;
            pieChart1.Legend.ChartLegendItemVisualStyles.Default.TextColor = System.Drawing.Color.Black;
            background3.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            pieChart1.Legend.ChartLegendVisualStyles.Default.Background = background3;
            borderColor3.Bottom = System.Drawing.Color.Black;
            borderColor3.Left = System.Drawing.Color.Black;
            borderColor3.Right = System.Drawing.Color.Black;
            borderColor3.Top = System.Drawing.Color.Black;
            pieChart1.Legend.ChartLegendVisualStyles.Default.BorderColor = borderColor3;
            padding3.Bottom = 4;
            padding3.Left = 4;
            padding3.Right = 4;
            padding3.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Margin = padding3;
            padding4.Bottom = 4;
            padding4.Left = 4;
            padding4.Right = 4;
            padding4.Top = 4;
            pieChart1.Legend.ChartLegendVisualStyles.Default.Padding = padding4;
            pieChart1.Legend.Direction = DevComponents.DotNetBar.Charts.Direction.LeftToRight;
            pieChart1.Legend.MaxVerticalPct = 90D;
            pieChart1.Legend.Placement = DevComponents.DotNetBar.Charts.Placement.Outside;
            pieChart1.Legend.Visible = true;
            pieChart1.Name = "PieChart1";
            chartTitle1.ChartTitleVisualStyle.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.MiddleCenter;
            chartTitle1.ChartTitleVisualStyle.Font = new System.Drawing.Font("Georgia", 16F);
            padding5.Bottom = 8;
            padding5.Left = 8;
            padding5.Right = 8;
            padding5.Top = 8;
            chartTitle1.ChartTitleVisualStyle.Padding = padding5;
            chartTitle1.ChartTitleVisualStyle.TextColor = System.Drawing.Color.Navy;
            chartTitle1.Name = "Title1";
            chartTitle1.Text = "Chart Title";
            chartTitle1.Visible = false;
            chartTitle1.XyAlignment = DevComponents.DotNetBar.Charts.XyAlignment.Top;
            pieChart1.Titles.Add(chartTitle1);
            this._chart.ChartPanel.ChartContainers.Add(pieChart1);
            this._chart.ChartPanel.Legend.Visible = false;
            this._chart.ChartPanel.Name = "PrimaryPanel";
            this._chart.ContextMenuStrip = this._contextMenu;
            background4.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ArrowBackground = background4;
            background5.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.MouseOver.ThumbBackground = background5;
            background6.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background6;
            background7.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.HScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background7;
            background8.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ArrowBackground = background8;
            background9.Color1 = System.Drawing.Color.AliceBlue;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.MouseOver.ThumbBackground = background9;
            background10.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ArrowBackground = background10;
            background11.Color1 = System.Drawing.Color.White;
            this._chart.DefaultVisualStyles.VScrollBarVisualStyles.SelectedMouseOver.ThumbBackground = background11;
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chart.ForeColor = System.Drawing.Color.Black;
            this._chart.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._chart.Location = new System.Drawing.Point(0, 0);
            this._chart.Name = "_chart";
            this._chart.Size = new System.Drawing.Size(305, 293);
            this._chart.TabIndex = 0;
            this._chart.Text = "chartControl1";
            // 
            // _contextMenu
            // 
            this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._snapshot});
            this._contextMenu.Name = "_contextMenu";
            this._contextMenu.Size = new System.Drawing.Size(212, 48);
            // 
            // _snapshot
            // 
            this._snapshot.Name = "_snapshot";
            this._snapshot.Size = new System.Drawing.Size(211, 22);
            this._snapshot.Text = "Get snapshot in Clipboard";
            this._snapshot.Click += new System.EventHandler(this._snapshot_Click);
            // 
            // MitigationStatusChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._chart);
            this.Name = "MitigationStatusChart";
            this.Size = new System.Drawing.Size(305, 293);
            this._contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Charts.ChartControl _chart;
        private System.Windows.Forms.ContextMenuStrip _contextMenu;
        private System.Windows.Forms.ToolStripMenuItem _snapshot;
    }
}
