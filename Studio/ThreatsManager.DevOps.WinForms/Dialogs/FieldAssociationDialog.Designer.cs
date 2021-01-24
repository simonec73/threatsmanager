namespace ThreatsManager.DevOps.Dialogs
{
    partial class FieldAssociationDialog
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
            this._properties = new System.Windows.Forms.ComboBox();
            this._property = new System.Windows.Forms.RadioButton();
            this._priority = new System.Windows.Forms.RadioButton();
            this._description = new System.Windows.Forms.RadioButton();
            this._name = new System.Windows.Forms.RadioButton();
            this._id = new System.Windows.Forms.RadioButton();
            this._fields = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._schema = new System.Windows.Forms.Label();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._ok);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 178);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(551, 46);
            this.panel1.TabIndex = 1;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(197, 11);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 1;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(278, 11);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Close";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._schema);
            this.layoutControl1.Controls.Add(this._properties);
            this.layoutControl1.Controls.Add(this._property);
            this.layoutControl1.Controls.Add(this._priority);
            this.layoutControl1.Controls.Add(this._description);
            this.layoutControl1.Controls.Add(this._name);
            this.layoutControl1.Controls.Add(this._id);
            this.layoutControl1.Controls.Add(this._fields);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.layoutControl1.Size = new System.Drawing.Size(551, 178);
            this.layoutControl1.TabIndex = 2;
            // 
            // _properties
            // 
            this._properties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._properties.Enabled = false;
            this._properties.FormattingEnabled = true;
            this._properties.Location = new System.Drawing.Point(78, 108);
            this._properties.Margin = new System.Windows.Forms.Padding(0);
            this._properties.Name = "_properties";
            this._properties.Size = new System.Drawing.Size(193, 21);
            this._properties.TabIndex = 6;
            this._properties.SelectedIndexChanged += new System.EventHandler(this._properties_SelectedIndexChanged);
            // 
            // _property
            // 
            this._property.AutoSize = true;
            this._property.Location = new System.Drawing.Point(4, 83);
            this._property.Margin = new System.Windows.Forms.Padding(0);
            this._property.Name = "_property";
            this._property.Size = new System.Drawing.Size(543, 17);
            this._property.TabIndex = 5;
            this._property.TabStop = true;
            this._property.Text = "Mitigation Property";
            this._property.UseVisualStyleBackColor = true;
            this._property.CheckedChanged += new System.EventHandler(this._property_CheckedChanged);
            // 
            // _priority
            // 
            this._priority.AutoSize = true;
            this._priority.Location = new System.Drawing.Point(279, 58);
            this._priority.Margin = new System.Windows.Forms.Padding(0);
            this._priority.Name = "_priority";
            this._priority.Size = new System.Drawing.Size(268, 17);
            this._priority.TabIndex = 4;
            this._priority.TabStop = true;
            this._priority.Text = "Mitigation Priority";
            this._priority.UseVisualStyleBackColor = true;
            this._priority.CheckedChanged += new System.EventHandler(this._priority_CheckedChanged);
            // 
            // _description
            // 
            this._description.AutoSize = true;
            this._description.Location = new System.Drawing.Point(4, 58);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(267, 17);
            this._description.TabIndex = 3;
            this._description.TabStop = true;
            this._description.Text = "Mitigation Description";
            this._description.UseVisualStyleBackColor = true;
            this._description.CheckedChanged += new System.EventHandler(this._description_CheckedChanged);
            // 
            // _name
            // 
            this._name.AutoSize = true;
            this._name.Location = new System.Drawing.Point(279, 33);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(268, 17);
            this._name.TabIndex = 2;
            this._name.TabStop = true;
            this._name.Text = "Mitigation Name";
            this._name.UseVisualStyleBackColor = true;
            this._name.CheckedChanged += new System.EventHandler(this._name_CheckedChanged);
            // 
            // _id
            // 
            this._id.AutoSize = true;
            this._id.Location = new System.Drawing.Point(4, 33);
            this._id.Margin = new System.Windows.Forms.Padding(0);
            this._id.Name = "_id";
            this._id.Size = new System.Drawing.Size(267, 17);
            this._id.TabIndex = 1;
            this._id.TabStop = true;
            this._id.Text = "Mitigation ID";
            this._id.UseVisualStyleBackColor = true;
            this._id.CheckedChanged += new System.EventHandler(this._id_CheckedChanged);
            // 
            // _fields
            // 
            this._fields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._fields.FormattingEnabled = true;
            this._fields.Location = new System.Drawing.Point(78, 4);
            this._fields.Margin = new System.Windows.Forms.Padding(0);
            this._fields.Name = "_fields";
            this._fields.Size = new System.Drawing.Size(469, 21);
            this._fields.TabIndex = 0;
            this._fields.SelectedIndexChanged += new System.EventHandler(this._fields_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._fields;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Field";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._id;
            this.layoutControlItem2.Height = 25;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 50;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._name;
            this.layoutControlItem3.Height = 25;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 50;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._description;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 50;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._priority;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 50;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._property;
            this.layoutControlItem6.Height = 25;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._properties;
            this.layoutControlItem7.Height = 29;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Property";
            this.layoutControlItem7.Width = 50;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _schema
            // 
            this._schema.AutoSize = true;
            this._schema.Location = new System.Drawing.Point(353, 108);
            this._schema.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._schema.Name = "_schema";
            this._schema.Size = new System.Drawing.Size(194, 21);
            this._schema.TabIndex = 7;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._schema;
            this.layoutControlItem8.Height = 21;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "from Schema";
            this.layoutControlItem8.Width = 50;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // FieldAssociationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(551, 224);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "FieldAssociationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Field Association";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Button _cancel;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.ComboBox _properties;
        private System.Windows.Forms.RadioButton _property;
        private System.Windows.Forms.RadioButton _priority;
        private System.Windows.Forms.RadioButton _description;
        private System.Windows.Forms.RadioButton _name;
        private System.Windows.Forms.RadioButton _id;
        private System.Windows.Forms.ComboBox _fields;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.Label _schema;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
    }
}