
namespace ThreatsManager.Extensions.Dialogs
{
    partial class AssociateMitigationsDialog
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
            this._standardMitigations = new System.Windows.Forms.CheckBox();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._item = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._standardMitigationsContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 371);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 48);
            this.panel1.TabIndex = 3;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(386, 13);
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
            this._ok.Location = new System.Drawing.Point(305, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._standardMitigations);
            this.layoutControl1.Controls.Add(this._grid);
            this.layoutControl1.Controls.Add(this._item);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this._standardMitigationsContainer});
            this.layoutControl1.Size = new System.Drawing.Size(767, 371);
            this.layoutControl1.TabIndex = 4;
            // 
            // _standardMitigations
            // 
            this._standardMitigations.AutoSize = true;
            this._standardMitigations.Checked = true;
            this._standardMitigations.CheckState = System.Windows.Forms.CheckState.Checked;
            this._standardMitigations.Location = new System.Drawing.Point(4, 350);
            this._standardMitigations.Margin = new System.Windows.Forms.Padding(0);
            this._standardMitigations.Name = "_standardMitigations";
            this._standardMitigations.Size = new System.Drawing.Size(759, 17);
            this._standardMitigations.TabIndex = 2;
            this._standardMitigations.Text = "Set as Standard Mitigations";
            this._standardMitigations.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(78, 28);
            this._grid.Margin = new System.Windows.Forms.Padding(0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(685, 314);
            this._grid.TabIndex = 1;
            this._grid.Text = "superGridControl1";
            // 
            // _item
            // 
            this._item.AutoSize = true;
            this._item.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._item.Location = new System.Drawing.Point(78, 4);
            this._item.Margin = new System.Windows.Forms.Padding(0);
            this._item.Name = "_item";
            this._item.Size = new System.Drawing.Size(685, 16);
            this._item.TabIndex = 0;
            this._item.Text = "item";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._item;
            this.layoutControlItem1.Height = 24;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Affected Item";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._grid;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Mitigations";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _standardMitigationsContainer
            // 
            this._standardMitigationsContainer.Control = this._standardMitigations;
            this._standardMitigationsContainer.Height = 25;
            this._standardMitigationsContainer.MinSize = new System.Drawing.Size(32, 20);
            this._standardMitigationsContainer.Name = "_standardMitigationsContainer";
            this._standardMitigationsContainer.Width = 100;
            this._standardMitigationsContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // AssociateMitigationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(767, 419);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssociateMitigationsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Associate Multiple Mitigations";
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
        private System.Windows.Forms.Label _item;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.CheckBox _standardMitigations;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _standardMitigationsContainer;
    }
}