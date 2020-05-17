namespace SampleWinFormExtensions.Panels.Definitions
{
    partial class DefinitionsPanel
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
            this._data = new System.Windows.Forms.DataGridView();
            this._propertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._propertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._data)).BeginInit();
            this.SuspendLayout();
            // 
            // _data
            // 
            this._data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._propertyName,
            this._propertyValue});
            this._data.Dock = System.Windows.Forms.DockStyle.Fill;
            this._data.Location = new System.Drawing.Point(0, 0);
            this._data.Name = "_data";
            this._data.Size = new System.Drawing.Size(1089, 612);
            this._data.TabIndex = 0;
            this._data.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this._data_CellValueChanged);
            this._data.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this._data_UserDeletingRow);
            // 
            // _propertyName
            // 
            this._propertyName.HeaderText = "Name";
            this._propertyName.Name = "_propertyName";
            this._propertyName.Width = 200;
            // 
            // _propertyValue
            // 
            this._propertyValue.HeaderText = "Value";
            this._propertyValue.Name = "_propertyValue";
            this._propertyValue.Width = 800;
            // 
            // DefinitionsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._data);
            this.Name = "DefinitionsPanel";
            this.Size = new System.Drawing.Size(1089, 612);
            ((System.ComponentModel.ISupportInitialize)(this._data)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _data;
        private System.Windows.Forms.DataGridViewTextBoxColumn _propertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn _propertyValue;
    }
}
