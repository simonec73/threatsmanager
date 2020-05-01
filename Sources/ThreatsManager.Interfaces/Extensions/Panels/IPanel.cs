using System.Windows.Forms;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel.
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// Form containing the Panel.
        /// </summary>
        Form ContainingForm { get; set; }

        IActionDefinition ActionDefinition { get; }
    }
}