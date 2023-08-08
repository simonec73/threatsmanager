using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Panels;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Interface implemented by Marker Providers.
    /// </summary>
    public interface IMarkerProvider : IDisposable
    {
        /// <summary>
        /// Event raised when the marker status is changed.
        /// </summary>
        event Action<IMarkerProvider> StatusUpdated;

        /// <summary>
        /// Flag stating if the marker should be visible.
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Flag returning the icon to use for the marker.
        /// </summary>
        /// <param name="size">Marker size in pixels.</param>
        Image GetIcon(int size);

        /// <summary>
        /// Get Panel Items associated to the current Marker.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PanelItem> GetPanelItems();
    }
}
