using System;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Template for Flows.
    /// </summary>
    public interface IFlowTemplate : IItemTemplate
    {
        /// <summary>
        /// Type of Data Flow.
        /// </summary>
        FlowType FlowType { get; set; }

        /// <summary>
        /// Create a new Flow based on the Template.
        /// </summary>
        /// <param name="name">Name of the new Flow.</param>
        /// <param name="sourceId">Identifier of the source Entity for the new Flow.</param>
        /// <param name="targetId">Identifier of the target Entity for the new Flow.</param>
        /// <returns>New Flow created from the Template.</returns>
        IDataFlow CreateFlow(string name, Guid sourceId, Guid targetId);

        /// <summary>
        /// Apply the Template to an existing Flow.
        /// </summary>
        /// <param name="flow">Flow which needs to receive the Template.</param>
        /// <remarks>Applies all the properties to the Flow and changes its Template to point to the new one.</remarks>
        void ApplyTo(IDataFlow flow);
        
        /// <summary>
        /// Creates a duplicate of the current Template and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Template.</returns>
        IFlowTemplate Clone(IFlowTemplatesContainer container);
    }
}