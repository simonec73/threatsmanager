namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    partial class CheckPanel
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
            _grid.CellClick -= _grid_CellClick;
            _grid.MouseClick -= _grid_MouseClick;

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
            DevComponents.Instrumentation.GradientFillColor gradientFillColor1 = new DevComponents.Instrumentation.GradientFillColor();
            DevComponents.Instrumentation.GradientFillColor gradientFillColor2 = new DevComponents.Instrumentation.GradientFillColor();
            DevComponents.Instrumentation.GaugeLinearScale gaugeLinearScale1 = new DevComponents.Instrumentation.GaugeLinearScale();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckPanel));
            DevComponents.Instrumentation.GaugePointer gaugePointer1 = new DevComponents.Instrumentation.GaugePointer();
            DevComponents.Instrumentation.GaugeSection gaugeSection1 = new DevComponents.Instrumentation.GaugeSection();
            DevComponents.Instrumentation.GaugeSection gaugeSection2 = new DevComponents.Instrumentation.GaugeSection();
            DevComponents.Instrumentation.GaugeSection gaugeSection3 = new DevComponents.Instrumentation.GaugeSection();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._gauge = new DevComponents.Instrumentation.GaugeControl();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this._header = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._missingThreatEventsLabel = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutSpacerItem1 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gauge)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._gauge);
            this.layoutControl1.Controls.Add(this._grid);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._header,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControl1.Size = new System.Drawing.Size(259, 334);
            this.layoutControl1.TabIndex = 0;
            // 
            // _gauge
            // 
            gradientFillColor1.Color1 = System.Drawing.Color.Gainsboro;
            gradientFillColor1.Color2 = System.Drawing.Color.DarkGray;
            this._gauge.Frame.BackColor = gradientFillColor1;
            gradientFillColor2.BorderColor = System.Drawing.Color.Gainsboro;
            gradientFillColor2.BorderWidth = 1;
            gradientFillColor2.Color1 = System.Drawing.Color.White;
            gradientFillColor2.Color2 = System.Drawing.Color.DimGray;
            this._gauge.Frame.FrameColor = gradientFillColor2;
            this._gauge.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            gaugeLinearScale1.Labels.Visible = false;
            gaugeLinearScale1.Location = ((System.Drawing.PointF)(resources.GetObject("gaugeLinearScale1.Location")));
            gaugeLinearScale1.MajorTickMarks.Visible = false;
            gaugeLinearScale1.MaxPin.Name = "MaxPin";
            gaugeLinearScale1.MinorTickMarks.Visible = false;
            gaugeLinearScale1.MinPin.Name = "MinPin";
            gaugeLinearScale1.Name = "MainScale";
            gaugePointer1.CapFillColor.BorderColor = System.Drawing.Color.DimGray;
            gaugePointer1.CapFillColor.BorderWidth = 1;
            gaugePointer1.CapFillColor.Color1 = System.Drawing.Color.WhiteSmoke;
            gaugePointer1.CapFillColor.Color2 = System.Drawing.Color.DimGray;
            gaugePointer1.FillColor.BorderColor = System.Drawing.Color.White;
            gaugePointer1.FillColor.Color1 = System.Drawing.Color.Black;
            gaugePointer1.FillColor.Color2 = System.Drawing.Color.Black;
            gaugePointer1.Length = 0.5F;
            gaugePointer1.MarkerStyle = DevComponents.Instrumentation.GaugeMarkerStyle.Circle;
            gaugePointer1.Name = "Pointer1";
            gaugePointer1.ThermoBackColor.BorderColor = System.Drawing.Color.Black;
            gaugePointer1.ThermoBackColor.BorderWidth = 1;
            gaugePointer1.ThermoBackColor.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            gaugePointer1.Value = 40D;
            gaugePointer1.Width = 0.5F;
            gaugeLinearScale1.Pointers.AddRange(new DevComponents.Instrumentation.GaugePointer[] {
            gaugePointer1});
            gaugeSection1.EndValue = 100D;
            gaugeSection1.FillColor.Color1 = System.Drawing.Color.Red;
            gaugeSection1.Name = "Red";
            gaugeSection1.StartValue = 68D;
            gaugeSection2.EndValue = 67D;
            gaugeSection2.FillColor.Color1 = System.Drawing.Color.Yellow;
            gaugeSection2.Name = "Yellow";
            gaugeSection2.StartValue = 34D;
            gaugeSection3.EndValue = 33D;
            gaugeSection3.FillColor.Color1 = System.Drawing.Color.Lime;
            gaugeSection3.Name = "Green";
            gaugeSection3.StartValue = 0D;
            gaugeLinearScale1.Sections.AddRange(new DevComponents.Instrumentation.GaugeSection[] {
            gaugeSection1,
            gaugeSection2,
            gaugeSection3});
            gaugeLinearScale1.Width = 0.75F;
            this._gauge.LinearScales.AddRange(new DevComponents.Instrumentation.GaugeLinearScale[] {
            gaugeLinearScale1});
            this._gauge.Location = new System.Drawing.Point(0, 24);
            this._gauge.Margin = new System.Windows.Forms.Padding(0);
            this._gauge.Name = "_gauge";
            this._gauge.Size = new System.Drawing.Size(259, 32);
            this._gauge.TabIndex = 1;
            this._gauge.Text = "gaugeControl2";
            // 
            // _grid
            // 
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(4, 60);
            this._grid.Margin = new System.Windows.Forms.Padding(0);
            this._grid.Name = "_grid";
            // 
            // 
            // 
            this._grid.PrimaryGrid.ActiveRowIndicatorStyle = DevComponents.DotNetBar.SuperGrid.ActiveRowIndicatorStyle.None;
            this._grid.PrimaryGrid.AllowEdit = false;
            this._grid.PrimaryGrid.Columns.Add(this.gridColumn1);
            this._grid.PrimaryGrid.InitialSelection = DevComponents.DotNetBar.SuperGrid.RelativeSelection.None;
            this._grid.PrimaryGrid.ShowCheckBox = false;
            this._grid.PrimaryGrid.ShowRowDirtyMarker = false;
            this._grid.PrimaryGrid.ShowRowHeaders = false;
            this._grid.PrimaryGrid.ShowTreeButton = false;
            this._grid.Size = new System.Drawing.Size(251, 270);
            this._grid.TabIndex = 2;
            this._grid.Text = "superGridControl1";
            this._grid.CellClick += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellClickEventArgs>(this._grid_CellClick);
            this._grid.MouseClick += new System.Windows.Forms.MouseEventHandler(this._grid_MouseClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.gridColumn1.Name = "Findings";
            // 
            // _header
            // 
            this._header.Height = 24;
            this._header.Image = global::ThreatsManager.Quality.Properties.Resources.emoticon_smile;
            this._header.ImagePosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Left;
            this._header.Name = "_header";
            this._header.Text = "Text";
            this._header.Width = 100;
            this._header.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._gauge;
            this.layoutControlItem1.Height = 32;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(0);
            this.layoutControlItem1.Text = "Label:";
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._grid;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Label:";
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _missingThreatEventsLabel
            // 
            this._missingThreatEventsLabel.Height = 24;
            this._missingThreatEventsLabel.Image = global::ThreatsManager.Quality.Properties.Resources.emoticon_smile;
            this._missingThreatEventsLabel.ImagePosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Left;
            this._missingThreatEventsLabel.Name = "_missingThreatEventsLabel";
            this._missingThreatEventsLabel.Text = "Missing Threat Events";
            this._missingThreatEventsLabel.Width = 100;
            this._missingThreatEventsLabel.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem1
            // 
            this.layoutSpacerItem1.Height = 24;
            this.layoutSpacerItem1.Image = global::ThreatsManager.Quality.Properties.Resources.emoticon_smile;
            this.layoutSpacerItem1.ImagePosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Left;
            this.layoutSpacerItem1.Name = "layoutSpacerItem1";
            this.layoutSpacerItem1.Text = "Missing Threat Events";
            this.layoutSpacerItem1.Width = 100;
            this.layoutSpacerItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // CheckPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.layoutControl1);
            this.Name = "CheckPanel";
            this.Size = new System.Drawing.Size(259, 334);
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gauge)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem _header;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem _missingThreatEventsLabel;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem1;
        private DevComponents.Instrumentation.GaugeControl _gauge;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1;
    }
}
