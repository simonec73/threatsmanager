using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;

namespace ThreatsManager.Utilities.Aspects
{
    /// <summary>
    /// Aspect to set the status Dirty upon entry of a method.
    /// </summary>
    /// <remarks>It holds the Dirty status of the system.</remarks>
    [PSerializable]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation)]
    public class Dirty : OnMethodBoundaryAspect
    {
        private static bool _dirty;
        private static bool _suspendDirty;

        /// <summary>
        /// Event raised when the Dirty status changes.
        /// </summary>
        /// <remarks>Returns the new status.</remarks>
        public static event Action<bool> DirtyChanged; 

        /// <summary>
        /// Property to get or set the Dirty status.
        /// </summary>
        public static bool IsDirty
        {
            get => _dirty;

            set
            {
                if (!_suspendDirty && _dirty != value)
                {
                    _dirty = value;
                    DirtyChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// Dirty has been suspended.
        /// </summary>
        public static bool Suspended => _suspendDirty;

        /// <summary>
        /// Event called when the method is entered.
        /// </summary>
        /// <param name="args"></param>
        public sealed override void OnEntry(MethodExecutionArgs args)
        {
            IsDirty = true;
        }

        /// <summary>
        /// Suspends Dirty tracking.
        /// </summary>
        public static void SuspendDirty()
        {
            _suspendDirty = true;
        }

        /// <summary>
        /// Resume Dirty tracking.
        /// </summary>
        public static void ResumeDirty()
        {
            _suspendDirty = false;
        }
    }
}
