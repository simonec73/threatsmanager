using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Universal identifier used to identify the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UniversalIdentifierAttribute : Attribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Universal name for the class.</param>
        public UniversalIdentifierAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Universal name for the class.
        /// </summary>
        public string Name { get; }
    }
}
