namespace ThreatsManager.Dialogs
{
    partial class PasswordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordDialog));
            this._password = new ThreatsManager.Controls.SecureTextBox();
            this._repeatPassword = new ThreatsManager.Controls.SecureTextBox();
            this._ok = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this._validator = new DevComponents.DotNetBar.Validator.SuperValidator();
            this._passwordDefinedValidator = new DevComponents.DotNetBar.Validator.CustomValidator();
            this._passwordRepeatValidator = new DevComponents.DotNetBar.Validator.CustomValidator();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._highlighter = new DevComponents.DotNetBar.Validator.Highlighter();
            this._quality = new DevComponents.DotNetBar.ProgressSteps();
            this._negligible = new DevComponents.DotNetBar.StepItem();
            this._veryWeak = new DevComponents.DotNetBar.StepItem();
            this._weak = new DevComponents.DotNetBar.StepItem();
            this._medium = new DevComponents.DotNetBar.StepItem();
            this._strong = new DevComponents.DotNetBar.StepItem();
            this._veryStrong = new DevComponents.DotNetBar.StepItem();
            this._styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._layoutPassword = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutQuality = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._layoutRepeatPassword = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.layoutControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _password
            // 
            this._password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._password.Location = new System.Drawing.Point(4, 21);
            this._password.Margin = new System.Windows.Forms.Padding(0);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.Size = new System.Drawing.Size(439, 23);
            this._password.TabIndex = 0;
            this._validator.SetValidator1(this._password, this._passwordDefinedValidator);
            this._password.SecureTextChanged += new System.Action<ThreatsManager.Controls.SecureTextBox>(this._password_SecureTextChanged);
            // 
            // _repeatPassword
            // 
            this._repeatPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._repeatPassword.Location = new System.Drawing.Point(4, 102);
            this._repeatPassword.Margin = new System.Windows.Forms.Padding(0);
            this._repeatPassword.Name = "_repeatPassword";
            this._repeatPassword.PasswordChar = '●';
            this._repeatPassword.Size = new System.Drawing.Size(439, 23);
            this._repeatPassword.TabIndex = 2;
            this._validator.SetValidator1(this._repeatPassword, this._passwordRepeatValidator);
            // 
            // _ok
            // 
            this._ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(155, 14);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 4;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            this._ok.Click += new System.EventHandler(this._ok_Click);
            // 
            // _cancel
            // 
            this._cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(236, 14);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 5;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _validator
            // 
            this._validator.ContainerControl = this;
            this._validator.ErrorProvider = this._errorProvider;
            this._validator.Highlighter = this._highlighter;
            this._validator.ValidationType = DevComponents.DotNetBar.Validator.eValidationType.ValidatingEventOnContainer;
            // 
            // _passwordDefinedValidator
            // 
            this._passwordDefinedValidator.ErrorMessage = "Password cannot be empty.";
            this._passwordDefinedValidator.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this._passwordDefinedValidator.ValidateValue += new DevComponents.DotNetBar.Validator.ValidateValueEventHandler(this._passwordDefinedValidator_ValidateValue);
            // 
            // _passwordRepeatValidator
            // 
            this._passwordRepeatValidator.ErrorMessage = "Passwords must be the same.";
            this._passwordRepeatValidator.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this._passwordRepeatValidator.ValidateValue += new DevComponents.DotNetBar.Validator.ValidateValueEventHandler(this._passwordRepeatValidator_ValidateValue);
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            this._errorProvider.Icon = ((System.Drawing.Icon)(resources.GetObject("_errorProvider.Icon")));
            // 
            // _highlighter
            // 
            this._highlighter.ContainerControl = this;
            // 
            // _quality
            // 
            this._quality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._quality.AutoSize = true;
            // 
            // 
            // 
            this._quality.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this._quality.BackgroundStyle.Class = "ProgressSteps";
            this._quality.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._quality.ContainerControlProcessDialogKey = true;
            this._quality.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._negligible,
            this._veryWeak,
            this._weak,
            this._medium,
            this._strong,
            this._veryStrong});
            this._quality.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._quality.Location = new System.Drawing.Point(4, 52);
            this._quality.Margin = new System.Windows.Forms.Padding(0);
            this._quality.Name = "_quality";
            this._quality.Size = new System.Drawing.Size(459, 25);
            this._quality.TabIndex = 1;
            this._quality.TabStop = false;
            // 
            // _negligible
            // 
            this._negligible.HotTracking = false;
            this._negligible.Maximum = 10;
            this._negligible.MinimumSize = new System.Drawing.Size(75, 25);
            this._negligible.Name = "_negligible";
            this._negligible.SymbolSize = 13F;
            this._negligible.Text = "Negligible";
            // 
            // _veryWeak
            // 
            this._veryWeak.HotTracking = false;
            this._veryWeak.Maximum = 20;
            this._veryWeak.Minimum = 11;
            this._veryWeak.MinimumSize = new System.Drawing.Size(75, 25);
            this._veryWeak.Name = "_veryWeak";
            this._veryWeak.SymbolSize = 13F;
            this._veryWeak.Text = "Very Weak";
            // 
            // _weak
            // 
            this._weak.HotTracking = false;
            this._weak.Maximum = 30;
            this._weak.Minimum = 21;
            this._weak.MinimumSize = new System.Drawing.Size(75, 25);
            this._weak.Name = "_weak";
            this._weak.SymbolSize = 13F;
            this._weak.Text = "Weak";
            this._weak.Value = 2;
            // 
            // _medium
            // 
            this._medium.Maximum = 40;
            this._medium.Minimum = 31;
            this._medium.MinimumSize = new System.Drawing.Size(75, 25);
            this._medium.Name = "_medium";
            this._medium.SymbolSize = 13F;
            this._medium.Text = "Medium";
            this._medium.Value = 3;
            // 
            // _strong
            // 
            this._strong.Maximum = 50;
            this._strong.Minimum = 41;
            this._strong.MinimumSize = new System.Drawing.Size(75, 25);
            this._strong.Name = "_strong";
            this._strong.SymbolSize = 13F;
            this._strong.Text = "Strong";
            this._strong.Value = 4;
            // 
            // _veryStrong
            // 
            this._veryStrong.Maximum = 60;
            this._veryStrong.Minimum = 51;
            this._veryStrong.MinimumSize = new System.Drawing.Size(75, 25);
            this._veryStrong.Name = "_veryStrong";
            this._veryStrong.SymbolSize = 13F;
            this._veryStrong.Text = "Very Strong";
            this._veryStrong.Value = 5;
            // 
            // _styleManager
            // 
            this._styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2016;
            this._styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.layoutControl1.Controls.Add(this._password);
            this.layoutControl1.Controls.Add(this._quality);
            this.layoutControl1.Controls.Add(this._repeatPassword);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.ForeColor = System.Drawing.Color.Black;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this._layoutPassword,
            this._layoutQuality,
            this._layoutRepeatPassword});
            this.layoutControl1.Size = new System.Drawing.Size(467, 140);
            this.layoutControl1.TabIndex = 7;
            // 
            // _layoutPassword
            // 
            this._layoutPassword.Control = this._password;
            this._layoutPassword.Height = 48;
            this._layoutPassword.MinSize = new System.Drawing.Size(64, 18);
            this._layoutPassword.Name = "_layoutPassword";
            this._layoutPassword.Padding = new System.Windows.Forms.Padding(4, 4, 24, 4);
            this._layoutPassword.Text = "Please insert the password to use";
            this._layoutPassword.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._layoutPassword.Width = 100;
            this._layoutPassword.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutQuality
            // 
            this._layoutQuality.Control = this._quality;
            this._layoutQuality.Height = 33;
            this._layoutQuality.MinSize = new System.Drawing.Size(64, 18);
            this._layoutQuality.Name = "_layoutQuality";
            this._layoutQuality.Text = "Label:";
            this._layoutQuality.TextVisible = false;
            this._layoutQuality.Width = 100;
            this._layoutQuality.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _layoutRepeatPassword
            // 
            this._layoutRepeatPassword.Control = this._repeatPassword;
            this._layoutRepeatPassword.Height = 48;
            this._layoutRepeatPassword.MinSize = new System.Drawing.Size(64, 18);
            this._layoutRepeatPassword.Name = "_layoutRepeatPassword";
            this._layoutRepeatPassword.Padding = new System.Windows.Forms.Padding(4, 4, 24, 4);
            this._layoutRepeatPassword.Text = "Please repeat the password";
            this._layoutRepeatPassword.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this._layoutRepeatPassword.Width = 100;
            this._layoutRepeatPassword.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancel);
            this.panel1.Controls.Add(this._ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 140);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(467, 49);
            this.panel1.TabIndex = 8;
            // 
            // PasswordDialog
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(467, 189);
            this.ControlBox = false;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(461, 100);
            this.Name = "PasswordDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select password";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.SecureTextBox _password;
        private Controls.SecureTextBox _repeatPassword;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Button _cancel;
        private DevComponents.DotNetBar.Validator.SuperValidator _validator;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private DevComponents.DotNetBar.Validator.Highlighter _highlighter;
        private DevComponents.DotNetBar.ProgressSteps _quality;
        private DevComponents.DotNetBar.StepItem _negligible;
        private DevComponents.DotNetBar.StepItem _veryWeak;
        private DevComponents.DotNetBar.StepItem _weak;
        private DevComponents.DotNetBar.StepItem _medium;
        private DevComponents.DotNetBar.StepItem _strong;
        private DevComponents.DotNetBar.StepItem _veryStrong;
        private DevComponents.DotNetBar.StyleManager _styleManager;
        private DevComponents.DotNetBar.Validator.CustomValidator _passwordRepeatValidator;
        private DevComponents.DotNetBar.Validator.CustomValidator _passwordDefinedValidator;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutPassword;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutQuality;
        private DevComponents.DotNetBar.Layout.LayoutControlItem _layoutRepeatPassword;
    }
}