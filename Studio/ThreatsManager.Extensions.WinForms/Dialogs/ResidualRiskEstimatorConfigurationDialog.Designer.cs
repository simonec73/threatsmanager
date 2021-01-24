namespace ThreatsManager.Extensions.Dialogs
{
    partial class ResidualRiskEstimatorConfigurationDialog
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
            this._cap = new DevComponents.Editors.DoubleInput();
            this.label1 = new System.Windows.Forms.Label();
            this._parameters = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.gridColumn1 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this.gridColumn2 = new DevComponents.DotNetBar.SuperGrid.GridColumn();
            this._estimators = new System.Windows.Forms.ComboBox();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._labelNormalization = new System.Windows.Forms.Label();
            this._labelNormalizationContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cap)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 290);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 53);
            this.panel1.TabIndex = 0;
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(270, 18);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.Location = new System.Drawing.Point(189, 18);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 0;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._labelNormalization);
            this.layoutControl1.Controls.Add(this._cap);
            this.layoutControl1.Controls.Add(this.label1);
            this.layoutControl1.Controls.Add(this._parameters);
            this.layoutControl1.Controls.Add(this._estimators);
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
            this._labelNormalizationContainer});
            this.layoutControl1.Size = new System.Drawing.Size(534, 290);
            this.layoutControl1.TabIndex = 1;
            // 
            // _cap
            // 
            // 
            // 
            // 
            this._cap.BackgroundStyle.Class = "DateTimeInputBackground";
            this._cap.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._cap.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this._cap.Increment = 1D;
            this._cap.Location = new System.Drawing.Point(474, 245);
            this._cap.Margin = new System.Windows.Forms.Padding(0);
            this._cap.Name = "_cap";
            this._cap.ShowUpDown = true;
            this._cap.Size = new System.Drawing.Size(56, 20);
            this._cap.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 245);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Negative numbers are replaced with the maximum value\r\n.";
            // 
            // _parameters
            // 
            this._parameters.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._parameters.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._parameters.Location = new System.Drawing.Point(4, 50);
            this._parameters.Margin = new System.Windows.Forms.Padding(0);
            this._parameters.Name = "_parameters";
            // 
            // 
            // 
            this._parameters.PrimaryGrid.Columns.Add(this.gridColumn1);
            this._parameters.PrimaryGrid.Columns.Add(this.gridColumn2);
            this._parameters.PrimaryGrid.ShowRowDirtyMarker = false;
            this._parameters.PrimaryGrid.ShowRowHeaders = false;
            this._parameters.PrimaryGrid.ShowTreeButton = false;
            this._parameters.Size = new System.Drawing.Size(526, 187);
            this._parameters.TabIndex = 1;
            this._parameters.Text = "superGridControl1";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AllowEdit = false;
            this.gridColumn1.AutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.gridColumn1.HeaderText = "Parameter Name";
            this.gridColumn1.Name = "Parameter";
            // 
            // gridColumn2
            // 
            this.gridColumn2.EditorType = typeof(DevComponents.DotNetBar.SuperGrid.GridDoubleInputEditControl);
            this.gridColumn2.HeaderText = "Value";
            this.gridColumn2.Name = "Value";
            this.gridColumn2.Width = 150;
            // 
            // _estimators
            // 
            this._estimators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._estimators.FormattingEnabled = true;
            this._estimators.Location = new System.Drawing.Point(90, 4);
            this._estimators.Margin = new System.Windows.Forms.Padding(0);
            this._estimators.Name = "_estimators";
            this._estimators.Size = new System.Drawing.Size(440, 21);
            this._estimators.TabIndex = 0;
            this._estimators.SelectedIndexChanged += new System.EventHandler(this._estimator_SelectedIndexChanged);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._estimators;
            this.layoutControlItem1.Height = 29;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Estimator";
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._parameters;
            this.layoutControlItem2.Height = 100;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Parameters";
            this.layoutControlItem2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.label1;
            this.layoutControlItem3.Height = 21;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Label:";
            this.layoutControlItem3.TextVisible = false;
            this.layoutControlItem3.Width = 99;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._cap;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Maximum value";
            this.layoutControlItem4.Width = 150;
            // 
            // _labelNormalization
            // 
            this._labelNormalization.AutoSize = true;
            this._labelNormalization.Location = new System.Drawing.Point(4, 273);
            this._labelNormalization.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._labelNormalization.Name = "_labelNormalization";
            this._labelNormalization.Size = new System.Drawing.Size(526, 13);
            this._labelNormalization.TabIndex = 4;
            this._labelNormalization.Text = "Normalization";
            // 
            // _labelNormalizationContainer
            // 
            this._labelNormalizationContainer.Control = this._labelNormalization;
            this._labelNormalizationContainer.Height = 21;
            this._labelNormalizationContainer.MinSize = new System.Drawing.Size(64, 18);
            this._labelNormalizationContainer.Name = "_labelNormalizationContainer";
            this._labelNormalizationContainer.Text = "Label:";
            this._labelNormalizationContainer.TextVisible = false;
            this._labelNormalizationContainer.Width = 100;
            this._labelNormalizationContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // ResidualRiskEstimatorConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(534, 343);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "ResidualRiskEstimatorConfigurationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Residual Risk Estimator Configuration";
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _ok;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _parameters;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn1;
        private DevComponents.DotNetBar.SuperGrid.GridColumn gridColumn2;
        private System.Windows.Forms.ComboBox _estimators;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.Editors.DoubleInput _cap;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label _labelNormalization;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _labelNormalizationContainer;
    }
}