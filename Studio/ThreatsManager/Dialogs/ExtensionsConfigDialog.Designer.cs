namespace ThreatsManager.Dialogs
{
    partial class ExtensionsConfigDialog
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
            this._close = new System.Windows.Forms.Button();
            this._bottom = new System.Windows.Forms.Panel();
            this._side = new DevComponents.DotNetBar.Controls.SideNav();
            this._bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _close
            // 
            this._close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._close.Location = new System.Drawing.Point(332, 8);
            this._close.Margin = new System.Windows.Forms.Padding(6);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(75, 32);
            this._close.TabIndex = 1;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            // 
            // _bottom
            // 
            this._bottom.Controls.Add(this._close);
            this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottom.Location = new System.Drawing.Point(0, 419);
            this._bottom.Name = "_bottom";
            this._bottom.Size = new System.Drawing.Size(738, 49);
            this._bottom.TabIndex = 3;
            // 
            // _side
            // 
            this._side.Dock = System.Windows.Forms.DockStyle.Fill;
            this._side.EnableClose = false;
            this._side.EnableMaximize = false;
            this._side.Location = new System.Drawing.Point(0, 0);
            this._side.Name = "_side";
            this._side.Padding = new System.Windows.Forms.Padding(1);
            this._side.Size = new System.Drawing.Size(738, 419);
            this._side.TabIndex = 4;
            this._side.Text = "sideNav1";
            // 
            // ExtensionsConfigDialog
            // 
            this.AcceptButton = this._close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._close;
            this.ClientSize = new System.Drawing.Size(738, 468);
            this.Controls.Add(this._side);
            this.Controls.Add(this._bottom);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimizeBox = false;
            this.Name = "ExtensionsConfigDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extensions Configuration";
            this.Load += new System.EventHandler(this.ExtensionsConfigDialog_Load);
            this._bottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button _close;
        private System.Windows.Forms.Panel _bottom;
        private DevComponents.DotNetBar.Controls.SideNav _side;
    }
}