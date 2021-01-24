using DevComponents.DotNetBar;
using ThreatsManager.Utilities.WinForms.Rules;

namespace ThreatsManager.AutoThreatGeneration.Dialogs
{
    partial class MitigationRuleEditDialog
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
            this._threatType = new System.Windows.Forms.Label();
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._ruleEditor = new ThreatsManager.Utilities.WinForms.Rules.RuleEditor();
            this._strengthOverride = new System.Windows.Forms.ComboBox();
            this._severityOverride = new System.Windows.Forms.ComboBox();
            this._mitigation = new System.Windows.Forms.Label();
            this._modelSelectedLabel = new System.Windows.Forms.Label();
            this._selectedDataStores = new DevComponents.DotNetBar.ListBoxAdv();
            this._selectedProcesses = new DevComponents.DotNetBar.ListBoxAdv();
            this._selectedDataFlows = new DevComponents.DotNetBar.ListBoxAdv();
            this.panel1 = new System.Windows.Forms.Panel();
            this._test = new DevComponents.DotNetBar.ButtonX();
            this._selectedInteractors = new DevComponents.DotNetBar.ListBoxAdv();
            this._statusOverride = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._modelSelected = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this._ok.Location = new System.Drawing.Point(361, 6);
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
            this._cancel.Location = new System.Drawing.Point(523, 6);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._cancel.TabIndex = 5;
            this._cancel.Text = "Cancel";
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _threatType
            // 
            this._threatType.AutoSize = true;
            this._threatType.Location = new System.Drawing.Point(104, 4);
            this._threatType.Margin = new System.Windows.Forms.Padding(0);
            this._threatType.Name = "_threatType";
            this._threatType.Size = new System.Drawing.Size(859, 13);
            this._threatType.TabIndex = 0;
            this._threatType.Text = "threat type name";
            // 
            // _container
            // 
            this._container.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._container.Controls.Add(this._ruleEditor);
            this._container.Controls.Add(this._strengthOverride);
            this._container.Controls.Add(this._severityOverride);
            this._container.Controls.Add(this._mitigation);
            this._container.Controls.Add(this._modelSelectedLabel);
            this._container.Controls.Add(this._selectedDataStores);
            this._container.Controls.Add(this._selectedProcesses);
            this._container.Controls.Add(this._selectedDataFlows);
            this._container.Controls.Add(this.panel1);
            this._container.Controls.Add(this._threatType);
            this._container.Controls.Add(this._selectedInteractors);
            this._container.Controls.Add(this._statusOverride);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.ForeColor = System.Drawing.Color.Black;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem9,
            this.layoutControlItem2,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem10,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem5,
            this._modelSelected,
            this.layoutControlItem4});
            this._container.Size = new System.Drawing.Size(967, 598);
            this._container.TabIndex = 7;
            // 
            // _ruleEditor
            // 
            this._ruleEditor.Location = new System.Drawing.Point(4, 46);
            this._ruleEditor.Margin = new System.Windows.Forms.Padding(0);
            this._ruleEditor.Name = "_ruleEditor";
            selectionRule1.Root = null;
            this._ruleEditor.Rule = selectionRule1;
            this._ruleEditor.Size = new System.Drawing.Size(959, 304);
            this._ruleEditor.TabIndex = 2;
            // 
            // _strengthOverride
            // 
            this._strengthOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._strengthOverride.FormattingEnabled = true;
            this._strengthOverride.Items.AddRange(new object[] {
            "<Do not assign Strength>"});
            this._strengthOverride.Location = new System.Drawing.Point(104, 358);
            this._strengthOverride.Margin = new System.Windows.Forms.Padding(0);
            this._strengthOverride.Name = "_strengthOverride";
            this._strengthOverride.Size = new System.Drawing.Size(211, 21);
            this._strengthOverride.TabIndex = 3;
            // 
            // _severityOverride
            // 
            this._severityOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._severityOverride.Enabled = false;
            this._severityOverride.FormattingEnabled = true;
            this._severityOverride.ItemHeight = 13;
            this._severityOverride.Items.AddRange(new object[] {
            "<Use Threat Type Severity>"});
            this._severityOverride.Location = new System.Drawing.Point(742, 358);
            this._severityOverride.Margin = new System.Windows.Forms.Padding(0);
            this._severityOverride.Name = "_severityOverride";
            this._severityOverride.Size = new System.Drawing.Size(221, 21);
            this._severityOverride.TabIndex = 5;
            // 
            // _mitigation
            // 
            this._mitigation.AutoSize = true;
            this._mitigation.Location = new System.Drawing.Point(104, 25);
            this._mitigation.Margin = new System.Windows.Forms.Padding(0);
            this._mitigation.Name = "_mitigation";
            this._mitigation.Size = new System.Drawing.Size(859, 13);
            this._mitigation.TabIndex = 1;
            this._mitigation.Text = "mitigation name";
            // 
            // _modelSelectedLabel
            // 
            this._modelSelectedLabel.AutoSize = true;
            this._modelSelectedLabel.Location = new System.Drawing.Point(4, 537);
            this._modelSelectedLabel.Margin = new System.Windows.Forms.Padding(0);
            this._modelSelectedLabel.Name = "_modelSelectedLabel";
            this._modelSelectedLabel.Size = new System.Drawing.Size(959, 13);
            this._modelSelectedLabel.TabIndex = 10;
            this._modelSelectedLabel.Text = "The Rule would associate the Mitigation to the generated Threat Event for the Thr" +
    "eat Model.";
            // 
            // _selectedDataStores
            // 
            this._selectedDataStores.AutoScroll = true;
            // 
            // 
            // 
            this._selectedDataStores.BackgroundStyle.Class = "ListBoxAdv";
            this._selectedDataStores.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._selectedDataStores.ContainerControlProcessDialogKey = true;
            this._selectedDataStores.DragDropSupport = true;
            this._selectedDataStores.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._selectedDataStores.Location = new System.Drawing.Point(586, 387);
            this._selectedDataStores.Margin = new System.Windows.Forms.Padding(0);
            this._selectedDataStores.Name = "_selectedDataStores";
            this._selectedDataStores.Size = new System.Drawing.Size(133, 142);
            this._selectedDataStores.TabIndex = 8;
            this._selectedDataStores.Text = "listBoxAdv4";
            // 
            // _selectedProcesses
            // 
            this._selectedProcesses.AutoScroll = true;
            // 
            // 
            // 
            this._selectedProcesses.BackgroundStyle.Class = "ListBoxAdv";
            this._selectedProcesses.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._selectedProcesses.ContainerControlProcessDialogKey = true;
            this._selectedProcesses.DragDropSupport = true;
            this._selectedProcesses.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._selectedProcesses.Location = new System.Drawing.Point(345, 387);
            this._selectedProcesses.Margin = new System.Windows.Forms.Padding(0);
            this._selectedProcesses.Name = "_selectedProcesses";
            this._selectedProcesses.Size = new System.Drawing.Size(133, 142);
            this._selectedProcesses.TabIndex = 7;
            this._selectedProcesses.Text = "listBoxAdv3";
            // 
            // _selectedDataFlows
            // 
            this._selectedDataFlows.AutoScroll = true;
            // 
            // 
            // 
            this._selectedDataFlows.BackgroundStyle.Class = "ListBoxAdv";
            this._selectedDataFlows.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._selectedDataFlows.ContainerControlProcessDialogKey = true;
            this._selectedDataFlows.DragDropSupport = true;
            this._selectedDataFlows.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._selectedDataFlows.Location = new System.Drawing.Point(827, 387);
            this._selectedDataFlows.Margin = new System.Windows.Forms.Padding(0);
            this._selectedDataFlows.Name = "_selectedDataFlows";
            this._selectedDataFlows.Size = new System.Drawing.Size(136, 142);
            this._selectedDataFlows.TabIndex = 9;
            this._selectedDataFlows.Text = "listBoxAdv1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._test);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Location = new System.Drawing.Point(4, 558);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(959, 32);
            this.panel1.TabIndex = 11;
            // 
            // _test
            // 
            this._test.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this._test.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._test.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this._test.Location = new System.Drawing.Point(442, 6);
            this._test.Name = "_test";
            this._test.Size = new System.Drawing.Size(75, 23);
            this._test.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._test.TabIndex = 6;
            this._test.Text = "Test";
            this._test.Click += new System.EventHandler(this._test_Click);
            // 
            // _selectedInteractors
            // 
            this._selectedInteractors.AutoScroll = true;
            // 
            // 
            // 
            this._selectedInteractors.BackgroundStyle.Class = "ListBoxAdv";
            this._selectedInteractors.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._selectedInteractors.ContainerControlProcessDialogKey = true;
            this._selectedInteractors.DragDropSupport = true;
            this._selectedInteractors.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._selectedInteractors.Location = new System.Drawing.Point(104, 387);
            this._selectedInteractors.Margin = new System.Windows.Forms.Padding(0);
            this._selectedInteractors.Name = "_selectedInteractors";
            this._selectedInteractors.Size = new System.Drawing.Size(133, 142);
            this._selectedInteractors.TabIndex = 6;
            this._selectedInteractors.Text = "listBoxAdv2";
            // 
            // _statusOverride
            // 
            this._statusOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._statusOverride.FormattingEnabled = true;
            this._statusOverride.Items.AddRange(new object[] {
            "<Do not assign Status>"});
            this._statusOverride.Location = new System.Drawing.Point(423, 358);
            this._statusOverride.Margin = new System.Windows.Forms.Padding(0);
            this._statusOverride.Name = "_statusOverride";
            this._statusOverride.Size = new System.Drawing.Size(211, 21);
            this._statusOverride.TabIndex = 4;
            this._statusOverride.SelectedIndexChanged += new System.EventHandler(this._statusOverride_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._threatType;
            this.layoutControlItem1.Height = 21;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Threat Type";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._mitigation;
            this.layoutControlItem9.Height = 21;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "Mitigation";
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._strengthOverride;
            this.layoutControlItem11.Height = 29;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Strength Override";
            this.layoutControlItem11.Width = 33;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this._statusOverride;
            this.layoutControlItem12.Height = 29;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "Status Override";
            this.layoutControlItem12.Width = 33;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._severityOverride;
            this.layoutControlItem10.Height = 29;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "Maximum Severity";
            this.layoutControlItem10.Width = 34;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._selectedInteractors;
            this.layoutControlItem6.Height = 150;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Selected \r\nExternal \r\nInteractors";
            this.layoutControlItem6.Width = 25;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._selectedProcesses;
            this.layoutControlItem7.Height = 150;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Selected\r\nProcesses";
            this.layoutControlItem7.Width = 25;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._selectedDataStores;
            this.layoutControlItem8.Height = 150;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Selected\r\nData Stores";
            this.layoutControlItem8.Width = 25;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._selectedDataFlows;
            this.layoutControlItem5.Height = 150;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Selected\r\n Flows";
            this.layoutControlItem5.Width = 25;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _modelSelected
            // 
            this._modelSelected.Control = this._modelSelectedLabel;
            this._modelSelected.Height = 21;
            this._modelSelected.MinSize = new System.Drawing.Size(64, 18);
            this._modelSelected.Name = "_modelSelected";
            this._modelSelected.Text = "Label:";
            this._modelSelected.TextVisible = false;
            this._modelSelected.Visible = false;
            this._modelSelected.Width = 100;
            this._modelSelected.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
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
            // MitigationRuleEditDialog
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
            this.Name = "MitigationRuleEditDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Mitigation Association Rule";
            this._container.ResumeLayout(false);
            this._container.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.ButtonX _ok;
        private DevComponents.DotNetBar.ButtonX _cancel;
        private System.Windows.Forms.Label _threatType;
        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private ListBoxAdv _selectedDataStores;
        private ListBoxAdv _selectedProcesses;
        private ListBoxAdv _selectedDataFlows;
        private ButtonX _test;
        private ListBoxAdv _selectedInteractors;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.Label _modelSelectedLabel;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _modelSelected;
        private System.Windows.Forms.Label _mitigation;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.ComboBox _strengthOverride;
        private System.Windows.Forms.ComboBox _severityOverride;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.ComboBox _statusOverride;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private RuleEditor _ruleEditor;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
    }
}