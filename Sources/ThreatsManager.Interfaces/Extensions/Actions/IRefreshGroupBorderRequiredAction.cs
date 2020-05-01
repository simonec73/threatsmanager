using System;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by Context Aware Actions that can require to refresh the border of a Group.
    /// </summary>
    public interface IRefreshGroupBorderRequiredAction
    {
        /// <summary>
        /// Event generated when a refresh of the Group border is required.
        /// </summary>
        event Action<IDiagram, IGroup> RefreshGroupBorderRequired;
    }
}