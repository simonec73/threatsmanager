using System.Windows.Forms;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    partial class MitreCatalogControl
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
            this._download = new System.Windows.Forms.Button();
            this._source = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._topPanel = new System.Windows.Forms.Panel();
            this._applyFilter = new System.Windows.Forms.Button();
            this._filter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label2 = new System.Windows.Forms.Label();
            this._nodes = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this._splitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._leftPanel = new System.Windows.Forms.Panel();
            this._topLeftPanel = new System.Windows.Forms.Panel();
            this._views = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._properties = new ThreatsManager.Extensions.Panels.ThreatSources.PropertyControl();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._keywords = new DevComponents.DotNetBar.Controls.TokenEditor();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nodes)).BeginInit();
            this._leftPanel.SuspendLayout();
            this._topLeftPanel.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _download
            // 
            this._download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._download.Enabled = false;
            this._download.Location = new System.Drawing.Point(816, 3);
            this._download.Name = "_download";
            this._download.Size = new System.Drawing.Size(86, 24);
            this._download.TabIndex = 2;
            this._download.Text = "Download";
            this._download.UseVisualStyleBackColor = true;
            this._download.Click += new System.EventHandler(this._download_Click);
            // 
            // _source
            // 
            this._source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._source.Location = new System.Drawing.Point(67, 3);
            this._source.Name = "_source";
            this._source.ReadOnly = true;
            this._source.Size = new System.Drawing.Size(743, 20);
            this._source.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Source";
            // 
            // _topPanel
            // 
            this._topPanel.Controls.Add(this._applyFilter);
            this._topPanel.Controls.Add(this._filter);
            this._topPanel.Controls.Add(this.label2);
            this._topPanel.Controls.Add(this._download);
            this._topPanel.Controls.Add(this._source);
            this._topPanel.Controls.Add(this.label5);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(908, 59);
            this._topPanel.TabIndex = 0;
            // 
            // _applyFilter
            // 
            this._applyFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._applyFilter.Enabled = false;
            this._applyFilter.Location = new System.Drawing.Point(816, 29);
            this._applyFilter.Name = "_applyFilter";
            this._applyFilter.Size = new System.Drawing.Size(86, 24);
            this._applyFilter.TabIndex = 5;
            this._applyFilter.Text = "Apply";
            this._applyFilter.UseVisualStyleBackColor = true;
            this._applyFilter.Click += new System.EventHandler(this._applyFilter_Click);
            // 
            // _filter
            // 
            this._filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._filter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._filter.Border.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._filter.Border.BorderBottomWidth = 1;
            this._filter.Border.BorderColor = System.Drawing.Color.Black;
            this._filter.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._filter.Border.BorderLeftWidth = 1;
            this._filter.Border.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._filter.Border.BorderRightWidth = 1;
            this._filter.Border.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._filter.Border.BorderTopWidth = 1;
            this._filter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._filter.ButtonCustom.Symbol = "";
            this._filter.ButtonCustom.Visible = true;
            this._filter.DisabledBackColor = System.Drawing.Color.White;
            this._filter.ForeColor = System.Drawing.Color.Black;
            this._filter.Location = new System.Drawing.Point(67, 29);
            this._filter.MinimumSize = new System.Drawing.Size(0, 20);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(743, 20);
            this._filter.TabIndex = 4;
            this._filter.ButtonCustomClick += new System.EventHandler(this._filter_ButtonCustomClick);
            this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
            this._filter.KeyDown += new System.Windows.Forms.KeyEventHandler(this._filter_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Filter";
            // 
            // _nodes
            // 
            this._nodes.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this._nodes.AllowDrop = false;
            this._nodes.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this._nodes.BackgroundStyle.Class = "TreeBorderKey";
            this._nodes.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._nodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this._nodes.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._nodes.Location = new System.Drawing.Point(0, 34);
            this._nodes.Name = "_nodes";
            this._nodes.NodesConnector = this.nodeConnector1;
            this._nodes.NodeSpacing = 5;
            this._nodes.NodeStyle = this.elementStyle1;
            this._nodes.PathSeparator = ";";
            this._nodes.Size = new System.Drawing.Size(396, 447);
            this._nodes.Styles.Add(this.elementStyle1);
            this._nodes.TabIndex = 2;
            this._nodes.SelectedIndexChanged += new System.EventHandler(this._nodes_SelectedIndexChanged);
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // _splitter
            // 
            this._splitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._splitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._splitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._splitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._splitter.ForeColor = System.Drawing.Color.Black;
            this._splitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._splitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._splitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._splitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._splitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._splitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._splitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._splitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._splitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._splitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._splitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._splitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._splitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._splitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._splitter.Location = new System.Drawing.Point(396, 59);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(10, 481);
            this._splitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._splitter.TabIndex = 4;
            this._splitter.TabStop = false;
            // 
            // _leftPanel
            // 
            this._leftPanel.Controls.Add(this._nodes);
            this._leftPanel.Controls.Add(this._topLeftPanel);
            this._leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._leftPanel.Location = new System.Drawing.Point(0, 59);
            this._leftPanel.Name = "_leftPanel";
            this._leftPanel.Size = new System.Drawing.Size(396, 481);
            this._leftPanel.TabIndex = 5;
            // 
            // _topLeftPanel
            // 
            this._topLeftPanel.Controls.Add(this._views);
            this._topLeftPanel.Controls.Add(this.label1);
            this._topLeftPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topLeftPanel.Location = new System.Drawing.Point(0, 0);
            this._topLeftPanel.Name = "_topLeftPanel";
            this._topLeftPanel.Size = new System.Drawing.Size(396, 34);
            this._topLeftPanel.TabIndex = 1;
            // 
            // _views
            // 
            this._views.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._views.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._views.FormattingEnabled = true;
            this._views.Location = new System.Drawing.Point(67, 6);
            this._views.Name = "_views";
            this._views.Size = new System.Drawing.Size(313, 21);
            this._views.TabIndex = 1;
            this._views.SelectedIndexChanged += new System.EventHandler(this._views_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "View";
            // 
            // _properties
            // 
            this._properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._properties.Location = new System.Drawing.Point(406, 59);
            this._properties.Name = "_properties";
            this._properties.Size = new System.Drawing.Size(502, 446);
            this._properties.TabIndex = 3;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._keywords);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(406, 505);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1});
            this.layoutControl1.Size = new System.Drawing.Size(502, 35);
            this.layoutControl1.TabIndex = 6;
            // 
            // _keywords
            // 
            // 
            // 
            // 
            this._keywords.BackgroundStyle.Class = "DateTimeInputBackground";
            this._keywords.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._keywords.Enabled = false;
            this._keywords.Location = new System.Drawing.Point(61, 4);
            this._keywords.Margin = new System.Windows.Forms.Padding(0);
            this._keywords.Name = "_keywords";
            this._keywords.Separators.Add(";");
            this._keywords.Separators.Add(",");
            this._keywords.Size = new System.Drawing.Size(437, 21);
            this._keywords.TabIndex = 0;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._keywords;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Keywords";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // MitreCatalogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._properties);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._leftPanel);
            this.Controls.Add(this._topPanel);
            this.Name = "MitreCatalogControl";
            this.Size = new System.Drawing.Size(908, 540);
            this._topPanel.ResumeLayout(false);
            this._topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nodes)).EndInit();
            this._leftPanel.ResumeLayout(false);
            this._topLeftPanel.ResumeLayout(false);
            this._topLeftPanel.PerformLayout();
            this.layoutControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _download;
        private System.Windows.Forms.TextBox _source;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel _topPanel;
        private DevComponents.AdvTree.AdvTree _nodes;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ExpandableSplitter _splitter;
        private System.Windows.Forms.Panel _leftPanel;
        private System.Windows.Forms.Panel _topLeftPanel;
        private ComboBox _views;
        private Label label1;
        private Button _applyFilter;
        private DevComponents.DotNetBar.Controls.TextBoxX _filter;
        private Label label2;
        private PropertyControl _properties;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private DevComponents.DotNetBar.Controls.TokenEditor _keywords;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
    }
}
