using System;
using System.IO;

namespace ThreatsManager.Interfaces.Exceptions
{
    [Serializable]
    public class InvalidHMACException : Exception
    {
        public InvalidHMACException(string location) : base($"File '{Path.GetFileName(location)}' cannot be loaded because the password is not correct or because the file is corrupted")
        {
        }
    }
}
