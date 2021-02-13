
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
            this._enableEffortSupport = new System.Windows.Forms.CheckBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this._normalizationReference = new DevComponents.Editors.IntegerInput();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControl1.Size = new System.Drawing.Size(683, 466);
            this.layoutControl1.TabIndex = 0;
            // 
            // _enableEffortSupport
            // 
            this._enableEffortSupport.AutoSize = true;
            this._enableEffortSupport.Location = new System.Drawing.Point(4, 4);
            this._enableEffortSupport.Margin = new System.Windows.Forms.Padding(0);
            this._enableEffortSupport.Name = "_enableEffortSupport";
            this._enableEffortSupport.Size = new System.Drawing.Size(675, 17);
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
            this.layoutControlItem1.Height = 25;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _normalizationReference
            // 
            // 
            // 
            // 
            this._normalizationReference.BackgroundStyle.Class = "DateTimeInputBackground";
            this._normalizationReference.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._normalizationReference.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this._normalizationReference.Location = new System.Drawing.Point(272, 29);
            this._normalizationReference.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._normalizationReference.MinValue = 0;
            this._normalizationReference.Name = "_normalizationReference";
            this._normalizationReference.ShowUpDown = true;
            this._normalizationReference.Size = new System.Drawing.Size(124, 20);
            this._normalizationReference.TabIndex = 1;
            this._tooltip.SetToolTip(this._normalizationReference, resources.GetString("_normalizationReference.ToolTip"));
            this._normalizationReference.ValueChanged += new System.EventHandler(this._normalizationReference_ValueChanged);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._normalizationReference;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Roadmap Acceptable Risk Normalization Reference";
            this.layoutControlItem2.Width = 400;
            // 
            // ConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ConfigurationPanel";
            this.Size = new System.Drawing.Size(683, 466);
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
    }
}
