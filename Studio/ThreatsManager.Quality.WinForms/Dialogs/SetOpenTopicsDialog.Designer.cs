
namespace ThreatsManager.Quality.Dialogs
{
    partial class SetOpenTopicsDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this._askedBy = new DevComponents.DotNetBar.Controls.TextBoxX();
            this._askedOn = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this._askedVia = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._askedByContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._askedOnContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._askedViaContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._askedOn)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 112);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 44);
            this.panel1.TabIndex = 3;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(184, 10);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 22);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(103, 10);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 22);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this.label1);
            this.layoutControl1.Controls.Add(this._askedBy);
            this.layoutControl1.Controls.Add(this._askedOn);
            this.layoutControl1.Controls.Add(this._askedVia);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControl1.Size = new System.Drawing.Size(362, 112);
            this.layoutControl1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(358, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Assign the following details to every Topic to be clarified missing them.";
            // 
            // _askedBy
            // 
            this._askedBy.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._askedBy.Border.Class = "TextBoxBorder";
            this._askedBy.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedBy.ButtonCustom.Symbol = "";
            this._askedBy.ButtonCustom.Visible = true;
            this._askedBy.DisabledBackColor = System.Drawing.Color.White;
            this._askedBy.ForeColor = System.Drawing.Color.Black;
            this._askedBy.Location = new System.Drawing.Point(60, 22);
            this._askedBy.Margin = new System.Windows.Forms.Padding(0);
            this._askedBy.Name = "_askedBy";
            this._askedBy.PreventEnterBeep = true;
            this._askedBy.Size = new System.Drawing.Size(300, 20);
            this._askedBy.TabIndex = 1;
            this._askedBy.ButtonCustomClick += new System.EventHandler(this._askedBy_ButtonCustomClick);
            // 
            // _askedOn
            // 
            // 
            // 
            // 
            this._askedOn.BackgroundStyle.Class = "DateTimeInputBackground";
            this._askedOn.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.ButtonCustom.Symbol = "";
            this._askedOn.ButtonCustom.Visible = true;
            this._askedOn.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this._askedOn.ButtonDropDown.Visible = true;
            this._askedOn.IsPopupCalendarOpen = false;
            this._askedOn.Location = new System.Drawing.Point(60, 44);
            this._askedOn.Margin = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            // 
            // 
            // 
            this._askedOn.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this._askedOn.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this._askedOn.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.MonthCalendar.DisplayMonth = new System.DateTime(2020, 12, 1, 0, 0, 0, 0);
            // 
            // 
            // 
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._askedOn.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedOn.MonthCalendar.TodayButtonVisible = true;
            this._askedOn.Name = "_askedOn";
            this._askedOn.Size = new System.Drawing.Size(300, 20);
            this._askedOn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._askedOn.TabIndex = 2;
            this._askedOn.ButtonCustomClick += new System.EventHandler(this._askedOn_ButtonCustomClick);
            // 
            // _askedVia
            // 
            this._askedVia.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._askedVia.Border.Class = "TextBoxBorder";
            this._askedVia.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._askedVia.ButtonCustom.Symbol = "";
            this._askedVia.ButtonCustom.Visible = true;
            this._askedVia.ButtonCustom2.Symbol = "";
            this._askedVia.ButtonCustom2.Visible = true;
            this._askedVia.DisabledBackColor = System.Drawing.Color.White;
            this._askedVia.ForeColor = System.Drawing.Color.Black;
            this._askedVia.Location = new System.Drawing.Point(60, 66);
            this._askedVia.Margin = new System.Windows.Forms.Padding(0);
            this._askedVia.Name = "_askedVia";
            this._askedVia.Size = new System.Drawing.Size(300, 20);
            this._askedVia.TabIndex = 3;
            this._askedVia.ButtonCustomClick += new System.EventHandler(this._askedVia_ButtonCustomClick);
            this._askedVia.ButtonCustom2Click += new System.EventHandler(this._askedVia_ButtonCustom2Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.label1;
            this.layoutControlItem1.Height = 20;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 9);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.layoutControlItem1.Text = "Label:";
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._askedBy;
            this.layoutControlItem5.Height = 22;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(60, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.layoutControlItem5.Text = "Asked By";
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._askedOn;
            this.layoutControlItem6.Height = 22;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 9);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.layoutControlItem6.Text = "Asked On";
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._askedVia;
            this.layoutControlItem7.Height = 22;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(60, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.layoutControlItem7.Text = "Asked Via";
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _askedByContainer
            // 
            this._askedByContainer.Control = this._askedBy;
            this._askedByContainer.Height = 54;
            this._askedByContainer.MinSize = new System.Drawing.Size(240, 0);
            this._askedByContainer.Name = "_askedByContainer";
            this._askedByContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._askedByContainer.Text = "Asked By";
            this._askedByContainer.Width = 40;
            this._askedByContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _askedOnContainer
            // 
            this._askedOnContainer.Control = this._askedOn;
            this._askedOnContainer.Height = 54;
            this._askedOnContainer.MinSize = new System.Drawing.Size(128, 34);
            this._askedOnContainer.Name = "_askedOnContainer";
            this._askedOnContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._askedOnContainer.Text = "Asked On";
            this._askedOnContainer.Width = 30;
            this._askedOnContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _askedViaContainer
            // 
            this._askedViaContainer.Control = this._askedVia;
            this._askedViaContainer.Height = 54;
            this._askedViaContainer.MinSize = new System.Drawing.Size(240, 0);
            this._askedViaContainer.Name = "_askedViaContainer";
            this._askedViaContainer.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this._askedViaContainer.Text = "Asked Via";
            this._askedViaContainer.Width = 30;
            this._askedViaContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // SetOpenTopicsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(362, 156);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(378, 195);
            this.Name = "SetOpenTopicsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Open Topics";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._askedOn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private DevComponents.DotNetBar.Controls.TextBoxX _askedBy;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput _askedOn;
        private DevComponents.DotNetBar.Controls.TextBoxX _askedVia;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _askedByContainer;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _askedOnContainer;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _askedViaContainer;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
    }
}