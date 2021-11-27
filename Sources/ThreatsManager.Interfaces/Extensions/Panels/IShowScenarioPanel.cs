using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling instances of IThreatEventScenario.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IShowScenarioPanel<T> : IPanel<T>
    {
        /// <summary>
        /// Method to initialize the Scenario associated to the Panel.
        /// </summary>
        /// <param name="scenario">Scenario to be associated.</param>
        void SetScenario(IThreatEventScenario scenario);
    }
}