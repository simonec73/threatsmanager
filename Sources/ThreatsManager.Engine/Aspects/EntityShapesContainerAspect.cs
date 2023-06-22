using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
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
    //[JsonProperty("entities")]
    //private AdvisableCollection<EntityShape> _entities { get; set; }
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(NotifyPropertyChangedAttribute))]
    public class EntityShapesContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_entities))]
        public Property<AdvisableCollection<EntityShape>> _entities;
        #endregion

        #region Implementation of interface IEntityShapesContainer.
        private Action<IEntityShapesContainer, IEntityShape> _entityShapeAdded;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "EntityShapeAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnEntityShapeAddedAdd(EventInterceptionArgs args)
        {
            if (_entityShapeAdded == null || !_entityShapeAdded.GetInvocationList().Contains(args.Handler))
            {
                _entityShapeAdded += (Action<IEntityShapesContainer, IEntityShape>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnEntityShapeAddedAdd))]
        public void OnEntityShapeAddedRemove(EventInterceptionArgs args)
        {
            _entityShapeAdded -= (Action<IEntityShapesContainer, IEntityShape>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IEntityShapesContainer, IEntity> _entityShapeRemoved;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "EntityShapeRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnEntityShapeRemovedAdd(EventInterceptionArgs args)
        {
            if (_entityShapeRemoved == null || !_entityShapeRemoved.GetInvocationList().Contains(args.Handler))
            {
                _entityShapeRemoved += (Action<IEntityShapesContainer, IEntity>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnEntityShapeRemovedAdd))]
        public void OnEntityShapeRemovedRemove(EventInterceptionArgs args)
        {
            _entityShapeRemoved -= (Action<IEntityShapesContainer, IEntity>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IEntityShape> Entities => _entities?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IEntityShape GetShape(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return GetEntityShape(entity.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEntityShape GetEntityShape(Guid entityId)
        {
            return _entities?.Get()?.FirstOrDefault(x => x.AssociatedId == entityId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public void Add(IEntityShape entityShape)
        {
            if (entityShape is EntityShape es)
            {
                using (var scope = UndoRedoManager.OpenScope("Add shape for entity"))
                {
                    var entities = _entities?.Get();
                    if (entities == null)
                    {
                        entities = new AdvisableCollection<EntityShape>();
                        _entities?.Set(entities);
                    }

                    entities.Add(es);
                    UndoRedoManager.Attach(es);
                    scope?.Complete();

                    if (Instance is IEntityShapesContainer container)
                    {
                        _entityShapeAdded?.Invoke(container, es);
                    }
                }
            }
            else
                throw new ArgumentNullException(nameof(entityShape));
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public IEntityShape AddShape(IEntity entity, PointF position)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            IEntityShape result = null;

            if (GetEntityShape(entity.Id) == null)
            {
                result = new EntityShape(entity)
                {
                    Position = position
                };

                Add(result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public IEntityShape AddEntityShape(Guid entityId, PointF position)
        {
            IEntityShape result = null;

            if (Instance is IThreatModelChild child)
            {
                var entity = child.Model?.Entities.FirstOrDefault(x => x.Id == entityId);

                if (entity != null)
                {
                    result = AddShape(entity, position);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveEntityShape(Guid entityId)
        {
            bool result = false;

            var entityShape = GetEntityShape(entityId);
            if (entityShape != null)
            {
                result = RemoveShape(entityShape);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public bool RemoveShape(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return RemoveEntityShape(entity.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public bool RemoveShape(IEntityShape entityShape)
        {
            bool result = false;

            if (entityShape is EntityShape es)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove shape for entity"))
                {
                    result = _entities?.Get()?.Remove(es) ?? false;
                    if (result)
                    {
                        UndoRedoManager.Detach(es);
                        scope?.Complete();

                        if (es.Identity is IEntity entity && Instance is IEntityShapesContainer container)
                            _entityShapeRemoved?.Invoke(container, entity);
                    }
                }
            }
            else
                throw new ArgumentNullException(nameof(entityShape));

            return result;
        }
        #endregion
    }
}