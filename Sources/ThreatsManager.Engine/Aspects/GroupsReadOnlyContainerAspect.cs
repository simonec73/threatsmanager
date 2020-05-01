using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(IdentityAspect))]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class GroupsReadOnlyContainerAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("_id", IsRequired=true)]
        public Property<Guid> _id;

        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;
        #endregion

        #region Implementation of interface IGroupsReadOnlyContainer.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IGroup> Groups => Model?.Get().Groups?
            .Where(x => ((x as IGroupElement)?.ParentId ?? Guid.Empty) == _id?.Get());

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IGroup GetGroup(Guid id)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            return Model?.Get().Groups?
                .FirstOrDefault(x => x.Id == id && ((x as IGroupElement)?.ParentId ?? Guid.Empty) == _id?.Get());
        }
        #endregion
    }
}