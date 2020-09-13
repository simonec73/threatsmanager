using System;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface used to find a Threat Event.
    /// </summary>
    public interface IThreatEventFinder
    {
        /// <summary>
        /// Finds a Threat Event given its Identifier.
        /// </summary>
        /// <param name="id">Identifier of the Threat Event.</param>
        /// <returns>Threat Event, if found, otherwise null.</returns>
        /// <remarks>The Threat Event is searched in any object associated to the Threat Event Finder.</remarks>
        IThreatEvent FindThreatEvent(Guid id);
    }
}