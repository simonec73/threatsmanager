using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface representing a Group of Entities.
    /// </summary>
    /// <remarks>Trust Boundaries are an example of Groups.</remarks>
    public interface IGroup : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IEntitiesReadOnlyContainer, IGroupsReadOnlyContainer, IDirty//, ILockable
    {    
        /// <summary>
        /// Creates a duplicate of the current Group and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Group.</returns>
        IGroup Clone(IGroupsContainer container);
    }
}