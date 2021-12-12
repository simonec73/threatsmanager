
namespace ThreatsManager.Quality.Panels.Configuration
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
            this._enableCalculatedSeveritySupport = new System.Windows.Forms.CheckBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._enableCalculatedSeveritySupport);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1});
            this.layoutControl1.Size = new System.Drawing.Size(683, 466);
            this.layoutControl1.TabIndex = 0;
            // 
            // _enableCalculatedSeveritySupport
            // 
            this._enableCalculatedSeveritySupport.AutoSize = true;
            this._enableCalculatedSeveritySupport.Location = new System.Drawing.Point(4, 4);
            this._enableCalculatedSeveritySupport.Margin = new System.Windows.Forms.Padding(0);
            this._enableCalculatedSeveritySupport.Name = "_enableCalculatedSeveritySupport";
            this._enableCalculatedSeveritySupport.Size = new System.Drawing.Size(675, 17);
            this._enableCalculatedSeveritySupport.TabIndex = 0;
            this._enableCalculatedSeveritySupport.Text = "Enable support for Calculated Severity";
            this._tooltip.SetToolTip(this._enableCalculatedSeveritySupport, resources.GetString("_enableCalculatedSeveritySupport.ToolTip"));
            this._enableCalculatedSeveritySupport.UseVisualStyleBackColor = true;
            this._enableCalculatedSeveritySupport.CheckedChanged += new System.EventHandler(this._enableCalculatedSeveritySupport_CheckedChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._enableCalculatedSeveritySupport;
            this.layoutControlItem1.Height = 25;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.CheckBox _enableCalculatedSeveritySupport;
        private System.Windows.Forms.ToolTip _tooltip;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
    }
}
