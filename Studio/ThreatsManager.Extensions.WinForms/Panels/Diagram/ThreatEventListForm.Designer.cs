namespace ThreatsManager.Extensions.Panels.Diagram
{
    partial class ThreatEventListForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._panel = new DevComponents.DotNetBar.Metro.MetroTilePanel();
            this.SuspendLayout();
            // 
            // _panel
            // 
            // 
            // 
            // 
            this._panel.BackgroundStyle.BackColor = System.Drawing.Color.LimeGreen;
            this._panel.BackgroundStyle.Class = "MetroTilePanel";
            this._panel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._panel.ContainerControlProcessDialogKey = true;
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.DragDropSupport = true;
            this._panel.ItemSpacing = 5;
            this._panel.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this._panel.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.ReserveLeftSpace = false;
            this._panel.Size = new System.Drawing.Size(300, 400);
            this._panel.TabIndex = 0;
            this._panel.Text = "metroTilePanel1";
            this._panel.Leave += new System.EventHandler(this._panel_Leave);
            // 
            // ThreatEventListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.ClientSize = new System.Drawing.Size(300, 400);
            this.ControlBox = false;
            this.Controls.Add(this._panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThreatEventListForm";
            this.Opacity = 0.85D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TransparencyKey = System.Drawing.Color.LimeGreen;
            this.Deactivate += new System.EventHandler(this.ThreatEventListForm_Deactivate);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroTilePanel _panel;
    }
}