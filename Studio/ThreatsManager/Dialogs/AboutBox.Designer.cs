namespace ThreatsManager.Dialogs
{
    partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._links = new DevComponents.DotNetBar.LabelX();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this._moreInformation = new DevComponents.DotNetBar.LabelX();
            this.okButton = new System.Windows.Forms.Button();
            this._thirdParty = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.Controls.Add(this._links, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 1);
            this.tableLayoutPanel.Controls.Add(this._moreInformation, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.okButton, 2, 6);
            this.tableLayoutPanel.Controls.Add(this._thirdParty, 1, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(14, 14);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(912, 359);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // _links
            // 
            // 
            // 
            // 
            this._links.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tableLayoutPanel.SetColumnSpan(this._links, 2);
            this._links.Dock = System.Windows.Forms.DockStyle.Fill;
            this._links.Location = new System.Drawing.Point(410, 51);
            this._links.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this._links.MaximumSize = new System.Drawing.Size(0, 28);
            this._links.Name = "_links";
            this._links.PaddingLeft = 3;
            this._links.Size = new System.Drawing.Size(498, 17);
            this._links.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this._links.TabIndex = 29;
            this._links.Text = "Some resources to learn more on <a href=\"https://threatsmanager.com\">Threats Mana" +
    "ger Studio</a> and <a href=\"https://simoneonsecurity.com\">Threat Modeling</a>.";
            this._links.MarkupLinkClick += new DevComponents.DotNetBar.MarkupLinkClickEventHandler(this._links_MarkupLinkClick);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Image = global::ThreatsManager.Properties.Resources.ThreatModeling;
            this.logoPictureBox.Location = new System.Drawing.Point(4, 4);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.logoPictureBox.Name = "logoPictureBox";
            this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 7);
            this.logoPictureBox.Size = new System.Drawing.Size(392, 351);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            // 
            // labelProductName
            // 
            this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductName.Location = new System.Drawing.Point(410, 0);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.labelProductName.MaximumSize = new System.Drawing.Size(0, 28);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(327, 17);
            this.labelProductName.TabIndex = 19;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVersion
            // 
            this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVersion.Location = new System.Drawing.Point(751, 0);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.labelVersion.MaximumSize = new System.Drawing.Size(0, 28);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(157, 17);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCopyright
            // 
            this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCopyright.Location = new System.Drawing.Point(410, 17);
            this.labelCopyright.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 28);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(327, 17);
            this.labelCopyright.TabIndex = 21;
            this.labelCopyright.Text = "Copyright";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _moreInformation
            // 
            // 
            // 
            // 
            this._moreInformation.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tableLayoutPanel.SetColumnSpan(this._moreInformation, 2);
            this._moreInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this._moreInformation.Location = new System.Drawing.Point(410, 34);
            this._moreInformation.Margin = new System.Windows.Forms.Padding(10, 0, 4, 0);
            this._moreInformation.MaximumSize = new System.Drawing.Size(0, 28);
            this._moreInformation.Name = "_moreInformation";
            this._moreInformation.PaddingLeft = 3;
            this._moreInformation.Size = new System.Drawing.Size(498, 17);
            this._moreInformation.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this._moreInformation.TabIndex = 25;
            this._moreInformation.Text = "Based on <a>Threats Manager Platform SDK</a> v{ENGINE_VERSION}, available under M" +
    "IT license.";
            this._moreInformation.MarkupLinkClick += new DevComponents.DotNetBar.MarkupLinkClickEventHandler(this._moreInformation_MarkupLinkClick);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(788, 328);
            this.okButton.Margin = new System.Windows.Forms.Padding(4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(120, 27);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "&OK";
            // 
            // _thirdParty
            // 
            // 
            // 
            // 
            this._thirdParty.BackgroundStyle.Class = "RichTextBoxBorder";
            this._thirdParty.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tableLayoutPanel.SetColumnSpan(this._thirdParty, 2);
            this._thirdParty.Dock = System.Windows.Forms.DockStyle.Fill;
            this._thirdParty.Location = new System.Drawing.Point(406, 74);
            this._thirdParty.Margin = new System.Windows.Forms.Padding(6);
            this._thirdParty.Name = "_thirdParty";
            this._thirdParty.ReadOnly = true;
            this.tableLayoutPanel.SetRowSpan(this._thirdParty, 2);
            this._thirdParty.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1040{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.22621}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this._thirdParty.Size = new System.Drawing.Size(500, 238);
            this._thirdParty.TabIndex = 27;
            this._thirdParty.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._thirdParty_LinkClicked);
            // 
            // AboutBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(940, 387);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(14);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About...";
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Button okButton;
        private DevComponents.DotNetBar.LabelX _moreInformation;
        private DevComponents.DotNetBar.Controls.RichTextBoxEx _thirdParty;
        private DevComponents.DotNetBar.LabelX _links;
    }
}
