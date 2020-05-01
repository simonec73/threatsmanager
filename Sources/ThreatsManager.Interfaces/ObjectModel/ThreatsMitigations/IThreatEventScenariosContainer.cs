using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by containers of Threat Event Scenarios,
    /// which detail the specific situations that may represent a concern for the solution.
    /// This is typically dependent on the type of attacker.
    /// </summary>
    public interface IThreatEventScenariosContainer
    {
        /// <summary>
        /// Event raised when a Threat Event Scenario is added to the Container.
        /// </summary>
        event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioAdded;

        /// <summary>
        /// Event raised when a Threat Event Scenario is removed from the Container.
        /// </summary>
        event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioRemoved;

        /// <summary>
        /// Enumeration of the Threat Event Scenarios associated to the Container.
        /// </summary>
        IEnumerable<IThreatEventScenario> Scenarios { get; }

        /// <summary>
        /// Get a Threat Event Scenario from the container, given its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Threat Event Scenario.</param>
        /// <returns>Searched Threat Event Scenario, if found, otherwise null.</returns>
        IThreatEventScenario GetScenario(Guid id);
        
        /// <summary>
        /// Adds the Threat Event Scenario passed as argument to the container.
        /// </summary>
        /// <param name="scenario">Threat Event Scenario to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatEventScenario scenario);

        /// <summary>
        /// Creates a new Threat Event Scenario.
        /// </summary>
        /// <param name="threatActor">Threat Actor to be associated to the Threat Event Scenario.</param>
        /// <param name="severity">Severity to be associated to the new Threat Event Scenario.</param>
        /// <param name="name">[Optional] Name of the Scenario. If missing, the name of the Threat Actor will be used.</param>
        /// <returns>The new Threat Event Scenario.</returns>
        IThreatEventScenario AddScenario(IThreatActor threatActor, ISeverity severity, string name = null);

        /// <summary>
        /// Remove an existing Threat Event Scenario.
        /// </summary>
        /// <param name="id">Identifier of the Threat Event Scenario.</param>
        /// <returns>True if the Threat Event Scenario has been removed successfully, otherwise false.</returns>
        bool RemoveScenario(Guid id);
    }
}