using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions
{
    /// <summary>
    /// Filter for the Roadmap.
    /// </summary>
    public interface IRoadmapFilter
    {
        /// <summary>
        /// Name of the Filter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Filter the Mitigations passed as argument. 
        /// </summary>
        /// <param name="mitigations">Mitigations to be filtered.</param>
        /// <returns>Selected mitigations.</returns>
        IEnumerable<IMitigation> Filter(IEnumerable<IMitigation> mitigations);
    }
}
