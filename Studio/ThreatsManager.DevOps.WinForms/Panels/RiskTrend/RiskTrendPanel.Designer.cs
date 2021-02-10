
using System.Drawing;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels.RiskTrend
{
    partial class RiskTrendPanel
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
            DevComponents.DotNetBar.Charts.ChartXy chartXy1 = new DevComponents.DotNetBar.Charts.ChartXy();
            DevComponents.DotNetBar.Charts.ReferenceLine referenceLine1 = new DevComponents.DotNetBar.Charts.ReferenceLine();
            DevComponents.DotNetBar.Charts.Style.Background background1 = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.ChartSeries chartSeries1 = new DevComponents.DotNetBar.Charts.ChartSeries();
            DevComponents.DotNetBar.Charts.Style.Padding padding1 = new DevComponents.DotNetBar.Charts.Style.Padding();
            this._chart = new DevComponents.DotNetBar.Charts.ChartControl();
            this.SuspendLayout();
            // 
            // _chart
            // 
            chartXy1.AxisX.MajorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.Gainsboro;
            chartXy1.AxisX.MinorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.WhiteSmoke;
            chartXy1.AxisX.MinorGridLines.Visible = false;
            chartXy1.AxisX.MinorTickmarks.Visible = false;
            chartXy1.AxisX.Title.ChartTitleVisualStyle.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.BottomRight;
            chartXy1.AxisX.Title.Name = null;
            chartXy1.AxisX.Title.Text = "Iterations";
            chartXy1.AxisY.AxisAlignment = DevComponents.DotNetBar.Charts.AxisAlignment.Far;
            chartXy1.AxisY.MajorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.Gainsboro;
            chartXy1.AxisY.MajorGridLines.Visible = false;
            chartXy1.AxisY.MajorTickmarks.ShowLabels = false;
            chartXy1.AxisY.MajorTickmarks.Visible = false;
            chartXy1.AxisY.MinorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.WhiteSmoke;
            chartXy1.AxisY.MinorGridLines.Visible = false;
            chartXy1.AxisY.MinorTickmarks.Visible = false;
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
            chartXy1.AxisY.Title.Text = "Risk";
            chartXy1.ChartCrosshair.CrosshairLabelMode = DevComponents.DotNetBar.Charts.CrosshairLabelMode.NearestSeries;
            background1.Color1 = System.Drawing.Color.White;
            chartXy1.ChartCrosshair.CrosshairVisualStyle.Background = background1;
            chartXy1.ChartCrosshair.HighlightPoints = true;
            chartXy1.ChartCrosshair.ShowValueXLabels = true;
            chartXy1.ChartCrosshair.ShowValueXLine = true;
            chartXy1.ChartCrosshair.ShowValueYLabels = true;
            chartXy1.ChartCrosshair.ShowValueYLine = true;
            chartXy1.ChartCrosshair.Visible = false;
            chartSeries1.ChartSeriesVisualStyle.LineStyle.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            chartSeries1.ChartSeriesVisualStyle.LineStyle.LineWidth = 3;
            chartSeries1.CrosshairHighlightPoints = DevComponents.DotNetBar.Charts.Style.Tbool.True;
            chartSeries1.EmptyValues = null;
            chartSeries1.Name = "Series1";
            chartSeries1.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.Line;
            chartSeries1.ShowInLegend = false;
            chartXy1.ChartSeries.Add(chartSeries1);
            padding1.Bottom = 6;
            padding1.Left = 6;
            padding1.Right = 6;
            padding1.Top = 6;
            chartXy1.ContainerVisualStyles.Default.Padding = padding1;
            chartXy1.Legend.Visible = false;
            this._chart.ChartPanel.ChartContainers.Add(chartXy1);
            this._chart.ChartPanel.Legend.Visible = false;
            this._chart.ChartPanel.Name = "PrimaryPanel";
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chart.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._chart.Location = new System.Drawing.Point(0, 0);
            this._chart.Name = "_chart";
            this._chart.Size = new System.Drawing.Size(896, 560);
            this._chart.TabIndex = 0;
            // 
            // RiskTrendPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._chart);
            this.Name = "RiskTrendPanel";
            this.Size = new System.Drawing.Size(896, 560);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Charts.ChartControl _chart;
    }
}
