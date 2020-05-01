using System;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by the children of groups.
    /// </summary>
    public interface IGroupElement
    {
        /// <summary>
        /// Event raised when the parent changes.
        /// </summary>
        /// <remarks>The first parameter represents Group Element raising the event,
        /// the second the old parent, while the third represents the new parent.</remarks>
        event Action<IGroupElement, IGroup, IGroup> ParentChanged;

        /// <summary>
        /// Identifier of the parent.
        /// </summary>
        Guid ParentId { get; }

        /// <summary>
        /// Parent.
        /// </summary>
        IGroup Parent { get; }

        /// <summary>
        /// Sets the parent.
        /// </summary>
        /// <param name="parent">New parent.</param>
        void SetParent(IGroup parent);
    }
}