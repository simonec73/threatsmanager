using ThreatsManager.Utilities.WinForms.Rules;

namespace ThreatsManager.AutoThreatGeneration.Dialogs
{
    partial class RuleFilterDialog
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
            ThreatsManager.AutoGenRules.Engine.SelectionRule selectionRule1 = new ThreatsManager.AutoGenRules.Engine.SelectionRule();
            this._ok = new DevComponents.DotNetBar.ButtonX();
            this._cancel = new DevComponents.DotNetBar.ButtonX();
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._ruleEditor = new ThreatsManager.Utilities.WinForms.Rules.RuleEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._container.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _ok
            // 
            this._ok.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this._ok.Location = new System.Drawing.Point(401, 6);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ok.TabIndex = 4;
            this._ok.Text = "OK";
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _cancel
            // 
            this._cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.CausesValidation = false;
            this._cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(482, 6);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._cancel.TabIndex = 5;
            this._cancel.Text = "Cancel";
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._container.Controls.Add(this._ruleEditor);
            this._container.Controls.Add(this.panel1);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.ForeColor = System.Drawing.Color.Black;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem4});
            this._container.Size = new System.Drawing.Size(967, 598);
            this._container.TabIndex = 7;
            // 
            // _ruleEditor
            // 
            this._ruleEditor.Location = new System.Drawing.Point(4, 4);
            this._ruleEditor.Margin = new System.Windows.Forms.Padding(0);
            this._ruleEditor.Name = "_ruleEditor";
            selectionRule1.Root = null;
            this._ruleEditor.Rule = selectionRule1;
            this._ruleEditor.Size = new System.Drawing.Size(959, 544);
            this._ruleEditor.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._ok);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Location = new System.Drawing.Point(4, 556);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(959, 32);
            this.panel1.TabIndex = 1;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._ruleEditor;
            this.layoutControlItem2.Height = 99;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Label:";
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.panel1;
            this.layoutControlItem4.Height = 40;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Label:";
            this.layoutControlItem4.TextVisible = false;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // RuleFilterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(967, 598);
            this.ControlBox = false;
            this.Controls.Add(this._container);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuleFilterDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Rule Filter";
            this._container.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.ButtonX _ok;
        private DevComponents.DotNetBar.ButtonX _cancel;
        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private RuleEditor _ruleEditor;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
    }
}