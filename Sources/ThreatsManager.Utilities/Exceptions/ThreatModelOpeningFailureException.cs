using System;

namespace ThreatsManager.Utilities.Exceptions
{
    [Serializable]
    public class ThreatModelOpeningFailureException : Exception
    {
        public ThreatModelOpeningFailureException()
        {

        }

        public ThreatModelOpeningFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}