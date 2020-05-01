namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Secure Package Manager Extensions.
    /// </summary>
    /// <remarks>Secure Package Manager extensions implement ways to save and load Threat Models,
    /// adding encryption protection.</remarks>
    public interface ISecurePackageManager : IPackageManager
    {
        /// <summary>
        /// Supported protection types.
        /// </summary>
        ProtectionType RequiredProtection { get; }

        /// <summary>
        /// Set the Protection Data.
        /// </summary>
        /// <param name="protectionData">Protection data to be used for encryption or decryption.</param>
        void SetProtectionData(IProtectionData protectionData);
    }
}