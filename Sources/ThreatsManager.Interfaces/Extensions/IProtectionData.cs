namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface to represent the Protection Data to be used to encrypt or decrypt the data.
    /// </summary>
    public interface IProtectionData
    {
        /// <summary>
        /// Type of protection.
        /// </summary>
        ProtectionType ProtectionType { get; }
    }
}