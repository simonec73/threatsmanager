namespace SampleWinFormExtensions.Panels.Validate
{
    partial class ValidatePanel
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(3D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint4 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(4D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint5 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(5D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint6 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(6D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint7 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(7D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint8 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(8D, 1D);
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint9 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 1D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint10 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(2D, 1D);
            this._chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this._chart)).BeginInit();
            this.SuspendLayout();
            // 
            // _chart
            // 
            chartArea1.Name = "ChartArea1";
            chartArea2.Name = "ChartArea2";
            this._chart.ChartAreas.Add(chartArea1);
            this._chart.ChartAreas.Add(chartArea2);
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chart.Location = new System.Drawing.Point(0, 0);
            this._chart.Name = "_chart";
            series1.ChartArea = "ChartArea1";
            series1.Label = "#LABEL";
            series1.LabelAngle = 30;
            series1.Name = "ThreatEventsCoverage";
            dataPoint1.Color = System.Drawing.Color.LightGreen;
            dataPoint1.Label = "External Interactors w/Threats";
            dataPoint2.Color = System.Drawing.Color.Orange;
            dataPoint2.Label = "External Interactors w/o Threats";
            dataPoint3.Color = System.Drawing.Color.LightGreen;
            dataPoint3.Label = "Processes w/Threats";
            dataPoint4.Color = System.Drawing.Color.Orange;
            dataPoint4.Label = "Processes w/o Threats";
            dataPoint5.Color = System.Drawing.Color.LightGreen;
            dataPoint5.Label = "Data Stores w/Threats";
            dataPoint6.Color = System.Drawing.Color.Orange;
            dataPoint6.Label = "Data Stores w/o Threats";
            dataPoint7.Color = System.Drawing.Color.LightGreen;
            dataPoint7.Label = "Flows w/Threats";
            dataPoint8.Color = System.Drawing.Color.Orange;
            dataPoint8.Label = "Flows w/o Threats";
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            series1.Points.Add(dataPoint4);
            series1.Points.Add(dataPoint5);
            series1.Points.Add(dataPoint6);
            series1.Points.Add(dataPoint7);
            series1.Points.Add(dataPoint8);
            series2.ChartArea = "ChartArea2";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Name = "MitigationsCoverage";
            dataPoint9.Color = System.Drawing.Color.LightGreen;
            dataPoint9.Label = "Threats w/Mitigations";
            dataPoint10.Color = System.Drawing.Color.Orange;
            dataPoint10.Label = "Threats w/o Mitigations";
            series2.Points.Add(dataPoint9);
            series2.Points.Add(dataPoint10);
            this._chart.Series.Add(series1);
            this._chart.Series.Add(series2);
            this._chart.Size = new System.Drawing.Size(1089, 612);
            this._chart.TabIndex = 0;
            this._chart.Text = "chart1";
            // 
            // ValidatePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._chart);
            this.Name = "ValidatePanel";
            this.Size = new System.Drawing.Size(1089, 612);
            ((System.ComponentModel.ISupportInitialize)(this._chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart _chart;
    }
}
