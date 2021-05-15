using System.Linq;
using DevComponents.DotNetBar.SuperGrid;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    partial class PropertySchemaListPanel
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
            GridTextBoxDropDownEditControl ddc = _grid.PrimaryGrid.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
            if (ddc != null)
            {
                ddc.ButtonClearClick -= DdcButtonClearClick;
            }
            ddc = _grid.PrimaryGrid.Columns["Description"].EditControl as GridTextBoxDropDownEditControl;
            if (ddc != null)
            {
                ddc.ButtonClearClick -= DdcButtonClearClick;
            }

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                    RemoveEventSubscriptions(row);
            }

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
            this.components = new System.ComponentModel.Container();
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._requiredExecutionMode = new System.Windows.Forms.ComboBox();
            this._visible = new System.Windows.Forms.CheckBox();
            this._description = new System.Windows.Forms.TextBox();
            this._autoApply = new System.Windows.Forms.CheckBox();
            this._system = new System.Windows.Forms.CheckBox();
            this._priority = new DevComponents.Editors.IntegerInput();
            this._appliesTo = new DevComponents.DotNetBar.Controls.TokenEditor();
            this._namespace = new System.Windows.Forms.TextBox();
            this._name = new System.Windows.Forms.TextBox();
            this._schemas = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._notExportable = new System.Windows.Forms.CheckBox();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this._container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._priority)).BeginInit();
            this.SuspendLayout();
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._container.Controls.Add(this._notExportable);
            this._container.Controls.Add(this._requiredExecutionMode);
            this._container.Controls.Add(this._visible);
            this._container.Controls.Add(this._description);
            this._container.Controls.Add(this._autoApply);
            this._container.Controls.Add(this._system);
            this._container.Controls.Add(this._priority);
            this._container.Controls.Add(this._appliesTo);
            this._container.Controls.Add(this._namespace);
            this._container.Controls.Add(this._name);
            this._container.Controls.Add(this._schemas);
            this._container.Dock = System.Windows.Forms.DockStyle.Top;
            this._container.ForeColor = System.Drawing.Color.Black;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem8,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem10,
            this.layoutControlItem9,
            this.layoutControlItem6,
            this.layoutControlItem11});
            this._container.Size = new System.Drawing.Size(936, 212);
            this._container.TabIndex = 0;
            // 
            // _requiredExecutionMode
            // 
            this._requiredExecutionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._requiredExecutionMode.Enabled = false;
            this._requiredExecutionMode.FormattingEnabled = true;
            this._requiredExecutionMode.Location = new System.Drawing.Point(481, 174);
            this._requiredExecutionMode.Margin = new System.Windows.Forms.Padding(0);
            this._requiredExecutionMode.Name = "_requiredExecutionMode";
            this._requiredExecutionMode.Size = new System.Drawing.Size(155, 21);
            this._requiredExecutionMode.TabIndex = 7;
            this._requiredExecutionMode.SelectedIndexChanged += new System.EventHandler(this._requiredExecutionMode_SelectedIndexChanged);
            // 
            // _visible
            // 
            this._visible.AutoSize = true;
            this._visible.Enabled = false;
            this._visible.Location = new System.Drawing.Point(644, 174);
            this._visible.Margin = new System.Windows.Forms.Padding(0);
            this._visible.Name = "_visible";
            this._visible.Size = new System.Drawing.Size(80, 21);
            this._visible.TabIndex = 8;
            this._visible.Text = "Visible";
            this._visible.UseVisualStyleBackColor = true;
            this._visible.CheckedChanged += new System.EventHandler(this._visible_CheckedChanged);
            // 
            // _description
            // 
            this._description.Location = new System.Drawing.Point(141, 61);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Multiline = true;
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(791, 76);
            this._description.TabIndex = 3;
            this._description.TextChanged += new System.EventHandler(this._description_TextChanged);
            // 
            // _autoApply
            // 
            this._autoApply.AutoSize = true;
            this._autoApply.Location = new System.Drawing.Point(204, 174);
            this._autoApply.Margin = new System.Windows.Forms.Padding(0);
            this._autoApply.Name = "_autoApply";
            this._autoApply.Size = new System.Drawing.Size(132, 21);
            this._autoApply.TabIndex = 6;
            this._autoApply.Text = "Apply Automatically";
            this._autoApply.UseVisualStyleBackColor = true;
            this._autoApply.CheckedChanged += new System.EventHandler(this._autoApply_CheckedChanged);
            // 
            // _system
            // 
            this._system.AutoSize = true;
            this._system.Enabled = false;
            this._system.Location = new System.Drawing.Point(732, 174);
            this._system.Margin = new System.Windows.Forms.Padding(0);
            this._system.Name = "_system";
            this._system.Size = new System.Drawing.Size(80, 21);
            this._system.TabIndex = 9;
            this._system.Text = "System";
            this._system.UseVisualStyleBackColor = true;
            // 
            // _priority
            // 
            // 
            // 
            // 
            this._priority.BackgroundStyle.Class = "DateTimeInputBackground";
            this._priority.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._priority.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this._priority.Enabled = false;
            this._priority.Location = new System.Drawing.Point(141, 174);
            this._priority.Margin = new System.Windows.Forms.Padding(0);
            this._priority.MaxValue = 100;
            this._priority.MinValue = 1;
            this._priority.Name = "_priority";
            this._priority.ShowUpDown = true;
            this._priority.Size = new System.Drawing.Size(55, 20);
            this._priority.TabIndex = 5;
            this._priority.Value = 50;
            this._priority.ValueChanged += new System.EventHandler(this._priority_ValueChanged);
            // 
            // _appliesTo
            // 
            // 
            // 
            // 
            this._appliesTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this._appliesTo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._appliesTo.Enabled = false;
            this._appliesTo.Location = new System.Drawing.Point(141, 145);
            this._appliesTo.Margin = new System.Windows.Forms.Padding(0);
            this._appliesTo.Name = "_appliesTo";
            this._appliesTo.Separators.Add(";");
            this._appliesTo.Separators.Add(",");
            this._appliesTo.Size = new System.Drawing.Size(791, 21);
            this._appliesTo.TabIndex = 4;
            this._appliesTo.TextSeparator = ";";
            this._appliesTo.SelectedTokensChanged += new System.EventHandler(this._appliesTo_SelectedTokensChanged);
            this._appliesTo.ValidateToken += new DevComponents.DotNetBar.Controls.ValidateTokenEventHandler(this._appliesTo_ValidateToken);
            // 
            // _namespace
            // 
            this._namespace.Enabled = false;
            this._namespace.Location = new System.Drawing.Point(609, 33);
            this._namespace.Margin = new System.Windows.Forms.Padding(0);
            this._namespace.Name = "_namespace";
            this._namespace.Size = new System.Drawing.Size(323, 20);
            this._namespace.TabIndex = 2;
            this._namespace.TextChanged += new System.EventHandler(this._namespace_TextChanged);
            // 
            // _name
            // 
            this._name.Enabled = false;
            this._name.Location = new System.Drawing.Point(141, 33);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(323, 20);
            this._name.TabIndex = 1;
            this._name.TextChanged += new System.EventHandler(this._name_TextChanged);
            // 
            // _schemas
            // 
            this._schemas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._schemas.FormattingEnabled = true;
            this._schemas.Location = new System.Drawing.Point(141, 4);
            this._schemas.Margin = new System.Windows.Forms.Padding(0);
            this._schemas.Name = "_schemas";
            this._schemas.Size = new System.Drawing.Size(791, 21);
            this._schemas.TabIndex = 0;
            this._schemas.SelectedIndexChanged += new System.EventHandler(this._schemas_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._schemas;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Property Schema";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._name;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Name";
            this.layoutControlItem2.Width = 50;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._namespace;
            this.layoutControlItem3.Height = 28;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Namespace";
            this.layoutControlItem3.Width = 50;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._description;
            this.layoutControlItem8.Height = 84;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Description";
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._appliesTo;
            this.layoutControlItem4.Height = 29;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Applies To";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._priority;
            this.layoutControlItem5.Height = 28;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Priority";
            this.layoutControlItem5.Width = 200;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._autoApply;
            this.layoutControlItem7.Height = 25;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 140;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._requiredExecutionMode;
            this.layoutControlItem10.Height = 29;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "Required Execution Mode";
            this.layoutControlItem10.Width = 300;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._visible;
            this.layoutControlItem9.Height = 25;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 88;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._system;
            this.layoutControlItem6.Height = 25;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 88;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(0, 212);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(936, 499);
            this._grid.TabIndex = 1;
            this._grid.Text = "superGridControl1";
            // 
            // _contextMenu
            // 
            this._contextMenu.Name = "_contextMenu";
            this._contextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // _notExportable
            // 
            this._notExportable.AutoSize = true;
            this._notExportable.Enabled = false;
            this._notExportable.Location = new System.Drawing.Point(820, 174);
            this._notExportable.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._notExportable.Name = "_notExportable";
            this._notExportable.Size = new System.Drawing.Size(107, 21);
            this._notExportable.TabIndex = 10;
            this._notExportable.Text = "Not Exportable";
            this._notExportable.UseVisualStyleBackColor = true;
            this._notExportable.CheckedChanged += new System.EventHandler(this._notExportable_CheckedChanged);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._notExportable;
            this.layoutControlItem11.Height = 25;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Width = 115;
            // 
            // PropertySchemaListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._container);
            this.Name = "PropertySchemaListPanel";
            this.Size = new System.Drawing.Size(936, 711);
            this._container.ResumeLayout(false);
            this._container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._priority)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private System.Windows.Forms.ComboBox _schemas;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.TextBox _namespace;
        private System.Windows.Forms.TextBox _name;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Controls.TokenEditor _appliesTo;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.Editors.IntegerInput _priority;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.CheckBox _system;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private System.Windows.Forms.CheckBox _autoApply;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.TextBox _description;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.ContextMenuStrip _contextMenu;
        private System.Windows.Forms.CheckBox _visible;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.ComboBox _requiredExecutionMode;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private System.Windows.Forms.CheckBox _notExportable;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.ToolTip _tooltip;
    }
}
