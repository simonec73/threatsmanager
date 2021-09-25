using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.QuantitativeRisk.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    /// <summary>
    /// The main interface for managing Facts.
    /// </summary>
    public class FactsManager
    {
        private static readonly FactsManager _instance = new FactsManager();
        private IThreatModel _model;
        private QuantitativeRiskSchemaManager _schemaManager;

        private FactsManager()
        {
        }

        static internal void RegisterThreatModel([NotNull] IThreatModel model)
        {
            _instance._model = model;
            _instance._schemaManager = new QuantitativeRiskSchemaManager(model);
        }

        /// <summary>
        /// The unique instance of the Facts Manager.
        /// </summary>
        public static FactsManager Instance => _instance;

        #region Used Facts.
        /// <summary>
        /// Lists all the known Facts.
        /// </summary>
        public IEnumerable<Fact> UsedFacts => _schemaManager?.GetFacts();

        /// <summary>
        /// Register a Fact as used.
        /// </summary>
        /// <param name="fact">The used fact.</param>
        /// <returns>True if the fact is successfully marked as used.</returns>
        /// <remarks>Marking a new Fact as used does not imply that it is added to the current Fact Provider.
        /// In fact, it is entirely possible to work without a Fact Provider.
        /// This method simply adds the Fact among the known Facts within the Threat Model, without registering it anywhere else.</remarks>
        public bool MakeFactUsed([NotNull] Fact fact)
        {
            return _schemaManager?.AddFact(fact) ?? false;
        }

        /// <summary>
        /// Register a Fact as unused.
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <returns>True if the fact is successfully marked as unused.</returns>
        /// <remarks>Marking a Fact as unused does not imply that it is removed or added to the current Fact Provider.
        /// In fact, it is entirely possible to work without a Fact Provider.
        /// This method simply removes the Fact from the known Facts within the Threat Model, without removing it from anywhere else.</remarks>
        public bool MakeFactNotUsed([NotNull] Fact fact)
        {
            return _schemaManager?.RemoveFact(fact) ?? false;
        }
        #endregion

        #region Facts Providers.
        /// <summary>
        /// Enumeration of the available Fact Providers.
        /// </summary>
        public IEnumerable<IFactProvider> FactProviders
        {
            get
            {
                return ExtensionUtils.GetExtensions<IFactProvider>();
            }
        }

        /// <summary>
        /// Get or set the current Fact Provider.
        /// </summary>
        public IFactProvider FactProvider
        {
            get;
            set;
        }
        #endregion

        #region Search.
        /// <summary>
        /// Returns the Facts belonging to a specific Context.
        /// </summary>
        /// <param name="context">Context for the Facts.</param>
        /// <param name="filter">[Optional] Filter to be applied.</param>
        /// <returns>Enumeration of the selected Facts.</returns>
        /// <remarks>Searches the facts among those marked as used and from the reference Fact Provider, if configured.</remarks>
        public IEnumerable<Fact> Search(string context, string filter = null)
        {
            IEnumerable<Fact> result = null;

            var usedFacts = _schemaManager?.GetFacts()?
                .Where(x => string.Compare(context, x.Context, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                            (string.IsNullOrWhiteSpace(filter) ||
                             (x.Text?.ToLower().Contains(filter.ToLower()) ?? false) ||
                             (x.Source?.ToLower().Contains(filter.ToLower()) ?? false)))
                .ToArray();

            var facts = FactProvider?.GetFacts(context, filter)?.ToArray();

            if (usedFacts?.Any() ?? false)
            {
                if (facts?.Any() ?? false)
                {
                    result = usedFacts.Union(facts).Distinct(new FactComparer());
                }
                else
                {
                    result = usedFacts;
                }
            }
            else
            {
                if (facts?.Any() ?? false)
                {
                    result = facts;
                }
            }

            return result;
        }
        #endregion
    }
}
