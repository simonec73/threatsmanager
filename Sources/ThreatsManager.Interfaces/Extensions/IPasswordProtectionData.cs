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
    }
}