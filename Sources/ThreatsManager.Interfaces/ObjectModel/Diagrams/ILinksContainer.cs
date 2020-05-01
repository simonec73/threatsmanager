using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the containers of links.
    /// </summary>
    public interface ILinksContainer
    {
        /// <summary>
        /// Event raised when a Link is added to the Container.
        /// </summary>
        event Action<ILinksContainer, ILink> LinkAdded;

        /// <summary>
        /// Event raised when a Link is removed from the Container.
        /// </summary>
        event Action<ILinksContainer, IDataFlow> LinkRemoved;

        /// <summary>
        /// Enumeration of Links.
        /// </summary>
        IEnumerable<ILink> Links { get; }

        /// <summary>
        /// Get a Link associated to a Data Flow.
        /// </summary>
        /// <param name="dataFlowId">Identifier of the Data Flow whose link is to be found.</param>
        /// <returns>Link associated to the Data Flow if found, null otherwise.</returns>
        ILink GetLink(Guid dataFlowId);

        /// <summary>
        /// Adds the Link passed as argument to the container.
        /// </summary>
        /// <param name="link">Link to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(ILink link);

        /// <summary>
        /// Adds a Link associated to a Data Flow.
        /// </summary>
        /// <param name="dataFlow">Data Flow to be associated to the Link.</param>
        /// <returns>New Link.</returns>
        ILink AddLink(IDataFlow dataFlow);

        /// <summary>
        /// Removes a Link associated to a Data Flow.
        /// </summary>
        /// <param name="dataFlowId">Identifier of the Data Flow associated to the Link.</param>
        /// <returns>True if the Link has been removed successfully, false otherwise.</returns>
        bool RemoveLink(Guid dataFlowId);
    }
}