using Keyoti.RapidSpell;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreatsManager.Utilities.WinForms
{
    public static class SpellExtensions
    {
        public static RichTextBoxSpellAsYouTypeAdapter AddSpellCheck(this RapidSpellAsYouType spellAsYouType, 
            TextBoxBase control)
        {
            RichTextBoxSpellAsYouTypeAdapter result = null;

            if (control is RichTextBox richTextBox)
            {
                result = new RichTextBoxSpellAsYouTypeAdapter(richTextBox,
                    spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase);
                spellAsYouType.AddTextComponent(result);
            }
            else
            {
                spellAsYouType.AddTextBoxBase(control);
            }

            return result;
        }
    }
}
