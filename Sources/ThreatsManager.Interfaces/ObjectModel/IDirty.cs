using System;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface used to represent an object as dirty.
    /// </summary>
    public interface IDirty
    {
        /// <summary>
        /// Event raised when the Dirty status changes.
        /// </summary>
        /// <remarks>Returns the new status.</remarks>
        event Action<IDirty, bool> DirtyChanged;

        /// <summary>
        /// Property to get or set the Dirty status.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Set the object as Dirty, if it is not already.
        /// </summary>
        /// <remarks>If the object is contained, the container will be set as Dirty as well.</remarks>
        void SetDirty();

        /// <summary>
        /// Reset the Dirty status, if it is set.
        /// </summary>
        /// <remarks>If the object is a container, the contained objects will be reset as well.</remarks>
        void ResetDirty();

        /// <summary>
        /// Dirty has been suspended.
        /// </summary>
        bool IsDirtySuspended { get; }

        /// <summary>
        /// Suspends Dirty tracking.
        /// </summary>
        void SuspendDirty();

        /// <summary>
        /// Resume Dirty tracking.
        /// </summary>
        void ResumeDirty();
    }
}
