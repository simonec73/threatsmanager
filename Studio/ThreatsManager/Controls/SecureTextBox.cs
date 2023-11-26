using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows.Forms;

namespace ThreatsManager.Controls
{
    public partial class SecureTextBox : UserControl
    {
        private char _passwordChar = '●';
        private readonly SecureString _secureString = new SecureString();
        private readonly List<CharacterCategory> _category = new List<CharacterCategory>();

        public event Action<SecureTextBox> SecureTextChanged;

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

        public PasswordScore Score
        {
            get
            {
                int count = _category.Count;

                PasswordScore result;
                if (count < 4)
                {
                    result = PasswordScore.Negligible;
                }
                else if (count <= 6)
                {
                    result = PasswordScore.VeryWeak;
                }
                else
                {
                    var score = Math.DivRem(count, 4, out var remainder) - 1;

                    var digit = _category.Where(x => x == CharacterCategory.Digit).Count();
                    var lowercase = _category.Where(x => x == CharacterCategory.AlphaLowercase).Count();
                    var uppercase = _category.Where(x => x == CharacterCategory.AlphaUppercase).Count();
                    var common = _category.Where(x => x == CharacterCategory.CommonSymbol).Count();
                    var other = _category.Where(x => x == CharacterCategory.Other).Count();

                    if (digit > 0)
                        score++;
                    if (digit > 3)
                        score++;
                    if (lowercase > 0)
                        score++;
                    if (lowercase > 3)
                        score++;
                    if (uppercase > 0)
                        score++;
                    if (uppercase > 3)
                        score++;
                    if (common > 0)
                        score++;
                    if (common > 3)
                        score++;
                    if (other > 0)
                        score++;
                    if (other > 3)
                        score++;

                    if (score < 6)
                        result = PasswordScore.VeryWeak;
                    else if (score < 8)
                        result = PasswordScore.Weak;
                    else if (score < 10)
                        result = PasswordScore.Medium;
                    else if (score < 12)
                        result = PasswordScore.Strong;
                    else
                        result = PasswordScore.VeryStrong;
                }

                return result;
            }
        }

        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            if (e.KeyChar == '\b')
                ProcessBackspace();
            else if (e.KeyChar == '\r')
                e.Handled = false;
            else
                ProcessNewCharacter(e.KeyChar);
        }

        private void ResetDisplayCharacters(int caretPosition)
        {
            InputBox.Text = new string(_passwordChar, _secureString.Length);
            InputBox.SelectionStart = caretPosition;
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

        private void ProcessNewCharacter(char character)
        {
            if (InputBox.SelectionLength > 0)
            {
                RemoveSelectedCharacters();
            }

            _secureString.InsertAt(InputBox.SelectionStart, character);
            _category.Insert(InputBox.SelectionStart, CalculateCategory(character));
            ResetDisplayCharacters(InputBox.SelectionStart + 1);

            SecureTextChanged?.Invoke(this);
        }

        private void RemoveSelectedCharacters()
        {
            for (int i = 0; i < InputBox.SelectionLength; i++)
            {
                _secureString.RemoveAt(InputBox.SelectionStart);
                _category.RemoveAt(InputBox.SelectionStart);
            }

            SecureTextChanged?.Invoke(this);
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
                _category.RemoveAt(InputBox.SelectionStart - 1);
                ResetDisplayCharacters(InputBox.SelectionStart - 1);
            }

            SecureTextChanged?.Invoke(this);
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
                _category.RemoveAt(InputBox.SelectionStart);

                SecureTextChanged?.Invoke(this);
            }

            ResetDisplayCharacters(InputBox.SelectionStart);
        }

        private CharacterCategory CalculateCategory(char character)
        {
            var result = CharacterCategory.Unknown;

            if (character >= '0' && character <= '9')
                result = CharacterCategory.Digit;
            else if (character >= 'A' && character <= 'Z')
                result = CharacterCategory.AlphaUppercase;
            else if (character >= 'a' && character <= 'z')
                result = CharacterCategory.AlphaLowercase;
            else if (character >= '!' && character <= '/')
                result = CharacterCategory.CommonSymbol;
            else
                result = CharacterCategory.Other;

            return result;
        }
    }
}
