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
    //private IGroup _parent { get; set; }
    //#endregion    

    [PSerializable]
    public class GroupElementAspect : InstanceLevelAspect
    {
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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
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
                if (_parent == null && _parentId != Guid.Empty && Instance is IThreatModelChild child)
                {
                    _parent = child.Model?.GetGroup(_parentId);
                }

                return _parent;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public void SetParent(IGroup parent)
        {
            IGroup oldParent = null;

            if (((parent == null && _parentId != Guid.Empty) || (parent != null && _parentId != parent.Id)))
            {
                oldParent = Parent;
                _parentId = parent?.Id ?? Guid.Empty;
                _parent = parent;
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
                if (Instance is IGroupElement groupElement)
                    _parentChanged?.Invoke(groupElement, oldParent, parent);
            }
        }
        #endregion
    }
}
