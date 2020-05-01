using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Enumeration used to specify order of the enumerations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumOrderAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="order">Order of the item in the enumeration.</param>
        public EnumOrderAttribute(int order)
        {
            Order = order;
        }

        /// <summary>
        /// Order of the enumeration.
        /// </summary>
        public int Order { get; }
    }
}
