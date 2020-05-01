using System;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// Exception to be raised when an the Package Manager requires encryption.
    /// </summary>
    [Serializable]
    public class EncryptionRequiredException : Exception
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        public EncryptionRequiredException(ProtectionType protection) : base("An encryption is required (see RequiredProtection)")
        {
            RequiredProtection = protection;
        }

        /// <summary>
        /// Required Protection.
        /// </summary>
        public ProtectionType RequiredProtection { get; }
    }

}
