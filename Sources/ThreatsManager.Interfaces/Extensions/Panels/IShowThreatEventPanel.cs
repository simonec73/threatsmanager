using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IThreatEvents.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IShowThreatEventPanel<T> : IPanel<T>
    {
        /// <summary>
        /// Method to initialize the Panel with the Threat Event.
        /// </summary>
        /// <param name="threatEvent">Threat Event to be shown.</param>
        void SetThreatEvent(IThreatEvent threatEvent);
    }
}