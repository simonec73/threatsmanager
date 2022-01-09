using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    /// <summary>
    /// Interface defining a module providing Facts to be leveraged for Quantitative Analysis.
    /// </summary>
    [ExtensionDescription("Fact Provider")]
    public interface IFactProvider : IExtension
    {
        /// <summary>
        /// Get configuration parameters for the Fact Provider.
        /// </summary>
        /// <returns>Definition of the required parameters.</returns>
        IEnumerable<FactProviderParameter> GetParameters();

        /// <summary>
        /// Set configuration parameters for the Fact Provider.
        /// </summary>
        /// <param name="parameters">Dictionary containing the parameters and their value.</param>
        void SetParameters(IDictionary<string, string> parameters);

        /// <summary>
        /// Get a Fact given is identifier.
        /// </summary>
        /// <param name="id">Fact identifier.</param>
        /// <returns>Fact, if retrieved, otherwise null.</returns>
        Fact GetFact(Guid id);

        /// <summary>
        /// Registers a fact in the Fact Provider.
        /// </summary>
        /// <param name="fact">Fact to be registered.</param>
        /// <returns>True if the registration succeeds, false otherwise.</returns>
        /// <remarks>If the Fact is new, then it is added, otherwise it is updated.</remarks>
        bool RegisterFact(Fact fact);

        /// <summary>
        /// Removes a fact from the Fact Provider.
        /// </summary>
        /// <param name="fact">Fact to be removed from the Fact Provider.</param>
        /// <returns>True if the removal succeeds, false otherwise.</returns>
        bool RemoveFact(Fact fact);

        /// <summary>
        /// Removes a fact from the Fact Provider.
        /// </summary>
        /// <param name="factId">Identifier of the fact to be removed from the Fact Provider.</param>
        /// <returns>True if the removal succeeds, false otherwise.</returns>
        bool RemoveFact(Guid factId);

        /// <summary>
        /// Gets the available Contexts.
        /// </summary>
        IEnumerable<string> Contexts { get; }

        /// <summary>
        /// Gets the list of already defined Tags.
        /// </summary>
        IEnumerable<string> Tags { get; }

        /// <summary>
        /// Returns the Facts belonging to a specific Context.
        /// </summary>
        /// <param name="context">Context for the Facts.</param>
        /// <param name="tags">[Optional] Tags for the Facts.</param>
        /// <param name="filter">[Optional] Filter to be applied.</param>
        /// <param name="includeObsolete">[Optional] Flag specifying if obsolete Facts should be included.</param>
        /// <returns>Enumeration of the Facts assigned to the specified context,
        /// containing at least a tag from the provided list, and containing the text provided as filter in their Text or Notes.</returns>
        IEnumerable<Fact> GetFacts(string context, IEnumerable<string> tags = null, string filter = null, bool includeObsolete = false);
    }
}
