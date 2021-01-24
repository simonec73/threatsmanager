namespace ThreatsManager.Controls
{
    partial class ExtensionConfig
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
            this._componentName = new System.Windows.Forms.TextBox();
            this._componentType = new System.Windows.Forms.TextBox();
            this._enabled = new System.Windows.Forms.CheckBox();
            this._properties = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._propertyName = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this._propertyValue = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._extensionNameLayout = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._extensionTypeLayout = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._propertiesLayout = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _componentName
            // 
            this._componentName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._componentName.Location = new System.Drawing.Point(94, 4);
            this._componentName.Margin = new System.Windows.Forms.Padding(0);
            this._componentName.Name = "_componentName";
            this._componentName.ReadOnly = true;
            this._componentName.Size = new System.Drawing.Size(446, 20);
            this._componentName.TabIndex = 0;
            // 
            // _componentType
            // 
            this._componentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._componentType.Location = new System.Drawing.Point(94, 32);
            this._componentType.Margin = new System.Windows.Forms.Padding(0);
            this._componentType.Name = "_componentType";
            this._componentType.ReadOnly = true;
            this._componentType.Size = new System.Drawing.Size(371, 20);
            this._componentType.TabIndex = 1;
            // 
            // _enabled
            // 
            this._enabled.AutoSize = true;
            this._enabled.Location = new System.Drawing.Point(473, 32);
            this._enabled.Margin = new System.Windows.Forms.Padding(0);
            this._enabled.Name = "_enabled";
            this._enabled.Size = new System.Drawing.Size(67, 20);
            this._enabled.TabIndex = 2;
            this._enabled.Text = "Enabled";
            this._enabled.UseVisualStyleBackColor = true;
            this._enabled.CheckedChanged += new System.EventHandler(this._enabled_CheckedChanged);
            // 
            // _properties
            // 
            this._properties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._properties.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._properties.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._properties.ForeColor = System.Drawing.Color.Black;
            this._properties.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._properties.Location = new System.Drawing.Point(4, 77);
            this._properties.Margin = new System.Windows.Forms.Padding(0);
            this._properties.Name = "_properties";
            // 
            // 
            // 
            this._properties.PrimaryGrid.Columns.Add(this._propertyName);
            this._properties.PrimaryGrid.Columns.Add(this._propertyValue);
            this._properties.PrimaryGrid.ShowRowDirtyMarker = false;
            this._properties.PrimaryGrid.ShowRowHeaders = false;
            this._properties.Size = new System.Drawing.Size(536, 152);
            this._properties.TabIndex = 3;
            this._properties.Text = "superGridControl1";
            // 
            // _propertyName
            // 
            this._propertyName.AllowEdit = false;
            this._propertyName.HeaderText = "Name";
            this._propertyName.Name = "_propertyName";
            this._propertyName.Width = 150;
            // 
            // _propertyValue
            // 
            this._propertyValue.HeaderText = "Value";
            this._propertyValue.Name = "_propertyValue";
            this._propertyValue.Width = 300;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._componentName);
            this.layoutControl1.Controls.Add(this._componentType);
            this.layoutControl1.Controls.Add(this._enabled);
            this.layoutControl1.Controls.Add(this._properties);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._extensionNameLayout,
            this._extensionTypeLayout,
            this.layoutControlItem3,
            this._propertiesLayout});
            this.layoutControl1.Size = new System.Drawing.Size(544, 233);
            this.layoutControl1.TabIndex = 7;
            // 
            // _extensionNameLayout
            // 
            this._extensionNameLayout.Control = this._componentName;
            this._extensionNameLayout.Height = 28;
            this._extensionNameLayout.MinSize = new System.Drawing.Size(120, 0);
            this._extensionNameLayout.Name = "_extensionNameLayout";
            this._extensionNameLayout.Text = "Extension Name";
            this._extensionNameLayout.Width = 100;
            this._extensionNameLayout.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _extensionTypeLayout
            // 
            this._extensionTypeLayout.Control = this._componentType;
            this._extensionTypeLayout.Height = 28;
            this._extensionTypeLayout.MinSize = new System.Drawing.Size(120, 0);
            this._extensionTypeLayout.Name = "_extensionTypeLayout";
            this._extensionTypeLayout.Text = "Extension Type";
            this._extensionTypeLayout.Width = 99;
            this._extensionTypeLayout.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._enabled;
            this.layoutControlItem3.Height = 25;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 75;
            // 
            // _propertiesLayout
            // 
            this._propertiesLayout.Control = this._properties;
            this._propertiesLayout.Height = 100;
            this._propertiesLayout.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._propertiesLayout.MinSize = new System.Drawing.Size(64, 18);
            this._propertiesLayout.Name = "_propertiesLayout";
            this._propertiesLayout.Text = "Properties";
            this._propertiesLayout.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._propertiesLayout.Width = 100;
            this._propertiesLayout.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ExtensionConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "ExtensionConfig";
            this.Size = new System.Drawing.Size(544, 233);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox _componentName;
        private System.Windows.Forms.TextBox _componentType;
        private System.Windows.Forms.CheckBox _enabled;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _properties;
        private DevComponents.DotNetBar.SuperGrid.GridColumn _propertyName;
        private DevComponents.DotNetBar.SuperGrid.GridColumn _propertyValue;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _extensionNameLayout;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _extensionTypeLayout;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _propertiesLayout;
    }
}
