namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    partial class OverallPanel
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
            Syncfusion.Windows.Forms.Gauge.Range range1 = new Syncfusion.Windows.Forms.Gauge.Range();
            this._overall = new Syncfusion.Windows.Forms.Gauge.RadialGauge();
            this.SuspendLayout();
            // 
            // _overall
            // 
            this._overall.ArcThickness = 2F;
            this._overall.Dock = System.Windows.Forms.DockStyle.Fill;
            this._overall.EnableCustomNeedles = false;
            this._overall.FillColor = System.Drawing.Color.DarkGray;
            this._overall.ForeColor = System.Drawing.Color.Black;
            this._overall.FrameThickness = 20;
            this._overall.GaugeLabel = "Overall";
            this._overall.GaugeLableFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._overall.GaugeValueFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._overall.Location = new System.Drawing.Point(0, 0);
            this._overall.MajorDifference = 10F;
            this._overall.MajorTickMarkHeight = 5;
            this._overall.Margin = new System.Windows.Forms.Padding(0);
            this._overall.MaximumValue = 30F;
            this._overall.MinimumSize = new System.Drawing.Size(125, 125);
            this._overall.MinorInnerLinesHeight = 0;
            this._overall.MinorTickMarkHeight = 0;
            this._overall.Name = "_overall";
            range1.Color = System.Drawing.Color.Yellow;
            range1.EndValue = 16F;
            range1.Height = 15;
            range1.InRange = true;
            range1.Name = "GaugeRange1";
            range1.RangePlacement = Syncfusion.Windows.Forms.Gauge.TickPlacement.OutSide;
            range1.StartValue = 0F;
            this._overall.Ranges.Add(range1);
            this._overall.ShowBackgroundFrame = false;
            this._overall.ShowNeedle = false;
            this._overall.ShowScaleLabel = false;
            this._overall.ShowTicks = true;
            this._overall.Size = new System.Drawing.Size(339, 339);
            this._overall.TabIndex = 1;
            this._overall.ThemeName = "";
            this._overall.ThemeStyle.ArcThickness = 2F;
            this._overall.Value = 5F;
            // 
            // OverallPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._overall);
            this.Name = "OverallPanel";
            this.Size = new System.Drawing.Size(341, 339);
            this.ResumeLayout(false);

        }

        #endregion

        private Syncfusion.Windows.Forms.Gauge.RadialGauge _overall;
    }
}
