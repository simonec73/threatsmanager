
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
            DevComponents.DotNetBar.Charts.ChartXy chartXy = new DevComponents.DotNetBar.Charts.ChartXy();
            DevComponents.DotNetBar.Charts.ReferenceLine referenceLine = new DevComponents.DotNetBar.Charts.ReferenceLine();
            DevComponents.DotNetBar.Charts.Style.Background background = new DevComponents.DotNetBar.Charts.Style.Background();
            DevComponents.DotNetBar.Charts.ChartSeries series = new DevComponents.DotNetBar.Charts.ChartSeries();
            DevComponents.DotNetBar.Charts.Style.Padding padding = new DevComponents.DotNetBar.Charts.Style.Padding();
            this._chart = new DevComponents.DotNetBar.Charts.ChartControl();
            this.SuspendLayout();
            // 
            // _chart
            // 
            chartXy.AxisX.MajorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.Gainsboro;
            chartXy.AxisX.MinorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.WhiteSmoke;
            chartXy.AxisX.MinorGridLines.Visible = false;
            chartXy.AxisX.MinorTickmarks.Visible = false;
            chartXy.AxisX.Title.ChartTitleVisualStyle.Alignment = DevComponents.DotNetBar.Charts.Style.Alignment.BottomRight;
            chartXy.AxisX.Title.Name = null;
            chartXy.AxisX.Title.Text = "Iterations";
            chartXy.AxisY.AxisAlignment = DevComponents.DotNetBar.Charts.AxisAlignment.Far;
            chartXy.AxisY.MajorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.Gainsboro;
            chartXy.AxisY.MajorGridLines.Visible = false;
            chartXy.AxisY.MajorTickmarks.ShowLabels = false;
            chartXy.AxisY.MajorTickmarks.Visible = false;
            chartXy.AxisY.MinorGridLines.GridLinesVisualStyle.LineColor = System.Drawing.Color.WhiteSmoke;
            chartXy.AxisY.MinorGridLines.Visible = false;
            chartXy.AxisY.MinorTickmarks.Visible = false;
            referenceLine.AxisValue = 0F;
            referenceLine.Name = "AcceptableRisk";
            referenceLine.ReferenceLineVisualStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            referenceLine.ReferenceLineVisualStyle.LineColor = System.Drawing.Color.Gray;
            referenceLine.ReferenceLineVisualStyle.LinePattern = DevComponents.DotNetBar.Charts.Style.LinePattern.Dash;
            referenceLine.ReferenceLineVisualStyle.TextAlignment = DevComponents.DotNetBar.Charts.Style.Alignment.TopRight;
            referenceLine.ReferenceLineVisualStyle.TextColor = System.Drawing.Color.Gray;
            referenceLine.Text = "Acceptable Risk";
            chartXy.AxisY.ReferenceLines.Add(referenceLine);
            chartXy.AxisY.Title.Name = null;
            chartXy.AxisY.Title.Text = "Risk";
            chartXy.ChartCrosshair.CrosshairLabelMode = DevComponents.DotNetBar.Charts.CrosshairLabelMode.NearestSeries;
            background.Color1 = System.Drawing.Color.White;
            chartXy.ChartCrosshair.CrosshairVisualStyle.Background = background;
            chartXy.ChartCrosshair.HighlightPoints = true;
            chartXy.ChartCrosshair.ShowCrosshairLabels = true;
            chartXy.ChartCrosshair.ShowValueXLabels = true;
            chartXy.ChartCrosshair.ShowValueXLine = true;
            chartXy.ChartCrosshair.ShowValueYLabels = true;
            chartXy.ChartCrosshair.ShowValueYLine = true;
            series.ChartSeriesVisualStyle.LineStyle.LineWidth = 3;
            series.ChartSeriesVisualStyle.LineStyle.LineColor = ThreatModelManager.StandardColor;
            series.CrosshairHighlightPoints = DevComponents.DotNetBar.Charts.Style.Tbool.True;
            series.EmptyValues = null;
            series.Name = "Series1";
            series.SeriesType = DevComponents.DotNetBar.Charts.SeriesType.Line;
            series.ShowInLegend = false;
            chartXy.ChartSeries.Add(series);
            padding.Bottom = 6;
            padding.Left = 6;
            padding.Right = 6;
            padding.Top = 6;
            chartXy.ContainerVisualStyles.Default.Padding = padding;
            chartXy.Legend.Visible = false;
            this._chart.ChartPanel.ChartContainers.Add(chartXy);
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
