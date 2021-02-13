
namespace ThreatsManager.DevOps.Panels.Configuration
{
    partial class ConfigurationPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._raiseAllEvents = new System.Windows.Forms.RadioButton();
            this._raiseSomeEvents = new System.Windows.Forms.RadioButton();
            this._raiseNoEvents = new System.Windows.Forms.RadioButton();
            this._interval = new DevComponents.Editors.IntegerInput();
            this._scheduledRefresh = new System.Windows.Forms.CheckBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._interval)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._raiseAllEvents);
            this.layoutControl1.Controls.Add(this._raiseSomeEvents);
            this.layoutControl1.Controls.Add(this._raiseNoEvents);
            this.layoutControl1.Controls.Add(this._interval);
            this.layoutControl1.Controls.Add(this._scheduledRefresh);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.layoutControlItem5});
            this.layoutControl1.Size = new System.Drawing.Size(683, 466);
            this.layoutControl1.TabIndex = 0;
            // 
            // _raiseAllEvents
            // 
            this._raiseAllEvents.AutoSize = true;
            this._raiseAllEvents.Enabled = false;
            this._raiseAllEvents.Location = new System.Drawing.Point(21, 107);
            this._raiseAllEvents.Margin = new System.Windows.Forms.Padding(0);
            this._raiseAllEvents.Name = "_raiseAllEvents";
            this._raiseAllEvents.Size = new System.Drawing.Size(658, 17);
            this._raiseAllEvents.TabIndex = 4;
            this._raiseAllEvents.Text = "Raise a notification every time a Refresh is performed";
            this._raiseAllEvents.UseVisualStyleBackColor = true;
            this._raiseAllEvents.CheckedChanged += new System.EventHandler(this._raiseAllEvents_CheckedChanged);
            // 
            // _raiseSomeEvents
            // 
            this._raiseSomeEvents.AutoSize = true;
            this._raiseSomeEvents.Enabled = false;
            this._raiseSomeEvents.Location = new System.Drawing.Point(21, 82);
            this._raiseSomeEvents.Margin = new System.Windows.Forms.Padding(0);
            this._raiseSomeEvents.Name = "_raiseSomeEvents";
            this._raiseSomeEvents.Size = new System.Drawing.Size(658, 17);
            this._raiseSomeEvents.TabIndex = 3;
            this._raiseSomeEvents.Text = "Raise a notification only if one or more updates are found\r\n";
            this._raiseSomeEvents.UseVisualStyleBackColor = true;
            this._raiseSomeEvents.CheckedChanged += new System.EventHandler(this._raiseSomeEvents_CheckedChanged);
            // 
            // _raiseNoEvents
            // 
            this._raiseNoEvents.AutoSize = true;
            this._raiseNoEvents.Checked = true;
            this._raiseNoEvents.Enabled = false;
            this._raiseNoEvents.Location = new System.Drawing.Point(21, 57);
            this._raiseNoEvents.Margin = new System.Windows.Forms.Padding(0);
            this._raiseNoEvents.Name = "_raiseNoEvents";
            this._raiseNoEvents.Size = new System.Drawing.Size(658, 17);
            this._raiseNoEvents.TabIndex = 2;
            this._raiseNoEvents.TabStop = true;
            this._raiseNoEvents.Text = "Do not raise any notification as a result of a Refresh";
            this._raiseNoEvents.UseVisualStyleBackColor = true;
            this._raiseNoEvents.CheckedChanged += new System.EventHandler(this._raiseNoEvents_CheckedChanged);
            // 
            // _interval
            // 
            // 
            // 
            // 
            this._interval.BackgroundStyle.Class = "DateTimeInputBackground";
            this._interval.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._interval.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this._interval.Enabled = false;
            this._interval.Location = new System.Drawing.Point(100, 29);
            this._interval.Margin = new System.Windows.Forms.Padding(0);
            this._interval.MaxValue = 120;
            this._interval.MinValue = 1;
            this._interval.Name = "_interval";
            this._interval.ShowUpDown = true;
            this._interval.Size = new System.Drawing.Size(96, 20);
            this._interval.TabIndex = 1;
            this._tooltip.SetToolTip(this._interval, "Interval between two repeated refreshes of the Mitigation statuses.\r\nIt is recomm" +
        "ended to avoid refreshing too frequently, to limit resource consumption.\r\nAllowe" +
        "d values are between 1 and 120.");
            this._interval.Value = 1;
            this._interval.ValueChanged += new System.EventHandler(this._interval_ValueChanged);
            // 
            // _scheduledRefresh
            // 
            this._scheduledRefresh.AutoSize = true;
            this._scheduledRefresh.Location = new System.Drawing.Point(4, 4);
            this._scheduledRefresh.Margin = new System.Windows.Forms.Padding(0);
            this._scheduledRefresh.Name = "_scheduledRefresh";
            this._scheduledRefresh.Size = new System.Drawing.Size(675, 17);
            this._scheduledRefresh.TabIndex = 0;
            this._scheduledRefresh.Text = "Enable scheduled status refreshes\r\n";
            this._tooltip.SetToolTip(this._scheduledRefresh, "Scheduled refresh of the status of the Mitigations.\r\nIt is recommended to avoid r" +
        "efreshing too frequently, to limit resource consumption.");
            this._scheduledRefresh.UseVisualStyleBackColor = true;
            this._scheduledRefresh.CheckedChanged += new System.EventHandler(this._scheduledRefresh_CheckedChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._scheduledRefresh;
            this.layoutControlItem1.Height = 25;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._interval;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(21, 4, 4, 4);
            this.layoutControlItem2.Text = "Interval (mins)";
            this.layoutControlItem2.Width = 200;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._raiseNoEvents;
            this.layoutControlItem3.Height = 25;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new System.Windows.Forms.Padding(21, 4, 4, 4);
            this.layoutControlItem3.Width = 101;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._raiseSomeEvents;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new System.Windows.Forms.Padding(21, 4, 4, 4);
            this.layoutControlItem4.Width = 101;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._raiseAllEvents;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new System.Windows.Forms.Padding(21, 4, 4, 4);
            this.layoutControlItem5.Width = 101;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "ConfigurationPanel";
            this.Size = new System.Drawing.Size(683, 466);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._interval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.CheckBox _scheduledRefresh;
        private System.Windows.Forms.ToolTip _tooltip;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.Editors.IntegerInput _interval;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.RadioButton _raiseAllEvents;
        private System.Windows.Forms.RadioButton _raiseSomeEvents;
        private System.Windows.Forms.RadioButton _raiseNoEvents;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
    }
}
