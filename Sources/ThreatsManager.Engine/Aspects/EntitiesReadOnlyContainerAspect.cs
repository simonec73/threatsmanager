using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    public class EntitiesReadOnlyContainerAspect : InstanceLevelAspect
    {
        #region Implementation of interface IEntitiesReadOnlyContainer.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
        public IEnumerable<IEntity> Entities
        {
            get
            {
                var model = (Instance as IThreatModelChild)?.Model;
                var id = (Instance as IIdentity)?.Id;
                
                return model?.Entities?.Where(x => x.ParentId == (id ?? Guid.Empty));
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IEntity GetEntity(Guid id)
        {
            var model = (Instance as IThreatModelChild)?.Model;
            var parentId = (Instance as IIdentity)?.Id;

            return model?.Entities?.FirstOrDefault(x => x.Id == id && x.ParentId == (parentId ?? Guid.Empty));
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public IEnumerable<IEntity> GetEntities(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var model = (Instance as IThreatModelChild)?.Model;
            var parentId = (Instance as IIdentity)?.Id;

            return model?.Entities?.Where(x => name.IsEqual(x.Name) && x.ParentId == (parentId ?? Guid.Empty));
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
        public IEnumerable<IEntity> SearchEntities(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                throw new ArgumentNullException(nameof(filter));

            var rx = new Regex(filter, RegexOptions.IgnoreCase);

            return Entities?.Where(x => (x.Name != null && rx.IsMatch(x.Name)));
        }
        #endregion
    }
}