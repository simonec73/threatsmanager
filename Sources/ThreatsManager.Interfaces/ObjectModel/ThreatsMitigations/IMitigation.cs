using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface to define a mitigation.
    /// </summary>
    public interface IMitigation : IIdentity, IPropertiesContainer, IThreatModelChild, ISourceInfo
    {
        /// <summary>
        /// Type of Security Control.
        /// </summary>
        SecurityControlType ControlType { get; set; }

        /// <summary>
        /// Enumeration of the configured specialized mitigations.
        /// </summary>
        IEnumerable<ISpecializedMitigation> Specialized { get; }

        /// <summary>
        /// Add a specialized mitigation.
        /// </summary>
        /// <param name="template">Item Template that is associated to the specialized mitigation.</param>
        /// <param name="name">Name of the specialized mitigation.</param>
        /// <param name="description">Description of the specialized mitigations.</param>
        /// <returns>The Specialized Mitigation that has been just created, or null in case of failure.</returns>
        ISpecializedMitigation AddSpecializedMitigation(IItemTemplate template, string name, string description);

        /// <summary>
        /// Remove the specialized mitigation associated to a specific Item Template.
        /// </summary>
        /// <param name="template">Item Template associated to the specialized mitigation to be removed.</param>
        /// <returns>True if the Specialized Mitigation has been removed successfully. False otherwise.</returns>
        bool RemoveSpecializedMitigation(IItemTemplate template);

        /// <summary>
        /// Remove the specialized mitigation associated to a specific Item Template identifier.
        /// </summary>
        /// <param name="templateId">Identifier of the Item Template associated to the specialized mitigation to be removed.</param>
        /// <returns>True if the Specialized Mitigation has been removed successfully. False otherwise.</returns>
        bool RemoveSpecializedMitigation(Guid templateId);

        /// <summary>
        /// Get the specialized mitigation associated to a given Item Template.
        /// </summary>
        /// <param name="template">Item Template related to the Specialized Mitigation to be searched.</param>
        /// <returns>The wanted Specialized Mitigation, if found, otherwise null.</returns>
        ISpecializedMitigation GetSpecializedMitigation(IItemTemplate template);

        /// <summary>
        /// Get the specialized mitigation associated to a given Item Template.
        /// </summary>
        /// <param name="templateId">Identifier of the Item Template related to the Specialized Mitigation to be searched.</param>
        /// <returns>The wanted Specialized Mitigation, if found, otherwise null.</returns>
        ISpecializedMitigation GetSpecializedMitigation(Guid templateId);

        /// <summary>
        /// Get the name of the Mitigation to be shown for the current object.
        /// </summary>
        /// <param name="identity">Reference object.</param>
        /// <returns>The name to be shown.</returns>
        /// <remarks>If the object is associated to an Item Template that has a Specialized Mitigation associated to it,
        /// then the name of such Specialized Mitigation is used. 
        /// Otherwise, the function returns the name of the Mitigation.</remarks>
        string GetName(IIdentity identity);

        /// <summary>
        /// Get the name of the Mitigation to be shown for the current object.
        /// </summary>
        /// <param name="identityId">Identifier of the reference object.</param>
        /// <returns>The name to be shown.</returns>
        /// <remarks>If the object is associated to an Item Template that has a Specialized Mitigation associated to it,
        /// then the name of such Specialized Mitigation is used. 
        /// Otherwise, the function returns the name of the Mitigation.</remarks>
        string GetName(Guid identityId);

        /// <summary>
        /// Get the description of the Mitigation to be shown for the current object.
        /// </summary>
        /// <param name="identity">Reference object.</param>
        /// <returns>The description to be shown.</returns>
        /// <remarks>If the object is associated to an Item Template that has a Specialized Mitigation associated to it,
        /// then the description of such Specialized Mitigation is used. 
        /// Otherwise, the function returns the description of the Mitigation.</remarks>
        string GetDescription(IIdentity identity);

        /// <summary>
        /// Get the description of the Mitigation to be shown for the current object.
        /// </summary>
        /// <param name="identityId">Identifier of the reference object.</param>
        /// <returns>The description to be shown.</returns>
        /// <remarks>If the object is associated to an Item Template that has a Specialized Mitigation associated to it,
        /// then the description of such Specialized Mitigation is used. 
        /// Otherwise, the function returns the description of the Mitigation.</remarks>
        string GetDescription(Guid identityId);

        /// <summary>
        /// Creates a duplicate of the current Mitigation and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Mitigation.</returns>
        IMitigation Clone(IMitigationsContainer container);
    }
}