using ThreatsManager.Utilities.WinForms;

namespace SampleWinFormExtensions.Panels.AzureDevOps
{
    partial class AzureDevOpsPanel
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
            this._item = new ItemEditor();
            this._list = new System.Windows.Forms.ListView();
            this._mitigationName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._appliesTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._threatEventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // _item
            // 
            this._item.BackColor = System.Drawing.Color.White;
            this._item.Dock = System.Windows.Forms.DockStyle.Right;
            this._item.Item = null;
            this._item.Location = new System.Drawing.Point(766, 0);
            this._item.Name = "_item";
            this._item.ReadOnly = false;
            this._item.Size = new System.Drawing.Size(323, 612);
            this._item.TabIndex = 0;
            // 
            // _list
            // 
            this._list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._mitigationName,
            this._threatEventName,
            this._appliesTo,
            this._state});
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            this._list.FullRowSelect = true;
            this._list.GridLines = true;
            this._list.HideSelection = false;
            this._list.Location = new System.Drawing.Point(0, 0);
            this._list.MultiSelect = false;
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(766, 612);
            this._list.TabIndex = 2;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.Details;
            this._list.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this._list_ItemSelectionChanged);
            // 
            // _mitigationName
            // 
            this._mitigationName.Text = "Mitigation Name";
            this._mitigationName.Width = 800;
            // 
            // _appliesTo
            // 
            this._appliesTo.Text = "Applies To";
            this._appliesTo.Width = 400;
            // 
            // _threatEventName
            // 
            this._threatEventName.Text = "Threat Event Name";
            this._threatEventName.Width = 600;
            // 
            // _state
            // 
            this._state.Text = "State";
            this._state.Width = 200;
            // 
            // AzureDevOpsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._list);
            this.Controls.Add(this._item);
            this.Name = "AzureDevOpsPanel";
            this.Size = new System.Drawing.Size(1089, 612);
            this.ResumeLayout(false);

        }

        #endregion

        private ItemEditor _item;
        private System.Windows.Forms.ListView _list;
        private System.Windows.Forms.ColumnHeader _mitigationName;
        private System.Windows.Forms.ColumnHeader _threatEventName;
        private System.Windows.Forms.ColumnHeader _appliesTo;
        private System.Windows.Forms.ColumnHeader _state;
    }
}
