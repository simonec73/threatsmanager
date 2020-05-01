using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Enumeration used to add a description to enumerations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="text">Text to be used as description for the enumeration value.</param>
        public EnumDescriptionAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Label for the enumeration.
        /// </summary>
        public string Text { get; }
    }
}
