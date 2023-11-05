using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Patterns.Recording;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// Attribute applied to objects supporting Undo and Redo.
    /// </summary>
    [PSerializable]
    [IntroduceInterface(typeof(IUndoable))]
    [IntroduceInterface(typeof(IRecordableCallback))]
    public class UndoableAttribute : InstanceLevelAspect, IUndoable, IRecordableCallback
    {
        /// <summary>
        /// True if Undo/Redo is enabled for the object.
        /// </summary>
        /// <remarks>Do not change it! This is managed by the Engine.</remarks>
        public bool IsUndoEnabled { get; set; }

        /// <summary>
        /// True if the object is attached to a Recorder.
        /// </summary>
        public bool IsAttached => (this as IRecordable)?.Recorder != null;

        /// <summary>
        /// Event raised when some change on the object has been undone or redone.
        /// </summary>
        /// <remarks>The first argument is the object that has been interested by the undoing/redoing.
        /// The second argument indicates if the object has been removed as a result of the action, false otherwise.</remarks>
        public event Action<object, bool> Undone;

        /// <summary>
        /// Method invoked after the undo or redo operation has started.
        /// </summary>
        /// <param name="kind">Undo or Redo.</param>
        /// <param name="context">Reserved for future use.</param>
        public void OnReplayed(ReplayKind kind, ReplayContext context)
        {
            TriggerUndone(false);
        }

        /// <summary>
        /// Method invoked before the undo or redo operation has started.
        /// </summary>
        /// <param name="kind">Undo or Redo.</param>
        /// <param name="context">Reserved for future use.</param>
        public void OnReplaying(ReplayKind kind, ReplayContext context)
        {
        }

        /// <summary>
        /// Trigger the Undone event.
        /// </summary>
        /// <param name="removed">Flag indicating if the object has been removed as a result of undoing it.</param>
        public void TriggerUndone(bool removed)
        {
            Undone?.Invoke(Instance, removed);
        }
    }
}
