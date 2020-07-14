using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by the containers of Data Flows.
    /// </summary>
    public interface IDataFlowsContainer
    {
        /// <summary>
        /// Event raised when a Threat Event is associated to a Data Flow.
        /// </summary>
        event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAddedToDataFlow;

        /// <summary>
        /// Event raised when a Threat Event is removed from a Data Flow.
        /// </summary>
        event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemovedFromDataFlow;

        /// <summary>
        /// Enumeration of Data Flows.
        /// </summary>
        IEnumerable<IDataFlow> DataFlows { get; }
        
        /// <summary>
        /// Get a Data Flow by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Data Flow.</param>
        /// <returns>Data Flow found in the container, otherwise null.</returns>
        IDataFlow GetDataFlow(Guid id);

        /// <summary>
        /// Get the Data Flows that are associated to the specified Source and Target.
        /// </summary>
        /// <param name="sourceId">Identifier of the Source.</param>
        /// <param name="targetId">Identifier of the Target.</param>
        /// <returns>Enumeration of the Data Flows.</returns>
        IDataFlow GetDataFlow(Guid sourceId, Guid targetId);

        /// <summary>
        /// Adds the Data Flow passed as argument to the container.
        /// </summary>
        /// <param name="dataFlow">Data Flow to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IDataFlow dataFlow);

        /// <summary>
        /// Add a Data Flow to the Container.
        /// </summary>
        /// <param name="name">Name of the Data Flow.</param>
        /// <param name="sourceId">Identifier of the Source.</param>
        /// <param name="targetId">Identifier of the Target.</param>
        /// <returns>New Data Flow. If a Data Flow having the same Source and Target already exist, then the new instance is not created and it returns null.</returns>
        IDataFlow AddDataFlow(string name, Guid sourceId, Guid targetId);
        
        /// <summary>
        /// Add a Data Flow to the Container and associating it to the Flow Template passed as argument.
        /// </summary>
        /// <param name="name">Name of the Data Flow.</param>
        /// <param name="sourceId">Identifier of the Source.</param>
        /// <param name="targetId">Identifier of the Target.</param>
        /// <param name="template">Template to associate to the Data Flow.</param>
        /// <returns>New Data Flow. If a Data Flow having the same Source and Target already exist, then the new instance is not created and it returns null.</returns>
        IDataFlow AddDataFlow(string name, Guid sourceId, Guid targetId, IFlowTemplate template);

        /// <summary>
        /// Remove the Data Flow whose identifier is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Data Flow.</param>
        /// <returns>True if the Data Flow has been found and removed, false otherwise.</returns>
        /// <remarks>It also removes the eventual Links associated to the Data Flow.</remarks>
        bool RemoveDataFlow(Guid id);
    }
}