using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Modes of execution of the Platform.
    /// </summary>
    /// <remarks>Execution modes support the configuration of the interface
    /// to allow different roles to use the Platform in a way that is more
    /// suitable for their specific needs.</remarks>
    public enum ExecutionMode
    {
        /// <summary>
        /// This is the mode which includes all possible Extensions.
        /// </summary>
        [EnumDescription("Expert mode is the most complete view, which includes all the installed Extensions.\nIt is designed for the most Expert users.")]
        Expert,
        /// <summary>
        /// This is a simplified mode which allows beginners to focus on a reduced set of capabilities.
        /// </summary>
        [EnumDescription("Simplified mode provides a slightly limited view which provides still most of the features.\nIt is thought for Threat Modeling beginners and people starting to work with Threats Manager Platform.")]
        Simplified,
        /// <summary>
        /// The most limited technical mode, with no possibility to create Threat Models.
        /// </summary>
        [EnumDescription("The most limited technical mode, with features thought for Project Managers and similar roles.")]
        Management,
        /// <summary>
        /// This is a view that includes only Extensions suitable for Business decision makers.
        /// </summary>
        [EnumDescription("Non-technical mode, offering read only access to the Threat Model through a few capabilities targeted to the Business Decision Maker.")]
        Business
    }
}
