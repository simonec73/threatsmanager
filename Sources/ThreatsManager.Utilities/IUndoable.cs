using System;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Interface implemented by objects that have been attached to the Undo/Redo Manager.
    /// </summary>
    public interface IUndoable
    {
        /// <summary>
        /// Event raised when an object has changed due to an Undo or a Redo.
        /// </summary>
        /// <remarks>The first argument indicates if the object has been removed as a result of undoing it, false otherwise.</remarks>
        event Action<object, bool> Undone;

        /// <summary>
        /// True if Undo/Redo has been enabled for the object.
        /// </summary>
        bool IsUndoEnabled { get; set; }

        /// <summary>
        /// True if the object is attached to a Recorder.
        /// </summary>
        bool IsAttached { get; }

        /// <summary>
        /// Trigger the Undone event.
        /// </summary>
        /// <param name="removed">Flag indicating if the object has been removed as a result of undoing it.</param>
        void TriggerUndone(bool removed);
    }
}
