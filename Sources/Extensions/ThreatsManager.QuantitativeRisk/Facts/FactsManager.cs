using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
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
            _instance.Provider = _instance._schemaManager.Provider;
        }

        /// <summary>
        /// The unique instance of the Facts Manager.
        /// </summary>
        public static FactsManager Instance => _instance;

        /// <summary>
        /// Current Context.
        /// </summary>
        public string Context => _schemaManager.Context;

        /// <summary>
        /// Get all the known Contexts.
        /// </summary>
        public IEnumerable<string> Contexts => Merge(Provider?.Contexts, _schemaManager?.Contexts);

        /// <summary>
        /// Get all the known Contexts.
        /// </summary>
        public IEnumerable<string> Tags => Merge(Provider?.Tags, _schemaManager?.Tags);

        private IEnumerable<string> Merge(IEnumerable<string> first, IEnumerable<string> second)
        {
            IEnumerable<string> result = null;

            var list = new List<string>();
            var items = first?.ToArray();
            if (items?.Any() ?? false)
                list.AddRange(items);
            items = second?.ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    if (!list.Contains(item))
                        list.Add(item);
                }
            }

            if (list.Any())
                result = list.OrderBy(x => x).AsEnumerable();

            return result;
        }

        #region Known Facts.
        /// <summary>
        /// Get the Fact with a specific identifier.
        /// </summary>
        /// <param name="id">Identifier of the Fact to be searched.</param>
        /// <returns>Fact, if found.</returns>
        /// <remarks>The Fact is searched among the known Facts and then from the Provider Facts.</remarks>
        public Fact Get(Guid id)
        {
            Fact result = _schemaManager?.GetFacts()?.FirstOrDefault(x => x.Id == id);

            if (result == null)
                result = Provider?.GetFact(id);
            
            return result;
        }

        /// <summary>
        /// Lists all the known Facts.
        /// </summary>
        public IEnumerable<Fact> KnownFacts => _schemaManager?.GetFacts();

        /// <summary>
        /// Register a Fact as used.
        /// </summary>
        /// <param name="fact">The used fact.</param>
        /// <returns>True if the fact is successfully marked as used.</returns>
        /// <remarks>Marking a new Fact as used does not imply that it is added to the current Fact Provider.
        /// In fact, it is entirely possible to work without a Fact Provider.
        /// This method simply adds the Fact among the known Facts within the Threat Model, without registering it anywhere else.</remarks>
        public bool MakeFactKnown([NotNull] Fact fact)
        {
            return _schemaManager?.AddFact(fact) ?? false;
        }

        /// <summary>
        /// Register a Fact as unused.
        /// </summary>
        /// <param name="fact">The fact to be removed.</param>
        /// <returns>True if the fact is successfully marked as unused. It returns false if the Fact is not known or if it is used anywhere.</returns>
        /// <remarks>Marking a Fact as unused does not imply that it is removed from or added to the current Fact Provider.
        /// In fact, it is entirely possible to work without a Fact Provider.
        /// This method simply removes the Fact from the known Facts within the Threat Model, without removing it from anywhere else.</remarks>
        public bool MakeFactUnknown([NotNull] Fact fact)
        {
            bool result = false;

            if (!IsUsed(fact))
                result = _schemaManager?.RemoveFact(fact) ?? false;

            return result;
        }

        /// <summary>
        /// Checks if a Fact is used in the Threat Model.
        /// </summary>
        /// <param name="fact">Fact to be searched.</param>
        /// <returns>True if the Fact is used anywhere, false otherwise.</returns>
        public bool IsUsed([NotNull] Fact fact)
        {
            return _model.GetThreatEvents()?.Any(x => x.Scenarios?.Any(y => IsUsed(fact, y)) ?? false) ?? false;
        }

        /// <summary>
        /// Checks if a Fact is used in a Threat Event Scenario.
        /// </summary>
        /// <param name="fact">Fact to be searched.</param>
        /// <returns>True if the Fact is used in the Scenario, false otherwise.</returns>
        public bool IsUsed([NotNull] Fact fact, [NotNull] IThreatEventScenario scenario)
        {
            var risk = _schemaManager.GetRisk(scenario);
            var result = (risk?.AssociatedFacts?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.AssociatedFacts?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.ThreatEventFrequency?.AssociatedFacts?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.ThreatEventFrequency?.ContactFrequency?.AssociatedFacts
                       ?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.ThreatEventFrequency?.ProbabilityOfAction?.AssociatedFacts
                       ?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.Vulnerability?.AssociatedFacts?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.Vulnerability?.Difficulty?.AssociatedFacts?.Contains(fact.Id) ?? false) ||
                   (risk?.LossEventFrequency?.Vulnerability?.ThreatCapability?.AssociatedFacts?.Contains(fact.Id) ??
                    false) ||
                   (risk?.LossMagnitude?.AssociatedFacts?.Contains(fact.Id) ?? false) ||
                   (risk?.LossMagnitude?.SecondaryLossEventFrequency?.AssociatedFacts?.Contains(fact.Id) ?? false);

            if (!result)
            {
                var primaryLosses = risk?.LossMagnitude?.PrimaryLosses?.ToArray();
                if (primaryLosses?.Any() ?? false)
                {
                    foreach (var loss in primaryLosses)
                    {
                        if (loss.AssociatedFacts?.Contains(fact.Id) ?? false)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            if (!result)
            {
                var secondaryLosses = risk?.LossMagnitude?.SecondaryLosses?.ToArray();
                if (secondaryLosses?.Any() ?? false)
                {
                    foreach (var loss in secondaryLosses)
                    {
                        if (loss.AssociatedFacts?.Contains(fact.Id) ?? false)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region Fact Providers.
        /// <summary>
        /// Enumeration of the available Fact Providers.
        /// </summary>
        public IEnumerable<IFactProvider> Providers
        {
            get
            {
                return ExtensionUtils.GetExtensions<IFactProvider>();
            }
        }

        /// <summary>
        /// Get or set the current Fact Provider.
        /// </summary>
        public IFactProvider Provider
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
        /// <param name="tags">Tags for the Facts.</param>
        /// <param name="filter">[Optional] Filter to be applied.</param>
        /// <returns>Enumeration of the selected Facts.</returns>
        /// <remarks>Searches the facts among those marked as used and from the reference Fact Provider, if configured.</remarks>
        public IEnumerable<Fact> Search(string context, IEnumerable<string> tags = null, string filter = null)
        {
            IEnumerable<Fact> result = null;

            var usedFacts = _schemaManager?.GetFacts()?
                .Where(x => string.Compare(context, x.Context, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                            (string.IsNullOrWhiteSpace(filter) ||
                             (x.Name?.ToLower().Contains(filter.ToLower()) ?? false) ||
                             (x.Source?.ToLower().Contains(filter.ToLower()) ?? false) ||
                             (x.Details?.ToLower().Contains(filter.ToLower()) ?? false)))
                .ToArray();

            var facts = Provider?.GetFacts(context, tags, filter)?.ToArray();

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
