using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Attribute used to specify the description of an Extension Interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ExtensionDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Description of the Extension.</param>
        public ExtensionDescriptionAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Description of the Extension.
        /// </summary>
        public string Text { get; }
    }
}
