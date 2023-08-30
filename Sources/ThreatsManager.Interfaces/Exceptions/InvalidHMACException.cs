using System;

namespace ThreatsManager.Interfaces.Exceptions
{
    [Serializable]
    public class InvalidHMACException : Exception
    {
        public InvalidHMACException(string location) : base($"File '{location}' cannot be loaded due to invalid HMAC")
        {
        }
    }
}
