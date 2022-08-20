using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

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
        /// Attach the Recorder to an object.
        /// </summary>
        /// <param name="item">Object that should be considered for Undo/Redo.</param>
        public static void Attach([NotNull] object item)
        {
            if (item is IThreatModel model)
            {
                RecordingServices.DefaultRecorder.Attach(model);
                if (item is IUndoable undoable)
                    undoable.IsUndoEnabled = true;

                Attach(model.DataFlows);
                Attach(model.Diagrams);
                Attach(model.Entities);
                Attach(model.EntityTemplates);
                Attach(model.FlowTemplates);
                Attach(model.Groups);
                Attach(model.Mitigations);
                Attach(model.Properties);
                Attach(model.Schemas);
                Attach(model.Severities);
                Attach(model.Strengths);
                Attach(model.ThreatActors);
                Attach(model.ThreatEvents);
                Attach(model.ThreatTypes);
                Attach(model.TrustBoundaryTemplates);
                Attach(model.Vulnerabilities);
                Attach(model.Weaknesses);
            }
            else
            {
                if (item is IThreatModelChild child && child.Model is IUndoable modelUndoable && modelUndoable.IsUndoEnabled)
                {
                    RecordingServices.DefaultRecorder.Attach(item);
                    if (item is IUndoable u)
                        u.IsUndoEnabled = true;

                    AttachContainer(item);
                }
            }
        }

        /// <summary>
        /// Detach the Recorder from an object.
        /// </summary>
        /// <param name="item">Object that should be removed from Undo/Redo.</param>
        public static void Detach([NotNull] object item)
        {
            if (item is IUndoable undoable && undoable.IsUndoEnabled)
            {
                RecordingServices.DefaultRecorder.Detach(item);
                undoable.IsUndoEnabled = false;

                DetachContainer(item);

                if (item is IThreatModel model)
                { 
                    Detach(model.DataFlows);
                    Detach(model.Diagrams);
                    Detach(model.Entities);
                    Detach(model.EntityTemplates);
                    Detach(model.FlowTemplates);
                    Detach(model.Groups);
                    Detach(model.Mitigations);
                    Detach(model.Schemas);
                    Detach(model.Severities);
                    Detach(model.Strengths);
                    Detach(model.ThreatActors);
                    Detach(model.ThreatTypes);
                    Detach(model.TrustBoundaryTemplates);
                    Detach(model.Weaknesses);
                }
            }
        }

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

        private static void DefaultRecorder_ChildPropertyChanged(object sender, EventArgs e)
        {
            bool dirty = RecordingServices.DefaultRecorder.UndoOperations.Count > 0;
            if (_isDirty != dirty)
            {
                _isDirty = dirty;
                DirtyChanged?.Invoke(dirty);
            }
        }

        private static void Attach(IEnumerable<object> items)
        {
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    RecordingServices.DefaultRecorder.Attach(item);
                    if (item is IUndoable undoable)
                        undoable.IsUndoEnabled = true;

                    AttachContainer(item);
                }
            }
        }

        private static void AttachContainer(object item)
        {
            if (item is IPropertyTypesContainer ptContainer)
            {
                Attach(ptContainer.PropertyTypes);
            }

            if (item is IPropertiesContainer pContainer)
            {
                Attach(pContainer.Properties);
            }

            if (item is IThreatEventsContainer teContainer)
            {
                Attach(teContainer.ThreatEvents);
            }

            if (item is IVulnerabilitiesContainer vContainer)
            {
                Attach(vContainer.Vulnerabilities);
            }

            if (item is IThreatEventMitigationsContainer temContainer)
            {
                Attach(temContainer.Mitigations);
            }

            if (item is IThreatTypeMitigationsContainer ttmContainer)
            {
                Attach(ttmContainer.Mitigations);
            }

            if (item is IThreatTypeWeaknessesContainer ttwContainer)
            {
                Attach(ttwContainer.Weaknesses);
            }

            if (item is IThreatEventScenariosContainer tesContainer)
            {
                Attach(tesContainer.Scenarios);
            }

            if (item is IWeaknessMitigationsContainer wmContainer)
            {
                Attach(wmContainer.Mitigations);
            }

            if (item is ILinksContainer lContainer)
            {
                Attach(lContainer.Links);
            }

            if (item is IEntityShapesContainer esContainer)
            {
                Attach(esContainer.Entities);
            }

            if (item is IGroupShapesContainer gsContainer)
            {
                Attach(gsContainer.Groups);
            }
        }

        private static void Detach(IEnumerable<object> items)
        {
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    try
                    {
                        if (item is IUndoable undoable && undoable.IsUndoEnabled)
                        {
                            RecordingServices.DefaultRecorder.Detach(item);
                            undoable.IsUndoEnabled = false;

                            DetachContainer(item);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // Ignore this exception because it most probably is due to the fact that the object is not tracked by the Recorder.
                    }
                }
            }
        }

        private static void DetachContainer(object item)
        {
            if (item is IPropertyTypesContainer ptContainer)
            {
                Detach(ptContainer.PropertyTypes);
            }

            if (item is IPropertiesContainer pContainer)
            {
                Detach(pContainer.Properties);
            }

            if (item is IThreatEventsContainer teContainer)
            {
                Detach(teContainer.ThreatEvents);
            }

            if (item is IVulnerabilitiesContainer vContainer)
            {
                Detach(vContainer.Vulnerabilities);
            }

            if (item is IThreatEventMitigationsContainer temContainer)
            {
                Detach(temContainer.Mitigations);
            }

            if (item is IThreatTypeMitigationsContainer ttmContainer)
            {
                Detach(ttmContainer.Mitigations);
            }

            if (item is IThreatTypeWeaknessesContainer ttwContainer)
            {
                Detach(ttwContainer.Weaknesses);
            }

            if (item is IThreatEventScenariosContainer tesContainer)
            {
                Detach(tesContainer.Scenarios);
            }

            if (item is IWeaknessMitigationsContainer wmContainer)
            {
                Detach(wmContainer.Mitigations);
            }

            if (item is ILinksContainer lContainer)
            {
                Detach(lContainer.Links);
            }

            if (item is IEntityShapesContainer esContainer)
            {
                Detach(esContainer.Entities);
            }

            if (item is IGroupShapesContainer gsContainer)
            {
                Detach(gsContainer.Groups);
            }
        }
    }
}
