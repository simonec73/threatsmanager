
namespace ThreatsManager.DevOps.Dialogs
{
    partial class DevOpsIterationAssignmentDialog
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
            this._ok = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._add = new System.Windows.Forms.Button();
            this._iterations = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._ok);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 46);
            this.panel1.TabIndex = 1;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(203, 11);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 1;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(284, 11);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._add);
            this.layoutControl1.Controls.Add(this._iterations);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControl1.Size = new System.Drawing.Size(562, 46);
            this.layoutControl1.TabIndex = 2;
            // 
            // _add
            // 
            this._add.Location = new System.Drawing.Point(483, 4);
            this._add.Margin = new System.Windows.Forms.Padding(0);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(75, 23);
            this._add.TabIndex = 1;
            this._add.Text = "Add New";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _iterations
            // 
            this._iterations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._iterations.FormattingEnabled = true;
            this._iterations.Location = new System.Drawing.Point(52, 4);
            this._iterations.Margin = new System.Windows.Forms.Padding(0);
            this._iterations.Name = "_iterations";
            this._iterations.Size = new System.Drawing.Size(423, 21);
            this._iterations.TabIndex = 0;
            this._iterations.SelectedIndexChanged += new System.EventHandler(this._iterations_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._iterations;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Iteration";
            this.layoutControlItem1.Width = 99;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._add;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 83;
            // 
            // DevOpsIterationAssignmentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(562, 92);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevOpsIterationAssignmentDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign to Iteration";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Button _cancel;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.Button _add;
        private System.Windows.Forms.ComboBox _iterations;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
    }
}