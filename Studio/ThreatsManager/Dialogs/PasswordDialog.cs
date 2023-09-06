using System.Runtime.InteropServices;
using System;
using System.Security;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ThreatsManager.Dialogs
{
    public partial class PasswordDialog : Form
    {
        public PasswordDialog()
        {
            InitializeComponent();
        }

        public bool VerificationRequired
        {
            get { return _repeatPassword.Visible; }
            
            set
            {
                _layoutRepeatPassword.Visible = value;
                _layoutQuality.Visible = value;
                _passwordRepeatValidator.Enabled = value;

                Height = (int) ((value ? 220 : 140) * Dpi.Factor.Height);
            }
        }

        public SecureString Password => _password.SecureString;

        private bool ArePasswordsEqual()
        {
            bool result = false;

            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;

            try
            {
                bstr1 = Marshal.SecureStringToBSTR(_password.SecureString);
                bstr2 = Marshal.SecureStringToBSTR(_repeatPassword.SecureString);

                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    result = true;
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }

            return result;
        }

        private void _password_SecureTextChanged(Controls.SecureTextBox textBox)
        {
            var value = ((int)textBox.Score + 1) * 10;
            _negligible.Value = value;
            _veryWeak.Value = value;
            _weak.Value = value;
            _medium.Value = value;
            _strong.Value = value;
            _veryStrong.Value = value;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _passwordRepeatValidator_ValidateValue(object sender, DevComponents.DotNetBar.Validator.ValidateValueEventArgs e)
        {
            e.IsValid = ArePasswordsEqual();
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            _validator.Enabled = false;
            this.Close();
        }

        private void _passwordDefinedValidator_ValidateValue(object sender, DevComponents.DotNetBar.Validator.ValidateValueEventArgs e)
        {
            if (e.ControlToValidate == _password)
            {
                var password = _password.SecureString;
                e.IsValid = (password?.Length ?? 0) > 0;
            }
        }
    }
}
