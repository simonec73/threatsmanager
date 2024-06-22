
using DevComponents.DotNetBar.SuperGrid;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

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
            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    RemoveSuperTooltipProvider(row[0]);
                }
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
            this._standardMitigations = new System.Windows.Forms.CheckBox();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._item = new System.Windows.Forms.Label();
            this._filter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this._apply = new System.Windows.Forms.Button();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._standardMitigationsContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._tooltipTimer = new System.Windows.Forms.Timer();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 357);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(722, 46);
            this.panel1.TabIndex = 3;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(364, 12);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 22);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(283, 12);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 22);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._standardMitigations);
            this.layoutControl1.Controls.Add(this._grid);
            this.layoutControl1.Controls.Add(this._item);
            this.layoutControl1.Controls.Add(this._filter);
            this.layoutControl1.Controls.Add(this._apply);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem2,
            this._standardMitigationsContainer});
            this.layoutControl1.Size = new System.Drawing.Size(722, 357);
            this.layoutControl1.TabIndex = 4;
            // 
            // _standardMitigations
            // 
            this._standardMitigations.AutoSize = true;
            this._standardMitigations.Checked = true;
            this._standardMitigations.CheckState = System.Windows.Forms.CheckState.Checked;
            this._standardMitigations.Location = new System.Drawing.Point(4, 336);
            this._standardMitigations.Margin = new System.Windows.Forms.Padding(0);
            this._standardMitigations.Name = "_standardMitigations";
            this._standardMitigations.Size = new System.Drawing.Size(714, 18);
            this._standardMitigations.TabIndex = 4;
            this._standardMitigations.Text = "Set as Standard Mitigations";
            this._standardMitigations.UseVisualStyleBackColor = true;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(78, 57);
            this._grid.Margin = new System.Windows.Forms.Padding(0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(640, 273);
            this._grid.TabIndex = 3;
            this._grid.Text = "superGridControl1";
            this._grid.CellMouseDown += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this._grid_CellMouseDown);
            this._grid.CellMouseLeave += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellEventArgs>(this._grid_CellMouseLeave);
            this._grid.CellMouseMove += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellMouseEventArgs>(this._grid_CellMouseMove);
            // 
            // _item
            // 
            this._item.AutoSize = true;
            this._item.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this._item.Location = new System.Drawing.Point(78, 3);
            this._item.Margin = new System.Windows.Forms.Padding(0);
            this._item.Name = "_item";
            this._item.Size = new System.Drawing.Size(640, 17);
            this._item.TabIndex = 0;
            this._item.Text = "item";
            // 
            // _filter
            // 
            this._filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._filter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._filter.Border.Class = "TextBoxBorder";
            this._filter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._filter.ButtonCustom.Symbol = "";
            this._filter.ButtonCustom.Visible = true;
            this._filter.DisabledBackColor = System.Drawing.Color.White;
            this._filter.ForeColor = System.Drawing.Color.Black;
            this._filter.Location = new System.Drawing.Point(78, 27);
            this._filter.Margin = new System.Windows.Forms.Padding(0);
            this._filter.Name = "_filter";
            this._filter.PreventEnterBeep = true;
            this._filter.Size = new System.Drawing.Size(557, 20);
            this._filter.TabIndex = 1;
            this._filter.ButtonCustomClick += new System.EventHandler(this._filter_ButtonCustomClick);
            // 
            // _apply
            // 
            this._apply.Location = new System.Drawing.Point(643, 27);
            this._apply.Margin = new System.Windows.Forms.Padding(0);
            this._apply.Name = "_apply";
            this._apply.Size = new System.Drawing.Size(75, 23);
            this._apply.TabIndex = 2;
            this._apply.Text = "Apply";
            this._apply.UseVisualStyleBackColor = true;
            this._apply.Click += new System.EventHandler(this._apply_Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._item;
            this.layoutControlItem1.Height = 23;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 17);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControlItem1.Text = "Affected Item";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._filter;
            this.layoutControlItem3.Height = 28;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Filter";
            this.layoutControlItem3.Width = 99;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._apply;
            this.layoutControlItem4.Height = 31;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 83;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._grid;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 17);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControlItem2.Text = "Mitigations";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _standardMitigationsContainer
            // 
            this._standardMitigationsContainer.Control = this._standardMitigations;
            this._standardMitigationsContainer.Height = 24;
            this._standardMitigationsContainer.MinSize = new System.Drawing.Size(32, 19);
            this._standardMitigationsContainer.Name = "_standardMitigationsContainer";
            this._standardMitigationsContainer.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._standardMitigationsContainer.Width = 100;
            this._standardMitigationsContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.DelayTooltipHideDuration = 250;
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            // 
            // _tooltipTimer
            // 
            this._tooltipTimer.Interval = 1000;
            this._tooltipTimer.Tick += new System.EventHandler(this._tooltipTimer_Tick);
            // 
            // AssociateMitigationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(722, 403);
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
        private DevComponents.DotNetBar.Controls.TextBoxX _filter;
        private System.Windows.Forms.Button _apply;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private System.Windows.Forms.Timer _tooltipTimer;
    }
}