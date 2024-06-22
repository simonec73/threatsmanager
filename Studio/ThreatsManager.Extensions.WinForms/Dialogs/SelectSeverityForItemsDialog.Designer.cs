
namespace ThreatsManager.Extensions.Dialogs
{
    partial class SelectSeverityForItemsDialog
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
            this._severities = new System.Windows.Forms.ComboBox();
            this._count = new System.Windows.Forms.Label();
            this._countLayoutControlItem = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 48);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(244, 13);
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
            this._ok.Location = new System.Drawing.Point(163, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._severities);
            this.layoutControl1.Controls.Add(this._count);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._countLayoutControlItem,
            this.layoutControlItem2});
            this.layoutControl1.Size = new System.Drawing.Size(482, 74);
            this.layoutControl1.TabIndex = 3;
            // 
            // _severities
            // 
            this._severities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._severities.FormattingEnabled = true;
            this._severities.Location = new System.Drawing.Point(105, 25);
            this._severities.Margin = new System.Windows.Forms.Padding(0);
            this._severities.Name = "_severities";
            this._severities.Size = new System.Drawing.Size(373, 21);
            this._severities.TabIndex = 1;
            this._severities.SelectedIndexChanged += new System.EventHandler(this._severities_SelectedIndexChanged);
            // 
            // _count
            // 
            this._count.AutoSize = true;
            this._count.Location = new System.Drawing.Point(105, 4);
            this._count.Margin = new System.Windows.Forms.Padding(0);
            this._count.Name = "_count";
            this._count.Size = new System.Drawing.Size(373, 13);
            this._count.TabIndex = 0;
            this._count.Text = "label1";
            // 
            // _countLayoutControlItem
            // 
            this._countLayoutControlItem.Control = this._count;
            this._countLayoutControlItem.Height = 21;
            this._countLayoutControlItem.MinSize = new System.Drawing.Size(64, 18);
            this._countLayoutControlItem.Name = "_countLayoutControlItem";
            this._countLayoutControlItem.Text = "# of selected items";
            this._countLayoutControlItem.Width = 100;
            this._countLayoutControlItem.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._severities;
            this.layoutControlItem2.Height = 29;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Severity";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // SelectSeverityForItemsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(482, 122);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectSeverityForItemsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select the Severity to be applied to the selected items";
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
        private System.Windows.Forms.ComboBox _severities;
        private System.Windows.Forms.Label _count;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _countLayoutControlItem;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
    }
}