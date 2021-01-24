namespace ThreatsManager.Dialogs
{
    partial class StatusBarItemSelectionDialog
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
            this._remove = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._description = new System.Windows.Forms.Label();
            this._providers = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._remove);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 137);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _remove
            // 
            this._remove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._remove.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this._remove.Enabled = false;
            this._remove.Location = new System.Drawing.Point(183, 13);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(75, 23);
            this._remove.TabIndex = 1;
            this._remove.Text = "Remove";
            this._remove.UseVisualStyleBackColor = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(264, 13);
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
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(102, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._description);
            this._layout.Controls.Add(this._providers);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this._layout.Size = new System.Drawing.Size(440, 137);
            this._layout.TabIndex = 0;
            // 
            // _description
            // 
            this._description.AutoSize = true;
            this._description.Location = new System.Drawing.Point(68, 33);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(368, 98);
            this._description.TabIndex = 1;
            // 
            // _providers
            // 
            this._providers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._providers.FormattingEnabled = true;
            this._providers.Location = new System.Drawing.Point(68, 4);
            this._providers.Margin = new System.Windows.Forms.Padding(0);
            this._providers.Name = "_providers";
            this._providers.Size = new System.Drawing.Size(368, 21);
            this._providers.TabIndex = 0;
            this._providers.SelectedIndexChanged += new System.EventHandler(this._providers_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._providers;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Provider";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._description;
            this.layoutControlItem2.Height = 99;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Description";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // StatusBarItemSelectionDialog
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(440, 185);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "StatusBarItemSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select the Status Info Provider";
            this.panel1.ResumeLayout(false);
            this._layout.ResumeLayout(false);
            this._layout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl _layout;
        private System.Windows.Forms.ComboBox _providers;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.Label _description;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button _remove;
    }
}