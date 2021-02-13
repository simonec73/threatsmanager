namespace ThreatsManager.Dialogs
{
    partial class InitializationDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializationDialog));
            this._initWizard = new DevComponents.DotNetBar.Wizard();
            this._pageIntro = new DevComponents.DotNetBar.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._pageExtensions = new DevComponents.DotNetBar.WizardPage();
            this.label4 = new System.Windows.Forms.Label();
            this._automaticConfiguration = new System.Windows.Forms.CheckBox();
            this._pageSimplified = new DevComponents.DotNetBar.WizardPage();
            this.label9 = new System.Windows.Forms.Label();
            this._executionModes = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this._wizardSmartSave = new DevComponents.DotNetBar.WizardPage();
            this._smartSaveHigh = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this._smartSaveLow = new System.Windows.Forms.RadioButton();
            this._smartSaveOff = new System.Windows.Forms.RadioButton();
            this.wizardPage1 = new DevComponents.DotNetBar.WizardPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._tooltip = new System.Windows.Forms.ToolTip(this.components);
            this._initWizard.SuspendLayout();
            this._pageIntro.SuspendLayout();
            this._pageExtensions.SuspendLayout();
            this._pageSimplified.SuspendLayout();
            this._wizardSmartSave.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _initWizard
            // 
            this._initWizard.CancelButtonText = "Cancel";
            this._initWizard.Cursor = System.Windows.Forms.Cursors.Default;
            this._initWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._initWizard.FinishButtonTabIndex = 3;
            // 
            // 
            // 
            this._initWizard.FooterStyle.BackColor = System.Drawing.SystemColors.Control;
            this._initWizard.FooterStyle.BackColorGradientAngle = 90;
            this._initWizard.FooterStyle.BorderBottomWidth = 1;
            this._initWizard.FooterStyle.BorderColor = System.Drawing.SystemColors.Control;
            this._initWizard.FooterStyle.BorderLeftWidth = 1;
            this._initWizard.FooterStyle.BorderRightWidth = 1;
            this._initWizard.FooterStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Etched;
            this._initWizard.FooterStyle.BorderTopColor = System.Drawing.SystemColors.Control;
            this._initWizard.FooterStyle.BorderTopWidth = 1;
            this._initWizard.FooterStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._initWizard.FooterStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this._initWizard.FooterStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._initWizard.HeaderCaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._initWizard.HeaderDescriptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._initWizard.HeaderDescriptionIndent = 16;
            // 
            // 
            // 
            this._initWizard.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._initWizard.HeaderStyle.BackColorGradientAngle = 90;
            this._initWizard.HeaderStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Etched;
            this._initWizard.HeaderStyle.BorderBottomWidth = 1;
            this._initWizard.HeaderStyle.BorderColor = System.Drawing.SystemColors.Control;
            this._initWizard.HeaderStyle.BorderLeftWidth = 1;
            this._initWizard.HeaderStyle.BorderRightWidth = 1;
            this._initWizard.HeaderStyle.BorderTopWidth = 1;
            this._initWizard.HeaderStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._initWizard.HeaderStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this._initWizard.HeaderStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this._initWizard.HelpButtonVisible = false;
            this._initWizard.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._initWizard.Location = new System.Drawing.Point(0, 0);
            this._initWizard.Name = "_initWizard";
            this._initWizard.Size = new System.Drawing.Size(653, 412);
            this._initWizard.TabIndex = 0;
            this._initWizard.WizardPages.AddRange(new DevComponents.DotNetBar.WizardPage[] {
            this._pageIntro,
            this._pageExtensions,
            this._pageSimplified,
            this._wizardSmartSave,
            this.wizardPage1});
            this._initWizard.FinishButtonClick += new System.ComponentModel.CancelEventHandler(this._initWizard_FinishButtonClick);
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
            this.label1.Text = "Welcome to the Initialization Wizard";
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
            this.label2.Text = "This wizard will guide you through the Initialization process.\r\n\r\nGiven that this" +
    " is the first time you have started Threats Manager Studio on this machine, some" +
    " details need to be ironed out.";
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
            // _pageExtensions
            // 
            this._pageExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageExtensions.AntiAlias = false;
            this._pageExtensions.Controls.Add(this.label4);
            this._pageExtensions.Controls.Add(this._automaticConfiguration);
            this._pageExtensions.Location = new System.Drawing.Point(7, 72);
            this._pageExtensions.Name = "_pageExtensions";
            this._pageExtensions.PageDescription = "Configuration of the system to load the additional extensions";
            this._pageExtensions.PageTitle = "Extensions Configuration";
            this._pageExtensions.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageExtensions.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageExtensions.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageExtensions.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageExtensions.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(54, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(530, 152);
            this.label4.TabIndex = 1;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // _automaticConfiguration
            // 
            this._automaticConfiguration.AutoSize = true;
            this._automaticConfiguration.Checked = true;
            this._automaticConfiguration.CheckState = System.Windows.Forms.CheckState.Checked;
            this._automaticConfiguration.Location = new System.Drawing.Point(57, 38);
            this._automaticConfiguration.Name = "_automaticConfiguration";
            this._automaticConfiguration.Size = new System.Drawing.Size(191, 17);
            this._automaticConfiguration.TabIndex = 0;
            this._automaticConfiguration.Text = "Automatic Extensions configuration";
            this._automaticConfiguration.UseVisualStyleBackColor = true;
            // 
            // _pageSimplified
            // 
            this._pageSimplified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pageSimplified.AntiAlias = false;
            this._pageSimplified.Controls.Add(this.label9);
            this._pageSimplified.Controls.Add(this._executionModes);
            this._pageSimplified.Controls.Add(this.label5);
            this._pageSimplified.Location = new System.Drawing.Point(7, 72);
            this._pageSimplified.Name = "_pageSimplified";
            this._pageSimplified.PageDescription = "Simplified Mode configuration";
            this._pageSimplified.PageTitle = "Simplified Mode";
            this._pageSimplified.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._pageSimplified.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageSimplified.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._pageSimplified.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._pageSimplified.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(54, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Execution Mode";
            // 
            // _executionModes
            // 
            this._executionModes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._executionModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._executionModes.FormattingEnabled = true;
            this._executionModes.Location = new System.Drawing.Point(144, 36);
            this._executionModes.Name = "_executionModes";
            this._executionModes.Size = new System.Drawing.Size(440, 21);
            this._executionModes.TabIndex = 6;
            this._executionModes.SelectedIndexChanged += new System.EventHandler(this._executionModes_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(54, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(530, 182);
            this.label5.TabIndex = 5;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // _wizardSmartSave
            // 
            this._wizardSmartSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._wizardSmartSave.AntiAlias = false;
            this._wizardSmartSave.Controls.Add(this._smartSaveHigh);
            this._wizardSmartSave.Controls.Add(this.label6);
            this._wizardSmartSave.Controls.Add(this._smartSaveLow);
            this._wizardSmartSave.Controls.Add(this._smartSaveOff);
            this._wizardSmartSave.Location = new System.Drawing.Point(7, 72);
            this._wizardSmartSave.Name = "_wizardSmartSave";
            this._wizardSmartSave.PageDescription = "Configure automatic save for Threat Models";
            this._wizardSmartSave.PageTitle = "Smart Save";
            this._wizardSmartSave.Size = new System.Drawing.Size(639, 282);
            // 
            // 
            // 
            this._wizardSmartSave.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._wizardSmartSave.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._wizardSmartSave.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._wizardSmartSave.TabIndex = 10;
            // 
            // _smartSaveHigh
            // 
            this._smartSaveHigh.AutoSize = true;
            this._smartSaveHigh.Checked = true;
            this._smartSaveHigh.Location = new System.Drawing.Point(57, 90);
            this._smartSaveHigh.Name = "_smartSaveHigh";
            this._smartSaveHigh.Size = new System.Drawing.Size(348, 17);
            this._smartSaveHigh.TabIndex = 7;
            this._smartSaveHigh.TabStop = true;
            this._smartSaveHigh.Text = "High Frequency: automatic save every 3 minutes, maintains 5 saves.";
            this._smartSaveHigh.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(54, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(530, 126);
            this.label6.TabIndex = 6;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // _smartSaveLow
            // 
            this._smartSaveLow.AutoSize = true;
            this._smartSaveLow.Location = new System.Drawing.Point(57, 67);
            this._smartSaveLow.Name = "_smartSaveLow";
            this._smartSaveLow.Size = new System.Drawing.Size(352, 17);
            this._smartSaveLow.TabIndex = 1;
            this._smartSaveLow.Text = "Low Frequency: automatic save every 10 minutes, maintains 3 saves.";
            this._smartSaveLow.UseVisualStyleBackColor = true;
            // 
            // _smartSaveOff
            // 
            this._smartSaveOff.AutoSize = true;
            this._smartSaveOff.Location = new System.Drawing.Point(57, 44);
            this._smartSaveOff.Name = "_smartSaveOff";
            this._smartSaveOff.Size = new System.Drawing.Size(97, 17);
            this._smartSaveOff.TabIndex = 0;
            this._smartSaveOff.Text = "No Smart Save";
            this._smartSaveOff.UseVisualStyleBackColor = true;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wizardPage1.AntiAlias = false;
            this.wizardPage1.Controls.Add(this.label7);
            this.wizardPage1.Controls.Add(this.label8);
            this.wizardPage1.InteriorPage = false;
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(653, 366);
            // 
            // 
            // 
            this.wizardPage1.Style.BackColor = System.Drawing.Color.White;
            this.wizardPage1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.wizardPage1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.wizardPage1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.wizardPage1.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 16F);
            this.label7.Location = new System.Drawing.Point(212, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(429, 66);
            this.label7.TabIndex = 5;
            this.label7.Text = "Initialization";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(212, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(428, 234);
            this.label8.TabIndex = 6;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // InitializationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(653, 412);
            this.Controls.Add(this._initWizard);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitializationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Initialization Wizard";
            this._initWizard.ResumeLayout(false);
            this._pageIntro.ResumeLayout(false);
            this._pageExtensions.ResumeLayout(false);
            this._pageExtensions.PerformLayout();
            this._pageSimplified.ResumeLayout(false);
            this._pageSimplified.PerformLayout();
            this._wizardSmartSave.ResumeLayout(false);
            this._wizardSmartSave.PerformLayout();
            this.wizardPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Wizard _initWizard;
        private DevComponents.DotNetBar.WizardPage _pageIntro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.WizardPage _pageExtensions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox _automaticConfiguration;
        private DevComponents.DotNetBar.WizardPage _pageSimplified;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.WizardPage _wizardSmartSave;
        private System.Windows.Forms.RadioButton _smartSaveOff;
        private System.Windows.Forms.RadioButton _smartSaveHigh;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton _smartSaveLow;
        private DevComponents.DotNetBar.WizardPage wizardPage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox _executionModes;
        private System.Windows.Forms.ToolTip _tooltip;
    }
}