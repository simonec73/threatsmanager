namespace ThreatsManager.Quality.Dialogs
{
    partial class FalsePositivesListDialog
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
            this._botton = new System.Windows.Forms.Panel();
            this._close = new System.Windows.Forms.Button();
            this._removeAll = new System.Windows.Forms.Button();
            this._remove = new System.Windows.Forms.Button();
            this._left = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._qualityAnalyzers = new System.Windows.Forms.ListBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._right = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._falsePositives = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.Finding = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.Reason = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.Author = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.Timestamp = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._botton.SuspendLayout();
            this._left.SuspendLayout();
            this._right.SuspendLayout();
            this.SuspendLayout();
            // 
            // _botton
            // 
            this._botton.Controls.Add(this._close);
            this._botton.Controls.Add(this._removeAll);
            this._botton.Controls.Add(this._remove);
            this._botton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._botton.Location = new System.Drawing.Point(0, 502);
            this._botton.Name = "_botton";
            this._botton.Size = new System.Drawing.Size(921, 46);
            this._botton.TabIndex = 0;
            // 
            // _close
            // 
            this._close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._close.Location = new System.Drawing.Point(504, 11);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(75, 23);
            this._close.TabIndex = 2;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            // 
            // _removeAll
            // 
            this._removeAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._removeAll.Location = new System.Drawing.Point(423, 11);
            this._removeAll.Name = "_removeAll";
            this._removeAll.Size = new System.Drawing.Size(75, 23);
            this._removeAll.TabIndex = 1;
            this._removeAll.Text = "Remove All";
            this._removeAll.UseVisualStyleBackColor = true;
            this._removeAll.Click += new System.EventHandler(this._removeAll_Click);
            // 
            // _remove
            // 
            this._remove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._remove.Location = new System.Drawing.Point(342, 11);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(75, 23);
            this._remove.TabIndex = 0;
            this._remove.Text = "Remove";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
            // 
            // _left
            // 
            this._left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._left.Controls.Add(this._qualityAnalyzers);
            this._left.Dock = System.Windows.Forms.DockStyle.Left;
            this._left.ForeColor = System.Drawing.Color.Black;
            this._left.Location = new System.Drawing.Point(0, 0);
            this._left.Name = "_left";
            // 
            // 
            // 
            this._left.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1});
            this._left.Size = new System.Drawing.Size(200, 502);
            this._left.TabIndex = 1;
            // 
            // _qualityAnalyzers
            // 
            this._qualityAnalyzers.FormattingEnabled = true;
            this._qualityAnalyzers.Location = new System.Drawing.Point(4, 21);
            this._qualityAnalyzers.Margin = new System.Windows.Forms.Padding(0);
            this._qualityAnalyzers.Name = "_qualityAnalyzers";
            this._qualityAnalyzers.Size = new System.Drawing.Size(192, 472);
            this._qualityAnalyzers.TabIndex = 0;
            this._qualityAnalyzers.SelectedIndexChanged += new System.EventHandler(this._qualityAnalyzers_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._qualityAnalyzers;
            this.layoutControlItem1.Height = 100;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Quality Analyzers";
            this.layoutControlItem1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.ExpandableControl = this._left;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.Black;
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.ForeColor = System.Drawing.Color.Black;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.Black;
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.Empty;
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.Black;
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(200, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(6, 502);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 2;
            this.expandableSplitter1.TabStop = false;
            // 
            // _right
            // 
            this._right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._right.Controls.Add(this._falsePositives);
            this._right.Dock = System.Windows.Forms.DockStyle.Fill;
            this._right.ForeColor = System.Drawing.Color.Black;
            this._right.Location = new System.Drawing.Point(206, 0);
            this._right.Name = "_right";
            // 
            // 
            // 
            this._right.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2});
            this._right.Size = new System.Drawing.Size(715, 502);
            this._right.TabIndex = 3;
            // 
            // _falsePositives
            // 
            this._falsePositives.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._falsePositives.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._falsePositives.ForeColor = System.Drawing.Color.Black;
            this._falsePositives.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._falsePositives.Location = new System.Drawing.Point(4, 21);
            this._falsePositives.Margin = new System.Windows.Forms.Padding(0);
            this._falsePositives.Name = "_falsePositives";
            // 
            // 
            // 
            this._falsePositives.PrimaryGrid.AllowEdit = false;
            this._falsePositives.PrimaryGrid.Columns.Add(this.Finding);
            this._falsePositives.PrimaryGrid.Columns.Add(this.Reason);
            this._falsePositives.PrimaryGrid.Columns.Add(this.Author);
            this._falsePositives.PrimaryGrid.Columns.Add(this.Timestamp);
            this._falsePositives.PrimaryGrid.ShowRowDirtyMarker = false;
            this._falsePositives.PrimaryGrid.ShowRowHeaders = false;
            this._falsePositives.Size = new System.Drawing.Size(707, 477);
            this._falsePositives.TabIndex = 0;
            this._falsePositives.Text = "superGridControl1";
            // 
            // Finding
            // 
            this.Finding.AllowEdit = false;
            this.Finding.Name = "Finding";
            this.Finding.Width = 200;
            // 
            // Reason
            // 
            this.Reason.AllowEdit = false;
            this.Reason.Name = "Reason";
            this.Reason.Width = 300;
            // 
            // Author
            // 
            this.Author.AllowEdit = false;
            this.Author.Name = "Marked By";
            // 
            // Timestamp
            // 
            this.Timestamp.AllowEdit = false;
            this.Timestamp.Name = "Marked On";
            this.Timestamp.RenderType = typeof(DevComponents.DotNetBar.SuperGrid.GridDateTimeInputEditControl);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._falsePositives;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "False Positives";
            this.layoutControlItem2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // FalsePositivesListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._close;
            this.ClientSize = new System.Drawing.Size(921, 548);
            this.Controls.Add(this._right);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this._left);
            this.Controls.Add(this._botton);
            this.MinimizeBox = false;
            this.Name = "FalsePositivesListDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "False Positives List";
            this.Load += new System.EventHandler(this.FalsePositivesList_Load);
            this._botton.ResumeLayout(false);
            this._left.ResumeLayout(false);
            this._right.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _botton;
        private DevComponents.DotNetBar.Layout.LayoutControl _left;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.Layout.LayoutControl _right;
        private System.Windows.Forms.Button _close;
        private System.Windows.Forms.Button _removeAll;
        private System.Windows.Forms.Button _remove;
        private System.Windows.Forms.ListBox _qualityAnalyzers;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _falsePositives;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.SuperGrid.GridColumn Finding;
        private DevComponents.DotNetBar.SuperGrid.GridColumn Reason;
        private DevComponents.DotNetBar.SuperGrid.GridColumn Author;
        private DevComponents.DotNetBar.SuperGrid.GridColumn Timestamp;
    }
}