using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private List<IEntityShape> _entities { get; set; }
    //#endregion    

    [PSerializable]
    public class EntityShapesContainerAspect : InstanceLevelAspect
    {
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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
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

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEntityShape GetEntityShape(Guid entityId)
        {
            return _entities?.FirstOrDefault(x => x.AssociatedId == entityId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public void Add(IEntityShape entityShape)
        {
            if (entityShape == null)
                throw new ArgumentNullException(nameof(entityShape));

            if (_entities == null)
                _entities = new List<IEntityShape>();

            _entities.Add(entityShape);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 13)]
        public IEntityShape AddShape(IEntity entity, PointF position)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            IEntityShape result = null;

            if (Instance is IEntityShapesContainer container && Instance is IThreatModelChild child && GetEntityShape(entity.Id) == null)
            {
                if (_entities == null)
                    _entities = new List<IEntityShape>();
                result = new EntityShape(child.Model, entity)
                {
                    Position = position
                };
                _entities.Add(result);
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
                _entityShapeAdded?.Invoke(container, result);
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
                result = _entities.Remove(entityShape);
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (entityShape.Identity is IEntity entity && Instance is IEntityShapesContainer container)
                        _entityShapeRemoved?.Invoke(container, entity);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public bool RemoveShape(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            bool result = false;

            var entityShape = GetShape(entity);
            if (entityShape != null)
            {
                result = _entities.Remove(entityShape);
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (Instance is IEntityShapesContainer container)
                        _entityShapeRemoved?.Invoke(container, entity);
                }
            }
 
            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public bool RemoveShape(IEntityShape entityShape)
        {
            if (entityShape == null)
                throw new ArgumentNullException(nameof(entityShape));

            var result = _entities?.Remove(entityShape) ?? false;
            if (result)
            {
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
                if (entityShape.Identity is IEntity entity && Instance is IEntityShapesContainer container)
                    _entityShapeRemoved?.Invoke(container, entity);
            }

            return result;
        }
        #endregion
    }
}