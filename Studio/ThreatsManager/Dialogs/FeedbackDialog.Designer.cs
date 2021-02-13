namespace ThreatsManager.Dialogs
{
    partial class FeedbackDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedbackDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this._publishAuthorization = new System.Windows.Forms.CheckBox();
            this._email = new System.Windows.Forms.TextBox();
            this._comments = new System.Windows.Forms.TextBox();
            this._frown = new System.Windows.Forms.RadioButton();
            this._smile = new System.Windows.Forms.RadioButton();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.panel1.Size = new System.Drawing.Size(496, 50);
            this.panel1.TabIndex = 0;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(251, 15);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(170, 15);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this.labelX1);
            this.layoutControl1.Controls.Add(this._publishAuthorization);
            this.layoutControl1.Controls.Add(this._email);
            this.layoutControl1.Controls.Add(this._comments);
            this.layoutControl1.Controls.Add(this._frown);
            this.layoutControl1.Controls.Add(this._smile);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControl1.Size = new System.Drawing.Size(496, 357);
            this.layoutControl1.TabIndex = 1;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(4, 261);
            this.labelX1.Margin = new System.Windows.Forms.Padding(0);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(488, 92);
            this.labelX1.TabIndex = 5;
            this.labelX1.Text = resources.GetString("labelX1.Text");
            // 
            // _publishAuthorization
            // 
            this._publishAuthorization.AutoSize = true;
            this._publishAuthorization.Location = new System.Drawing.Point(4, 236);
            this._publishAuthorization.Margin = new System.Windows.Forms.Padding(0);
            this._publishAuthorization.Name = "_publishAuthorization";
            this._publishAuthorization.Size = new System.Drawing.Size(488, 17);
            this._publishAuthorization.TabIndex = 4;
            this._publishAuthorization.Text = "Do you authorize to publish your comments? Your email address will not be publish" +
    "ed.";
            this._publishAuthorization.UseVisualStyleBackColor = true;
            // 
            // _email
            // 
            this._email.Location = new System.Drawing.Point(154, 200);
            this._email.Margin = new System.Windows.Forms.Padding(0);
            this._email.Name = "_email";
            this._email.Size = new System.Drawing.Size(338, 20);
            this._email.TabIndex = 3;
            // 
            // _comments
            // 
            this._comments.Location = new System.Drawing.Point(154, 52);
            this._comments.Margin = new System.Windows.Forms.Padding(0);
            this._comments.Multiline = true;
            this._comments.Name = "_comments";
            this._comments.Size = new System.Drawing.Size(338, 140);
            this._comments.TabIndex = 2;
            // 
            // _frown
            // 
            this._frown.Appearance = System.Windows.Forms.Appearance.Button;
            this._frown.AutoSize = true;
            this._frown.Image = global::ThreatsManager.Properties.Resources.emoticon_frown;
            this._frown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._frown.Location = new System.Drawing.Point(252, 4);
            this._frown.Margin = new System.Windows.Forms.Padding(0);
            this._frown.Name = "_frown";
            this._frown.Size = new System.Drawing.Size(240, 40);
            this._frown.TabIndex = 1;
            this._frown.TabStop = true;
            this._frown.Text = "Send a Frown";
            this._frown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._frown.UseVisualStyleBackColor = true;
            // 
            // _smile
            // 
            this._smile.Appearance = System.Windows.Forms.Appearance.Button;
            this._smile.AutoSize = true;
            this._smile.Image = global::ThreatsManager.Properties.Resources.emoticon_smile;
            this._smile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._smile.Location = new System.Drawing.Point(4, 4);
            this._smile.Margin = new System.Windows.Forms.Padding(0);
            this._smile.Name = "_smile";
            this._smile.Size = new System.Drawing.Size(240, 40);
            this._smile.TabIndex = 0;
            this._smile.TabStop = true;
            this._smile.Text = "Send a Smile";
            this._smile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._smile.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._smile;
            this.layoutControlItem1.Height = 48;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 50;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._frown;
            this.layoutControlItem2.Height = 48;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 50;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._comments;
            this.layoutControlItem3.Height = 100;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Comments";
            this.layoutControlItem3.Width = 101;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._email;
            this.layoutControlItem4.Height = 36;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Email address\r\n(only to ask for clarifications)";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._publishAuthorization;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.labelX1;
            this.layoutControlItem6.Height = 100;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Label:";
            this.layoutControlItem6.TextVisible = false;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // FeedbackDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(496, 407);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(512, 400);
            this.Name = "FeedbackDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please share your feedback";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.RadioButton _frown;
        private System.Windows.Forms.RadioButton _smile;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.CheckBox _publishAuthorization;
        private System.Windows.Forms.TextBox _email;
        private System.Windows.Forms.TextBox _comments;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
    }
}