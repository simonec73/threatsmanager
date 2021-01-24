namespace ThreatsManager.Extensions.Dialogs
{
    partial class DiagramSelectionDialog
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
            this._diagramName = new System.Windows.Forms.TextBox();
            this._diagrams = new System.Windows.Forms.ComboBox();
            this._createNew = new System.Windows.Forms.RadioButton();
            this._assignExisting = new System.Windows.Forms.RadioButton();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._layout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 48);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(223, 13);
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
            this._ok.Location = new System.Drawing.Point(142, 13);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _layout
            // 
            this._layout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layout.Controls.Add(this._diagramName);
            this._layout.Controls.Add(this._diagrams);
            this._layout.Controls.Add(this._createNew);
            this._layout.Controls.Add(this._assignExisting);
            this._layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layout.ForeColor = System.Drawing.Color.Black;
            this._layout.Location = new System.Drawing.Point(0, 0);
            this._layout.Name = "_layout";
            // 
            // 
            // 
            this._layout.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem2,
            this.layoutControlItem4});
            this._layout.Size = new System.Drawing.Size(440, 140);
            this._layout.TabIndex = 0;
            // 
            // _diagramName
            // 
            this._diagramName.Location = new System.Drawing.Point(108, 83);
            this._diagramName.Margin = new System.Windows.Forms.Padding(0);
            this._diagramName.Name = "_diagramName";
            this._diagramName.Size = new System.Drawing.Size(328, 20);
            this._diagramName.TabIndex = 3;
            this._diagramName.TextChanged += new System.EventHandler(this._diagramName_TextChanged);
            // 
            // _diagrams
            // 
            this._diagrams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._diagrams.FormattingEnabled = true;
            this._diagrams.Location = new System.Drawing.Point(108, 29);
            this._diagrams.Margin = new System.Windows.Forms.Padding(0);
            this._diagrams.Name = "_diagrams";
            this._diagrams.Size = new System.Drawing.Size(328, 21);
            this._diagrams.TabIndex = 1;
            this._diagrams.SelectedIndexChanged += new System.EventHandler(this._diagrams_SelectedIndexChanged);
            // 
            // _createNew
            // 
            this._createNew.AutoSize = true;
            this._createNew.Location = new System.Drawing.Point(4, 58);
            this._createNew.Margin = new System.Windows.Forms.Padding(0);
            this._createNew.Name = "_createNew";
            this._createNew.Size = new System.Drawing.Size(432, 17);
            this._createNew.TabIndex = 2;
            this._createNew.TabStop = true;
            this._createNew.Text = "Create and Assign a new Diagram";
            this._createNew.UseVisualStyleBackColor = true;
            this._createNew.CheckedChanged += new System.EventHandler(this._createNew_CheckedChanged);
            // 
            // _assignExisting
            // 
            this._assignExisting.AutoSize = true;
            this._assignExisting.Location = new System.Drawing.Point(4, 4);
            this._assignExisting.Margin = new System.Windows.Forms.Padding(0);
            this._assignExisting.Name = "_assignExisting";
            this._assignExisting.Size = new System.Drawing.Size(432, 17);
            this._assignExisting.TabIndex = 0;
            this._assignExisting.TabStop = true;
            this._assignExisting.Text = "Assign Existing Diagram";
            this._assignExisting.UseVisualStyleBackColor = true;
            this._assignExisting.CheckedChanged += new System.EventHandler(this._assignExisting_CheckedChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._assignExisting;
            this.layoutControlItem1.Height = 25;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._diagrams;
            this.layoutControlItem3.Height = 29;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Diagram";
            this.layoutControlItem3.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._createNew;
            this.layoutControlItem2.Height = 25;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._diagramName;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Name";
            this.layoutControlItem4.TextPadding = new System.Windows.Forms.Padding(25, 4, 4, 4);
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // DiagramSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(440, 188);
            this.Controls.Add(this._layout);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "DiagramSelectionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign Diagram";
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
        private System.Windows.Forms.TextBox _diagramName;
        private System.Windows.Forms.ComboBox _diagrams;
        private System.Windows.Forms.RadioButton _createNew;
        private System.Windows.Forms.RadioButton _assignExisting;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
    }
}