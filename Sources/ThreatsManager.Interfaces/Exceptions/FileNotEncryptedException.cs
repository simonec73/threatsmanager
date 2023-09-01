using System;
using System.IO;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// The file should be encrypted, but it is not.
    /// </summary>
    [Serializable]
    public class FileNotEncryptedException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="location">Location of the file.</param>
        public FileNotEncryptedException(string location) : base($"File '{Path.GetFileName(location)}' is supposed to be encrypted, but it is not")
        {
        }
    }
}
