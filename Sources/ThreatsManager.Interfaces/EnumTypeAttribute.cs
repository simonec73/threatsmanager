using System;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Enumeration used to associate a Type to enumerations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumTypeAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="type">Name of the associated Type.</param>
        public EnumTypeAttribute([Required] string type)
        {
            AssociatedType = type;
        }

        /// <summary>
        /// Associated Type.
        /// </summary>
        public string AssociatedType { get; }
    }
}
