using System;

namespace ThreatsManager.Engine
{
    /// <summary>
    /// Attribute used to add labels to classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeLabelAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="label">Label to be used for the type value.</param>
        public TypeLabelAttribute(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Label for the type.
        /// </summary>
        public string Label { get; }
    }
}
