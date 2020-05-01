namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface implemented by the objects that maintain a reference to the Threat Model.
    /// </summary>
    public interface IThreatModelChild
    {
        /// <summary>
        /// Threat Model containing the object.
        /// </summary>
        IThreatModel Model { get; }
    }
}