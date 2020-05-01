using System;
using System.Drawing;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Text color associated to an enumeration value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class TextColorAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="color">Color.</param>
        public TextColorAttribute(KnownColor color)
        {
            Color = color;
        }

        /// <summary>
        /// Color.
        /// </summary>
        public KnownColor Color { get; }
    }
}