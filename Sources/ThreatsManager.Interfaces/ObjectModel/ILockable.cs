using System;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface that allows to set an object as Locked and thus is not modifiable.
    /// </summary>
    /// <remarks>Locking is not intended to be a Secure Lock of objects. The Engine will prevent changes on locked objects.</remarks>
    public interface ILockable
    {
        /// <summary>
        /// Event raised when the object is Locked.
        /// </summary>
        event Action<object> ObjectLocked;

        /// <summary>
        /// Event raised when the object is Unlocked.
        /// </summary>
        event Action<object> ObjectUnlocked;

        /// <summary>
        /// Read-only property showing if the object is locked.
        /// </summary>
        /// <remarks>If true, the object cannot be changed.</remarks>
        bool Locked { get; }

        /// <summary>
        /// Lock the object.
        /// </summary>
        /// <param name="key">Locking Key.</param>
        /// <returns>True if the Lock succeeds. The Lock will fail if the object has already been locked with a different Locking Key.</returns>
        /// <remarks>A locked object can be unlocked only by providing the correct Locking Key.
        /// Lock may try to propagate to associated objects: the outcome of this attempt is not going to impact the operation as a whole.</remarks>
        bool Lock(string key);

        /// <summary>
        /// Unlocks the object.
        /// </summary>
        /// <param name="key">Locking Key.</param>
        /// <returns>True if the Unlock succeeds. The Unlock will fail if the object is locked and the Locking Key does not correspond to the stored one.</returns>
        /// <remarks>A locked object can be unlocked only by providing the correct Locking Key.
        /// Unlock may try to propagate to associated objects: the outcome of this attempt is not going to impact the operation as a whole.</remarks>
        bool Unlock(string key);
    }
}