using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IThreatModels.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IShowThreatModelPanel<T> : IPanel<T>
    {
        /// <summary>
        /// Method to initialize the Panel with the Threat Model.
        /// </summary>
        /// <param name="threatModel">Threat Model to be shown.</param>
        void SetThreatModel(IThreatModel threatModel);
    }
}