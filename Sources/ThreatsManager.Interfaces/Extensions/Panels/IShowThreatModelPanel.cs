using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IThreatModels.
    /// </summary>
    public interface IShowThreatModelPanel : IPanel
    {
        /// <summary>
        /// Method to initialize the Panel with the Threat Model.
        /// </summary>
        /// <param name="threatModel">Threat Model to be shown.</param>
        void SetThreatModel(IThreatModel threatModel);
    }
}