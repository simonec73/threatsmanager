using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by the containers of Flow Templates.
    /// </summary>
    public interface IFlowTemplatesContainer
    {
        /// <summary>
        /// Enumeration of Flow Templates.
        /// </summary>
        IEnumerable<IFlowTemplate> FlowTemplates { get; }
        
        /// <summary>
        /// Get a Flow Template given its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Flow Template.</param>
        /// <returns>Flow Template found in the container, otherwise null.</returns>
        IFlowTemplate GetFlowTemplate(Guid id);

        /// <summary>
        /// Adds the Flow Template passed as argument to the container.
        /// </summary>
        /// <param name="flowTemplate">Flow Template to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IFlowTemplate flowTemplate);

        /// <summary>
        /// Add an Flow Template to the Container.
        /// </summary>
        /// <param name="name">Name of the Flow Template.</param>
        /// <param name="description">Description of the Flow Template.</param>
        /// <param name="source">Source Flow for the Template.</param>
        /// <returns>New Flow Template.</returns>
        IFlowTemplate AddFlowTemplate(string name, string description, IDataFlow source = null);

        /// <summary>
        /// Remove the Flow Template whose identifier is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Flow Template.</param>
        /// <returns>True if the Flow Template has been found and removed, false otherwise.</returns>
        bool RemoveFlowTemplate(Guid id);
    }
}