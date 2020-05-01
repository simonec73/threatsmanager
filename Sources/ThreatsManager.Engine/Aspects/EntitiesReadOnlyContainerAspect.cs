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
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(IdentityAspect))]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class EntitiesReadOnlyContainerAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("_id", IsRequired=true)]
        public Property<Guid> _id;

        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;
        #endregion

        #region Implementation of interface IEntitiesReadOnlyContainer.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IEntity> Entities => Model?.Get().Entities?.Where(x => x.ParentId == _id?.Get());

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IEntity GetEntity(Guid id)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            return Model?.Get().Entities?.FirstOrDefault(x => x.Id == id && x.ParentId == _id?.Get());
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public IEnumerable<IEntity> GetEntities(string name)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return Model?.Get().Entities?.Where(x => name.IsEqual(x.Name) && x.ParentId == _id?.Get());
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public IEnumerable<IEntity> SearchEntities(string filter)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (string.IsNullOrWhiteSpace(filter))
                throw new ArgumentNullException(nameof(filter));

            var rx = new Regex(filter, RegexOptions.IgnoreCase);

            return Entities?.Where(x => (x.Name != null && rx.IsMatch(x.Name)));
        }
        #endregion
    }
}