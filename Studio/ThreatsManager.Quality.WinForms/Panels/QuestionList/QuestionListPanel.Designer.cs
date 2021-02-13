using System.Linq;
using DevComponents.DotNetBar.Layout;
using DevComponents.DotNetBar.SuperGrid;
using ItemEditor = ThreatsManager.Utilities.WinForms.ItemEditor;

namespace ThreatsManager.Quality.Panels.QuestionList
{
    partial class QuestionListPanel
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
            var bc = _grid.PrimaryGrid.Columns["AutoGenRule"].EditControl as GridButtonXEditControl;
            if (bc != null)
            {
                bc.Click -= BcButtonClick;
            }

            var ddc = _grid.PrimaryGrid.Columns["Question"].EditControl as GridTextBoxDropDownEditControl;
            if (ddc != null)
            {
                ddc.ButtonClearClick -= DdcButtonClearClick;
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
            this._topLeftPanel = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._ruleFilter = new System.Windows.Forms.CheckBox();
            this._apply = new System.Windows.Forms.Button();
            this._filter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this._saveFile = new System.Windows.Forms.SaveFileDialog();
            this._topLeftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _topLeftPanel
            // 
            this._topLeftPanel.BackColor = System.Drawing.Color.White;
            this._topLeftPanel.Controls.Add(this._ruleFilter);
            this._topLeftPanel.Controls.Add(this._apply);
            this._topLeftPanel.Controls.Add(this._filter);
            this._topLeftPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topLeftPanel.ForeColor = System.Drawing.Color.Black;
            this._topLeftPanel.Location = new System.Drawing.Point(0, 0);
            this._topLeftPanel.Name = "_topLeftPanel";
            // 
            // 
            // 
            this._topLeftPanel.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem4,
            this.layoutControlItem2});
            this._topLeftPanel.Size = new System.Drawing.Size(834, 36);
            this._topLeftPanel.TabIndex = 2;
            // 
            // _ruleFilter
            // 
            this._ruleFilter.Appearance = System.Windows.Forms.Appearance.Button;
            this._ruleFilter.AutoSize = true;
            this._ruleFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._ruleFilter.Location = new System.Drawing.Point(671, 4);
            this._ruleFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._ruleFilter.Name = "_ruleFilter";
            this._ruleFilter.Size = new System.Drawing.Size(72, 23);
            this._ruleFilter.TabIndex = 1;
            this._ruleFilter.Text = "Rule Filter";
            this._ruleFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._ruleFilter.UseVisualStyleBackColor = true;
            this._ruleFilter.CheckedChanged += new System.EventHandler(this._ruleFilter_CheckedChanged);
            // 
            // _apply
            // 
            this._apply.Location = new System.Drawing.Point(755, 4);
            this._apply.Margin = new System.Windows.Forms.Padding(0);
            this._apply.Name = "_apply";
            this._apply.Size = new System.Drawing.Size(75, 23);
            this._apply.TabIndex = 2;
            this._apply.Text = "Apply";
            this._apply.UseVisualStyleBackColor = true;
            this._apply.Click += new System.EventHandler(this._apply_Click);
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
            this._filter.Location = new System.Drawing.Point(36, 4);
            this._filter.Margin = new System.Windows.Forms.Padding(0);
            this._filter.Name = "_filter";
            this._filter.PreventEnterBeep = true;
            this._filter.Size = new System.Drawing.Size(623, 20);
            this._filter.TabIndex = 0;
            this._filter.ButtonCustomClick += new System.EventHandler(this._filter_ButtonCustomClick);
            this._filter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._filter_KeyPress);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._filter;
            this.layoutControlItem1.Height = 31;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Filter";
            this.layoutControlItem1.Width = 99;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._ruleFilter;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 88;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._apply;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 83;
            // 
            // _grid
            // 
            this._grid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.ForeColor = System.Drawing.Color.Black;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(0, 36);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(834, 476);
            this._grid.TabIndex = 3;
            this._grid.Text = "superGridControl1";
            this._grid.CellActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridCellActivatedEventArgs>(this._grid_CellActivated);
            this._grid.RowActivated += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridRowActivatedEventArgs>(this._grid_RowActivated);
            // 
            // _saveFile
            // 
            this._saveFile.DefaultExt = "tmq";
            this._saveFile.Filter = "Questions file (*.tmq)|*.tmq";
            this._saveFile.RestoreDirectory = true;
            this._saveFile.Title = "Save Questions";
            // 
            // QuestionListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._grid);
            this.Controls.Add(this._topLeftPanel);
            this.Name = "QuestionListPanel";
            this.Size = new System.Drawing.Size(834, 512);
            this._topLeftPanel.ResumeLayout(false);
            this._topLeftPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Layout.LayoutControl _topLeftPanel;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private System.Windows.Forms.Button _apply;
        private DevComponents.DotNetBar.Controls.TextBoxX _filter;
        private LayoutControlItem layoutControlItem1;
        private LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.CheckBox _ruleFilter;
        private LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.SaveFileDialog _saveFile;
    }
}
