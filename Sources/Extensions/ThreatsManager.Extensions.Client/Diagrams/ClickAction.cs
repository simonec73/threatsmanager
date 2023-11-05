using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Actions to be performed when the user clicks the panel item.
    /// </summary>
    public enum ClickAction
    {
        /// <summary>
        /// Show the object in the Item Editor.
        /// </summary>
        ShowObject,

        /// <summary>
        /// Show a tooltip.
        /// </summary>
        ShowTooltip,

        /// <summary>
        /// Executes a method.
        /// </summary>
        CallMethod
    }
}
