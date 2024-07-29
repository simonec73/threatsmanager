using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.ImportersExporters.Exporters.Excel
{
    /// <summary>
    /// Item to consider
    /// </summary>
    public enum ReferencedItemType
    {
        /// <summary>
        /// The current item (default).
        /// </summary>
        Current,
        /// <summary>
        /// Any associated ThreatTypeMitigation.
        /// </summary>
        /// <remarks>The current item must be a Mitigation.</remarks>
        ThreatTypeMitigation,
        /// <summary>
        /// Any associated ThreatEventMitigation.
        /// </summary>
        /// <remarks>The current item must be a Mitigation.</remarks>
        ThreatEventMitigation
    }
}
