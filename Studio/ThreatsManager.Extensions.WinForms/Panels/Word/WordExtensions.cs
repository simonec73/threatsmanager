using Syncfusion.DocIO.DLS;

namespace ThreatsManager.Extensions.Panels.Word
{
    internal static class WordExtensions
    {
        internal static bool IsToc(this WParagraph paragraph)
        {
            return paragraph?.StyleName.StartsWith("TOC ") ?? false;
        }

        internal static bool IsToc(this TextSelection textSelection)
        {
            return textSelection?.GetAsOneRange().OwnerParagraph.IsToc() ?? false;
        }
    }
}
