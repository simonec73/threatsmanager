using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IEntities.
    /// </summary>
    public interface IShowEntityPanel : IPanel
    {
        /// <summary>
        /// Method to initialize the Entity associated to the Panel.
        /// </summary>
        /// <param name="entity">Entity to be associated.</param>
        void SetEntity(IEntity entity);
    }
}