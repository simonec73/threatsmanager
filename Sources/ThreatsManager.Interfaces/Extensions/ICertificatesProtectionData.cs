using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface to represent the Protection Data to be used to encrypt or decrypt the data for Certificates.
    /// </summary>
    public interface ICertificatesProtectionData : IProtectionData
    {
        /// <summary>
        /// Certificates whose owners will be allowed to open and read the data.
        /// </summary>
        IEnumerable<X509Certificate2> Certificates { get; }
    }
}