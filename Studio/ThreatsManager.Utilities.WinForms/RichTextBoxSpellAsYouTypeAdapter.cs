using System;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Keyoti.RapidSpell;

namespace ThreatsManager.Utilities.WinForms
{
    public class RichTextBoxSpellAsYouTypeAdapter : RichTextBoxIAYTAdapter, IDisposable
    {
        private RichTextBox _textBox;

        public RichTextBoxSpellAsYouTypeAdapter(RichTextBox rtb, bool contextMenu) : base(rtb, contextMenu)
        {
            _textBox = rtb;
        }

        public override int GetBaselineOffsetAtCharIndex(int i)
        {
            return base.GetBaselineOffsetAtCharIndex(i);
        }

        public void Dispose()
        {
            RemoveEventHandlers();
        }

        public RichTextBox TextBox => _textBox;
    }
}
