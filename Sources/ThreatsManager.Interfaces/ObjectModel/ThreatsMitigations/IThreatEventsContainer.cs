using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by containers of Threat Events.
    /// </summary>
    public interface IThreatEventsContainer
    {
        /// <summary>
        /// Event raised when a Threat Event is added to the Container.
        /// </summary>
        event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAdded;

        /// <summary>
        /// Event raised when a Threat Event is removed from the Container.
        /// </summary>
        event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemoved;

        /// <summary>
        /// Enumeration of the Threat Events associated to the Container.
        /// </summary>
        IEnumerable<IThreatEvent> ThreatEvents { get; }
        
        /// <summary>
        /// Get a Threat Event from the container, given its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Threat Event.</param>
        /// <returns>Searched Threat Event, if found, otherwise null.</returns>
        IThreatEvent GetThreatEvent(Guid id);

        /// <summary>
        /// Get a Threat Event from the container, given the identifier of the corresponding Threat Type.
        /// </summary>
        /// <param name="threatTypeId">Identifier of the Threat Type associated to the Threat Event.</param>
        /// <returns>Searched Threat Event, if found, otherwise null.</returns>
        IThreatEvent GetThreatEventByThreatType(Guid threatTypeId);

        /// <summary>
        /// Adds the Threat Event passed as argument to the container.
        /// </summary>
        /// <param name="threatEvent">Threat Event to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatEvent threatEvent);

        /// <summary>
        /// Creates a new Threat Event from a Threat Type passed as argument.
        /// </summary>
        /// <param name="threatType">Source Threat Type.</param>
        /// <returns>The new Threat Event if there is no event of the same Threat Type, otherwise null.</returns>
        IThreatEvent AddThreatEvent(IThreatType threatType);

        /// <summary>
        /// Delete an existing Threat Event.
        /// </summary>
        /// <param name="id">Identifier of the Threat Event.</param>
        /// <returns>True if the Threat Event has been deleted successfully, otherwise false.</returns>
        bool RemoveThreatEvent(Guid id);
    }
}