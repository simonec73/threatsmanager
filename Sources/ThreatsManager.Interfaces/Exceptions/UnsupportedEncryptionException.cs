using System;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// Exception to be raised when an encryption type is not supported by the specific Package Manager.
    /// </summary>
    [Serializable]
    public class UnsupportedEncryptionException : Exception
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        public UnsupportedEncryptionException() : base("The selected encryption type is not supported")
        {
        }
    }

}
