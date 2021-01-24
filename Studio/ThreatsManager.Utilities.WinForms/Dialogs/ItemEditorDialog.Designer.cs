using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    partial class ItemEditorDialog
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
            this._item = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this.SuspendLayout();
            // 
            // _item
            // 
            this._item.BackColor = System.Drawing.Color.White;
            this._item.Dock = System.Windows.Forms.DockStyle.Fill;
            this._item.Item = null;
            this._item.Location = new System.Drawing.Point(0, 0);
            this._item.Name = "_item";
            this._item.ReadOnly = false;
            this._item.Size = new System.Drawing.Size(800, 650);
            this._item.TabIndex = 2;
            // 
            // ItemEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Controls.Add(this._item);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "ItemEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Item Editor";
            this.Load += new System.EventHandler(this.ItemEditorDialog_Load);
            this.LocationChanged += new System.EventHandler(this.ItemEditorDialog_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.ItemEditorDialog_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Resize += new System.EventHandler(this.ItemEditorDialog_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private ItemEditor _item;
    }
}