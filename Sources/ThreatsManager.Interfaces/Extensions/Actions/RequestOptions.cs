namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Type of requests to be sent.
    /// </summary>
    public enum RequestOptions
    {
        /// <summary>
        /// Possible answers are Yes and No.
        /// </summary>
        YesNo,
        /// <summary>
        /// Possible answers are Yes, No and Cancel.
        /// </summary>
        YesNoCancel,
        /// <summary>
        /// Possible answers are Ok and Cancel.
        /// </summary>
        OkCancel
    }
}