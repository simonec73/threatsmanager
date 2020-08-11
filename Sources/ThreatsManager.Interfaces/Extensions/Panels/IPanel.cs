using System;
using System.Windows.Forms;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel.
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// Identifier of the Panel.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Form containing the Panel.
        /// </summary>
        Form ContainingForm { get; set; }

        ///// <summary>
        ///// Definition of the action associated to the Panel.
        ///// </summary>
        //IActionDefinition ActionDefinition { get; }
    }
}