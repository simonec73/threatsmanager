using System.Drawing;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A row for an List Item containing simple text.
    /// </summary>
    public class TextRow : ItemRow
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label of the Row.</param>
        /// <param name="text">Text to be shown.</param>
        /// <param name="textColor">[Optional] Color of the text. If missing, the default value will be used.</param>
        /// <param name="backColor">[Optional] Color of the background. If missing, the default value will be used.</param>
        public TextRow(string label, string text, KnownColor? textColor = null, KnownColor? backColor = null) : base(label)
        {
            Text = text;
            TextColor = textColor;
            BackColor = backColor;
        }

        /// <summary>
        /// Flag specifying if the row should be visible or not.
        /// </summary>
        public override bool Visible => !string.IsNullOrWhiteSpace(Text);

        /// <summary>
        /// Text of the Row.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Color of the Text.
        /// </summary>
        public KnownColor? TextColor { get; private set; }

        /// <summary>
        /// Color of the Background.
        /// </summary>
        public KnownColor? BackColor { get; private set; }
    }
}