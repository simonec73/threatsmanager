using System;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    /// <summary>
    /// Attribute applied to automatically inject the code needed by classes implementing IDirty and being contained within the Threat Model.
    /// </summary>
    /// <remarks>PostSharp is required to make this attribute effective.</remarks>
    [PSerializable]
    public class DirtyAspect : InstanceLevelAspect
    {
        #region Implementation of interface IDirty.
        private Action<IDirty, bool> _dirtyChanged;

        /// <summary>
        /// Event raised when the Dirty status changes.
        /// </summary>
        /// <remarks>Returns the new status.</remarks>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
        public event Action<IDirty, bool> DirtyChanged
        {
            add
            {
                if (_dirtyChanged == null || !_dirtyChanged.GetInvocationList().Contains(value))
                {
                    _dirtyChanged += value;
                }
            }
            remove { _dirtyChanged -= value; }
        }

        /// <summary>
        /// Property to get or set the Dirty status.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 1, Visibility = Visibility.Public)]
        [CopyCustomAttributes(typeof(AutoDirtyIgnoreAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Set the object as Dirty, if it is not already.
        /// </summary>
        /// <remarks>If the object is contained, the container will be set as Dirty as well.</remarks>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public void SetDirty()
        {
            if (Instance is IDirty dirtyObject)
            {
                if (!dirtyObject.IsDirtySuspended && !dirtyObject.IsDirty)
                {
                    IsDirty = true;

                    _dirtyChanged?.Invoke(dirtyObject, true);

                    if (Instance is IThreatModelChild child)
                        child.Model?.SetDirty();
                }
            }
        }

        /// <summary>
        /// Reset the Dirty status, if it is set.
        /// </summary>
        /// <remarks>If the object is a container, the contained objects will be reset as well.</remarks>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
        public void ResetDirty()
        {
            if (Instance is IDirty dirtyObject)
            {
                if (!dirtyObject.IsDirtySuspended && dirtyObject.IsDirty)
                {
                    IsDirty = false;

                    _dirtyChanged?.Invoke(dirtyObject, true);

                    if (Instance is IPropertiesContainer pC)
                        pC.Properties?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is IThreatEventsContainer teC)
                        teC.ThreatEvents?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is IThreatTypeMitigationsContainer ttmC)
                        ttmC.Mitigations?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is IThreatEventMitigationsContainer temC)
                        temC.Mitigations?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is IThreatEventScenariosContainer tesC)
                        tesC.Scenarios?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is IEntityShapesContainer esC)
                        esC.Entities?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is IGroupShapesContainer gsC)
                        gsC.Groups?.ToList().ForEach(x => x.ResetDirty());
                    if (Instance is ILinksContainer lC)
                        lC.Links?.ToList().ForEach(x => x.ResetDirty());
                }
            }
        }

        /// <summary>
        /// Dirty has been suspended.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 1, Visibility = Visibility.Public)]
        [CopyCustomAttributes(typeof(AutoDirtyIgnoreAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        public bool IsDirtySuspended { get; private set; }

        /// <summary>
        /// Suspends Dirty tracking.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public void SuspendDirty()
        {
            IsDirtySuspended = true;
        }

        /// <summary>
        /// Resume Dirty tracking.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public void ResumeDirty()
        {
            IsDirtySuspended = false;
        }
        #endregion
    }
}
