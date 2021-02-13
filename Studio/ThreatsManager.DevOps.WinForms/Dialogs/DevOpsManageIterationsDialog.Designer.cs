
namespace ThreatsManager.DevOps.Dialogs
{
    partial class DevOpsManageIterationsDialog
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
            this._close = new System.Windows.Forms.Button();
            this._container = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._validatorMessage = new System.Windows.Forms.Label();
            this._load = new System.Windows.Forms.Button();
            this._clear = new System.Windows.Forms.Button();
            this._remove = new System.Windows.Forms.Button();
            this._add = new System.Windows.Forms.Button();
            this._grid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._validatorMessageContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this._addContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._removeContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._clearContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._autoLoadContainer = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._validator = new DevComponents.DotNetBar.Validator.SuperValidator();
            this._overlapValidator = new DevComponents.DotNetBar.Validator.CustomValidator();
            this._startEndValidator = new DevComponents.DotNetBar.Validator.CustomValidator();
            this._highlighter = new DevComponents.DotNetBar.Validator.Highlighter();
            this._initialize = new System.Windows.Forms.Button();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._initializeGroup = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.label1 = new System.Windows.Forms.Label();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramStartDate = new System.Windows.Forms.DateTimePicker();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramDuration = new System.Windows.Forms.NumericUpDown();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramCount = new System.Windows.Forms.NumericUpDown();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._generate = new System.Windows.Forms.Button();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramMonday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramTuesday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramWednesday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramThursday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramFriday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramSaturday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._paramSunday = new System.Windows.Forms.CheckBox();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this._container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._paramDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._paramCount)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._close);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 409);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(726, 46);
            this.panel1.TabIndex = 2;
            // 
            // _close
            // 
            this._close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._close.Location = new System.Drawing.Point(326, 11);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(75, 23);
            this._close.TabIndex = 2;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            this._close.Click += new System.EventHandler(this._close_Click);
            // 
            // _container
            // 
            this._container.Controls.Add(this._paramSunday);
            this._container.Controls.Add(this._paramSaturday);
            this._container.Controls.Add(this._paramFriday);
            this._container.Controls.Add(this._paramThursday);
            this._container.Controls.Add(this._paramWednesday);
            this._container.Controls.Add(this._paramTuesday);
            this._container.Controls.Add(this._paramMonday);
            this._container.Controls.Add(this._generate);
            this._container.Controls.Add(this._paramCount);
            this._container.Controls.Add(this._paramDuration);
            this._container.Controls.Add(this._paramStartDate);
            this._container.Controls.Add(this.label1);
            this._container.Controls.Add(this._initialize);
            this._container.Controls.Add(this._validatorMessage);
            this._container.Controls.Add(this._load);
            this._container.Controls.Add(this._clear);
            this._container.Controls.Add(this._remove);
            this._container.Controls.Add(this._add);
            this._container.Controls.Add(this._grid);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            // 
            // 
            // 
            this._container.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this._validatorMessageContainer,
            this.layoutGroup1,
            this._initializeGroup});
            this._container.Size = new System.Drawing.Size(726, 409);
            this._container.TabIndex = 3;
            // 
            // _validatorMessage
            // 
            this._validatorMessage.AutoSize = true;
            this._validatorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._validatorMessage.ForeColor = System.Drawing.Color.Red;
            this._validatorMessage.Location = new System.Drawing.Point(4, 248);
            this._validatorMessage.Margin = new System.Windows.Forms.Padding(0);
            this._validatorMessage.Name = "_validatorMessage";
            this._validatorMessage.Size = new System.Drawing.Size(718, 13);
            this._validatorMessage.TabIndex = 1;
            this._validatorMessage.Text = "Validator message";
            // 
            // _load
            // 
            this._load.Location = new System.Drawing.Point(584, 269);
            this._load.Margin = new System.Windows.Forms.Padding(0);
            this._load.Name = "_load";
            this._load.Size = new System.Drawing.Size(138, 23);
            this._load.TabIndex = 6;
            this._load.Text = "Auto Load from DevOps";
            this._load.UseVisualStyleBackColor = true;
            this._load.Click += new System.EventHandler(this._load_Click);
            // 
            // _clear
            // 
            this._clear.Location = new System.Drawing.Point(294, 269);
            this._clear.Margin = new System.Windows.Forms.Padding(0);
            this._clear.Name = "_clear";
            this._clear.Size = new System.Drawing.Size(137, 23);
            this._clear.TabIndex = 4;
            this._clear.Text = "Clear";
            this._clear.UseVisualStyleBackColor = true;
            this._clear.Click += new System.EventHandler(this._clear_Click);
            // 
            // _remove
            // 
            this._remove.Location = new System.Drawing.Point(149, 269);
            this._remove.Margin = new System.Windows.Forms.Padding(0);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(137, 23);
            this._remove.TabIndex = 3;
            this._remove.Text = "Remove";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
            // 
            // _add
            // 
            this._add.Location = new System.Drawing.Point(4, 269);
            this._add.Margin = new System.Windows.Forms.Padding(0);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(137, 23);
            this._add.TabIndex = 2;
            this._add.Text = "Add";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _grid
            // 
            this._grid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this._grid.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._grid.Location = new System.Drawing.Point(4, 21);
            this._grid.Margin = new System.Windows.Forms.Padding(0);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(718, 219);
            this._grid.TabIndex = 0;
            this._grid.Text = "superGridControl1";
            this._validator.SetValidator1(this._grid, this._overlapValidator);
            this._validator.SetValidator2(this._grid, this._startEndValidator);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._grid;
            this.layoutControlItem1.Height = 99;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Iterations";
            this.layoutControlItem1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _validatorMessageContainer
            // 
            this._validatorMessageContainer.Control = this._validatorMessage;
            this._validatorMessageContainer.Height = 21;
            this._validatorMessageContainer.MinSize = new System.Drawing.Size(64, 18);
            this._validatorMessageContainer.Name = "_validatorMessageContainer";
            this._validatorMessageContainer.Text = "Label:";
            this._validatorMessageContainer.TextVisible = false;
            this._validatorMessageContainer.Visible = false;
            this._validatorMessageContainer.Width = 100;
            this._validatorMessageContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 31;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._addContainer,
            this._removeContainer,
            this._clearContainer,
            this.layoutControlItem2,
            this._autoLoadContainer});
            this.layoutGroup1.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Padding = new System.Windows.Forms.Padding(0);
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 100;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _addContainer
            // 
            this._addContainer.Control = this._add;
            this._addContainer.Height = 31;
            this._addContainer.MinSize = new System.Drawing.Size(32, 20);
            this._addContainer.Name = "_addContainer";
            this._addContainer.Width = 20;
            this._addContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _removeContainer
            // 
            this._removeContainer.Control = this._remove;
            this._removeContainer.Height = 31;
            this._removeContainer.MinSize = new System.Drawing.Size(32, 20);
            this._removeContainer.Name = "_removeContainer";
            this._removeContainer.Width = 20;
            this._removeContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _clearContainer
            // 
            this._clearContainer.Control = this._clear;
            this._clearContainer.Height = 31;
            this._clearContainer.MinSize = new System.Drawing.Size(32, 20);
            this._clearContainer.Name = "_clearContainer";
            this._clearContainer.Width = 20;
            this._clearContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _autoLoadContainer
            // 
            this._autoLoadContainer.Control = this._load;
            this._autoLoadContainer.Height = 31;
            this._autoLoadContainer.MinSize = new System.Drawing.Size(32, 20);
            this._autoLoadContainer.Name = "_autoLoadContainer";
            this._autoLoadContainer.Width = 20;
            this._autoLoadContainer.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _validator
            // 
            this._validator.ContainerControl = this;
            this._validator.Highlighter = this._highlighter;
            this._validator.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._validator.SteppedValidation = true;
            // 
            // _overlapValidator
            // 
            this._overlapValidator.DisplayError = false;
            this._overlapValidator.ErrorMessage = "One or more iterations overlap.";
            this._overlapValidator.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this._overlapValidator.ValidateValue += new DevComponents.DotNetBar.Validator.ValidateValueEventHandler(this._overlapValidator_ValidateValue);
            // 
            // _startEndValidator
            // 
            this._startEndValidator.DisplayError = false;
            this._startEndValidator.ErrorMessage = "One or more iterations have End date before the Start date.";
            this._startEndValidator.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this._startEndValidator.ValidateValue += new DevComponents.DotNetBar.Validator.ValidateValueEventHandler(this._startEndValidator_ValidateValue);
            // 
            // _highlighter
            // 
            this._highlighter.ContainerControl = this;
            this._highlighter.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            // 
            // _initialize
            // 
            this._initialize.Enabled = false;
            this._initialize.Location = new System.Drawing.Point(439, 269);
            this._initialize.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._initialize.Name = "_initialize";
            this._initialize.Size = new System.Drawing.Size(137, 23);
            this._initialize.TabIndex = 5;
            this._initialize.Text = "Initialize";
            this._initialize.UseVisualStyleBackColor = true;
            this._initialize.Click += new System.EventHandler(this._initialize_Click);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._initialize;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 20;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _initializeGroup
            // 
            this._initializeGroup.Height = 110;
            this._initializeGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem13,
            this.layoutControlItem14,
            this.layoutControlItem7});
            this._initializeGroup.MinSize = new System.Drawing.Size(120, 32);
            this._initializeGroup.Name = "_initializeGroup";
            this._initializeGroup.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._initializeGroup.Visible = false;
            this._initializeGroup.Width = 100;
            this._initializeGroup.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 304);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(710, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Initialize the Iterations specifying a few parameters";
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.label1;
            this.layoutControlItem3.Height = 21;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Label:";
            this.layoutControlItem3.TextVisible = false;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramStartDate
            // 
            this._paramStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._paramStartDate.Location = new System.Drawing.Point(97, 325);
            this._paramStartDate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramStartDate.MaxDate = new System.DateTime(2199, 12, 31, 0, 0, 0, 0);
            this._paramStartDate.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this._paramStartDate.Name = "_paramStartDate";
            this._paramStartDate.Size = new System.Drawing.Size(147, 20);
            this._paramStartDate.TabIndex = 9;
            this._paramStartDate.Value = new System.DateTime(2021, 1, 23, 0, 0, 0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._paramStartDate;
            this.layoutControlItem4.Height = 28;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "Start Date";
            this.layoutControlItem4.Width = 34;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramDuration
            // 
            this._paramDuration.Location = new System.Drawing.Point(341, 325);
            this._paramDuration.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._paramDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._paramDuration.Name = "_paramDuration";
            this._paramDuration.Size = new System.Drawing.Size(139, 20);
            this._paramDuration.TabIndex = 10;
            this._paramDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._paramDuration;
            this.layoutControlItem5.Height = 28;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "Duration in days";
            this.layoutControlItem5.Width = 33;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramCount
            // 
            this._paramCount.Location = new System.Drawing.Point(577, 325);
            this._paramCount.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._paramCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._paramCount.Name = "_paramCount";
            this._paramCount.Size = new System.Drawing.Size(141, 20);
            this._paramCount.TabIndex = 11;
            this._paramCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._paramCount;
            this.layoutControlItem6.Height = 28;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Iterations Count";
            this.layoutControlItem6.Width = 33;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _generate
            // 
            this._generate.Location = new System.Drawing.Point(8, 378);
            this._generate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._generate.Name = "_generate";
            this._generate.Size = new System.Drawing.Size(710, 23);
            this._generate.TabIndex = 19;
            this._generate.Text = "Generate";
            this._generate.UseVisualStyleBackColor = true;
            this._generate.Click += new System.EventHandler(this._generate_Click);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._generate;
            this.layoutControlItem7.Height = 31;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramMonday
            // 
            this._paramMonday.AutoSize = true;
            this._paramMonday.Checked = true;
            this._paramMonday.CheckState = System.Windows.Forms.CheckState.Checked;
            this._paramMonday.Location = new System.Drawing.Point(8, 353);
            this._paramMonday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramMonday.Name = "_paramMonday";
            this._paramMonday.Size = new System.Drawing.Size(92, 17);
            this._paramMonday.TabIndex = 12;
            this._paramMonday.Text = "Monday";
            this._paramMonday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._paramMonday;
            this.layoutControlItem8.Height = 25;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 14;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramTuesday
            // 
            this._paramTuesday.AutoSize = true;
            this._paramTuesday.Checked = true;
            this._paramTuesday.CheckState = System.Windows.Forms.CheckState.Checked;
            this._paramTuesday.Location = new System.Drawing.Point(108, 353);
            this._paramTuesday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramTuesday.Name = "_paramTuesday";
            this._paramTuesday.Size = new System.Drawing.Size(92, 17);
            this._paramTuesday.TabIndex = 13;
            this._paramTuesday.Text = "Tuesday";
            this._paramTuesday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._paramTuesday;
            this.layoutControlItem9.Height = 25;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 14;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramWednesday
            // 
            this._paramWednesday.AutoSize = true;
            this._paramWednesday.Checked = true;
            this._paramWednesday.CheckState = System.Windows.Forms.CheckState.Checked;
            this._paramWednesday.Location = new System.Drawing.Point(208, 353);
            this._paramWednesday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramWednesday.Name = "_paramWednesday";
            this._paramWednesday.Size = new System.Drawing.Size(92, 17);
            this._paramWednesday.TabIndex = 14;
            this._paramWednesday.Text = "Wednesday";
            this._paramWednesday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._paramWednesday;
            this.layoutControlItem10.Height = 25;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 14;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramThursday
            // 
            this._paramThursday.AutoSize = true;
            this._paramThursday.Checked = true;
            this._paramThursday.CheckState = System.Windows.Forms.CheckState.Checked;
            this._paramThursday.Location = new System.Drawing.Point(308, 353);
            this._paramThursday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramThursday.Name = "_paramThursday";
            this._paramThursday.Size = new System.Drawing.Size(92, 17);
            this._paramThursday.TabIndex = 15;
            this._paramThursday.Text = "Thursday";
            this._paramThursday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._paramThursday;
            this.layoutControlItem11.Height = 25;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Width = 14;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramFriday
            // 
            this._paramFriday.AutoSize = true;
            this._paramFriday.Checked = true;
            this._paramFriday.CheckState = System.Windows.Forms.CheckState.Checked;
            this._paramFriday.Location = new System.Drawing.Point(408, 353);
            this._paramFriday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramFriday.Name = "_paramFriday";
            this._paramFriday.Size = new System.Drawing.Size(92, 17);
            this._paramFriday.TabIndex = 16;
            this._paramFriday.Text = "Friday";
            this._paramFriday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this._paramFriday;
            this.layoutControlItem12.Height = 25;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Width = 14;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramSaturday
            // 
            this._paramSaturday.AutoSize = true;
            this._paramSaturday.Location = new System.Drawing.Point(508, 353);
            this._paramSaturday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramSaturday.Name = "_paramSaturday";
            this._paramSaturday.Size = new System.Drawing.Size(92, 17);
            this._paramSaturday.TabIndex = 17;
            this._paramSaturday.Text = "Saturday";
            this._paramSaturday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this._paramSaturday;
            this.layoutControlItem13.Height = 25;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Width = 14;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _paramSunday
            // 
            this._paramSunday.AutoSize = true;
            this._paramSunday.Location = new System.Drawing.Point(608, 353);
            this._paramSunday.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this._paramSunday.Name = "_paramSunday";
            this._paramSunday.Size = new System.Drawing.Size(92, 17);
            this._paramSunday.TabIndex = 18;
            this._paramSunday.Text = "Sunday";
            this._paramSunday.UseVisualStyleBackColor = true;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this._paramSunday;
            this.layoutControlItem14.Height = 25;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Width = 14;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // DevOpsIterationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._close;
            this.ClientSize = new System.Drawing.Size(726, 455);
            this.Controls.Add(this._container);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "DevOpsIterationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Iterations";
            this.panel1.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this._container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._paramDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._paramCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _close;
        private DevComponents.DotNetBar.Layout.LayoutControl _container;
        private System.Windows.Forms.Button _load;
        private System.Windows.Forms.Button _clear;
        private System.Windows.Forms.Button _remove;
        private System.Windows.Forms.Button _add;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl _grid;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _addContainer;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _removeContainer;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _clearContainer;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _autoLoadContainer;
        private DevComponents.DotNetBar.Validator.SuperValidator _validator;
        private DevComponents.DotNetBar.Validator.Highlighter _highlighter;
        private DevComponents.DotNetBar.Validator.CustomValidator _overlapValidator;
        private DevComponents.DotNetBar.Validator.CustomValidator _startEndValidator;
        private System.Windows.Forms.Label _validatorMessage;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _validatorMessageContainer;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private System.Windows.Forms.Button _initialize;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutGroup _initializeGroup;
        private System.Windows.Forms.Button _generate;
        private System.Windows.Forms.NumericUpDown _paramCount;
        private System.Windows.Forms.NumericUpDown _paramDuration;
        private System.Windows.Forms.DateTimePicker _paramStartDate;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.CheckBox _paramSunday;
        private System.Windows.Forms.CheckBox _paramSaturday;
        private System.Windows.Forms.CheckBox _paramFriday;
        private System.Windows.Forms.CheckBox _paramThursday;
        private System.Windows.Forms.CheckBox _paramWednesday;
        private System.Windows.Forms.CheckBox _paramTuesday;
        private System.Windows.Forms.CheckBox _paramMonday;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
    }
}