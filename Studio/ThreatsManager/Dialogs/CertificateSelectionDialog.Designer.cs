namespace ThreatsManager.Dialogs
{
    partial class CertificateSelectionDialog
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
            DevComponents.DotNetBar.Layout.Background background1 = new DevComponents.DotNetBar.Layout.Background();
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._expiration = new System.Windows.Forms.Label();
            this._issuer = new System.Windows.Forms.Label();
            this._subject = new System.Windows.Forms.Label();
            this._fileName = new System.Windows.Forms.TextBox();
            this._file = new System.Windows.Forms.RadioButton();
            this._selectFromStore = new System.Windows.Forms.Button();
            this._storeLocation = new System.Windows.Forms.ComboBox();
            this._storeName = new System.Windows.Forms.ComboBox();
            this._store = new System.Windows.Forms.RadioButton();
            this._selectFromFile = new System.Windows.Forms.Button();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem1 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem4 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem2 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutSpacerItem3 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 203);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(288, 13);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 3;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(207, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 2;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._expiration);
            this._layout.Controls.Add(this._issuer);
            this._layout.Controls.Add(this._subject);
            this._layout.Controls.Add(this._fileName);
            this._layout.Controls.Add(this._file);
            this._layout.Controls.Add(this._selectFromStore);
            this._layout.Controls.Add(this._storeLocation);
            this._layout.Controls.Add(this._storeName);
            this._layout.Controls.Add(this._store);
            this._layout.Controls.Add(this._selectFromFile);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutSpacerItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutSpacerItem4,
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutGroup1});
            this._layout.Size = new System.Drawing.Size(570, 203);
            this._layout.TabIndex = 0;
            // 
            // _expiration
            // 
            this._expiration.AutoSize = true;
            this._expiration.Location = new System.Drawing.Point(93, 178);
            this._expiration.Margin = new System.Windows.Forms.Padding(0);
            this._expiration.Name = "_expiration";
            this._expiration.Size = new System.Drawing.Size(469, 13);
            this._expiration.TabIndex = 11;
            // 
            // _issuer
            // 
            this._issuer.AutoSize = true;
            this._issuer.Location = new System.Drawing.Point(93, 157);
            this._issuer.Margin = new System.Windows.Forms.Padding(0);
            this._issuer.Name = "_issuer";
            this._issuer.Size = new System.Drawing.Size(469, 13);
            this._issuer.TabIndex = 10;
            // 
            // _subject
            // 
            this._subject.AutoSize = true;
            this._subject.Location = new System.Drawing.Point(93, 136);
            this._subject.Margin = new System.Windows.Forms.Padding(0);
            this._subject.Name = "_subject";
            this._subject.Size = new System.Drawing.Size(469, 13);
            this._subject.TabIndex = 9;
            // 
            // _fileName
            // 
            this._fileName.Location = new System.Drawing.Point(100, 86);
            this._fileName.Margin = new System.Windows.Forms.Padding(0);
            this._fileName.Name = "_fileName";
            this._fileName.ReadOnly = true;
            this._fileName.Size = new System.Drawing.Size(373, 20);
            this._fileName.TabIndex = 7;
            // 
            // _file
            // 
            this._file.AutoSize = true;
            this._file.Location = new System.Drawing.Point(4, 61);
            this._file.Margin = new System.Windows.Forms.Padding(0);
            this._file.Name = "_file";
            this._file.Size = new System.Drawing.Size(562, 17);
            this._file.TabIndex = 5;
            this._file.Text = "From a signed file";
            this._file.UseVisualStyleBackColor = true;
            this._file.CheckedChanged += new System.EventHandler(this._file_CheckedChanged);
            // 
            // _selectFromStore
            // 
            this._selectFromStore.Location = new System.Drawing.Point(480, 29);
            this._selectFromStore.Margin = new System.Windows.Forms.Padding(0);
            this._selectFromStore.Name = "_selectFromStore";
            this._selectFromStore.Size = new System.Drawing.Size(75, 24);
            this._selectFromStore.TabIndex = 4;
            this._selectFromStore.Text = "Select";
            this._selectFromStore.UseVisualStyleBackColor = true;
            this._selectFromStore.Click += new System.EventHandler(this._selectFromStore_Click);
            // 
            // _storeLocation
            // 
            this._storeLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._storeLocation.FormattingEnabled = true;
            this._storeLocation.Location = new System.Drawing.Point(330, 29);
            this._storeLocation.Margin = new System.Windows.Forms.Padding(0);
            this._storeLocation.Name = "_storeLocation";
            this._storeLocation.Size = new System.Drawing.Size(142, 21);
            this._storeLocation.TabIndex = 3;
            this._storeLocation.SelectedIndexChanged += new System.EventHandler(this._storeLocation_SelectedIndexChanged);
            // 
            // _storeName
            // 
            this._storeName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._storeName.FormattingEnabled = true;
            this._storeName.Location = new System.Drawing.Point(100, 29);
            this._storeName.Margin = new System.Windows.Forms.Padding(0);
            this._storeName.Name = "_storeName";
            this._storeName.Size = new System.Drawing.Size(142, 21);
            this._storeName.TabIndex = 2;
            this._storeName.SelectedIndexChanged += new System.EventHandler(this._storeName_SelectedIndexChanged);
            // 
            // _store
            // 
            this._store.AutoSize = true;
            this._store.Checked = true;
            this._store.Location = new System.Drawing.Point(4, 4);
            this._store.Margin = new System.Windows.Forms.Padding(0);
            this._store.Name = "_store";
            this._store.Size = new System.Drawing.Size(562, 17);
            this._store.TabIndex = 0;
            this._store.TabStop = true;
            this._store.Text = "From a Certificate Store";
            this._store.UseVisualStyleBackColor = true;
            this._store.CheckedChanged += new System.EventHandler(this._store_CheckedChanged);
            // 
            // _selectFromFile
            // 
            this._selectFromFile.Location = new System.Drawing.Point(481, 86);
            this._selectFromFile.Margin = new System.Windows.Forms.Padding(0);
            this._selectFromFile.Name = "_selectFromFile";
            this._selectFromFile.Size = new System.Drawing.Size(75, 24);
            this._selectFromFile.TabIndex = 8;
            this._selectFromFile.Text = "Select";
            this._selectFromFile.UseVisualStyleBackColor = true;
            this._selectFromFile.Click += new System.EventHandler(this._selectFromFile_Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._store;
            this.layoutControlItem1.Height = 25;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem1
            // 
            this.layoutSpacerItem1.Height = 32;
            this.layoutSpacerItem1.Name = "layoutSpacerItem1";
            this.layoutSpacerItem1.Width = 16;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._storeName;
            this.layoutControlItem2.Height = 29;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Store Name";
            this.layoutControlItem2.Width = 49;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._storeLocation;
            this.layoutControlItem3.Height = 29;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Store Location";
            this.layoutControlItem3.Width = 49;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._selectFromStore;
            this.layoutControlItem4.Height = 31;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(83, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 83;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._file;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem4
            // 
            this.layoutSpacerItem4.Height = 32;
            this.layoutSpacerItem4.Name = "layoutSpacerItem4";
            this.layoutSpacerItem4.Width = 16;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._fileName;
            this.layoutControlItem6.Height = 28;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Signed File";
            this.layoutControlItem6.Width = 98;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._selectFromFile;
            this.layoutControlItem8.Height = 32;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 83;
            // 
            // layoutGroup1
            // 
            background1.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))))};
            this.layoutGroup1.CaptionStyle.Background = background1;
            this.layoutGroup1.CaptionStyle.TextColor = System.Drawing.Color.White;
            this.layoutGroup1.Height = 100;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem11});
            this.layoutGroup1.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Text = "Selected Certificate";
            this.layoutGroup1.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 100;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._subject;
            this.layoutControlItem9.Height = 21;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "Subject Name";
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._issuer;
            this.layoutControlItem10.Height = 21;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "Issuing CA";
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._expiration;
            this.layoutControlItem11.Height = 21;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Expiration Date";
            this.layoutControlItem11.Width = 100;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem2
            // 
            this.layoutSpacerItem2.Height = 32;
            this.layoutSpacerItem2.Name = "layoutSpacerItem2";
            this.layoutSpacerItem2.Width = 16;
            // 
            // layoutSpacerItem3
            // 
            this.layoutSpacerItem3.Height = 32;
            this.layoutSpacerItem3.Name = "layoutSpacerItem3";
            this.layoutSpacerItem3.Width = 16;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._selectFromFile;
            this.layoutControlItem7.Height = 31;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(83, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 83;
            // 
            // CertificateSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(570, 251);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificateSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a Certificate to be Trusted";
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.RadioButton _file;
        private System.Windows.Forms.Button _selectFromStore;
        private System.Windows.Forms.ComboBox _storeLocation;
        private System.Windows.Forms.ComboBox _storeName;
        private System.Windows.Forms.RadioButton _store;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem1;
        private System.Windows.Forms.TextBox _fileName;
        private System.Windows.Forms.Button _selectFromFile;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem2;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private System.Windows.Forms.Label _issuer;
        private System.Windows.Forms.Label _subject;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private System.Windows.Forms.Label _expiration;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
    }
}