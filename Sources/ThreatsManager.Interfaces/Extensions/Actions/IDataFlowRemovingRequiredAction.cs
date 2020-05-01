using System;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by Context Aware Actions that can require to remove a Data Flow from the Diagram.
    /// </summary>
    public interface IDataFlowRemovingRequiredAction
    {
        /// <summary>
        /// Event generated when Data Flow needs to be removed from the Diagram.
        /// </summary>
        event Action<ILink> DataFlowRemovingRequired;
    }
}