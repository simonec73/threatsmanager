namespace ThreatsManager.MsTmt.Dialogs
{
    partial class ThreatAssignmentDialog
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
            this._threatDescription = new System.Windows.Forms.TextBox();
            this._threatName = new System.Windows.Forms.TextBox();
            this._targetObject = new System.Windows.Forms.ComboBox();
            this._targetObjectType = new System.Windows.Forms.ComboBox();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 189);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(223, 13);
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
            this._ok.Location = new System.Drawing.Point(142, 13);
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
            this._layout.Controls.Add(this._threatDescription);
            this._layout.Controls.Add(this._threatName);
            this._layout.Controls.Add(this._targetObject);
            this._layout.Controls.Add(this._targetObjectType);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this._layout.Size = new System.Drawing.Size(440, 189);
            this._layout.TabIndex = 0;
            // 
            // _threatDescription
            // 
            this._threatDescription.Location = new System.Drawing.Point(108, 32);
            this._threatDescription.Margin = new System.Windows.Forms.Padding(0);
            this._threatDescription.Multiline = true;
            this._threatDescription.Name = "_threatDescription";
            this._threatDescription.ReadOnly = true;
            this._threatDescription.Size = new System.Drawing.Size(328, 76);
            this._threatDescription.TabIndex = 1;
            // 
            // _threatName
            // 
            this._threatName.Location = new System.Drawing.Point(108, 4);
            this._threatName.Margin = new System.Windows.Forms.Padding(0);
            this._threatName.Name = "_threatName";
            this._threatName.ReadOnly = true;
            this._threatName.Size = new System.Drawing.Size(328, 20);
            this._threatName.TabIndex = 0;
            // 
            // _targetObject
            // 
            this._targetObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._targetObject.FormattingEnabled = true;
            this._targetObject.Location = new System.Drawing.Point(108, 145);
            this._targetObject.Margin = new System.Windows.Forms.Padding(0);
            this._targetObject.Name = "_targetObject";
            this._targetObject.Size = new System.Drawing.Size(328, 21);
            this._targetObject.TabIndex = 3;
            this._targetObject.SelectedIndexChanged += new System.EventHandler(this._targetObject_SelectedIndexChanged);
            // 
            // _targetObjectType
            // 
            this._targetObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._targetObjectType.FormattingEnabled = true;
            this._targetObjectType.Location = new System.Drawing.Point(108, 116);
            this._targetObjectType.Margin = new System.Windows.Forms.Padding(0);
            this._targetObjectType.Name = "_targetObjectType";
            this._targetObjectType.Size = new System.Drawing.Size(328, 21);
            this._targetObjectType.TabIndex = 2;
            this._targetObjectType.SelectedIndexChanged += new System.EventHandler(this._targetObjectType_SelectedIndexChanged);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._threatName;
            this.layoutControlItem3.Height = 28;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Threat Name";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._threatDescription;
            this.layoutControlItem4.Height = 84;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Threat Description";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._targetObjectType;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Target Object Type";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._targetObject;
            this.layoutControlItem2.Height = 29;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Target Object";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ThreatAssignmentDialog
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(440, 237);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThreatAssignmentDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign Threat";
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
        private System.Windows.Forms.ComboBox _targetObjectType;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.ComboBox _targetObject;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.TextBox _threatDescription;
        private System.Windows.Forms.TextBox _threatName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
    }
}