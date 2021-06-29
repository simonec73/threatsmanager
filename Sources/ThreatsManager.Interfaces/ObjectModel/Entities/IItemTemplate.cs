using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface representing a generic Item Template.
    /// </summary>
    public interface IItemTemplate : IIdentity, IThreatModelChild, IPropertiesContainer, IDirty //, ILockable
    {
    }
}