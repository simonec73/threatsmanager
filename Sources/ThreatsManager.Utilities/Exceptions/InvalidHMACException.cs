using System;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.Exceptions
{
    [Serializable]
    public class InvalidHMACException : Exception
    {
        public InvalidHMACException(string location) : base($"File '{location}' cannot be loaded due to invalid HMAC")
        {
        }
    }
}
