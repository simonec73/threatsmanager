using System.Windows.Forms;
using DevComponents.DotNetBar;
using Keyoti.RapidSpell;

namespace ThreatsManager.Utilities.WinForms
{
    public class RichTextBoxSpellAsYouTypeAdapter : RichTextBoxIAYTAdapter
    {
        private RichTextBox _textBox;

        public RichTextBoxSpellAsYouTypeAdapter(RichTextBox rtb, bool contextMenu) : base(rtb, contextMenu)
        {
            _textBox = rtb;
        }

        public override int GetBaselineOffsetAtCharIndex(int i)
        {
            return (int)(base.GetBaselineOffsetAtCharIndex(i) * Dpi.Factor.Height);
        }

        public RichTextBox TextBox => _textBox;
    }
}
