using System;
using System.IO;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// Exception raised when an invalid HMAC is found.
    /// </summary>
    [Serializable]
    public class InvalidHMACException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">File name of the threat model which cannot be loeaded due to the invalid HMAC.</param>
        public InvalidHMACException(string location) : base($"File '{Path.GetFileName(location)}' cannot be loaded because the password is not correct or because the file is corrupted")
        {
        }
    }
}
