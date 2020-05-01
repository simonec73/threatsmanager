using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Contracts;
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
    //private List<IEntityShape> _entities { get; set; }
    //private IEntityShapesContainer EntityShapesContainer => this;
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class EntityShapesContainerAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;

        [ImportMember("EntityShapesContainer", IsRequired=true)]
        public Property<IEntityShapesContainer> EntityShapesContainer;
        #endregion

        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("entities")]
        public List<IEntityShape> _entities { get; set; }
        #endregion

        #region Implementation of interface IEntityShapesContainer.
        private Action<IEntityShapesContainer, IEntityShape> _entityShapeAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IEntityShapesContainer, IEntityShape> EntityShapeAdded
        {
            add
            {
                if (_entityShapeAdded == null || !_entityShapeAdded.GetInvocationList().Contains(value))
                {
                    _entityShapeAdded += value;
                }
            }
            remove
            {
                _entityShapeAdded -= value;
            }
        }

        private Action<IEntityShapesContainer, IEntity> _entityShapeRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IEntityShapesContainer, IEntity> EntityShapeRemoved
        {
            add
            {
                if (_entityShapeRemoved == null || !_entityShapeRemoved.GetInvocationList().Contains(value))
                {
                    _entityShapeRemoved += value;
                }
            }
            remove
            {
                _entityShapeRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IEntityShape> Entities => _entities?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IEntityShape GetShape(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return GetEntityShape(entity.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IEntityShape GetEntityShape(Guid entityId)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            return _entities?.FirstOrDefault(x => x.AssociatedId == entityId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IEntityShape entityShape)
        {
            if (!(IsInitialized?.Get() ?? false))
                return;
            if (entityShape == null)
                throw new ArgumentNullException(nameof(entityShape));

            if (_entities == null)
                _entities = new List<IEntityShape>();

            _entities.Add(entityShape);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 14)]
        public IEntityShape AddShape(IEntity entity, PointF position)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            IEntityShape result = null;

            if (GetEntityShape(entity.Id) == null)
            {
                if (_entities == null)
                    _entities = new List<IEntityShape>();
                result = new EntityShape(Model?.Get(), entity)
                {
                    Position = position
                };
                _entities.Add(result);
                Dirty.IsDirty = true;
                _entityShapeAdded?.Invoke(EntityShapesContainer?.Get(), result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public IEntityShape AddEntityShape(Guid entityId, PointF position)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            var entity = Model?.Get().Entities.FirstOrDefault(x => x.Id == entityId);

            IEntityShape result = null;
            if (entity != null)
            {
                result = AddShape(entity, position);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public bool RemoveEntityShape(Guid entityId)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;

            bool result = false;

            var entityShape = GetEntityShape(entityId);
            if (entityShape != null)
            {
                result = _entities.Remove(entityShape);
                if (result)
                {
                    Dirty.IsDirty = true;
                    if (entityShape.Identity is IEntity entity)
                        _entityShapeRemoved?.Invoke(EntityShapesContainer?.Get(), entity);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public bool RemoveShape(IEntity entity)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            bool result = false;

            var entityShape = GetShape(entity);
            if (entityShape != null)
            {
                result = _entities.Remove(entityShape);
                if (result)
                {
                    Dirty.IsDirty = true;
                    _entityShapeRemoved?.Invoke(EntityShapesContainer?.Get(), entity);
                }
            }
 
            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveShape(IEntityShape entityShape)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;
            if (entityShape == null)
                throw new ArgumentNullException(nameof(entityShape));

            var result = _entities?.Remove(entityShape) ?? false;
            if (result)
            {
                Dirty.IsDirty = true;
                if (entityShape.Identity is IEntity entity)
                    _entityShapeRemoved?.Invoke(EntityShapesContainer?.Get(), entity);
            }

            return result;
        }
        #endregion
    }
}