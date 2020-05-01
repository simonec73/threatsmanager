namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the objects that are associated to a Threat Event.
    /// </summary>
    public interface IThreatEventChild
    {
        /// <summary>
        /// Threat Event associated to the object.
        /// </summary>
        IThreatEvent ThreatEvent { get; }
    }
}