using System;
using ThreatsManager.Interfaces.ObjectModel;

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
        public FileEncryptedException(string location) : base($"File '{location}' cannot be open because it is encrypted")
        {
        }
    }
}
