using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private List<IGroupShape> _groups { get; set; }
    //private IGroupShapesContainer GroupShapesContainer => this;
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class GroupShapesContainerAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;

        [ImportMember("GroupShapesContainer", IsRequired=true)]
        public Property<IGroupShapesContainer> GroupShapesContainer;
        #endregion

        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("groups")]
        public List<IGroupShape> _groups { get; set; }
        #endregion

        #region Implementation of interface IGroupShapesContainerAspect.
        private Action<IGroupShapesContainer, IGroupShape> _groupShapeAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IGroupShapesContainer, IGroupShape> GroupShapeAdded
        {
            add
            {
                if (_groupShapeAdded == null || !_groupShapeAdded.GetInvocationList().Contains(value))
                {
                    _groupShapeAdded += value;
                }
            }
            remove
            {
                _groupShapeAdded -= value;
            }
        }

        private Action<IGroupShapesContainer, IGroup> _groupShapeRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IGroupShapesContainer, IGroup> GroupShapeRemoved
        {
            add
            {
                if (_groupShapeRemoved == null || !_groupShapeRemoved.GetInvocationList().Contains(value))
                {
                    _groupShapeRemoved += value;
                }
            }
            remove
            {
                _groupShapeRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IGroupShape> Groups => _groups?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public IGroupShape GetShape(IGroup group)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            return GetGroupShape(group.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IGroupShape GetGroupShape(Guid groupId)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            return _groups?.FirstOrDefault(x => x.AssociatedId == groupId);
        }
        
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IGroupShape groupShape)
        {
            if (!(IsInitialized?.Get() ?? false))
                return;
            if (groupShape == null)
                throw new ArgumentNullException(nameof(groupShape));

            if (_groups == null)
                _groups = new List<IGroupShape>();

            _groups.Add(groupShape);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 15)]
        public IGroupShape AddShape(IGroup group, PointF position, SizeF size)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            IGroupShape result = null;

            if (GetGroupShape(group.Id) == null)
            {
                if (_groups == null)
                    _groups = new List<IGroupShape>();
                result = new GroupShape(Model?.Get(), group)
                {
                    Position = position,
                    Size = size
                };
                _groups.Add(result);
                Dirty.IsDirty = true;
                _groupShapeAdded?.Invoke(GroupShapesContainer?.Get(), result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public IGroupShape AddGroupShape(Guid groupId, PointF position, SizeF size)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            var group = Model?.Get().Groups.FirstOrDefault(x => x.Id == groupId);

            IGroupShape result = null;
            if (group != null)
            {
                result = AddShape(group, position, size);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public bool RemoveGroupShape(Guid groupId)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;

            bool result = false;

            var groupShape = GetGroupShape(groupId);
            if (groupShape != null)
            {
                result = _groups.Remove(groupShape);
                if (result)
                {
                    Dirty.IsDirty = true;
                    if (groupShape.Identity is IGroup group)
                        _groupShapeRemoved?.Invoke(GroupShapesContainer?.Get(), group);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public bool RemoveShape(IGroup group)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            bool result = false;

            var groupShape = GetShape(group);
            if (groupShape != null)
            {
                result = _groups.Remove(groupShape);
                if (result)
                {
                    Dirty.IsDirty = true;
                    _groupShapeRemoved?.Invoke(GroupShapesContainer?.Get(), group);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public bool RemoveShape(IGroupShape groupShape)
        {
            var result = _groups?.Remove(groupShape) ?? false;
            if (result)
            {
                Dirty.IsDirty = true;
                if (groupShape.Identity is IGroup group)
                    _groupShapeRemoved?.Invoke(GroupShapesContainer?.Get(), group);
            }

            return result;
        }
        #endregion
    }
}