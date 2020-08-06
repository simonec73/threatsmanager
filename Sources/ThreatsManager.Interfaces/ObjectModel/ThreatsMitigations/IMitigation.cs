using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface to define a mitigation.
    /// </summary>
    public interface IMitigation : IIdentity, IPropertiesContainer, IThreatModelChild, IDirty
    {
        /// <summary>
        /// Type of Security Control.
        /// </summary>
        SecurityControlType ControlType { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Mitigation and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Mitigation.</returns>
        IMitigation Clone(IMitigationsContainer container);
    }
}