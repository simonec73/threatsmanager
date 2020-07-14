using System;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Container of Groups.
    /// </summary>
    public interface IGroupsContainer : IGroupsReadOnlyContainer
    {
        /// <summary>
        /// Adds the Group passed as argument to the container.
        /// </summary>
        /// <param name="group">Group to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IGroup group);

        /// <summary>
        /// Adds a group to the container, assigning the name automatically.
        /// </summary>
        /// <typeparam name="T">Type of the group to be added. It is the interface derived from IGroup.</typeparam>
        /// <returns>New group.</returns>
        T AddGroup<T>() where T : class, IGroup;

        /// <summary>
        /// Adds a group to the container.
        /// </summary>
        /// <typeparam name="T">Type of the group to be added. It is the interface derived from IGroup.</typeparam>
        /// <param name="name">Name of the group.</param>
        /// <returns>New group.</returns>
        T AddGroup<T>(string name) where T : class, IGroup;
        
        /// <summary>
        /// Adds a Trust Boundary to the container and associating it to the Trust Boundary Template passed as argument.
        /// </summary>
        /// <param name="name">Name of the group.</param>
        /// <param name="template">Template to associate to the Trust Boundary.</param>
        /// <returns>New Trust Boundary.</returns>
        ITrustBoundary AddTrustBoundary(string name, ITrustBoundaryTemplate template);

        /// <summary>
        /// Adds a group to the container.
        /// </summary>
        /// <param name="group">Group to be added.</param>
        /// <exception cref="System.ArgumentException">The Group passed as argument cannot be added to the Container.</exception>
        /// <remarks>The Group can be added to the container when it belongs to the same Model of the Container.</remarks>
        void AddGroup(IGroup group);

        /// <summary>
        /// Remove a group from the container.
        /// </summary>
        /// <param name="id">Identifier of the group to be removed from the container.</param>
        /// <returns>True if the group has been removed, otherwise false.</returns>
        /// <remarks>It also removes the related Group Shapes.</remarks>
        bool RemoveGroup(Guid id);
    }
}