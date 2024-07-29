namespace ThreatsManager.Extensions.Dialogs
{
    partial class ProgressDialog
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
            this._label = new System.Windows.Forms.Label();
            this._progress = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.SuspendLayout();
            // 
            // _label
            // 
            this._label.AutoSize = true;
            this._label.Location = new System.Drawing.Point(24, 18);
            this._label.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(203, 25);
            this._label.TabIndex = 0;
            this._label.Text = "Action in progress...";
            // 
            // _progress
            // 
            this._progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this._progress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._progress.Location = new System.Drawing.Point(30, 72);
            this._progress.Margin = new System.Windows.Forms.Padding(6);
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(856, 46);
            this._progress.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._progress.TabIndex = 1;
            this._progress.Text = "progressBarX1";
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(910, 142);
            this.ControlBox = false;
            this.Controls.Add(this._progress);
            this.Controls.Add(this._label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _label;
        private DevComponents.DotNetBar.Controls.ProgressBarX _progress;
    }
}