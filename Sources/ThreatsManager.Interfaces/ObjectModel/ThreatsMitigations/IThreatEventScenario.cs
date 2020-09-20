using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Scenario for a specific Threat Event.
    /// </summary>
    public interface IThreatEventScenario : IIdentity, IPropertiesContainer, IThreatModelChild, IThreatEventChild, IDirty
    {
        /// <summary>
        /// Identifier of the Severity.
        /// </summary>
        int SeverityId { get; }

        /// <summary>
        /// Severity of the specific Scenario.
        /// </summary>
        ISeverity Severity { get; set; }

        /// <summary>
        /// Identifier of the Threat Actor.
        /// </summary>
        Guid ActorId { get; }

        /// <summary>
        /// Threat Actor.
        /// </summary>
        IThreatActor Actor { get; set; }

        /// <summary>
        /// Motivation of the Actor.
        /// </summary>
        string Motivation { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Scenario and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Scenario.</returns>
        IThreatEventScenario Clone(IThreatEventScenariosContainer container);
    }
}