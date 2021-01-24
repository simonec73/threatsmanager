namespace ThreatsManager.DevOps.Dialogs
{
    partial class DevOpsConfigurationDialog
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this._close = new System.Windows.Forms.Button();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._remove = new System.Windows.Forms.Button();
            this._add = new System.Windows.Forms.Button();
            this._gridFields = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._gridStates = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._tag = new System.Windows.Forms.TextBox();
            this._itemTypes = new System.Windows.Forms.ComboBox();
            this._parents = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this._parentItemType = new System.Windows.Forms.Label();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._close);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 446);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(501, 46);
            this.panel1.TabIndex = 1;
            // 
            // _close
            // 
            this._close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._close.Location = new System.Drawing.Point(213, 11);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(75, 23);
            this._close.TabIndex = 2;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._remove);
            this.layoutControl1.Controls.Add(this._add);
            this.layoutControl1.Controls.Add(this._gridFields);
            this.layoutControl1.Controls.Add(this._gridStates);
            this.layoutControl1.Controls.Add(this._tag);
            this.layoutControl1.Controls.Add(this._itemTypes);
            this.layoutControl1.Controls.Add(this._parents);
            this.layoutControl1.Controls.Add(this._parentItemType);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem8,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControl1.Size = new System.Drawing.Size(501, 446);
            this.layoutControl1.TabIndex = 2;
            // 
            // _remove
            // 
            this._remove.Location = new System.Drawing.Point(87, 419);
            this._remove.Margin = new System.Windows.Forms.Padding(0);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(75, 23);
            this._remove.TabIndex = 7;
            this._remove.Text = "Remove";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
            // 
            // _add
            // 
            this._add.Location = new System.Drawing.Point(4, 419);
            this._add.Margin = new System.Windows.Forms.Padding(0);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(75, 23);
            this._add.TabIndex = 6;
            this._add.Text = "Add";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _gridFields
            // 
            this._gridFields.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._gridFields.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._gridFields.Location = new System.Drawing.Point(4, 282);
            this._gridFields.Margin = new System.Windows.Forms.Padding(0);
            this._gridFields.Name = "_gridFields";
            this._gridFields.Size = new System.Drawing.Size(493, 129);
            this._gridFields.TabIndex = 5;
            // 
            // _gridStates
            // 
            this._gridStates.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._gridStates.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._gridStates.Location = new System.Drawing.Point(4, 128);
            this._gridStates.Margin = new System.Windows.Forms.Padding(0);
            this._gridStates.Name = "_gridStates";
            this._gridStates.Size = new System.Drawing.Size(493, 129);
            this._gridStates.TabIndex = 4;
            // 
            // _tag
            // 
            this._tag.Location = new System.Drawing.Point(98, 83);
            this._tag.Margin = new System.Windows.Forms.Padding(0);
            this._tag.Name = "_tag";
            this._tag.Size = new System.Drawing.Size(399, 20);
            this._tag.TabIndex = 3;
            this._tooltip.SetToolTip(this._tag, "Specify an optional Tag to mark the objects managed by Threats Manager Studio.");
            // 
            // _itemTypes
            // 
            this._itemTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._itemTypes.FormattingEnabled = true;
            this._itemTypes.Location = new System.Drawing.Point(98, 54);
            this._itemTypes.Margin = new System.Windows.Forms.Padding(0);
            this._itemTypes.Name = "_itemTypes";
            this._itemTypes.Size = new System.Drawing.Size(399, 21);
            this._itemTypes.TabIndex = 2;
            this._tooltip.SetToolTip(this._itemTypes, "Select the type of the objects to be created in the DevOps system.");
            this._itemTypes.SelectedIndexChanged += new System.EventHandler(this._itemTypes_SelectedIndexChanged);
            // 
            // _parents
            // 
            this._parents.FormattingEnabled = true;
            this._parents.Location = new System.Drawing.Point(98, 4);
            this._parents.Margin = new System.Windows.Forms.Padding(0);
            this._parents.Name = "_parents";
            this._parents.Size = new System.Drawing.Size(399, 21);
            this._parents.TabIndex = 0;
            this._tooltip.SetToolTip(this._parents, "Type the first three characters of the name of the desired parent,\r\nand then sele" +
        "ct it from the list.");
            this._parents.SelectedIndexChanged += new System.EventHandler(this._parents_SelectedIndexChanged);
            this._parents.TextUpdate += new System.EventHandler(this.OnComboBoxTextUpdate);
            this._parents.KeyDown += new System.Windows.Forms.KeyEventHandler(this._parents_KeyDown);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._parents;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Parent";
            this.layoutControlItem1.Tooltip = "Parent node for the items to be created.";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._itemTypes;
            this.layoutControlItem2.Height = 29;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Item Type";
            this.layoutControlItem2.Tooltip = "Type of the items to be created.";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._tag;
            this.layoutControlItem3.Height = 28;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Tag";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._gridStates;
            this.layoutControlItem4.Height = 50;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "States";
            this.layoutControlItem4.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._gridFields;
            this.layoutControlItem5.Height = 50;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Fields";
            this.layoutControlItem5.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._add;
            this.layoutControlItem6.Height = 31;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 83;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._remove;
            this.layoutControlItem7.Height = 31;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 83;
            // 
            // _parentItemType
            // 
            this._parentItemType.AutoSize = true;
            this._parentItemType.Location = new System.Drawing.Point(98, 33);
            this._parentItemType.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._parentItemType.Name = "_parentItemType";
            this._parentItemType.Size = new System.Drawing.Size(399, 13);
            this._parentItemType.TabIndex = 1;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._parentItemType;
            this.layoutControlItem8.Height = 21;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Parent Item Type";
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // DevOpsConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._close;
            this.ClientSize = new System.Drawing.Size(501, 492);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "DevOpsConfigurationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DevOps Configuration";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _close;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.ComboBox _itemTypes;
        private System.Windows.Forms.ComboBox _parents;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.TextBox _tag;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _gridFields;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _gridStates;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.Button _remove;
        private System.Windows.Forms.Button _add;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.ToolTip _tooltip;
        private System.Windows.Forms.Label _parentItemType;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
    }
}