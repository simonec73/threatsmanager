namespace ThreatsManager.Extensions.Dialogs
{
    partial class ImportTemplateDialog
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
            this._wizard = new DevComponents.DotNetBar.Wizard();
            this._pageIntro = new DevComponents.DotNetBar.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._pageFile = new DevComponents.DotNetBar.WizardPage();
            this._browse = new System.Windows.Forms.Button();
            this._fileName = new System.Windows.Forms.TextBox();
            this._pageTMProperties = new DevComponents.DotNetBar.WizardPage();
            this._layoutDetails = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._description = new System.Windows.Forms.RichTextBox();
            this._name = new System.Windows.Forms.TextBox();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._pageFullyInclude = new DevComponents.DotNetBar.WizardPage();
            this._skipGranularSteps = new System.Windows.Forms.CheckBox();
            this._layoutFullyCopy = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._fullyItemTemplates = new System.Windows.Forms.CheckBox();
            this._fullySchemas = new System.Windows.Forms.CheckBox();
            this._fullyThreatTypes = new System.Windows.Forms.CheckBox();
            this._fullyMitigations = new System.Windows.Forms.CheckBox();
            this._fullyThreatActors = new System.Windows.Forms.CheckBox();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._pageSchemas = new DevComponents.DotNetBar.WizardPage();
            this._uncheckAllSchemas = new System.Windows.Forms.Button();
            this._checkAllSchemas = new System.Windows.Forms.Button();
            this._schemas = new System.Windows.Forms.CheckedListBox();
            this._pageItemTemplates = new DevComponents.DotNetBar.WizardPage();
            this._uncheckAllEntityTemplates = new System.Windows.Forms.Button();
            this._checkAllEntityTemplates = new System.Windows.Forms.Button();
            this._itemTemplates = new System.Windows.Forms.CheckedListBox();
            this._pageThreatTypes = new DevComponents.DotNetBar.WizardPage();
            this._uncheckAllThreatTypes = new System.Windows.Forms.Button();
            this._checkAllThreatTypes = new System.Windows.Forms.Button();
            this._threatTypes = new System.Windows.Forms.CheckedListBox();
            this._pageMitigations = new DevComponents.DotNetBar.WizardPage();
            this._uncheckAllMitigations = new System.Windows.Forms.Button();
            this._checkAllMitigations = new System.Windows.Forms.Button();
            this._mitigations = new System.Windows.Forms.CheckedListBox();
            this._pageThreatActors = new DevComponents.DotNetBar.WizardPage();
            this._uncheckAllThreatActors = new System.Windows.Forms.Button();
            this._checkAllThreatActors = new System.Windows.Forms.Button();
            this._threatActors = new System.Windows.Forms.CheckedListBox();
            this._pageFinish = new DevComponents.DotNetBar.WizardPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._openFile = new System.Windows.Forms.OpenFileDialog();
            this._wizard.SuspendLayout();
            this._pageIntro.SuspendLayout();
            this._pageFile.SuspendLayout();
            this._pageTMProperties.SuspendLayout();
            this._layoutDetails.SuspendLayout();
            this._pageFullyInclude.SuspendLayout();
            this._layoutFullyCopy.SuspendLayout();
            this._pageSchemas.SuspendLayout();
            this._pageItemTemplates.SuspendLayout();
            this._pageThreatTypes.SuspendLayout();
            this._pageMitigations.SuspendLayout();
            this._pageThreatActors.SuspendLayout();
            this._pageFinish.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wizard
            // 
            this._wizard.BackColor = System.Drawing.Color.White;
            this._wizard.CancelButtonText = "Cancel";
            this._wizard.Cursor = System.Windows.Forms.Cursors.Default;
            this._wizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wizard.FinishButtonTabIndex = 3;
            // 
            // 
            // 
            this._wizard.FooterStyle.BackColor = System.Drawing.SystemColors.Control;
            this._wizard.FooterStyle.BackColorGradientAngle = 90;
            this._wizard.FooterStyle.BorderBottomWidth = 1;
            this._wizard.FooterStyle.BorderColor = System.Drawing.SystemColors.Control;
            this._wizard.FooterStyle.BorderLeftWidth = 1;
            this._wizard.FooterStyle.BorderRightWidth = 1;
            this._wizard.FooterStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Etched;
            this._wizard.FooterStyle.BorderTopColor = System.Drawing.SystemColors.Control;
            this._wizard.FooterStyle.BorderTopWidth = 1;
            this._wizard.FooterStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._wizard.FooterStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this._wizard.FooterStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._wizard.ForeColor = System.Drawing.SystemColors.ControlText;
            this._wizard.HeaderCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._wizard.HeaderDescriptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._wizard.HeaderDescriptionIndent = 16;
            this._wizard.HeaderImage = global::ThreatsManager.Extensions.Properties.Resources.astrologer_big;
            this._wizard.HeaderImageSize = new System.Drawing.Size(64, 64);
            // 
            // 
            // 
            this._wizard.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._wizard.HeaderStyle.BackColorGradientAngle = 90;
            this._wizard.HeaderStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Etched;
            this._wizard.HeaderStyle.BorderBottomWidth = 1;
            this._wizard.HeaderStyle.BorderColor = System.Drawing.SystemColors.Control;
            this._wizard.HeaderStyle.BorderLeftWidth = 1;
            this._wizard.HeaderStyle.BorderRightWidth = 1;
            this._wizard.HeaderStyle.BorderTopWidth = 1;
            this._wizard.HeaderStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._wizard.HeaderStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this._wizard.HeaderStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._wizard.HelpButtonVisible = false;
            this._wizard.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._wizard.Location = new System.Drawing.Point(0, 0);
            this._wizard.Name = "_wizard";
            this._wizard.Size = new System.Drawing.Size(653, 412);
            this._wizard.TabIndex = 2;
            this._wizard.WizardPages.AddRange(new DevComponents.DotNetBar.WizardPage[] {
            this._pageIntro,
            this._pageFile,
            this._pageTMProperties,
            this._pageFullyInclude,
            this._pageSchemas,
            this._pageItemTemplates,
            this._pageThreatTypes,
            this._pageMitigations,
            this._pageThreatActors,
            this._pageFinish});
            this._wizard.FinishButtonClick += new System.ComponentModel.CancelEventHandler(this._wizard_FinishButtonClick);
            this._wizard.CancelButtonClick += new System.ComponentModel.CancelEventHandler(this._wizard_CancelButtonClick);
            this._wizard.WizardPageChanging += new DevComponents.DotNetBar.WizardCancelPageChangeEventHandler(this._wizard_WizardPageChanging);
            // 
            // _pageIntro
            // 
            this._pageIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageIntro.BackColor = System.Drawing.Color.White;
            this._pageIntro.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageIntro.Controls.Add(this.label1);
            this._pageIntro.Controls.Add(this.label2);
            this._pageIntro.Controls.Add(this.label3);
            this._pageIntro.InteriorPage = false;
            this._pageIntro.Location = new System.Drawing.Point(0, 0);
            this._pageIntro.Name = "_pageIntro";
            this._pageIntro.Size = new System.Drawing.Size(653, 366);
            // 
            // 
            // 
            this._pageIntro.Style.BackColor = System.Drawing.Color.White;
            this._pageIntro.Style.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.TopLeft;
            this._pageIntro.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageIntro.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageIntro.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageIntro.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 16F);
            this.label1.Location = new System.Drawing.Point(210, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 66);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to the Template Import Wizard";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(210, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(428, 234);
            this.label2.TabIndex = 1;
            this.label2.Text = "This wizard will guide you through the process to import an existing Template int" +
    "o the current Threat Model.\r\n\r\nYou will be given the option of selecting the par" +
    "t of the Template you want to import.";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(210, 343);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "To continue, click Next.";
            // 
            // _pageFile
            // 
            this._pageFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageFile.AntiAlias = false;
            this._pageFile.BackColor = System.Drawing.Color.White;
            this._pageFile.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageFile.Controls.Add(this._browse);
            this._pageFile.Controls.Add(this._fileName);
            this._pageFile.Location = new System.Drawing.Point(7, 72);
            this._pageFile.Name = "_pageFile";
            this._pageFile.NextButtonEnabled = DevComponents.DotNetBar.eWizardButtonState.False;
            this._pageFile.PageDescription = "Select the Template file to be imported.";
            this._pageFile.PageTitle = "Select Template file";
            this._pageFile.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageFile.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageFile.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageFile.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageFile.TabIndex = 22;
            // 
            // _browse
            // 
            this._browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._browse.Location = new System.Drawing.Point(508, 57);
            this._browse.Name = "_browse";
            this._browse.Size = new System.Drawing.Size(75, 23);
            this._browse.TabIndex = 1;
            this._browse.Text = "Browse...";
            this._browse.UseVisualStyleBackColor = true;
            this._browse.Click += new System.EventHandler(this._browse_Click);
            // 
            // _fileName
            // 
            this._fileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._fileName.Location = new System.Drawing.Point(57, 59);
            this._fileName.Name = "_fileName";
            this._fileName.ReadOnly = true;
            this._fileName.Size = new System.Drawing.Size(445, 20);
            this._fileName.TabIndex = 0;
            // 
            // _pageTMProperties
            // 
            this._pageTMProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageTMProperties.AntiAlias = false;
            this._pageTMProperties.BackColor = System.Drawing.Color.White;
            this._pageTMProperties.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageTMProperties.Controls.Add(this._layoutDetails);
            this._pageTMProperties.Location = new System.Drawing.Point(7, 72);
            this._pageTMProperties.Name = "_pageTMProperties";
            this._pageTMProperties.PageDescription = "Select the Threat Model Details to be included in the Template.";
            this._pageTMProperties.PageTitle = "Select Threat Model Details";
            this._pageTMProperties.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageTMProperties.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageTMProperties.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageTMProperties.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageTMProperties.TabIndex = 13;
            // 
            // _layoutDetails
            // 
            this._layoutDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._layoutDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layoutDetails.Controls.Add(this._description);
            this._layoutDetails.Controls.Add(this._name);
            this._layoutDetails.ForeColor = System.Drawing.Color.Black;
            this._layoutDetails.Location = new System.Drawing.Point(55, 0);
            this._layoutDetails.Name = "_layoutDetails";
            // 
            // 
            // 
            this._layoutDetails.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem3});
            this._layoutDetails.Size = new System.Drawing.Size(529, 282);
            this._layoutDetails.TabIndex = 1;
            // 
            // _description
            // 
            this._description.Location = new System.Drawing.Point(68, 32);
            this._description.Margin = new System.Windows.Forms.Padding(0);
            this._description.Name = "_description";
            this._description.ReadOnly = true;
            this._description.Size = new System.Drawing.Size(457, 243);
            this._description.TabIndex = 1;
            this._description.Text = "";
            this._description.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._description_LinkClicked);
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(68, 4);
            this._name.Margin = new System.Windows.Forms.Padding(0);
            this._name.Name = "_name";
            this._name.ReadOnly = true;
            this._name.Size = new System.Drawing.Size(457, 20);
            this._name.TabIndex = 0;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._name;
            this.layoutControlItem2.Height = 28;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Name";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._description;
            this.layoutControlItem3.Height = 99;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Description";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _pageFullyInclude
            // 
            this._pageFullyInclude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageFullyInclude.AntiAlias = false;
            this._pageFullyInclude.BackColor = System.Drawing.Color.White;
            this._pageFullyInclude.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageFullyInclude.Controls.Add(this._skipGranularSteps);
            this._pageFullyInclude.Controls.Add(this._layoutFullyCopy);
            this._pageFullyInclude.Location = new System.Drawing.Point(7, 72);
            this._pageFullyInclude.Name = "_pageFullyInclude";
            this._pageFullyInclude.PageDescription = "Select the categories that you want to fully include in the Template.";
            this._pageFullyInclude.PageTitle = "Select what to fully include";
            this._pageFullyInclude.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageFullyInclude.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageFullyInclude.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageFullyInclude.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageFullyInclude.TabIndex = 14;
            // 
            // _skipGranularSteps
            // 
            this._skipGranularSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._skipGranularSteps.AutoSize = true;
            this._skipGranularSteps.Checked = true;
            this._skipGranularSteps.CheckState = System.Windows.Forms.CheckState.Checked;
            this._skipGranularSteps.Location = new System.Drawing.Point(59, 265);
            this._skipGranularSteps.Margin = new System.Windows.Forms.Padding(0);
            this._skipGranularSteps.Name = "_skipGranularSteps";
            this._skipGranularSteps.Size = new System.Drawing.Size(254, 17);
            this._skipGranularSteps.TabIndex = 8;
            this._skipGranularSteps.Text = "Skip granular selection of unchecked categories";
            this._skipGranularSteps.UseVisualStyleBackColor = true;
            this._skipGranularSteps.CheckedChanged += new System.EventHandler(this._skipGranularSteps_CheckedChanged);
            // 
            // _layoutFullyCopy
            // 
            this._layoutFullyCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._layoutFullyCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._layoutFullyCopy.Controls.Add(this._fullyItemTemplates);
            this._layoutFullyCopy.Controls.Add(this._fullySchemas);
            this._layoutFullyCopy.Controls.Add(this._fullyThreatTypes);
            this._layoutFullyCopy.Controls.Add(this._fullyMitigations);
            this._layoutFullyCopy.Controls.Add(this._fullyThreatActors);
            this._layoutFullyCopy.ForeColor = System.Drawing.Color.Black;
            this._layoutFullyCopy.Location = new System.Drawing.Point(55, 0);
            this._layoutFullyCopy.Name = "_layoutFullyCopy";
            // 
            // 
            // 
            this._layoutFullyCopy.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem6,
            this.layoutControlItem5});
            this._layoutFullyCopy.Size = new System.Drawing.Size(529, 253);
            this._layoutFullyCopy.TabIndex = 2;
            // 
            // _fullyItemTemplates
            // 
            this._fullyItemTemplates.AutoSize = true;
            this._fullyItemTemplates.Checked = true;
            this._fullyItemTemplates.CheckState = System.Windows.Forms.CheckState.Checked;
            this._fullyItemTemplates.Location = new System.Drawing.Point(4, 29);
            this._fullyItemTemplates.Margin = new System.Windows.Forms.Padding(0);
            this._fullyItemTemplates.Name = "_fullyItemTemplates";
            this._fullyItemTemplates.Size = new System.Drawing.Size(521, 17);
            this._fullyItemTemplates.TabIndex = 1;
            this._fullyItemTemplates.Text = "Item Templates";
            this._fullyItemTemplates.UseVisualStyleBackColor = true;
            this._fullyItemTemplates.CheckedChanged += new System.EventHandler(this._fullyEntityTemplates_CheckedChanged);
            // 
            // _fullySchemas
            // 
            this._fullySchemas.AutoSize = true;
            this._fullySchemas.Checked = true;
            this._fullySchemas.CheckState = System.Windows.Forms.CheckState.Checked;
            this._fullySchemas.Location = new System.Drawing.Point(4, 4);
            this._fullySchemas.Margin = new System.Windows.Forms.Padding(0);
            this._fullySchemas.Name = "_fullySchemas";
            this._fullySchemas.Size = new System.Drawing.Size(521, 17);
            this._fullySchemas.TabIndex = 0;
            this._fullySchemas.Text = "Property Schemas";
            this._fullySchemas.UseVisualStyleBackColor = true;
            this._fullySchemas.CheckedChanged += new System.EventHandler(this._fullySchemas_CheckedChanged);
            // 
            // _fullyThreatTypes
            // 
            this._fullyThreatTypes.AutoSize = true;
            this._fullyThreatTypes.Checked = true;
            this._fullyThreatTypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this._fullyThreatTypes.Location = new System.Drawing.Point(4, 54);
            this._fullyThreatTypes.Margin = new System.Windows.Forms.Padding(0);
            this._fullyThreatTypes.Name = "_fullyThreatTypes";
            this._fullyThreatTypes.Size = new System.Drawing.Size(521, 17);
            this._fullyThreatTypes.TabIndex = 2;
            this._fullyThreatTypes.Text = "Threat Types";
            this._fullyThreatTypes.UseVisualStyleBackColor = true;
            this._fullyThreatTypes.CheckedChanged += new System.EventHandler(this._fullyThreatTypes_CheckedChanged);
            // 
            // _fullyMitigations
            // 
            this._fullyMitigations.AutoSize = true;
            this._fullyMitigations.Checked = true;
            this._fullyMitigations.CheckState = System.Windows.Forms.CheckState.Checked;
            this._fullyMitigations.Location = new System.Drawing.Point(4, 79);
            this._fullyMitigations.Margin = new System.Windows.Forms.Padding(0);
            this._fullyMitigations.Name = "_fullyMitigations";
            this._fullyMitigations.Size = new System.Drawing.Size(521, 17);
            this._fullyMitigations.TabIndex = 3;
            this._fullyMitigations.Text = "Mitigations";
            this._fullyMitigations.UseVisualStyleBackColor = true;
            this._fullyMitigations.CheckedChanged += new System.EventHandler(this._fullyMitigations_CheckedChanged);
            // 
            // _fullyThreatActors
            // 
            this._fullyThreatActors.AutoSize = true;
            this._fullyThreatActors.Checked = true;
            this._fullyThreatActors.CheckState = System.Windows.Forms.CheckState.Checked;
            this._fullyThreatActors.Location = new System.Drawing.Point(4, 104);
            this._fullyThreatActors.Margin = new System.Windows.Forms.Padding(0);
            this._fullyThreatActors.Name = "_fullyThreatActors";
            this._fullyThreatActors.Size = new System.Drawing.Size(521, 17);
            this._fullyThreatActors.TabIndex = 4;
            this._fullyThreatActors.Text = "Threat Actors";
            this._fullyThreatActors.UseVisualStyleBackColor = true;
            this._fullyThreatActors.CheckedChanged += new System.EventHandler(this._fullyThreatActors_CheckedChanged);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._fullySchemas;
            this.layoutControlItem8.Height = 25;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._fullyItemTemplates;
            this.layoutControlItem9.Height = 25;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._fullyThreatTypes;
            this.layoutControlItem10.Height = 25;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._fullyMitigations;
            this.layoutControlItem6.Height = 25;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 529;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._fullyThreatActors;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 529;
            // 
            // _pageSchemas
            // 
            this._pageSchemas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageSchemas.AntiAlias = false;
            this._pageSchemas.BackColor = System.Drawing.Color.White;
            this._pageSchemas.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageSchemas.Controls.Add(this._uncheckAllSchemas);
            this._pageSchemas.Controls.Add(this._checkAllSchemas);
            this._pageSchemas.Controls.Add(this._schemas);
            this._pageSchemas.Location = new System.Drawing.Point(7, 72);
            this._pageSchemas.Name = "_pageSchemas";
            this._pageSchemas.PageDescription = "Select the Schemas to be included in the Template.";
            this._pageSchemas.PageTitle = "Select Schemas";
            this._pageSchemas.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageSchemas.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageSchemas.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageSchemas.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageSchemas.TabIndex = 8;
            // 
            // _uncheckAllSchemas
            // 
            this._uncheckAllSchemas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._uncheckAllSchemas.Location = new System.Drawing.Point(138, 256);
            this._uncheckAllSchemas.Name = "_uncheckAllSchemas";
            this._uncheckAllSchemas.Size = new System.Drawing.Size(75, 23);
            this._uncheckAllSchemas.TabIndex = 7;
            this._uncheckAllSchemas.Text = "Uncheck All";
            this._uncheckAllSchemas.UseVisualStyleBackColor = true;
            this._uncheckAllSchemas.Click += new System.EventHandler(this._uncheckAllSchemas_Click);
            // 
            // _checkAllSchemas
            // 
            this._checkAllSchemas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkAllSchemas.Location = new System.Drawing.Point(57, 256);
            this._checkAllSchemas.Name = "_checkAllSchemas";
            this._checkAllSchemas.Size = new System.Drawing.Size(75, 23);
            this._checkAllSchemas.TabIndex = 6;
            this._checkAllSchemas.Text = "Check All";
            this._checkAllSchemas.UseVisualStyleBackColor = true;
            this._checkAllSchemas.Click += new System.EventHandler(this._checkAllSchemas_Click);
            // 
            // _schemas
            // 
            this._schemas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._schemas.FormattingEnabled = true;
            this._schemas.Location = new System.Drawing.Point(57, 0);
            this._schemas.Margin = new System.Windows.Forms.Padding(0);
            this._schemas.Name = "_schemas";
            this._schemas.Size = new System.Drawing.Size(526, 244);
            this._schemas.TabIndex = 5;
            this._schemas.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._schemas_ItemCheck);
            // 
            // _pageItemTemplates
            // 
            this._pageItemTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageItemTemplates.AntiAlias = false;
            this._pageItemTemplates.BackColor = System.Drawing.Color.White;
            this._pageItemTemplates.Controls.Add(this._uncheckAllEntityTemplates);
            this._pageItemTemplates.Controls.Add(this._checkAllEntityTemplates);
            this._pageItemTemplates.Controls.Add(this._itemTemplates);
            this._pageItemTemplates.Location = new System.Drawing.Point(7, 72);
            this._pageItemTemplates.Name = "_pageItemTemplates";
            this._pageItemTemplates.PageDescription = "Select the Item Templates to be included in the Template.";
            this._pageItemTemplates.PageTitle = "Select Item Templates";
            this._pageItemTemplates.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageItemTemplates.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageItemTemplates.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageItemTemplates.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageItemTemplates.TabIndex = 10;
            // 
            // _uncheckAllEntityTemplates
            // 
            this._uncheckAllEntityTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._uncheckAllEntityTemplates.Location = new System.Drawing.Point(138, 256);
            this._uncheckAllEntityTemplates.Name = "_uncheckAllEntityTemplates";
            this._uncheckAllEntityTemplates.Size = new System.Drawing.Size(75, 23);
            this._uncheckAllEntityTemplates.TabIndex = 10;
            this._uncheckAllEntityTemplates.Text = "Uncheck All";
            this._uncheckAllEntityTemplates.UseVisualStyleBackColor = true;
            this._uncheckAllEntityTemplates.Click += new System.EventHandler(this._uncheckAllItemTemplates_Click);
            // 
            // _checkAllEntityTemplates
            // 
            this._checkAllEntityTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkAllEntityTemplates.Location = new System.Drawing.Point(57, 256);
            this._checkAllEntityTemplates.Name = "_checkAllEntityTemplates";
            this._checkAllEntityTemplates.Size = new System.Drawing.Size(75, 23);
            this._checkAllEntityTemplates.TabIndex = 9;
            this._checkAllEntityTemplates.Text = "Check All";
            this._checkAllEntityTemplates.UseVisualStyleBackColor = true;
            this._checkAllEntityTemplates.Click += new System.EventHandler(this._checkAllItemTemplates_Click);
            // 
            // _itemTemplates
            // 
            this._itemTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._itemTemplates.FormattingEnabled = true;
            this._itemTemplates.Location = new System.Drawing.Point(57, 0);
            this._itemTemplates.Margin = new System.Windows.Forms.Padding(0);
            this._itemTemplates.Name = "_itemTemplates";
            this._itemTemplates.Size = new System.Drawing.Size(526, 244);
            this._itemTemplates.TabIndex = 8;
            this._itemTemplates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._entityTemplates_ItemCheck);
            // 
            // _pageThreatTypes
            // 
            this._pageThreatTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageThreatTypes.AntiAlias = false;
            this._pageThreatTypes.BackColor = System.Drawing.Color.White;
            this._pageThreatTypes.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageThreatTypes.Controls.Add(this._uncheckAllThreatTypes);
            this._pageThreatTypes.Controls.Add(this._checkAllThreatTypes);
            this._pageThreatTypes.Controls.Add(this._threatTypes);
            this._pageThreatTypes.Location = new System.Drawing.Point(7, 72);
            this._pageThreatTypes.Name = "_pageThreatTypes";
            this._pageThreatTypes.PageDescription = "Select the Threat Types to be included in the Template.";
            this._pageThreatTypes.PageTitle = "Select Threat Types";
            this._pageThreatTypes.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageThreatTypes.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageThreatTypes.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageThreatTypes.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageThreatTypes.TabIndex = 11;
            // 
            // _uncheckAllThreatTypes
            // 
            this._uncheckAllThreatTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._uncheckAllThreatTypes.Location = new System.Drawing.Point(138, 256);
            this._uncheckAllThreatTypes.Name = "_uncheckAllThreatTypes";
            this._uncheckAllThreatTypes.Size = new System.Drawing.Size(75, 23);
            this._uncheckAllThreatTypes.TabIndex = 13;
            this._uncheckAllThreatTypes.Text = "Uncheck All";
            this._uncheckAllThreatTypes.UseVisualStyleBackColor = true;
            this._uncheckAllThreatTypes.Click += new System.EventHandler(this._uncheckAllThreatTypes_Click);
            // 
            // _checkAllThreatTypes
            // 
            this._checkAllThreatTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkAllThreatTypes.Location = new System.Drawing.Point(57, 256);
            this._checkAllThreatTypes.Name = "_checkAllThreatTypes";
            this._checkAllThreatTypes.Size = new System.Drawing.Size(75, 23);
            this._checkAllThreatTypes.TabIndex = 12;
            this._checkAllThreatTypes.Text = "Check All";
            this._checkAllThreatTypes.UseVisualStyleBackColor = true;
            this._checkAllThreatTypes.Click += new System.EventHandler(this._checkAllThreatTypes_Click);
            // 
            // _threatTypes
            // 
            this._threatTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._threatTypes.FormattingEnabled = true;
            this._threatTypes.Location = new System.Drawing.Point(57, 0);
            this._threatTypes.Margin = new System.Windows.Forms.Padding(0);
            this._threatTypes.Name = "_threatTypes";
            this._threatTypes.Size = new System.Drawing.Size(526, 244);
            this._threatTypes.TabIndex = 11;
            this._threatTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._threatTypes_ItemCheck);
            // 
            // _pageMitigations
            // 
            this._pageMitigations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageMitigations.AntiAlias = false;
            this._pageMitigations.BackColor = System.Drawing.Color.White;
            this._pageMitigations.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageMitigations.Controls.Add(this._uncheckAllMitigations);
            this._pageMitigations.Controls.Add(this._checkAllMitigations);
            this._pageMitigations.Controls.Add(this._mitigations);
            this._pageMitigations.Location = new System.Drawing.Point(7, 72);
            this._pageMitigations.Name = "_pageMitigations";
            this._pageMitigations.PageDescription = "Select the Mitigations to be included in the Template.";
            this._pageMitigations.PageTitle = "Select Mitigations";
            this._pageMitigations.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageMitigations.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageMitigations.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageMitigations.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageMitigations.TabIndex = 12;
            // 
            // _uncheckAllMitigations
            // 
            this._uncheckAllMitigations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._uncheckAllMitigations.Location = new System.Drawing.Point(138, 256);
            this._uncheckAllMitigations.Name = "_uncheckAllMitigations";
            this._uncheckAllMitigations.Size = new System.Drawing.Size(75, 23);
            this._uncheckAllMitigations.TabIndex = 16;
            this._uncheckAllMitigations.Text = "Uncheck All";
            this._uncheckAllMitigations.UseVisualStyleBackColor = true;
            this._uncheckAllMitigations.Click += new System.EventHandler(this._uncheckAllMitigations_Click);
            // 
            // _checkAllMitigations
            // 
            this._checkAllMitigations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkAllMitigations.Location = new System.Drawing.Point(57, 256);
            this._checkAllMitigations.Name = "_checkAllMitigations";
            this._checkAllMitigations.Size = new System.Drawing.Size(75, 23);
            this._checkAllMitigations.TabIndex = 15;
            this._checkAllMitigations.Text = "Check All";
            this._checkAllMitigations.UseVisualStyleBackColor = true;
            this._checkAllMitigations.Click += new System.EventHandler(this._checkAllMitigations_Click);
            // 
            // _mitigations
            // 
            this._mitigations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._mitigations.FormattingEnabled = true;
            this._mitigations.Location = new System.Drawing.Point(57, 0);
            this._mitigations.Margin = new System.Windows.Forms.Padding(0);
            this._mitigations.Name = "_mitigations";
            this._mitigations.Size = new System.Drawing.Size(526, 244);
            this._mitigations.TabIndex = 14;
            this._mitigations.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._mitigations_ItemCheck);
            // 
            // _pageThreatActors
            // 
            this._pageThreatActors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageThreatActors.AntiAlias = false;
            this._pageThreatActors.BackColor = System.Drawing.Color.White;
            this._pageThreatActors.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._pageThreatActors.Controls.Add(this._uncheckAllThreatActors);
            this._pageThreatActors.Controls.Add(this._checkAllThreatActors);
            this._pageThreatActors.Controls.Add(this._threatActors);
            this._pageThreatActors.Location = new System.Drawing.Point(7, 72);
            this._pageThreatActors.Name = "_pageThreatActors";
            this._pageThreatActors.PageDescription = "Select the Threat Actors to be included in the Template.";
            this._pageThreatActors.PageTitle = "Select Threat Actors";
            this._pageThreatActors.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageThreatActors.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageThreatActors.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageThreatActors.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageThreatActors.TabIndex = 20;
            // 
            // _uncheckAllThreatActors
            // 
            this._uncheckAllThreatActors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._uncheckAllThreatActors.Location = new System.Drawing.Point(138, 256);
            this._uncheckAllThreatActors.Name = "_uncheckAllThreatActors";
            this._uncheckAllThreatActors.Size = new System.Drawing.Size(75, 23);
            this._uncheckAllThreatActors.TabIndex = 13;
            this._uncheckAllThreatActors.Text = "Uncheck All";
            this._uncheckAllThreatActors.UseVisualStyleBackColor = true;
            this._uncheckAllThreatActors.Click += new System.EventHandler(this._uncheckAllThreatActors_Click);
            // 
            // _checkAllThreatActors
            // 
            this._checkAllThreatActors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._checkAllThreatActors.Location = new System.Drawing.Point(57, 256);
            this._checkAllThreatActors.Name = "_checkAllThreatActors";
            this._checkAllThreatActors.Size = new System.Drawing.Size(75, 23);
            this._checkAllThreatActors.TabIndex = 12;
            this._checkAllThreatActors.Text = "Check All";
            this._checkAllThreatActors.UseVisualStyleBackColor = true;
            this._checkAllThreatActors.Click += new System.EventHandler(this._checkAllThreatActors_Click);
            // 
            // _threatActors
            // 
            this._threatActors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._threatActors.FormattingEnabled = true;
            this._threatActors.Location = new System.Drawing.Point(57, 0);
            this._threatActors.Margin = new System.Windows.Forms.Padding(0);
            this._threatActors.Name = "_threatActors";
            this._threatActors.Size = new System.Drawing.Size(526, 244);
            this._threatActors.TabIndex = 11;
            this._threatActors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._threatActors_ItemCheck);
            // 
            // _pageFinish
            // 
            this._pageFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageFinish.AntiAlias = false;
            this._pageFinish.Controls.Add(this.label4);
            this._pageFinish.Controls.Add(this.label5);
            this._pageFinish.InteriorPage = false;
            this._pageFinish.Location = new System.Drawing.Point(0, 0);
            this._pageFinish.Name = "_pageFinish";
            this._pageFinish.Size = new System.Drawing.Size(653, 366);
            // 
            // 
            // 
            this._pageFinish.Style.BackColor = System.Drawing.Color.White;
            this._pageFinish.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageFinish.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageFinish.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageFinish.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 16F);
            this.label4.Location = new System.Drawing.Point(213, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(429, 66);
            this.label4.TabIndex = 3;
            this.label4.Text = "Template Import";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(213, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(428, 234);
            this.label5.TabIndex = 4;
            this.label5.Text = "The required information has been collected.\r\n\r\nYou may click Finish to confirm i" +
    "mporting the Template, or Cancel to abort it.";
            // 
            // _openFile
            // 
            this._openFile.DefaultExt = "tmt";
            this._openFile.Filter = "Threat Model Template (*.tmt)|*.tmt|Threat Model Json Template (*.tmk)|*.tmk";
            this._openFile.Title = "Select the Template file";
            this._openFile.RestoreDirectory = true;
            // 
            // ImportTemplateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(653, 412);
            this.Controls.Add(this._wizard);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportTemplateDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template Import Wizard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImportTemplateDialog_FormClosed);
            this._wizard.ResumeLayout(false);
            this._pageIntro.ResumeLayout(false);
            this._pageFile.ResumeLayout(false);
            this._pageFile.PerformLayout();
            this._pageTMProperties.ResumeLayout(false);
            this._layoutDetails.ResumeLayout(false);
            this._layoutDetails.PerformLayout();
            this._pageFullyInclude.ResumeLayout(false);
            this._pageFullyInclude.PerformLayout();
            this._layoutFullyCopy.ResumeLayout(false);
            this._layoutFullyCopy.PerformLayout();
            this._pageSchemas.ResumeLayout(false);
            this._pageItemTemplates.ResumeLayout(false);
            this._pageThreatTypes.ResumeLayout(false);
            this._pageMitigations.ResumeLayout(false);
            this._pageThreatActors.ResumeLayout(false);
            this._pageFinish.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Wizard _wizard;
        private DevComponents.DotNetBar.WizardPage _pageIntro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.WizardPage _pageSchemas;
        private DevComponents.DotNetBar.WizardPage _pageItemTemplates;
        private System.Windows.Forms.Button _uncheckAllSchemas;
        private System.Windows.Forms.Button _checkAllSchemas;
        private System.Windows.Forms.CheckedListBox _schemas;
        private System.Windows.Forms.Button _uncheckAllEntityTemplates;
        private System.Windows.Forms.Button _checkAllEntityTemplates;
        private System.Windows.Forms.CheckedListBox _itemTemplates;
        private DevComponents.DotNetBar.WizardPage _pageThreatTypes;
        private System.Windows.Forms.Button _uncheckAllThreatTypes;
        private System.Windows.Forms.Button _checkAllThreatTypes;
        private System.Windows.Forms.CheckedListBox _threatTypes;
        private DevComponents.DotNetBar.WizardPage _pageMitigations;
        private System.Windows.Forms.Button _uncheckAllMitigations;
        private System.Windows.Forms.Button _checkAllMitigations;
        private System.Windows.Forms.CheckedListBox _mitigations;
        private DevComponents.DotNetBar.WizardPage _pageTMProperties;
        private DevComponents.DotNetBar.WizardPage _pageFullyInclude;
        private DevComponents.DotNetBar.Layout.LayoutControl _layoutDetails;
        private System.Windows.Forms.RichTextBox _description;
        private System.Windows.Forms.TextBox _name;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControl _layoutFullyCopy;
        private System.Windows.Forms.CheckBox _fullyItemTemplates;
        private System.Windows.Forms.CheckBox _fullySchemas;
        private System.Windows.Forms.CheckBox _fullyThreatTypes;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private System.Windows.Forms.CheckBox _fullyMitigations;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.CheckBox _fullyThreatActors;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.WizardPage _pageThreatActors;
        private System.Windows.Forms.Button _uncheckAllThreatActors;
        private System.Windows.Forms.Button _checkAllThreatActors;
        private System.Windows.Forms.CheckedListBox _threatActors;
        private DevComponents.DotNetBar.WizardPage _pageFinish;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.WizardPage _pageFile;
        private System.Windows.Forms.Button _browse;
        private System.Windows.Forms.TextBox _fileName;
        private System.Windows.Forms.CheckBox _skipGranularSteps;
        private System.Windows.Forms.OpenFileDialog _openFile;
    }
}