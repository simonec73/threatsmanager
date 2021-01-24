using System.Security;
using System.Windows.Forms;

namespace ThreatsManager.Controls
{
    public partial class SecureTextBox : UserControl
    {
        char _passwordChar = '●';
        SecureString _secureString = new SecureString();

        public SecureString SecureString
        {
            get { return _secureString; }
        }

        public char PasswordChar
        {
            get { return _passwordChar; }
            set { _passwordChar = value; }
        }
        
        public SecureTextBox()
        {
            InitializeComponent();
        }

        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
                ProcessBackspace();
            else
                ProcessNewCharacter(e.KeyChar);

            e.Handled = true;
        }

        private void ProcessNewCharacter(char character)
        {
            if (InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
            }

            _secureString.InsertAt(InputBox.SelectionStart, character);
            ResetDisplayCharacters(InputBox.SelectionStart + 1);
        }

        private void RemoveSelectedCharacters()
        {
            for (int i = 0; i < InputBox.SelectionLength; i++)
            {
                _secureString.RemoveAt(InputBox.SelectionStart);
            }
        }

        private void ResetDisplayCharacters(int caretPosition)
        {
            InputBox.Text = new string(_passwordChar, _secureString.Length);
            InputBox.SelectionStart = caretPosition;
        }

        private void ProcessBackspace()
        {
            if (InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
                ResetDisplayCharacters(InputBox.SelectionStart);
            }
            else if (InputBox.SelectionStart > 0)
            {
                _secureString.RemoveAt(InputBox.SelectionStart - 1);
                ResetDisplayCharacters(InputBox.SelectionStart - 1);
            }
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ProcessDelete();
                e.Handled = true;
            }
            else if (IsIgnorableKey(e.KeyCode))
            {
                e.Handled = true;
            }
        }

        private bool IsIgnorableKey(Keys key)
        {
            return key == Keys.Escape || key == Keys.Enter;
        }

        private void ProcessDelete()
        {
            if (InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
            }
            else if (InputBox.SelectionStart < InputBox.Text.Length)
            {
                _secureString.RemoveAt(InputBox.SelectionStart);
            }

            ResetDisplayCharacters(InputBox.SelectionStart);
        }
    }
}
