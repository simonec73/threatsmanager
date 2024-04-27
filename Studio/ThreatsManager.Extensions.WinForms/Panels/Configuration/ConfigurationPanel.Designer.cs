
namespace ThreatsManager.Extensions.Panels.Configuration
{
    partial class ConfigurationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationPanel));
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._normalizationReference = new DevComponents.Editors.IntegerInput();
            this._enableEffortSupport = new System.Windows.Forms.CheckBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem16 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem17 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._normalizationReference)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._normalizationReference);
            this.layoutControl1.Controls.Add(this._enableEffortSupport);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControl1.Size = new System.Drawing.Size(1366, 896);
            this.layoutControl1.TabIndex = 0;
            // 
            // _normalizationReference
            // 
            // 
            // 
            // 
            this._normalizationReference.BackgroundStyle.Class = "DateTimeInputBackground";
            this._normalizationReference.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._normalizationReference.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this._normalizationReference.Location = new System.Drawing.Point(517, 55);
            this._normalizationReference.Margin = new System.Windows.Forms.Padding(0);
            this._normalizationReference.MinValue = 0;
            this._normalizationReference.Name = "_normalizationReference";
            this._normalizationReference.ShowUpDown = true;
            this._normalizationReference.Size = new System.Drawing.Size(275, 31);
            this._normalizationReference.TabIndex = 1;
            this._tooltip.SetToolTip(this._normalizationReference, resources.GetString("_normalizationReference.ToolTip"));
            this._normalizationReference.ValueChanged += new System.EventHandler(this._normalizationReference_ValueChanged);
            // 
            // _enableEffortSupport
            // 
            this._enableEffortSupport.AutoSize = true;
            this._enableEffortSupport.Location = new System.Drawing.Point(8, 7);
            this._enableEffortSupport.Margin = new System.Windows.Forms.Padding(0);
            this._enableEffortSupport.Name = "_enableEffortSupport";
            this._enableEffortSupport.Size = new System.Drawing.Size(1350, 34);
            this._enableEffortSupport.TabIndex = 0;
            this._enableEffortSupport.Text = "Enable support for Effort";
            this._tooltip.SetToolTip(this._enableEffortSupport, "By enabling support for Effort, an additional property is added to Mitigations, t" +
        "o evaluate the potential effort.\r\nThe effort estimation is based on order of mag" +
        "nitude evaluation.");
            this._enableEffortSupport.UseVisualStyleBackColor = true;
            this._enableEffortSupport.CheckedChanged += new System.EventHandler(this._enableEffortSupport_CheckedChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._enableEffortSupport;
            this.layoutControlItem1.Height = 48;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 38);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._normalizationReference;
            this.layoutControlItem2.Height = 54;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem2.Text = "Roadmap Acceptable Risk Normalization Reference";
            this.layoutControlItem2.Width = 800;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Height = 53;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Icons size in diagrams";
            this.layoutControlItem3.Tooltip = "The icons size determines how big are the icons in the diagrams.";
            this.layoutControlItem3.Width = 99;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Height = 21;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Label:";
            this.layoutControlItem5.TextVisible = false;
            this.layoutControlItem5.Width = 40;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Height = 53;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Icon center size";
            this.layoutControlItem6.Tooltip = "This parameter determines how big is the central area used to draw the arrows.";
            this.layoutControlItem6.Width = 99;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Height = 21;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Label:";
            this.layoutControlItem7.TextVisible = false;
            this.layoutControlItem7.Width = 43;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Height = 53;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "Markers size";
            this.layoutControlItem10.Tooltip = "Size of the markers in the Diagrams.";
            this.layoutControlItem10.Width = 99;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Height = 21;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Label:";
            this.layoutControlItem11.TextVisible = false;
            this.layoutControlItem11.Width = 43;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Height = 53;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "Default zoom";
            this.layoutControlItem12.Width = 99;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Height = 21;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "100%";
            this.layoutControlItem13.TextVisible = false;
            this.layoutControlItem13.Width = 43;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Height = 53;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Text = "Horizontal spacing";
            this.layoutControlItem14.Width = 99;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Height = 21;
            this.layoutControlItem16.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Text = "Label:";
            this.layoutControlItem16.TextVisible = false;
            this.layoutControlItem16.Width = 30;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Height = 53;
            this.layoutControlItem15.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Text = "Vertical spacing";
            this.layoutControlItem15.Width = 99;
            this.layoutControlItem15.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Height = 21;
            this.layoutControlItem17.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Text = "Label:";
            this.layoutControlItem17.TextVisible = false;
            this.layoutControlItem17.Width = 43;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Height = 21;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "Label:";
            this.layoutControlItem9.TextVisible = false;
            this.layoutControlItem9.Width = 101;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ConfigurationPanel";
            this.Size = new System.Drawing.Size(1366, 896);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._normalizationReference)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.CheckBox _enableEffortSupport;
        private System.Windows.Forms.ToolTip _tooltip;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.Editors.IntegerInput _normalizationReference;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem16;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem17;
    }
}
