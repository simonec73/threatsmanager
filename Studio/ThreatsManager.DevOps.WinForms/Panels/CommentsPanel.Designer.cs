
namespace ThreatsManager.DevOps.Panels
{
    partial class CommentsPanel
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
            this._browser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // _browser
            // 
            this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._browser.Location = new System.Drawing.Point(0, 0);
            this._browser.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._browser.MinimumSize = new System.Drawing.Size(40, 38);
            this._browser.Name = "_browser";
            this._browser.Size = new System.Drawing.Size(1056, 794);
            this._browser.TabIndex = 0;
            // 
            // CommentsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._browser);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "CommentsPanel";
            this.Size = new System.Drawing.Size(1056, 794);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser _browser;
    }
}
