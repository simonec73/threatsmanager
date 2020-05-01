using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IThreatEvents.
    /// </summary>
    public interface IShowThreatEventPanel : IPanel
    {
        /// <summary>
        /// Method to initialize the Panel with the Threat Event.
        /// </summary>
        /// <param name="threatEvent">Threat Event to be shown.</param>
        void SetThreatEvent(IThreatEvent threatEvent);
    }
}