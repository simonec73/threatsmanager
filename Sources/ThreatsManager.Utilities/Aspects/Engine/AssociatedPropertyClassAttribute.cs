using System;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// Enumeration used to identify the associated Property object. It is used by Property Types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AssociatedPropertyClassAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="type">Type name of the associated Property Class.</param>
        public AssociatedPropertyClassAttribute(string type)
        {
            AssociatedType = type;
        }

        /// <summary>
        /// Associated Property class type name.
        /// </summary>
        public string AssociatedType { get; }
    }
}