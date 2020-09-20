using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IEntities.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IShowEntityPanel<T> : IPanel<T>
    {
        /// <summary>
        /// Method to initialize the Entity associated to the Panel.
        /// </summary>
        /// <param name="entity">Entity to be associated.</param>
        void SetEntity(IEntity entity);
    }
}