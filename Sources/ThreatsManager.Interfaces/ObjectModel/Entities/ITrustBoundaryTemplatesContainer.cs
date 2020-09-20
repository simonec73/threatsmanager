using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by the containers of Trust Boundary Templates.
    /// </summary>
    public interface ITrustBoundaryTemplatesContainer
    {
        /// <summary>
        /// Enumeration of Trust Boundary Templates.
        /// </summary>
        IEnumerable<ITrustBoundaryTemplate> TrustBoundaryTemplates { get; }
        
        /// <summary>
        /// Get a Trust Boundary Template given its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Trust Boundary Template.</param>
        /// <returns>Trust Boundary Template found in the container, otherwise null.</returns>
        ITrustBoundaryTemplate GetTrustBoundaryTemplate(Guid id);

        /// <summary>
        /// Adds the Trust Boundary Template passed as argument to the container.
        /// </summary>
        /// <param name="trustBoundaryTemplate">Trust Boundary Template to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(ITrustBoundaryTemplate trustBoundaryTemplate);

        /// <summary>
        /// Add an Trust Boundary Template to the Container.
        /// </summary>
        /// <param name="name">Name of the Trust Boundary Template.</param>
        /// <param name="description">Description of the Trust Boundary Template.</param>
        /// <param name="source">Source Trust Boundary for the Template.</param>
        /// <returns>New Trust Boundary Template.</returns>
        ITrustBoundaryTemplate AddTrustBoundaryTemplate(string name, string description, ITrustBoundary source = null);

        /// <summary>
        /// Remove the Trust Boundary Template whose identifier is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Trust Boundary Template.</param>
        /// <returns>True if the Trust Boundary Template has been found and removed, false otherwise.</returns>
        bool RemoveTrustBoundaryTemplate(Guid id);
    }
}