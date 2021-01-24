
namespace ThreatsManager.DevOps.Dialogs
{
    partial class DevOpsWorkItemAssociationDialog
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
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._objects = new System.Windows.Forms.ComboBox();
            this._objectType = new System.Windows.Forms.Label();
            this._auto = new System.Windows.Forms.Button();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._ok);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 46);
            this.panel1.TabIndex = 1;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.Enabled = false;
            this._ok.Location = new System.Drawing.Point(180, 11);
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
            this._cancel.Location = new System.Drawing.Point(261, 11);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _container
            // 
            this._container.Controls.Add(this._objects);
            this._container.Controls.Add(this._objectType);
            this._container.Controls.Add(this._auto);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem2});
            this._container.Size = new System.Drawing.Size(517, 74);
            this._container.TabIndex = 2;
            // 
            // _objects
            // 
            this._objects.FormattingEnabled = true;
            this._objects.Location = new System.Drawing.Point(152, 4);
            this._objects.Margin = new System.Windows.Forms.Padding(0);
            this._objects.Name = "_objects";
            this._objects.Size = new System.Drawing.Size(278, 21);
            this._objects.TabIndex = 0;
            this._objects.SelectedIndexChanged += new System.EventHandler(this._objects_SelectedIndexChanged);
            this._objects.TextUpdate += new System.EventHandler(this._objects_TextUpdate);
            this._objects.KeyDown += new System.Windows.Forms.KeyEventHandler(this._objects_KeyDown);
            // 
            // _objectType
            // 
            this._objectType.AutoSize = true;
            this._objectType.Location = new System.Drawing.Point(152, 35);
            this._objectType.Margin = new System.Windows.Forms.Padding(0);
            this._objectType.Name = "_objectType";
            this._objectType.Size = new System.Drawing.Size(361, 13);
            this._objectType.TabIndex = 2;
            // 
            // _auto
            // 
            this._auto.Location = new System.Drawing.Point(438, 4);
            this._auto.Margin = new System.Windows.Forms.Padding(0);
            this._auto.Name = "_auto";
            this._auto.Size = new System.Drawing.Size(75, 23);
            this._auto.TabIndex = 1;
            this._auto.Text = "Auto";
            this._auto.UseVisualStyleBackColor = true;
            this._auto.Click += new System.EventHandler(this._auto_Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._objects;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Selected DevOps Item";
            this.layoutControlItem1.Width = 99;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._auto;
            this.layoutControlItem3.Height = 31;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 83;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._objectType;
            this.layoutControlItem2.Height = 21;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Selected DevOps Item Type";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // DevOpsObjectAssociationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(517, 120);
            this.Controls.Add(this._container);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevOpsObjectAssociationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select associated Work Item";
            this.panel1.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this._container.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Button _cancel;
        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private System.Windows.Forms.ComboBox _objects;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.Label _objectType;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button _auto;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
    }
}