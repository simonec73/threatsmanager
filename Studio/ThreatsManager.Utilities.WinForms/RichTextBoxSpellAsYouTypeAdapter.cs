using System.Windows.Forms;
using DevComponents.DotNetBar;
using Keyoti.RapidSpell;

namespace ThreatsManager.Utilities.WinForms
{
    public class RichTextBoxSpellAsYouTypeAdapter : RichTextBoxIAYTAdapter
    {
        static readonly float _factor;
        private RichTextBox _textBox;

        static RichTextBoxSpellAsYouTypeAdapter()
        {
            _factor = Dpi.Factor.Height;
        }

        public RichTextBoxSpellAsYouTypeAdapter(RichTextBox rtb, bool contextMenu) : base(rtb, contextMenu)
        {
            _textBox = rtb;
        }

        public override int GetBaselineOffsetAtCharIndex(int i)
        {
            int offset = base.GetBaselineOffsetAtCharIndex(i);

            return (int)(offset * _factor);
        }

        public RichTextBox TextBox => _textBox;
    }
}
