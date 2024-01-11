using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface including the details of the source Threat Model from which the item is originated.
    /// </summary>
    /// <remarks>It refers to the Knowledge Base or the Threat Model containing the source for the object.</remarks>
    public interface ISourceInfo : IObjectVersion
    {
        /// <summary>
        /// Identifier of the source Threat Model.
        /// </summary>
        Guid SourceTMId { get; }

        /// <summary>
        /// Name of the source Threat Model.
        /// </summary>
        string SourceTMName { get; }

        /// <summary>
        /// Set the Threat Model passed as argument as the source for the object.
        /// </summary>
        /// <param name="source">Source Threat Model.</param>
        void SetSourceInfo(IThreatModel source);
    }
}
