using System;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Extensions that can require to create or open a panel.
    /// </summary>
    public interface IPanelOpenerExtension
    {
        /// <summary>
        /// Event generated when opening or creating a Panel is required.
        /// </summary>
        event Action<IPanelFactory, IIdentity> OpenPanel;
    }
}