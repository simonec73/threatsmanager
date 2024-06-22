namespace ThreatsManager.Utilities.WinForms
{
    partial class CheckedLink
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
            this._checkBox = new System.Windows.Forms.CheckBox();
            this._link = new System.Windows.Forms.LinkLabel();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // _checkBox
            // 
            this._checkBox.AutoSize = true;
            this._checkBox.Location = new System.Drawing.Point(0, 0);
            this._checkBox.Name = "_checkBox";
            this._checkBox.Size = new System.Drawing.Size(28, 27);
            this._checkBox.TabIndex = 0;
            this._checkBox.UseVisualStyleBackColor = true;
            // 
            // _link
            // 
            this._link.AutoSize = true;
            this._link.Location = new System.Drawing.Point(34, 0);
            this._link.Name = "_link";
            this._link.Size = new System.Drawing.Size(273, 25);
            this._link.TabIndex = 1;
            this._link.TabStop = true;
            this._link.Text = "https://threatsmanager.com";
            this._link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._link_LinkClicked);
            // 
            // CheckedLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this._link);
            this.Controls.Add(this._checkBox);
            this.Name = "CheckedLink";
            this.Size = new System.Drawing.Size(310, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _checkBox;
        private System.Windows.Forms.LinkLabel _link;
        private System.Windows.Forms.ToolTip _tooltip;
    }
}
