using System.Windows.Forms;
using DevComponents.DotNetBar;
using Keyoti.RapidSpell;

namespace ThreatsManager.Utilities.WinForms
{
    public class RichTextBoxSpellAsYouTypeAdapter : RichTextBoxIAYTAdapter
    {
        static readonly float _factor;

        static RichTextBoxSpellAsYouTypeAdapter()
        {
            _factor = Dpi.Factor.Height;
        }

        public RichTextBoxSpellAsYouTypeAdapter(RichTextBox rtb, bool contextMenu) : base(rtb, contextMenu)
        {
        }

        public override int GetBaselineOffsetAtCharIndex(int i)
        {
            int offset = base.GetBaselineOffsetAtCharIndex(i);

            return (int)(offset * _factor);
        }
    }
}
