using System;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by IPanelFactory objects which are IMainRibbonExtensions and in that role
    /// need to request actions on Panels to Threats Manager Studio. 
    /// </summary>
    public interface IPanelFactoryActionsRequestor
    {
        /// <summary>
        /// Event generated when the creation of a Panel is required.
        /// </summary>
        /// <remarks>The first parameter is the panel factory raising the event,
        /// the second is the Identity object that is used as a reference. The latter may be null.</remarks>
        event Action<IPanelFactory, IIdentity> PanelCreationRequired;

        /// <summary>
        /// Event generated when the deletion of a Panel is required.
        /// </summary>
        /// <remarks>The first parameter is the panel factory raising the event, the second is the panel to be removed.</remarks>
        event Action<IPanelFactory, IPanel> PanelDeletionRequired;
        
        /// <summary>
        /// Event generated when it is required to show a specific Panel.
        /// </summary>
        event Action<IPanelFactory, IPanel> PanelShowRequired;
    }
}