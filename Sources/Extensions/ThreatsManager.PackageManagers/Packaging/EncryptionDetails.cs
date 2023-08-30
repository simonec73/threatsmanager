using Newtonsoft.Json;

namespace ThreatsManager.PackageManagers.Packaging
{
    /// <summary>
    /// Algorithms used for the encryption.
    /// </summary>
    [JsonObject]
    public class EncryptionDetails
    {
        /// <summary>
        /// Algorithm used for encryption.
        /// </summary>
        /// <remarks>If not defined, the default AES256 is used.</remarks>
        [JsonProperty("algo")]
        public string Algorithm { get; set; }

        /// <summary>
        /// HMAC to use for checking the integrity.
        /// </summary>
        /// <remarks>If not defined, the default HMACSHA256 is used.</remarks>
        [JsonProperty("hmac")]
        public string HMAC { get; set; }

        /// <summary>
        /// Salt to use for deriving the encryption key.
        /// </summary>
        /// <remarks>If not defined, a random salt is generated.</remarks>
        [JsonProperty("salt")]
        public byte[] Salt { get; set; }

        /// <summary>
        /// Iterations used for deriving the encryption key.
        /// </summary>
        /// <remarks>If not defined, 20000 iterations will be applied.</remarks>
        [JsonProperty("iterations")]
        public int Iterations { get; set; }
    }
}