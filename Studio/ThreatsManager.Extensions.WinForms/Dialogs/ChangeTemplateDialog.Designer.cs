namespace ThreatsManager.Extensions.Dialogs
{
    partial class ChangeTemplateDialog
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
            this._cancel = new System.Windows.Forms.Button();
            this._ok = new System.Windows.Forms.Button();
            this._layout = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._templates = new ThreatsManager.Extensions.Dialogs.ImagedComboBox();
            this._currentTemplate = new DevComponents.DotNetBar.LabelX();
            this._itemType = new System.Windows.Forms.Label();
            this._affectedItem = new DevComponents.DotNetBar.LabelX();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(557, 63);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(284, 25);
            this._cancel.Margin = new System.Windows.Forms.Padding(6);
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
            this._ok.Location = new System.Drawing.Point(197, 25);
            this._ok.Margin = new System.Windows.Forms.Padding(6);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._templates);
            this._layout.Controls.Add(this._currentTemplate);
            this._layout.Controls.Add(this._itemType);
            this._layout.Controls.Add(this._affectedItem);
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
            this.layoutControlItem3,
            this.layoutControlItem4});
            this._layout.Size = new System.Drawing.Size(557, 92);
            this._layout.TabIndex = 3;
            // 
            // _templates
            // 
            this._templates.DisplayMember = "Text";
            this._templates.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._templates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._templates.ForeColor = System.Drawing.Color.Black;
            this._templates.FormattingEnabled = true;
            this._templates.ItemHeight = 16;
            this._templates.Location = new System.Drawing.Point(113, 48);
            this._templates.Margin = new System.Windows.Forms.Padding(0);
            this._templates.Name = "_templates";
            this._templates.Size = new System.Drawing.Size(440, 22);
            this._templates.TabIndex = 3;
            this._templates.SelectedIndexChanged += new System.EventHandler(this._templates_SelectedIndexChanged);
            // 
            // _currentTemplate
            // 
            this._currentTemplate.AutoSize = true;
            // 
            // 
            // 
            this._currentTemplate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._currentTemplate.Location = new System.Drawing.Point(391, 27);
            this._currentTemplate.Margin = new System.Windows.Forms.Padding(0);
            this._currentTemplate.Name = "_currentTemplate";
            this._currentTemplate.Size = new System.Drawing.Size(32, 15);
            this._currentTemplate.TabIndex = 2;
            this._currentTemplate.Text = "label1";
            // 
            // _itemType
            // 
            this._itemType.AutoSize = true;
            this._itemType.Location = new System.Drawing.Point(113, 27);
            this._itemType.Margin = new System.Windows.Forms.Padding(0);
            this._itemType.Name = "_itemType";
            this._itemType.Size = new System.Drawing.Size(161, 13);
            this._itemType.TabIndex = 1;
            this._itemType.Text = "label1";
            // 
            // _affectedItem
            // 
            this._affectedItem.AutoSize = true;
            // 
            // 
            // 
            this._affectedItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._affectedItem.Location = new System.Drawing.Point(113, 4);
            this._affectedItem.Margin = new System.Windows.Forms.Padding(0);
            this._affectedItem.Name = "_affectedItem";
            this._affectedItem.Size = new System.Drawing.Size(32, 15);
            this._affectedItem.TabIndex = 0;
            this._affectedItem.Text = "label1";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._affectedItem;
            this.layoutControlItem1.Height = 23;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Affected Item";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._itemType;
            this.layoutControlItem2.Height = 21;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Item Type";
            this.layoutControlItem2.Width = 50;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._currentTemplate;
            this.layoutControlItem3.Height = 21;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Current Template";
            this.layoutControlItem3.Width = 50;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._templates;
            this.layoutControlItem4.Height = 29;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Available Templates";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this._affectedItem;
            this.layoutControlItem13.Height = 25;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "Associated To";
            this.layoutControlItem13.Width = 100;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ChangeTemplateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(557, 155);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ChangeTemplateDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Template";
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
        private ImagedComboBox _templates;
        private DevComponents.DotNetBar.LabelX _currentTemplate;
        private System.Windows.Forms.Label _itemType;
        private DevComponents.DotNetBar.LabelX _affectedItem;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
    }
}