namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Type of answers received from the user.
    /// </summary>
    public enum AnswerType
    {
        /// <summary>
        /// No valid answer received.
        /// </summary>
        None,
        /// <summary>
        /// User pressed the OK button.
        /// </summary>
        Ok,
        /// <summary>
        /// User pressed the Yes button.
        /// </summary>
        Yes,
        /// <summary>
        /// User pressed the No button.
        /// </summary>
        No,
        /// <summary>
        /// User pressed the Cancel button.
        /// </summary>
        Cancel
    }
}