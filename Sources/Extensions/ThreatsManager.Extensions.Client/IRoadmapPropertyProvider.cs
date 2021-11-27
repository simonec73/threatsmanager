using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions
{
    /// <summary>
    /// Property Provider used to provide properties for the items in the Roadmap Panel.
    /// </summary>
    [ExtensionDescription("Roadmap Property Provider")]
    public interface IRoadmapPropertyProvider : IExtension
    {
        /// <summary>
        /// Name of the Property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Value of the Property, calculated for the Mitigation.
        /// </summary>
        /// <returns>Value of the Property.</returns>
        string GetValue(IMitigation mitigation);
    }
}
