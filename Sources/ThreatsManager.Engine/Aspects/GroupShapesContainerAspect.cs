using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("groups")]
    //private IList<IGroupShape> _groups { get; set; }
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(NotifyPropertyChangedAttribute))]
    public class GroupShapesContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_groups))]
        public Property<IList<IGroupShape>> _groups;
        #endregion

        #region Implementation of interface IGroupShapesContainerAspect.
        private Action<IGroupShapesContainer, IGroupShape> _groupShapeAdded;
        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "GroupShapeAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnGroupShapeAddedAdd(EventInterceptionArgs args)
        {
            if (_groupShapeAdded == null || !_groupShapeAdded.GetInvocationList().Contains(args.Handler))
            {
                _groupShapeAdded += (Action<IGroupShapesContainer, IGroupShape>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnGroupShapeAddedAdd))]
        public void OnGroupShapeAddedRemove(EventInterceptionArgs args)
        {
            _groupShapeAdded -= (Action<IGroupShapesContainer, IGroupShape>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IGroupShapesContainer, IGroup> _groupShapeRemoved;
        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "GroupShapeRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnGroupShapeRemovedAdd(EventInterceptionArgs args)
        {
            if (_groupShapeRemoved == null || !_groupShapeRemoved.GetInvocationList().Contains(args.Handler))
            {
                _groupShapeRemoved += (Action<IGroupShapesContainer, IGroup>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnGroupShapeRemovedAdd))]
        public void OnGroupShapeRemovedRemove(EventInterceptionArgs args)
        {
            _groupShapeRemoved -= (Action<IGroupShapesContainer, IGroup>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IGroupShape> Groups => _groups?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IGroupShape GetShape(IGroup group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            return GetGroupShape(group.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IGroupShape GetGroupShape(Guid groupId)
        {
            return _groups?.Get()?.FirstOrDefault(x => x.AssociatedId == groupId);
        }
        
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void Add(IGroupShape groupShape)
        {
            if (groupShape == null)
                throw new ArgumentNullException(nameof(groupShape));

            using (var scope = UndoRedoManager.OpenScope("Add shape for group"))
            {
                var groups = _groups?.Get();
                if (groups == null)
                {
                    groups = new AdvisableCollection<IGroupShape>();
                    _groups?.Set(groups);
                }

                groups.Add(groupShape);
                UndoRedoManager.Attach(groupShape);
                scope.Complete();

                if (Instance is IGroupShapesContainer container)
                {
                    _groupShapeAdded?.Invoke(container, groupShape);
                }
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 15)]
        public IGroupShape AddShape(IGroup group, PointF position, SizeF size)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            IGroupShape result = null;

            if (GetGroupShape(group.Id) == null)
            {
                result = new GroupShape(group)
                {
                    Position = position,
                    Size = size
                };

                Add(result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public IGroupShape AddGroupShape(Guid groupId, PointF position, SizeF size)
        {
            IGroupShape result = null;

            if (Instance is IThreatModelChild child)
            {
                var group = child.Model?.Groups?.FirstOrDefault(x => x.Id == groupId);

                if (group != null)
                {
                    result = AddShape(group, position, size);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveGroupShape(Guid groupId)
        {
            bool result = false;

            var groupShape = GetGroupShape(groupId);
            if (groupShape != null)
            {
                result = RemoveShape(groupShape);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public bool RemoveShape(IGroup group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            return RemoveGroupShape(group.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveShape(IGroupShape groupShape)
        {
            if (groupShape == null)
                throw new ArgumentNullException(nameof(groupShape));
            
            bool result;

            using (var scope = UndoRedoManager.OpenScope("Remove shape for group"))
            {
                result = _groups?.Get()?.Remove(groupShape) ?? false;
                if (result)
                {
                    UndoRedoManager.Detach(groupShape);
                    scope.Complete();

                    if (groupShape.Identity is IGroup group && Instance is IGroupShapesContainer container)
                        _groupShapeRemoved?.Invoke(container, group);
                }
            }

            return result;
        }
        #endregion
    }
}