namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Type of the Property Viewer Block.
    /// </summary>
    public enum PropertyViewerBlockType
    {
        /// <summary>
        /// The property Block is to be shown as a multi-line read-only text field.
        /// </summary>
        /// <remarks>Block returns a single Block with no Image.
        /// Double click executes the action associated with the Block.</remarks>
        Label,
        /// <summary>
        /// The property Block is to be shown as a single-line read-only text field.
        /// </summary>
        /// <remarks>Block returns a single Block with no Image.
        /// Double click executes the action associated with the Block.</remarks>
        SingleLineLabel,
        /// <summary>
        /// The property Block is to be shown as a multi-line text field.
        /// </summary>
        /// <remarks>Block returns a single Block with no Image. Text is editable.
        /// Double click executes the action associated with the Block.</remarks>
        String,
        /// <summary>
        /// The property Block is to be shown as a single-line text field.
        /// </summary>
        /// <remarks>Block returns a single Block with no Image. Text is editable.
        /// Double click executes the action associated with the Block.</remarks>
        SingleLineString,
        /// <summary>
        /// The property Block is to be shown as a button showing only the Image.
        /// </summary>
        /// <remarks>Block returns a single button showing only the Image.
        /// Double click executes the action associated with the Block.</remarks>
        ImageButton,
        /// <summary>
        /// The property Block is to be shown as a button showing the Text and optionally the Image.
        /// </summary>
        /// <remarks>Block returns a single button showing the text and optionally the Image.
        /// Double click executes the action associated with the Block.</remarks>
        Button,
        /// <summary>
        /// The property Block is to be shown as a Date Time Picker.
        /// </summary>
        /// <returns>Block returns a single Block with no Image. Text must be converted to DateTime.
        /// Double click executes the action associated with the Block.</returns>
        DateTimePicker
    }
}