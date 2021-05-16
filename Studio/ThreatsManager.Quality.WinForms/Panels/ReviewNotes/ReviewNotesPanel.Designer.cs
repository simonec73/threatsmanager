
namespace ThreatsManager.Quality.Panels.ReviewNotes
{
    partial class ReviewNotesPanel
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
            _objects.SelectedIndexChanged -= new System.EventHandler(this._objects_SelectedIndexChanged);
            _objectTypes.SelectedValueChanged -= new System.EventHandler(this._objectTypes_SelectedValueChanged);

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
            this._left = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._objects = new System.Windows.Forms.ListView();
            this.ObjectName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this._objectTypes = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._right = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._annotation = new ThreatsManager.Quality.Annotations.AnnotationControl();
            this._annotationLayoutControlItem = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._properties = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this.expandableSplitter2 = new DevComponents.DotNetBar.ExpandableSplitter();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._filterContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._filter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._apply = new System.Windows.Forms.Button();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._left.SuspendLayout();
            this._right.SuspendLayout();
            this.SuspendLayout();
            // 
            // _left
            // 
            this._left.BackColor = System.Drawing.Color.White;
            this._left.Controls.Add(this._objects);
            this._left.Controls.Add(this._objectTypes);
            this._left.Controls.Add(this._filter);
            this._left.Controls.Add(this._apply);
            this._left.Dock = System.Windows.Forms.DockStyle.Left;
            this._left.Location = new System.Drawing.Point(0, 0);
            this._left.Name = "_left";
            // 
            // 
            // 
            this._left.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem2});
            this._left.Size = new System.Drawing.Size(330, 624);
            this._left.TabIndex = 0;
            // 
            // _objects
            // 
            this._objects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ObjectName});
            this._objects.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._objects.HideSelection = false;
            this._objects.Location = new System.Drawing.Point(4, 64);
            this._objects.Margin = new System.Windows.Forms.Padding(0);
            this._objects.MultiSelect = false;
            this._objects.Name = "_objects";
            this._objects.Size = new System.Drawing.Size(322, 556);
            this._objects.SmallImageList = this._imageList;
            this._objects.TabIndex = 3;
            this._objects.UseCompatibleStateImageBehavior = false;
            this._objects.View = System.Windows.Forms.View.Details;
            this._objects.SelectedIndexChanged += new System.EventHandler(this._objects_SelectedIndexChanged);
            // 
            // _imageList
            // 
            this._imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this._imageList.ImageSize = new System.Drawing.Size(16, 16);
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _objectTypes
            // 
            this._objectTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._objectTypes.FormattingEnabled = true;
            this._objectTypes.Location = new System.Drawing.Point(72, 4);
            this._objectTypes.Margin = new System.Windows.Forms.Padding(0);
            this._objectTypes.Name = "_objectTypes";
            this._objectTypes.Size = new System.Drawing.Size(254, 21);
            this._objectTypes.TabIndex = 0;
            this._objectTypes.SelectedValueChanged += new System.EventHandler(this._objectTypes_SelectedValueChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._objectTypes;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Object Type";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._objects;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Label:";
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(330, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(6, 624);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 1;
            this.expandableSplitter1.TabStop = false;
            // 
            // _right
            // 
            this._right.BackColor = System.Drawing.Color.White;
            this._right.Controls.Add(this._annotation);
            this._right.Dock = System.Windows.Forms.DockStyle.Fill;
            this._right.Location = new System.Drawing.Point(336, 0);
            this._right.Name = "_right";
            // 
            // 
            // 
            this._right.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._annotationLayoutControlItem});
            this._right.Size = new System.Drawing.Size(485, 624);
            this._right.TabIndex = 2;
            // 
            // _annotation
            // 
            this._annotation.Annotation = null;
            this._annotation.BackColor = System.Drawing.Color.White;
            this._annotation.Location = new System.Drawing.Point(4, 4);
            this._annotation.Margin = new System.Windows.Forms.Padding(0);
            this._annotation.Name = "_annotation";
            this._annotation.Size = new System.Drawing.Size(477, 616);
            this._annotation.TabIndex = 0;
            // 
            // _annotationLayoutControlItem
            // 
            this._annotationLayoutControlItem.Control = this._annotation;
            this._annotationLayoutControlItem.Height = 100;
            this._annotationLayoutControlItem.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this._annotationLayoutControlItem.MinSize = new System.Drawing.Size(64, 18);
            this._annotationLayoutControlItem.Name = "_annotationLayoutControlItem";
            this._annotationLayoutControlItem.Text = "Label:";
            this._annotationLayoutControlItem.TextVisible = false;
            this._annotationLayoutControlItem.Visible = false;
            this._annotationLayoutControlItem.Width = 101;
            this._annotationLayoutControlItem.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _properties
            // 
            this._properties.BackColor = System.Drawing.Color.White;
            this._properties.Dock = System.Windows.Forms.DockStyle.Right;
            this._properties.Item = null;
            this._properties.Location = new System.Drawing.Point(827, 0);
            this._properties.Name = "_properties";
            this._properties.ReadOnly = false;
            this._properties.Size = new System.Drawing.Size(309, 624);
            this._properties.TabIndex = 3;
            // 
            // expandableSplitter2
            // 
            this.expandableSplitter2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.expandableSplitter2.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter2.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter2.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter2.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter2.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter2.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter2.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter2.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter2.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter2.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter2.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter2.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter2.Location = new System.Drawing.Point(821, 0);
            this.expandableSplitter2.Name = "expandableSplitter2";
            this.expandableSplitter2.Size = new System.Drawing.Size(6, 624);
            this.expandableSplitter2.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter2.TabIndex = 4;
            this.expandableSplitter2.TabStop = false;
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.DelayTooltipHideDuration = 250;
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._superTooltip.MaximumWidth = 400;
            // 
            // _filterContainer
            // 
            this._filterContainer.Control = this._filter;
            this._filterContainer.Height = 28;
            this._filterContainer.MinSize = new System.Drawing.Size(120, 0);
            this._filterContainer.Name = "_filterContainer";
            this._filterContainer.Text = "Filter";
            this._filterContainer.Width = 99;
            this._filterContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _filter
            // 
            // 
            // 
            // 
            this._filter.Border.Class = "TextBoxBorder";
            this._filter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._filter.ButtonCustom.Symbol = "";
            this._filter.ButtonCustom.Visible = true;
            this._filter.Location = new System.Drawing.Point(72, 33);
            this._filter.Margin = new System.Windows.Forms.Padding(0);
            this._filter.Name = "_filter";
            this._filter.PreventEnterBeep = true;
            this._filter.Size = new System.Drawing.Size(171, 20);
            this._filter.TabIndex = 1;
            this._filter.ButtonCustomClick += new System.EventHandler(this._filter_ButtonCustomClick);
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
            // _apply
            // 
            this._apply.Location = new System.Drawing.Point(251, 33);
            this._apply.Margin = new System.Windows.Forms.Padding(0);
            this._apply.Name = "_apply";
            this._apply.Size = new System.Drawing.Size(75, 23);
            this._apply.TabIndex = 2;
            this._apply.Text = "Apply";
            this._apply.UseVisualStyleBackColor = true;
            this._apply.Click += new System.EventHandler(this._apply_Click);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._apply;
            this.layoutControlItem5.Height = 31;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 83;
            // 
            // ReviewNotesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._right);
            this.Controls.Add(this.expandableSplitter2);
            this.Controls.Add(this._properties);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this._left);
            this.Name = "ReviewNotesPanel";
            this.Size = new System.Drawing.Size(1136, 624);
            this._left.ResumeLayout(false);
            this._right.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl _left;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.Layout.LayoutControl _right;
        private System.Windows.Forms.ListView _objects;
        private System.Windows.Forms.ComboBox _objectTypes;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private Utilities.WinForms.ItemEditor _properties;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter2;
        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.ColumnHeader ObjectName;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private Quality.Annotations.AnnotationControl _annotation;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _annotationLayoutControlItem;
        private DevComponents.DotNetBar.Controls.TextBoxX _filter;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _filterContainer;
        private System.Windows.Forms.Button _apply;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
    }
}
