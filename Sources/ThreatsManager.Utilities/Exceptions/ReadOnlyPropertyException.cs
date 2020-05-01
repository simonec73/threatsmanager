using System;

namespace ThreatsManager.Utilities.Exceptions
{
    /// <summary>
    /// Exception to be raised when someone tries to write a Locked Property.
    /// </summary>
    [Serializable]
    public class ReadOnlyPropertyException : Exception
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        public ReadOnlyPropertyException(string propertyName) : base($"Property '{propertyName}' is Locked and cannot be written.")
        {
        }
    }
}
