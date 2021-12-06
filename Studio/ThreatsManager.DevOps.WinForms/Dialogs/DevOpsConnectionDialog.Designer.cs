namespace ThreatsManager.DevOps.Dialogs
{
    partial class DevOpsConnectionDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._ok = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._loadProjects = new System.Windows.Forms.Button();
            this._projectList = new System.Windows.Forms.ListBox();
            this._serverUrl = new System.Windows.Forms.TextBox();
            this._factories = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._accessToken = new System.Windows.Forms.TextBox();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._ok);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 366);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(438, 46);
            this.panel1.TabIndex = 0;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(141, 11);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 1;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(222, 11);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Close";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._accessToken);
            this._layout.Controls.Add(this._loadProjects);
            this._layout.Controls.Add(this._projectList);
            this._layout.Controls.Add(this._serverUrl);
            this._layout.Controls.Add(this._factories);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem5,
            this.layoutControlItem4,
            this.layoutControlItem3});
            this._layout.Size = new System.Drawing.Size(438, 366);
            this._layout.TabIndex = 1;
            // 
            // _loadProjects
            // 
            this._loadProjects.Enabled = false;
            this._loadProjects.Location = new System.Drawing.Point(359, 61);
            this._loadProjects.Margin = new System.Windows.Forms.Padding(0);
            this._loadProjects.Name = "_loadProjects";
            this._loadProjects.Size = new System.Drawing.Size(75, 23);
            this._loadProjects.TabIndex = 3;
            this._loadProjects.Text = "Load";
            this._loadProjects.UseVisualStyleBackColor = true;
            this._loadProjects.Click += new System.EventHandler(this._loadProjects_Click);
            // 
            // _projectList
            // 
            this._projectList.FormattingEnabled = true;
            this._projectList.Location = new System.Drawing.Point(4, 109);
            this._projectList.Margin = new System.Windows.Forms.Padding(0);
            this._projectList.Name = "_projectList";
            this._projectList.Size = new System.Drawing.Size(430, 251);
            this._projectList.TabIndex = 4;
            this._projectList.SelectedIndexChanged += new System.EventHandler(this._projectList_SelectedIndexChanged);
            // 
            // _serverUrl
            // 
            this._serverUrl.Location = new System.Drawing.Point(107, 33);
            this._serverUrl.Margin = new System.Windows.Forms.Padding(0);
            this._serverUrl.Name = "_serverUrl";
            this._serverUrl.Size = new System.Drawing.Size(327, 20);
            this._serverUrl.TabIndex = 1;
            this._serverUrl.TextChanged += new System.EventHandler(this._serverUrl_TextChanged);
            // 
            // _factories
            // 
            this._factories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._factories.FormattingEnabled = true;
            this._factories.Location = new System.Drawing.Point(107, 4);
            this._factories.Margin = new System.Windows.Forms.Padding(0);
            this._factories.Name = "_factories";
            this._factories.Size = new System.Drawing.Size(327, 21);
            this._factories.TabIndex = 0;
            this._factories.SelectedIndexChanged += new System.EventHandler(this._connectors_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._factories;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "DevOps Connector";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._serverUrl;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Server URL";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._loadProjects;
            this.layoutControlItem4.Height = 31;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 83;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._projectList;
            this.layoutControlItem3.Height = 100;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Projects";
            this.layoutControlItem3.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _accessToken
            // 
            this._accessToken.Location = new System.Drawing.Point(107, 61);
            this._accessToken.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._accessToken.Name = "_accessToken";
            this._accessToken.Size = new System.Drawing.Size(244, 20);
            this._accessToken.TabIndex = 2;
            this._accessToken.UseSystemPasswordChar = true;
            this._accessToken.TextChanged += new System.EventHandler(this._accessToken_TextChanged);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._accessToken;
            this.layoutControlItem5.Height = 28;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Access Token";
            this.layoutControlItem5.Width = 99;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // DevOpsConnectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(438, 412);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "DevOpsConnectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to DevOps";
            this.Load += new System.EventHandler(this.DevOpsConnectionDialog_Load);
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.ComboBox _factories;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.TextBox _serverUrl;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.ListBox _projectList;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Button _loadProjects;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.TextBox _accessToken;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
    }
}