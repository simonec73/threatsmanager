using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by containers of Threat Actors.
    /// </summary>
    public interface IThreatActorsContainer
    {
        /// <summary>
        /// Enumeration of the Threat Actors associated to the Container.
        /// </summary>
        IEnumerable<IThreatActor> ThreatActors { get; }

        /// <summary>
        /// Get a Threat Actor from the container.
        /// </summary>
        /// <param name="id">Identifier of the Threat Actor.</param>
        /// <returns>Threat Actor if found, otherwise null.</returns>
        IThreatActor GetThreatActor(Guid id);

        /// <summary>
        /// Get a default Threat Actor from the container.
        /// </summary>
        /// <param name="actor">Default Threat Actor to be searched.</param>
        /// <returns>Threat Actor if found, otherwise null.
        /// <para>DefaultActor.Unknown returns null by design.</para></returns>
        IThreatActor GetThreatActor(DefaultActor actor);

        /// <summary>
        /// Adds the Threat Actor passed as argument to the container.
        /// </summary>
        /// <param name="actor">Threat Actor to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatActor actor);

        /// <summary>
        /// Create a new default Threat Actor.
        /// </summary>
        /// <param name="actor">Default Threat Actor to be created.
        /// <para>It cannot be DefaultActor.Unknown: this would not create a new instance.</para></param>
        /// <returns>New Threat Actor.</returns>
        IThreatActor AddThreatActor(DefaultActor actor);

        /// <summary>
        /// Create a custom Threat Actor.
        /// </summary>
        /// <param name="name">Name of the Threat Actor.</param>
        /// <param name="description">Description of the Threat Actor.</param>
        /// <returns>New Threat Actor.</returns>
        IThreatActor AddThreatActor(string name, string description);

        /// <summary>
        /// Remove an existing Threat Actor.
        /// </summary>
        /// <param name="id">Identifier of the Threat Actor.</param>
        /// <param name="force">Forces the removal of the Threat Actor even if it is in use.</param>
        /// <returns>True if the Threat Actor has been deleted successfully, otherwise false.</returns>
        /// <remarks>Only Threat Actors that are not actually used, can be deleted. Removal could be forced, by setting the flag argument.</remarks>
        bool RemoveThreatActor(Guid id, bool force = false);

    }
}