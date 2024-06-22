
namespace ThreatsManager.Extensions.Dialogs
{
    partial class ApplySchemaMultipleDialog
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
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._description = new System.Windows.Forms.Label();
            this._containersCount = new System.Windows.Forms.Label();
            this._schemas = new System.Windows.Forms.ComboBox();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 164);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 46);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(259, 12);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 22);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(178, 12);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 22);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._description);
            this.layoutControl1.Controls.Add(this._containersCount);
            this.layoutControl1.Controls.Add(this._schemas);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.layoutControl1.Size = new System.Drawing.Size(513, 164);
            this.layoutControl1.TabIndex = 3;
            // 
            // _description
            // 
            this._description.AutoSize = true;
            this._description.Location = new System.Drawing.Point(100, 51);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(409, 108);
            this._description.TabIndex = 2;
            // 
            // _containersCount
            // 
            this._containersCount.AutoSize = true;
            this._containersCount.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._containersCount.Location = new System.Drawing.Point(100, 3);
            this._containersCount.Margin = new System.Windows.Forms.Padding(0);
            this._containersCount.Name = "_containersCount";
            this._containersCount.Size = new System.Drawing.Size(409, 14);
            this._containersCount.TabIndex = 0;
            this._containersCount.Text = "container";
            // 
            // _schemas
            // 
            this._schemas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._schemas.FormattingEnabled = true;
            this._schemas.Location = new System.Drawing.Point(100, 23);
            this._schemas.Margin = new System.Windows.Forms.Padding(0);
            this._schemas.Name = "_schemas";
            this._schemas.Size = new System.Drawing.Size(409, 21);
            this._schemas.TabIndex = 1;
            this._schemas.SelectedIndexChanged += new System.EventHandler(this._schemas_SelectedIndexChanged);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._containersCount;
            this.layoutControlItem3.Height = 20;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 17);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControlItem3.Text = "# affected objects";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._schemas;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 17);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControlItem2.Text = "Schema";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._description;
            this.layoutControlItem1.Height = 99;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 17);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControlItem1.Text = "Description";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ApplySchemaMultipleDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(513, 210);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApplySchemaMultipleDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Apply Property Schema";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.Label _description;
        private System.Windows.Forms.Label _containersCount;
        private System.Windows.Forms.ComboBox _schemas;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
    }
}