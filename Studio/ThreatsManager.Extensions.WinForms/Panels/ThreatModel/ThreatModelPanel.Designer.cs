using ThreatsManager.Utilities;
using ItemEditor = ThreatsManager.Utilities.WinForms.ItemEditor;

namespace ThreatsManager.Extensions.Panels.ThreatModel
{
    partial class ThreatModelPanel
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
            _itemEditor.Item = null;

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
            this._itemEditor = new ItemEditor();
            this.SuspendLayout();
            // 
            // _itemEditor
            // 
            this._itemEditor.BackColor = System.Drawing.Color.White;
            this._itemEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._itemEditor.Item = null;
            this._itemEditor.Location = new System.Drawing.Point(0, 0);
            this._itemEditor.Name = "_itemEditor";
            this._itemEditor.ReadOnly = false;
            this._itemEditor.Size = new System.Drawing.Size(666, 612);
            this._itemEditor.TabIndex = 0;
            // 
            // ThreatModelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._itemEditor);
            this.Name = "ThreatModelPanel";
            this.Size = new System.Drawing.Size(666, 612);
            this.ResumeLayout(false);

        }

        #endregion

        private ItemEditor _itemEditor;
    }
}
