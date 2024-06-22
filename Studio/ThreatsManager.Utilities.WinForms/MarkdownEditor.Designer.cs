namespace ThreatsManager.Utilities.WinForms
{
    partial class MarkdownEditor
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
            this._tabContainer = new System.Windows.Forms.TabControl();
            this._tabEditor = new System.Windows.Forms.TabPage();
            this._tabView = new System.Windows.Forms.TabPage();
            this._tabContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tabContainer
            // 
            this._tabContainer.Controls.Add(this._tabEditor);
            this._tabContainer.Controls.Add(this._tabView);
            this._tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabContainer.Location = new System.Drawing.Point(0, 0);
            this._tabContainer.Name = "_tabContainer";
            this._tabContainer.SelectedIndex = 0;
            this._tabContainer.Size = new System.Drawing.Size(1307, 845);
            this._tabContainer.TabIndex = 0;
            // 
            // _tabEditor
            // 
            this._tabEditor.Location = new System.Drawing.Point(8, 39);
            this._tabEditor.Name = "_tabEditor";
            this._tabEditor.Padding = new System.Windows.Forms.Padding(3);
            this._tabEditor.Size = new System.Drawing.Size(1291, 798);
            this._tabEditor.TabIndex = 0;
            this._tabEditor.Text = "Editor";
            this._tabEditor.UseVisualStyleBackColor = true;
            // 
            // _tabView
            // 
            this._tabView.Location = new System.Drawing.Point(8, 39);
            this._tabView.Name = "_tabView";
            this._tabView.Padding = new System.Windows.Forms.Padding(3);
            this._tabView.Size = new System.Drawing.Size(1291, 798);
            this._tabView.TabIndex = 1;
            this._tabView.Text = "Viewer";
            this._tabView.UseVisualStyleBackColor = true;
            // 
            // MarkdownEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tabContainer);
            this.Name = "MarkdownEditor";
            this.Size = new System.Drawing.Size(1307, 845);
            this._tabContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tabContainer;
        private System.Windows.Forms.TabPage _tabEditor;
        private System.Windows.Forms.TabPage _tabView;
    }
}
