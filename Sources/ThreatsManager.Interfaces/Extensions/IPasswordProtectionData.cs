using System.Security;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface to represent the Protection Data to be used to encrypt or decrypt the data with a Password.
    /// </summary>
    public interface IPasswordProtectionData : IProtectionData
    {
        /// <summary>
        /// Password to be used to encrypt the data.
        /// </summary>
        SecureString Password { get; }

        /// <summary>
        /// Algorithm used for encryption.
        /// </summary>
        /// <remarks>If not defined, the default AES256 is used.</remarks>
        string Algorithm { get; }

        /// <summary>
        /// HMAC to use for checking the integrity.
        /// </summary>
        /// <remarks>If not defined, the default HMACSHA256 is used.</remarks>
        string HMAC { get; }

        /// <summary>
        /// Salt to use for deriving the encryption key.
        /// </summary>
        /// <remarks>If not defined, a random salt is generated.</remarks>
        byte[] Salt { get; }

        /// <summary>
        /// Iterations used for deriving the encryption key.
        /// </summary>
        /// <remarks>If not defined, 20000 iterations will be applied.</remarks>
        int Iterations { get; }
    }
}