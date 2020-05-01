using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Read-only container for Groups.
    /// </summary>
    public interface IGroupsReadOnlyContainer
    {
        /// <summary>
        /// Enumerate the Groups.
        /// </summary>
        IEnumerable<IGroup> Groups { get; }

        /// <summary>
        /// Get a group by ID.
        /// </summary>
        /// <param name="id">Identifier of the Group.</param>
        /// <returns>Instance of the group if found, otherwise null.</returns>
        IGroup GetGroup(Guid id);
    }
}