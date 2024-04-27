
namespace ThreatsManager.Extensions.Dialogs
{
    partial class RoadmapFilterDialog
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
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._filters = new System.Windows.Forms.CheckedListBox();
            this._operator = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 571);
            this.panel1.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1116, 121);
            this.panel1.TabIndex = 3;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(570, 48);
            this._cancel.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(150, 44);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(396, 48);
            this._ok.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(150, 44);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._container.Controls.Add(this._filters);
            this._container.Controls.Add(this._operator);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.ForeColor = System.Drawing.Color.Black;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem1});
            this._container.Size = new System.Drawing.Size(1116, 571);
            this._container.TabIndex = 4;
            // 
            // _filters
            // 
            this._filters.FormattingEnabled = true;
            this._filters.Location = new System.Drawing.Point(172, 7);
            this._filters.Margin = new System.Windows.Forms.Padding(0);
            this._filters.Name = "_filters";
            this._filters.Size = new System.Drawing.Size(936, 480);
            this._filters.TabIndex = 0;
            this._filters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._filters_ItemCheck);
            // 
            // _operator
            // 
            // 
            // 
            // 
            this._operator.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._operator.Location = new System.Drawing.Point(172, 514);
            this._operator.Margin = new System.Windows.Forms.Padding(0);
            this._operator.Name = "_operator";
            this._operator.OffText = "AND";
            this._operator.OnText = "OR";
            this._operator.Size = new System.Drawing.Size(220, 44);
            this._operator.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._operator.TabIndex = 1;
            this._operator.ValueChanged += new System.EventHandler(this._operator_ValueChanged);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._filters;
            this.layoutControlItem2.Height = 99;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem2.Text = "Available Filters";
            this.layoutControlItem2.Width = 101;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._operator;
            this.layoutControlItem1.Height = 58;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem1.Text = "Operator";
            this.layoutControlItem1.Width = 400;
            // 
            // RoadmapFilterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1116, 692);
            this.Controls.Add(this._container);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoadmapFilterDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Define Filter";
            this.panel1.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private System.Windows.Forms.CheckedListBox _filters;
        private DevComponents.DotNetBar.Controls.SwitchButton _operator;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
    }
}