namespace ThreatsManager.Quality.Dialogs
{
    partial class FalsePositiveReasonDialog
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
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._analyzerName = new System.Windows.Forms.Label();
            this._reason = new Keyoti.RapidSpell.AYTRichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._finding = new System.Windows.Forms.Label();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 202);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 46);
            this.panel1.TabIndex = 0;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(240, 11);
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
            this._ok.Location = new System.Drawing.Point(159, 11);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._analyzerName);
            this.layoutControl1.Controls.Add(this._reason);
            this.layoutControl1.Controls.Add(this.label2);
            this.layoutControl1.Controls.Add(this._finding);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem1,
            this.layoutControlItem3});
            this.layoutControl1.Size = new System.Drawing.Size(475, 202);
            this.layoutControl1.TabIndex = 1;
            // 
            // _analyzerName
            // 
            this._analyzerName.AutoSize = true;
            this._analyzerName.Location = new System.Drawing.Point(55, 25);
            this._analyzerName.Margin = new System.Windows.Forms.Padding(0);
            this._analyzerName.Name = "_analyzerName";
            this._analyzerName.Size = new System.Drawing.Size(416, 13);
            this._analyzerName.TabIndex = 1;
            this._analyzerName.Text = "label1";
            // 
            // _reason
            // 
            this._reason.ContextMenuDefault = null;
            this._reason.Location = new System.Drawing.Point(55, 67);
            this._reason.Margin = new System.Windows.Forms.Padding(0);
            this._reason.Name = "_reason";
            this._reason.ShowCutCopyPasteContextMenu = false;
            this._reason.ShowCutCopyPasteContextMenuStrip = true;
            this._reason.Size = new System.Drawing.Size(416, 131);
            this._reason.TabIndex = 3;
            this._reason.Text = "";
            this._reason.UnderlineYOffset = 0;
            this._reason.UseAccurateBaselineOffsetMethod = false;
            this._reason.UseUndoCompatibleTextAccess = false;
            this._reason.TextChanged += new System.EventHandler(this._reason_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(467, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Please specify the reason why the selected finding must be considered a False Pos" +
    "itive.";
            // 
            // _finding
            // 
            this._finding.AutoSize = true;
            this._finding.Location = new System.Drawing.Point(55, 46);
            this._finding.Margin = new System.Windows.Forms.Padding(0);
            this._finding.Name = "_finding";
            this._finding.Size = new System.Drawing.Size(416, 13);
            this._finding.TabIndex = 2;
            this._finding.Text = "label1";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.label2;
            this.layoutControlItem2.Height = 21;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Label:";
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._analyzerName;
            this.layoutControlItem4.Height = 21;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Analyzer";
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._finding;
            this.layoutControlItem1.Height = 21;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Finding";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._reason;
            this.layoutControlItem3.Height = 100;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Reason";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // FalsePositiveReasonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(475, 248);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FalsePositiveReasonDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "False Positive";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.Label _analyzerName;
        private Keyoti.RapidSpell.AYTRichTextBox _reason;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label _finding;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
    }
}