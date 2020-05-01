using System;

namespace ThreatsManager.Engine
{
    /// <summary>
    /// Attribute used to add initials to classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeInitialAttribute : Attribute
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="initial">Initial to be used for the type value.</param>
        public TypeInitialAttribute(string initial)
        {
            Initial = initial;
        }

        /// <summary>
        /// Label for the type.
        /// </summary>
        public string Initial { get; }
    }
}