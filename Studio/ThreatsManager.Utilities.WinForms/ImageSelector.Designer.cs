namespace ThreatsManager.Utilities.WinForms
{
    partial class ImageSelector
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
            this.components = new System.ComponentModel.Container();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._bigImage = new System.Windows.Forms.PictureBox();
            this._image = new System.Windows.Forms.PictureBox();
            this._smallImage = new System.Windows.Forms.PictureBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this._openImage = new System.Windows.Forms.OpenFileDialog();
            this._changeBigImage = new System.Windows.Forms.Button();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._changeMediumImage = new System.Windows.Forms.Button();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._changeSmallImage = new System.Windows.Forms.Button();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bigImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._smallImage)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._changeSmallImage);
            this.layoutControl1.Controls.Add(this._changeMediumImage);
            this.layoutControl1.Controls.Add(this._changeBigImage);
            this.layoutControl1.Controls.Add(this._bigImage);
            this.layoutControl1.Controls.Add(this._image);
            this.layoutControl1.Controls.Add(this._smallImage);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9});
            this.layoutControl1.Size = new System.Drawing.Size(573, 102);
            this.layoutControl1.TabIndex = 0;
            // 
            // _bigImage
            // 
            this._bigImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._bigImage.Location = new System.Drawing.Point(62, 4);
            this._bigImage.Margin = new System.Windows.Forms.Padding(0);
            this._bigImage.MaximumSize = new System.Drawing.Size(64, 64);
            this._bigImage.MinimumSize = new System.Drawing.Size(64, 64);
            this._bigImage.Name = "_bigImage";
            this._bigImage.Size = new System.Drawing.Size(64, 64);
            this._bigImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._bigImage.TabIndex = 0;
            this._bigImage.TabStop = false;
            this._tooltip.SetToolTip(this._bigImage, "Double click to change");
            this._bigImage.DoubleClick += new System.EventHandler(this._bigImage_DoubleClick);
            // 
            // _image
            // 
            this._image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._image.Location = new System.Drawing.Point(251, 4);
            this._image.Margin = new System.Windows.Forms.Padding(0);
            this._image.MaximumSize = new System.Drawing.Size(32, 32);
            this._image.MinimumSize = new System.Drawing.Size(32, 32);
            this._image.Name = "_image";
            this._image.Size = new System.Drawing.Size(32, 32);
            this._image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._image.TabIndex = 1;
            this._image.TabStop = false;
            this._tooltip.SetToolTip(this._image, "Double click to change");
            this._image.DoubleClick += new System.EventHandler(this._image_DoubleClick);
            // 
            // _smallImage
            // 
            this._smallImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._smallImage.Location = new System.Drawing.Point(440, 4);
            this._smallImage.Margin = new System.Windows.Forms.Padding(0);
            this._smallImage.MaximumSize = new System.Drawing.Size(16, 16);
            this._smallImage.MinimumSize = new System.Drawing.Size(16, 16);
            this._smallImage.Name = "_smallImage";
            this._smallImage.Size = new System.Drawing.Size(16, 16);
            this._smallImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._smallImage.TabIndex = 2;
            this._smallImage.TabStop = false;
            this._tooltip.SetToolTip(this._smallImage, "Double click to change");
            this._smallImage.DoubleClick += new System.EventHandler(this._smallImage_DoubleClick);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._bigImage;
            this.layoutControlItem1.Height = 72;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Big Image\r\n(64x64)";
            this.layoutControlItem1.Width = 33;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._image;
            this.layoutControlItem2.Height = 40;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Image\r\n(32x32)";
            this.layoutControlItem2.Width = 33;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._smallImage;
            this.layoutControlItem6.Height = 24;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Image\r\n(16x16)";
            this.layoutControlItem6.Width = 33;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._bigImage;
            this.layoutControlItem3.Height = 58;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Big Image\r\n(64x64)";
            this.layoutControlItem3.Width = 33;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._image;
            this.layoutControlItem4.Height = 72;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(72, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Image\r\n(32x32)";
            this.layoutControlItem4.Width = 33;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._smallImage;
            this.layoutControlItem5.Height = 58;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Small Image\r\n(16x16)";
            this.layoutControlItem5.Width = 33;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _openImage
            // 
            this._openImage.Filter = "PNG file (*.png)|*.png|JPG file (*.jpg)|*.jpg|SVG file (*.svg)|*.svg";
            this._openImage.Title = "Select image";
            // 
            // _changeBigImage
            // 
            this._changeBigImage.Location = new System.Drawing.Point(4, 76);
            this._changeBigImage.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._changeBigImage.Name = "_changeBigImage";
            this._changeBigImage.Size = new System.Drawing.Size(181, 23);
            this._changeBigImage.TabIndex = 3;
            this._changeBigImage.Text = "Change Big Image";
            this._changeBigImage.UseVisualStyleBackColor = true;
            this._changeBigImage.Click += new System.EventHandler(this._changeBigImage_Click);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._changeBigImage;
            this.layoutControlItem7.Height = 31;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 33;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _changeMediumImage
            // 
            this._changeMediumImage.Location = new System.Drawing.Point(193, 76);
            this._changeMediumImage.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._changeMediumImage.Name = "_changeMediumImage";
            this._changeMediumImage.Size = new System.Drawing.Size(181, 23);
            this._changeMediumImage.TabIndex = 4;
            this._changeMediumImage.Text = "Change Medium Image";
            this._changeMediumImage.UseVisualStyleBackColor = true;
            this._changeMediumImage.Click += new System.EventHandler(this._changeMediumImage_Click);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._changeMediumImage;
            this.layoutControlItem8.Height = 31;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 33;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _changeSmallImage
            // 
            this._changeSmallImage.Location = new System.Drawing.Point(382, 76);
            this._changeSmallImage.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._changeSmallImage.Name = "_changeSmallImage";
            this._changeSmallImage.Size = new System.Drawing.Size(181, 23);
            this._changeSmallImage.TabIndex = 5;
            this._changeSmallImage.Text = "Change Small Image";
            this._changeSmallImage.UseVisualStyleBackColor = true;
            this._changeSmallImage.Click += new System.EventHandler(this._changeSmallImage_Click);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._changeSmallImage;
            this.layoutControlItem9.Height = 31;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 33;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ImageSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ImageSelector";
            this.Size = new System.Drawing.Size(573, 102);
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._bigImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._smallImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.PictureBox _bigImage;
        private System.Windows.Forms.PictureBox _image;
        private System.Windows.Forms.PictureBox _smallImage;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.ToolTip _tooltip;
        private System.Windows.Forms.OpenFileDialog _openImage;
        private System.Windows.Forms.Button _changeBigImage;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.Button _changeSmallImage;
        private System.Windows.Forms.Button _changeMediumImage;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
    }
}
