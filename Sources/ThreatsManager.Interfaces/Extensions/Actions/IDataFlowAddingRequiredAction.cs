using System;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by Context Aware Actions that can require to add a Data Flow to the Diagram.
    /// </summary>
    public interface IDataFlowAddingRequiredAction
    {
        /// <summary>
        /// Event generated when a freshly created Data Flow needs to be added to the Diagram.
        /// </summary>
        event Action<IDiagram, IDataFlow> DataFlowAddingRequired;
    }
}