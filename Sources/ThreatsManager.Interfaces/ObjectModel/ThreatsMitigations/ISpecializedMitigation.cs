using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface describing a mitigation specialized for a target Item Template.
    /// </summary>
    public interface ISpecializedMitigation : ISourceInfo
    {
        /// <summary>
        /// The identifier of the target Item Template. 
        /// </summary>
        Guid TargetId { get; }

        /// <summary>
        /// Specialized Name of the Mitigation.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Specialized Description of the Mitigation.
        /// </summary>
        string Description { get; set; }
    }
}
