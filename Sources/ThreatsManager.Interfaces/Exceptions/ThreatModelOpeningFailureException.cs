using System;

namespace ThreatsManager.Interfaces.Exceptions
{
    /// <summary>
    /// Exception raised when the Threat Model cannot be opened for some reason.
    /// </summary>
    /// <remarks>It is typically associated with serialization issues.</remarks>
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