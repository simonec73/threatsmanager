using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    public class GroupsReadOnlyContainerAspect : InstanceLevelAspect
    {
        #region Implementation of interface IGroupsReadOnlyContainer.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 2)]
        public IEnumerable<IGroup> Groups
        {
            get
            {
                var id = (Instance as IIdentity)?.Id ?? Guid.Empty;

                return (Instance as IThreatModelChild)?.Model?.Groups?
                    .Where(x => ((x as IGroupElement)?.ParentId ?? Guid.Empty) == id);
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 2)]
        public IGroup GetGroup(Guid id)
        {
            var parentId = (Instance as IIdentity)?.Id ?? Guid.Empty;

            return (Instance as IThreatModelChild)?.Model?.Groups?
                .FirstOrDefault(x => x.Id == id && ((x as IGroupElement)?.ParentId ?? Guid.Empty) == parentId);
        }
        #endregion
    }
}