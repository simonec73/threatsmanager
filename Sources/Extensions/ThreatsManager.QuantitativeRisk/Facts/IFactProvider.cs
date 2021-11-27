using System;
using System.Collections.Generic;
using System.Text;
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
        /// Returns the Facts belonging to a specific Context.
        /// </summary>
        /// <param name="context">Context for the Facts.</param>
        /// <param name="filter">[Optional] Filter to be applied.</param>
        /// <returns>Enumeration of the selected Facts.</returns>
        IEnumerable<Fact> GetFacts(string context, string filter = null);
    }
}
