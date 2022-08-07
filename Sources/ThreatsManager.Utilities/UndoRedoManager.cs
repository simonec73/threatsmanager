using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Main entry point to manage Undo and Redo activities.
    /// </summary>
    public static class UndoRedoManager
    {
        private static bool _isDirty = RecordingServices.DefaultRecorder.UndoOperations.Count > 0;
        private static bool _isUndoing = false;
        private static bool _isRedoing = false;

        static UndoRedoManager()
        {
            RecordingServices.DefaultRecorder.ChildPropertyChanged += DefaultRecorder_ChildPropertyChanged;
        }

        /// <summary>
        /// Event raised if any action fails with an error.
        /// </summary>
        public static event Action<string> ErrorRaised;

        /// <summary>
        /// Event raised if the dirty status changes.
        /// </summary>
        public static event Action<bool> DirtyChanged;

        /// <summary>
        /// Maximum number of recordable operations.
        /// </summary>
        /// <remarks>Default is 2147483647.</remarks>
        public static int MaximumOperationsCount
        {
            get { return RecordingServices.DefaultRecorder.MaximumOperationsCount; }
            set { RecordingServices.DefaultRecorder.MaximumOperationsCount = value; }
        }

        /// <summary>
        /// Indicates if the status is Dirty.
        /// </summary>
        public static bool IsDirty => _isDirty;

        /// <summary>
        /// Indicates if an Undo action is in progress.
        /// </summary>
        public static bool IsUndoing => _isUndoing;

        /// <summary>
        /// Indicates if a Redo action is in progress.
        /// </summary>
        public static bool IsRedoing => _isRedoing;

        /// <summary>
        /// Number of recorded Undo operations.
        /// </summary>
        public static int UndoOperationsCount => RecordingServices.DefaultRecorder.UndoOperations.Count;

        /// <summary>
        /// Number of recorded Redo operations.
        /// </summary>
        public static int RedoOperationsCount => RecordingServices.DefaultRecorder.RedoOperations.Count;

        /// <summary>
        ///Recorded Undo operations.
        /// </summary>
        public static IEnumerable<Operation> UndoOperations => 
            RecordingServices.DefaultRecorder.UndoOperations.OfType<Operation>();

        /// <summary>
        /// Recorded Redo operations.
        /// </summary>
        public static IEnumerable<Operation> RedoOperations =>
            RecordingServices.DefaultRecorder.RedoOperations.OfType<Operation>();

        /// <summary>
        /// Clear the recorded operations.
        /// </summary>
        public static void Clear()
        {
            RecordingServices.DefaultRecorder.Clear();
            ResetDirty();
        }

        /// <summary>
        /// Reset the Dirty status.
        /// </summary>
        public static void ResetDirty()
        {
            if (_isDirty)
            {
                _isDirty = false;
                DirtyChanged?.Invoke(IsDirty);
            }
        }

        /// <summary>
        /// Undo the latest operation.
        /// </summary>
        public static void Undo()
        {
            try
            {
                _isUndoing = true;
                RecordingServices.DefaultRecorder.Undo();
                if (RecordingServices.DefaultRecorder.UndoOperations.Count == 0)
                    ResetDirty();
            }
            catch (Exception exc)
            {
                ErrorRaised?.Invoke(exc.Message);
            }
            finally
            {
                _isUndoing = false;
            }
        }

        /// <summary>
        /// Undo all operations up to the one passed as argument.
        /// </summary>
        /// <param name="operation">Last operation to undo.</param>
        public static void UndoTo([NotNull] Operation operation)
        {
            try
            {
                _isUndoing = true;
                RecordingServices.DefaultRecorder.UndoTo(operation);
                if (RecordingServices.DefaultRecorder.UndoOperations.Count == 0)
                    ResetDirty();
            }
            catch (Exception exc)
            {
                ErrorRaised?.Invoke(exc.Message);
            }
            finally
            {
                _isUndoing = false;
            }
        }

        /// <summary>
        /// Redo the last operation.
        /// </summary>
        public static void Redo()
        {
            try
            {
                _isRedoing = true;
                RecordingServices.DefaultRecorder.Redo();
            }
            catch (Exception exc)
            {
                ErrorRaised?.Invoke(exc.Message);
            }
            finally
            {
                _isRedoing = false;
            }
        }

        /// <summary>
        /// Redo all operations up to the one passed as argument.
        /// </summary>
        /// <param name="operation"></param>
        public static void RedoTo([NotNull] Operation operation)
        {
            try
            {
                _isRedoing = true;
                RecordingServices.DefaultRecorder.RedoTo(operation);
            }
            catch (Exception exc)
            {
                ErrorRaised?.Invoke(exc.Message);
            }
            finally
            {
                _isRedoing = false;
            }
        }

        /// <summary>
        /// Creates a scope of the action.
        /// </summary>
        /// <param name="name">Name of the scope.</param>
        /// <returns>Scope for the recording.</returns>
        public static RecordingScope OpenScope([Required] string name)
        {
            return RecordingServices.DefaultRecorder.OpenScope(name, RecordingScopeOption.Atomic);
        }

        private static void DefaultRecorder_ChildPropertyChanged(object sender, PostSharp.Patterns.Model.ChildPropertyChangedEventArgs e)
        {
            bool dirty = RecordingServices.DefaultRecorder.UndoOperations.Count > 0;
            if (_isDirty != dirty)
            {
                _isDirty = dirty;
                DirtyChanged?.Invoke(dirty);
            }
        }
    }
}
