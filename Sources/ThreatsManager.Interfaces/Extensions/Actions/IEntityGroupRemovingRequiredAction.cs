using System;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by Context Aware Actions that can require to remove an Entity or Group from the Diagram.
    /// </summary>
    public interface IEntityGroupRemovingRequiredAction
    {
        /// <summary>
        /// Event generated when an entity or group needs to be removed from the Diagram.
        /// </summary>
        event Action<IShape> EntityGroupRemovingRequired;
    }
}