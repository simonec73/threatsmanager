using System;
using System.Drawing;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Background color associated to an enumeration value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class BackColorAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="color">Color.</param>
        public BackColorAttribute(KnownColor color)
        {
            Color = color;
        }

        /// <summary>
        /// Color name.
        /// </summary>
        public KnownColor Color { get; }
    }
}