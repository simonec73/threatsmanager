using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private Guid _parentId { get; set; }
    //private IGroupElement GroupElement => this;
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class GroupElementAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;

        [ImportMember("GroupElement", IsRequired=true)]
        public Property<IGroupElement> GroupElement;
        #endregion

        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("parentId")]
        public Guid _parentId { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 0, Visibility = Visibility.Private)]
        public IGroup _parent { get; set; }
        #endregion

        #region Implementation of interface IGroupElement.
        private Action<IGroupElement, IGroup, IGroup> _parentChanged;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IGroupElement, IGroup, IGroup> ParentChanged
        {
            add
            {
                if (_parentChanged == null || !_parentChanged.GetInvocationList().Contains(value))
                {
                    _parentChanged += value;
                }
            }
            remove
            {
                _parentChanged -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid ParentId => _parentId;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IGroup Parent
        {
            get
            {
                if (_parent == null && _parentId != Guid.Empty)
                {
                    _parent = Model?.Get()?.GetGroup(_parentId);
                }

                return _parent;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 15)]
        public void SetParent(IGroup parent)
        {
            if (!(IsInitialized?.Get() ?? false))
                return;

            IGroup oldParent = null;

            if (((parent == null && _parentId != Guid.Empty) || (parent != null && _parentId != parent.Id)))
            {
                oldParent = Parent;
                _parentId = parent?.Id ?? Guid.Empty;
                _parent = parent;
                Dirty.IsDirty = true;
                _parentChanged?.Invoke(GroupElement?.Get(), oldParent, parent);
            }
        }
        #endregion
    }
}
