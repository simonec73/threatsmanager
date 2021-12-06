namespace ThreatsManager.Extensions.Panels.Word
{
    partial class WordReportingPanel
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
            _reportGenerator.ProgressUpdated -= ReportGeneratorOnProgressUpdated;
            _reportGenerator.ShowMessage -= ReportGeneratorOnShowMessage;
            _reportGenerator.ShowWarning -= ReportGeneratorOnShowWarning;

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
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._browse = new System.Windows.Forms.Button();
            this._wordFile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem16 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this._openFile = new System.Windows.Forms.OpenFileDialog();
            this.metroTileItem1 = new DevComponents.DotNetBar.Metro.MetroTileItem();
            this._docStructure = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Height = 150;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Processes";
            this.layoutControlItem6.TextPadding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._browse);
            this.layoutControl1.Controls.Add(this._wordFile);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem4,
            this.layoutControlItem3});
            this.layoutControl1.Size = new System.Drawing.Size(922, 32);
            this.layoutControl1.TabIndex = 0;
            // 
            // _browse
            // 
            this._browse.Location = new System.Drawing.Point(843, 4);
            this._browse.Margin = new System.Windows.Forms.Padding(0);
            this._browse.Name = "_browse";
            this._browse.Size = new System.Drawing.Size(75, 23);
            this._browse.TabIndex = 1;
            this._browse.Text = "Browse...";
            this._browse.UseVisualStyleBackColor = true;
            this._browse.Click += new System.EventHandler(this._browse_Click);
            // 
            // _wordFile
            // 
            this._wordFile.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._wordFile.Border.Class = "TextBoxBorder";
            this._wordFile.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._wordFile.ButtonCustom.Symbol = "";
            this._wordFile.DisabledBackColor = System.Drawing.Color.White;
            this._wordFile.ForeColor = System.Drawing.Color.Black;
            this._wordFile.Location = new System.Drawing.Point(115, 6);
            this._wordFile.Margin = new System.Windows.Forms.Padding(0);
            this._wordFile.Name = "_wordFile";
            this._wordFile.PreventEnterBeep = true;
            this._wordFile.ReadOnly = true;
            this._wordFile.Size = new System.Drawing.Size(720, 20);
            this._wordFile.TabIndex = 0;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._wordFile;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.layoutControlItem4.Text = "Reference Word File";
            this.layoutControlItem4.Width = 99;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._browse;
            this.layoutControlItem3.Height = 31;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 83;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Height = 31;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Width = 83;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Height = 31;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Width = 83;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Height = 31;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Width = 83;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Height = 31;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Width = 83;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Height = 31;
            this.layoutControlItem15.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Width = 83;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Height = 31;
            this.layoutControlItem16.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Width = 83;
            // 
            // buttonItem1
            // 
            this.buttonItem1.GlobalItem = false;
            this.buttonItem1.Name = "buttonItem1";
            // 
            // _openFile
            // 
            this._openFile.DefaultExt = "docx";
            this._openFile.Filter = "Word documents (*.docx)|*.docx|All files (*.*)|*.*";
            this._openFile.Title = "Select the Word document";
            this._openFile.RestoreDirectory = true;
            // 
            // metroTileItem1
            // 
            this.metroTileItem1.Name = "metroTileItem1";
            this.metroTileItem1.SymbolColor = System.Drawing.Color.Empty;
            this.metroTileItem1.TileColor = DevComponents.DotNetBar.Metro.eMetroTileColor.Default;
            // 
            // 
            // 
            this.metroTileItem1.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // _docStructure
            // 
            this._docStructure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._docStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this._docStructure.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._docStructure.ForeColor = System.Drawing.Color.Black;
            this._docStructure.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._docStructure.Location = new System.Drawing.Point(0, 32);
            this._docStructure.Name = "_docStructure";
            this._docStructure.Size = new System.Drawing.Size(922, 654);
            this._docStructure.TabIndex = 1;
            this._docStructure.Text = "superGridControl1";
            this._docStructure.AfterCheck += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridAfterCheckEventArgs>(this._docStructure_AfterCheck);
            // 
            // WordReportingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._docStructure);
            this.Controls.Add(this.layoutControl1);
            this.Name = "WordReportingPanel";
            this.Size = new System.Drawing.Size(922, 686);
            this.layoutControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private DevComponents.DotNetBar.Controls.TextBoxX _wordFile;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.OpenFileDialog _openFile;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.Metro.MetroTileItem metroTileItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem16;
        private System.Windows.Forms.Button _browse;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _docStructure;
    }
}
