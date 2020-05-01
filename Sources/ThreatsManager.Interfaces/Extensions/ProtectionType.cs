using System;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Type of protection to be used.
    /// </summary>
    [Flags]
    public enum ProtectionType
    {
        /// <summary>
        /// No protection is required.
        /// </summary>
        None = 0,

        /// <summary>
        /// Password protection is required.
        /// </summary>
        Password = 1,

        /// <summary>
        /// Encryption with certificates is required.
        /// </summary>
        Certificates = 2
    }
}