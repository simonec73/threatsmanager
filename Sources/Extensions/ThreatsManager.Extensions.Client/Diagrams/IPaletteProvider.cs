using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using System.Drawing;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Interface implemented by 
    /// </summary>
    [ExtensionDescription("Palette Provider")]
    public interface IPaletteProvider : IExtension
    {
        /// <summary>
        /// Name of the Palette.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Icon used for the Palette.
        /// </summary>
        /// <remarks>It should be a bitmap </remarks>
        Image Icon { get; }

        /// <summary>
        /// Get the items to be shown in a Palette.
        /// </summary>
        /// <param name="model">Threat Model containing the items to be shown in the palette.</param>
        /// <param name="filter">Filter to be applied to select the items.</param>
        /// <returns>Palette items associated to the Threat Model and identified by the filter.</returns>
        IEnumerable<PaletteItem> GetPaletteItems(IThreatModel model, string filter);
    }
}
