
using DevComponents.DotNetBar.SuperGrid;

namespace ThreatsManager.Extensions.Dialogs
{
    partial class ManageMitigationsRelationshipsDialog
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
            var cb = _grid.PrimaryGrid.Columns["Main"].EditControl as GridCheckBoxEditControl;
            if (cb != null)
            {
                cb.CheckedChanged -= OnMainChanged;
            }

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
            this._clear = new System.Windows.Forms.Button();
            this._main = new System.Windows.Forms.CheckBox();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._mitigationName = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._mainContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 714);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1534, 92);
            this.panel1.TabIndex = 3;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(772, 25);
            this._cancel.Margin = new System.Windows.Forms.Padding(6);
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
            this._ok.Location = new System.Drawing.Point(610, 25);
            this._ok.Margin = new System.Windows.Forms.Padding(6);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(150, 44);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._clear);
            this.layoutControl1.Controls.Add(this._main);
            this.layoutControl1.Controls.Add(this._grid);
            this.layoutControl1.Controls.Add(this._mitigationName);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(6);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this._mainContainer,
            this.layoutControlItem3});
            this.layoutControl1.Size = new System.Drawing.Size(1534, 714);
            this.layoutControl1.TabIndex = 4;
            // 
            // _clear
            // 
            this._clear.Location = new System.Drawing.Point(1338, 670);
            this._clear.Margin = new System.Windows.Forms.Padding(0);
            this._clear.Name = "_clear";
            this._clear.Size = new System.Drawing.Size(192, 40);
            this._clear.TabIndex = 3;
            this._clear.Text = "Clear All";
            this._clear.UseVisualStyleBackColor = true;
            this._clear.Click += new System.EventHandler(this._clear_Click);
            // 
            // _main
            // 
            this._main.AutoSize = true;
            this._main.Checked = true;
            this._main.CheckState = System.Windows.Forms.CheckState.Checked;
            this._main.Location = new System.Drawing.Point(8, 673);
            this._main.Margin = new System.Windows.Forms.Padding(0);
            this._main.Name = "_main";
            this._main.Size = new System.Drawing.Size(1318, 34);
            this._main.TabIndex = 2;
            this._main.Text = "Mark the Reference Mitigation as Main for the whole set of associated Mitigations" +
    ".";
            this._main.UseVisualStyleBackColor = true;
            this._main.CheckedChanged += new System.EventHandler(this._main_CheckedChanged);
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.HScrollBarVisible = false;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(234, 53);
            this._grid.Margin = new System.Windows.Forms.Padding(0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(1292, 606);
            this._grid.TabIndex = 1;
            this._grid.Text = "superGridControl1";
            // 
            // _mitigationName
            // 
            this._mitigationName.AutoSize = true;
            this._mitigationName.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._mitigationName.Location = new System.Drawing.Point(234, 7);
            this._mitigationName.Margin = new System.Windows.Forms.Padding(0);
            this._mitigationName.Name = "_mitigationName";
            this._mitigationName.Size = new System.Drawing.Size(1292, 32);
            this._mitigationName.TabIndex = 0;
            this._mitigationName.Text = "Mitigation Name";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._mitigationName;
            this.layoutControlItem1.Height = 46;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem1.Text = "Reference Mitigation";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._grid;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(128, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.layoutControlItem2.Text = "Associated Mitigations";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _mainContainer
            // 
            this._mainContainer.Control = this._main;
            this._mainContainer.Height = 48;
            this._mainContainer.MinSize = new System.Drawing.Size(64, 38);
            this._mainContainer.Name = "_mainContainer";
            this._mainContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._mainContainer.Width = 99;
            this._mainContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._clear;
            this.layoutControlItem3.Height = 31;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 200;
            // 
            // ManageMitigationsRelationshipsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(1534, 806);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageMitigationsRelationshipsDialog";
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
        private System.Windows.Forms.Label _mitigationName;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.CheckBox _main;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _mainContainer;
        private System.Windows.Forms.Button _clear;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
    }
}