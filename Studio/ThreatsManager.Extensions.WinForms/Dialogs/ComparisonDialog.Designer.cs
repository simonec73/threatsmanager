namespace ThreatsManager.Extensions.Dialogs
{
    partial class ComparisonDialog
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
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._sourceEditor = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this._targetEditor = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this._sourceGroup = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._targetGroup = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._sourceEditor);
            this.layoutControl1.Controls.Add(this._targetEditor);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._sourceGroup,
            this._targetGroup});
            this.layoutControl1.Size = new System.Drawing.Size(853, 657);
            this.layoutControl1.TabIndex = 0;
            // 
            // _sourceEditor
            // 
            this._sourceEditor.BackColor = System.Drawing.Color.White;
            this._sourceEditor.Item = null;
            this._sourceEditor.Location = new System.Drawing.Point(8, 30);
            this._sourceEditor.Margin = new System.Windows.Forms.Padding(0);
            this._sourceEditor.Name = "_sourceEditor";
            this._sourceEditor.ReadOnly = true;
            this._sourceEditor.Size = new System.Drawing.Size(410, 619);
            this._sourceEditor.TabIndex = 0;
            // 
            // _targetEditor
            // 
            this._targetEditor.BackColor = System.Drawing.Color.White;
            this._targetEditor.Item = null;
            this._targetEditor.Location = new System.Drawing.Point(435, 30);
            this._targetEditor.Margin = new System.Windows.Forms.Padding(0);
            this._targetEditor.Name = "_targetEditor";
            this._targetEditor.ReadOnly = true;
            this._targetEditor.Size = new System.Drawing.Size(410, 619);
            this._targetEditor.TabIndex = 10000;
            // 
            // _sourceGroup
            // 
            this._sourceGroup.Appearance = DevComponents.DotNetBar.Layout.eGroupAppearance.Panel;
            this._sourceGroup.CaptionHeight = 22;
            this._sourceGroup.Height = 100;
            this._sourceGroup.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._sourceGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3});
            this._sourceGroup.MinSize = new System.Drawing.Size(120, 32);
            this._sourceGroup.Name = "_sourceGroup";
            this._sourceGroup.Text = "Threat Model to be merged";
            this._sourceGroup.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this._sourceGroup.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._sourceGroup.Width = 50;
            this._sourceGroup.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._sourceEditor;
            this.layoutControlItem3.Height = 100;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Label:";
            this.layoutControlItem3.TextVisible = false;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _targetGroup
            // 
            this._targetGroup.Appearance = DevComponents.DotNetBar.Layout.eGroupAppearance.Panel;
            this._targetGroup.CaptionHeight = 22;
            this._targetGroup.Height = 100;
            this._targetGroup.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._targetGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem4});
            this._targetGroup.MinSize = new System.Drawing.Size(120, 32);
            this._targetGroup.Name = "_targetGroup";
            this._targetGroup.Text = "Current Threat Model";
            this._targetGroup.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this._targetGroup.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._targetGroup.Width = 50;
            this._targetGroup.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._targetEditor;
            this.layoutControlItem4.Height = 100;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Label:";
            this.layoutControlItem4.TextVisible = false;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ComparisonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(853, 657);
            this.Controls.Add(this.layoutControl1);
            this.MinimizeBox = false;
            this.Name = "ComparisonDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Comparison";
            this.layoutControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private Utilities.WinForms.ItemEditor _sourceEditor;
        private DevComponents.DotNetBar.Layout.LayoutGroup _sourceGroup;
        private DevComponents.DotNetBar.Layout.LayoutGroup _targetGroup;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private Utilities.WinForms.ItemEditor _targetEditor;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
    }
}