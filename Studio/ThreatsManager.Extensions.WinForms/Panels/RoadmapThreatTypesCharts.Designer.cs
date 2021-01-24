namespace ThreatsManager.Extensions.Panels
{
    partial class RoadmapThreatTypesCharts
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
            this._shortTermChart = new ThreatsManager.Extensions.Panels.ThreatTypesChart();
            this._longTermChart = new ThreatsManager.Extensions.Panels.ThreatTypesChart();
            this._chartName = new System.Windows.Forms.Label();
            this._chart = new ThreatsManager.Extensions.Panels.ThreatTypesChart();
            this._previous = new DevComponents.DotNetBar.ButtonX();
            this._next = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // _shortTermChart
            // 
            this._shortTermChart.Location = new System.Drawing.Point(0, 0);
            this._shortTermChart.Name = "_shortTermChart";
            this._shortTermChart.Size = new System.Drawing.Size(448, 401);
            this._shortTermChart.TabIndex = 0;
            // 
            // _longTermChart
            // 
            this._longTermChart.Location = new System.Drawing.Point(0, 0);
            this._longTermChart.Name = "_longTermChart";
            this._longTermChart.Size = new System.Drawing.Size(448, 401);
            this._longTermChart.TabIndex = 2;
            // 
            // _chartName
            // 
            this._chartName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._chartName.Location = new System.Drawing.Point(0, 384);
            this._chartName.Name = "_chartName";
            this._chartName.Size = new System.Drawing.Size(448, 17);
            this._chartName.TabIndex = 3;
            this._chartName.Text = "Chart Name";
            this._chartName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._chartName.Visible = false;
            // 
            // _chart
            // 
            this._chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this._chart.Location = new System.Drawing.Point(0, 0);
            this._chart.Name = "_chart";
            this._chart.Size = new System.Drawing.Size(448, 384);
            this._chart.TabIndex = 4;
            // 
            // _previous
            // 
            this._previous.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this._previous.BackColor = System.Drawing.Color.Transparent;
            this._previous.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this._previous.Dock = System.Windows.Forms.DockStyle.Left;
            this._previous.Location = new System.Drawing.Point(0, 0);
            this._previous.Name = "_previous";
            this._previous.Size = new System.Drawing.Size(20, 384);
            this._previous.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._previous.Symbol = "";
            this._previous.TabIndex = 5;
            this._previous.TextAlignment = DevComponents.DotNetBar.eButtonTextAlignment.Left;
            this._previous.Visible = false;
            this._previous.Click += new System.EventHandler(this._previous_Click);
            // 
            // _next
            // 
            this._next.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this._next.BackColor = System.Drawing.Color.Transparent;
            this._next.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this._next.Dock = System.Windows.Forms.DockStyle.Right;
            this._next.Location = new System.Drawing.Point(428, 0);
            this._next.Name = "_next";
            this._next.Size = new System.Drawing.Size(20, 384);
            this._next.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._next.Symbol = "";
            this._next.TabIndex = 6;
            this._next.Visible = false;
            this._next.Click += new System.EventHandler(this._next_Click);
            // 
            // RoadmapThreatTypesCharts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._next);
            this.Controls.Add(this._previous);
            this.Controls.Add(this._chart);
            this.Controls.Add(this._chartName);
            this.Controls.Add(this._longTermChart);
            this.Controls.Add(this._shortTermChart);
            this.Name = "RoadmapThreatTypesCharts";
            this.Size = new System.Drawing.Size(448, 401);
            this.ResumeLayout(false);

        }

        #endregion
        private ThreatTypesChart _shortTermChart;
        private ThreatTypesChart _longTermChart;
        private System.Windows.Forms.Label _chartName;
        private ThreatTypesChart _chart;
        private DevComponents.DotNetBar.ButtonX _previous;
        private DevComponents.DotNetBar.ButtonX _next;
    }
}
