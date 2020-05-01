using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Enumeration used to add labels to enumerations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumLabelAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="label">Label to be used for the enumeration value.</param>
        public EnumLabelAttribute(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Label for the enumeration.
        /// </summary>
        public string Label { get; }
    }
}
