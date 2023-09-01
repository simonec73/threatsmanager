using System;
using System.IO;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// The file cannot be opened because it is encrypted.
    /// </summary>
    [Serializable]
    public class FileEncryptedException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location of the file that cannot be opened.</param>
        public FileEncryptedException(string location) : base($"File '{Path.GetFileName(location)}' cannot be opened because it is encrypted")
        {
        }
    }
}
