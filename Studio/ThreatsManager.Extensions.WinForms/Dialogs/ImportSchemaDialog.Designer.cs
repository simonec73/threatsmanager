namespace ThreatsManager.Extensions.Dialogs
{
    partial class ImportSchemaDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._applySchemas = new System.Windows.Forms.CheckBox();
            this._uncheckAll = new System.Windows.Forms.Button();
            this._checkAll = new System.Windows.Forms.Button();
            this._schemas = new System.Windows.Forms.CheckedListBox();
            this._browse = new System.Windows.Forms.Button();
            this._fileName = new System.Windows.Forms.TextBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._openFile = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 304);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(274, 13);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(193, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._applySchemas);
            this._layout.Controls.Add(this._uncheckAll);
            this._layout.Controls.Add(this._checkAll);
            this._layout.Controls.Add(this._schemas);
            this._layout.Controls.Add(this._browse);
            this._layout.Controls.Add(this._fileName);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutGroup1,
            this.layoutControlItem6});
            this._layout.Size = new System.Drawing.Size(543, 304);
            this._layout.TabIndex = 0;
            // 
            // _applySchemas
            // 
            this._applySchemas.AutoSize = true;
            this._applySchemas.Checked = true;
            this._applySchemas.CheckState = System.Windows.Forms.CheckState.Checked;
            this._applySchemas.Location = new System.Drawing.Point(4, 280);
            this._applySchemas.Margin = new System.Windows.Forms.Padding(0);
            this._applySchemas.Name = "_applySchemas";
            this._applySchemas.Size = new System.Drawing.Size(535, 17);
            this._applySchemas.TabIndex = 6;
            this._applySchemas.Text = "Apply selected Schemas automatically";
            this._applySchemas.UseVisualStyleBackColor = true;
            // 
            // _uncheckAll
            // 
            this._uncheckAll.Location = new System.Drawing.Point(464, 66);
            this._uncheckAll.Margin = new System.Windows.Forms.Padding(0);
            this._uncheckAll.Name = "_uncheckAll";
            this._uncheckAll.Size = new System.Drawing.Size(75, 23);
            this._uncheckAll.TabIndex = 4;
            this._uncheckAll.Text = "Uncheck All";
            this._uncheckAll.UseVisualStyleBackColor = true;
            this._uncheckAll.Click += new System.EventHandler(this._uncheckAll_Click);
            // 
            // _checkAll
            // 
            this._checkAll.Location = new System.Drawing.Point(464, 35);
            this._checkAll.Margin = new System.Windows.Forms.Padding(0);
            this._checkAll.Name = "_checkAll";
            this._checkAll.Size = new System.Drawing.Size(75, 23);
            this._checkAll.TabIndex = 3;
            this._checkAll.Text = "Check All";
            this._checkAll.UseVisualStyleBackColor = true;
            this._checkAll.Click += new System.EventHandler(this._checkAll_Click);
            // 
            // _schemas
            // 
            this._schemas.FormattingEnabled = true;
            this._schemas.Location = new System.Drawing.Point(63, 35);
            this._schemas.Margin = new System.Windows.Forms.Padding(0);
            this._schemas.Name = "_schemas";
            this._schemas.Size = new System.Drawing.Size(393, 229);
            this._schemas.TabIndex = 2;
            this._schemas.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._schemas_ItemCheck);
            // 
            // _browse
            // 
            this._browse.Location = new System.Drawing.Point(464, 4);
            this._browse.Margin = new System.Windows.Forms.Padding(0);
            this._browse.Name = "_browse";
            this._browse.Size = new System.Drawing.Size(75, 23);
            this._browse.TabIndex = 1;
            this._browse.Text = "Browse...";
            this._browse.UseVisualStyleBackColor = true;
            this._browse.Click += new System.EventHandler(this._browse_Click);
            // 
            // _fileName
            // 
            this._fileName.Location = new System.Drawing.Point(63, 4);
            this._fileName.Margin = new System.Windows.Forms.Padding(0);
            this._fileName.Name = "_fileName";
            this._fileName.ReadOnly = true;
            this._fileName.Size = new System.Drawing.Size(393, 20);
            this._fileName.TabIndex = 0;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._fileName;
            this.layoutControlItem1.Height = 28;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "File Name";
            this.layoutControlItem1.Width = 99;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._browse;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 83;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._schemas;
            this.layoutControlItem3.Height = 99;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Schemas";
            this.layoutControlItem3.Width = 99;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 100;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutGroup1.MinSize = new System.Drawing.Size(83, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Padding = new System.Windows.Forms.Padding(0);
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 83;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._checkAll;
            this.layoutControlItem4.Height = 31;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 83;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._uncheckAll;
            this.layoutControlItem5.Height = 31;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 83;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._applySchemas;
            this.layoutControlItem6.Height = 25;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _openFile
            // 
            this._openFile.DefaultExt = "tmt";
            this._openFile.Filter = "Threat Model Template (*.tmt)|*.tmt|Threat Model Json Template (*.tmk)|*.tmk";
            this._openFile.Title = "Select template file";
            this._openFile.RestoreDirectory = true;
            // 
            // ImportSchemaDialog
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(543, 352);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ImportSchemaDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Schema Import";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportSchemaDialog_FormClosed);
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.Button _browse;
        private System.Windows.Forms.TextBox _fileName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.CheckedListBox _schemas;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.OpenFileDialog _openFile;
        private System.Windows.Forms.Button _uncheckAll;
        private System.Windows.Forms.Button _checkAll;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.CheckBox _applySchemas;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
    }
}