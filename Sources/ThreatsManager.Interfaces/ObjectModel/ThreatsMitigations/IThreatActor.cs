using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface representing a Threat Actor.
    /// </summary>
    public interface IThreatActor : IIdentity, IPropertiesContainer, IThreatModelChild, IDirty
    {
        /// <summary>
        /// Type of Threat Actor.
        /// </summary>
        DefaultActor ActorType { get; }

        /// <summary>
        /// Creates a duplicate of the current Threat Actor and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Threat Actor.</returns>
        IThreatActor Clone(IThreatActorsContainer container);
    }
}