using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Collections;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
    //private AdvisableCollection<IProperty> _properties { get; set; }
    //#endregion    

    /// <summary>
    /// Aspect adding the standard implementation for Property Containers.
    /// </summary>
    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class PropertiesContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        /// <summary>
        /// Importer for local property _properties.
        /// </summary>
        [ImportMember(nameof(_properties))]
        public Property<AdvisableCollection<IProperty>> _properties;

        /// <summary>
        /// Implementation of method OnPropertyChanged.
        /// </summary>
        /// <param name="property"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 2, Visibility = Visibility.Private)]
        public void OnPropertyChanged(IProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            if (Instance is IPropertiesContainer container)
                _propertyValueChanged?.Invoke(container, property);
        }
        #endregion

        #region Implementation of interface IPropertiesContainer.
        private Action<IPropertiesContainer, IProperty> _propertyAdded;

        /// <summary>
        /// Implementation of Event Handler.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "PropertyAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnPropertyAddedAdd(EventInterceptionArgs args)
        {
            if (_propertyAdded == null || !_propertyAdded.GetInvocationList().Contains(args.Handler))
            {
                _propertyAdded += (Action<IPropertiesContainer, IProperty>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        /// <summary>
        /// Implementation of Event Handler.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        [OnEventRemoveHandlerAdvice(Master = nameof(OnPropertyAddedAdd))]
        public void OnPropertyAddedRemove(EventInterceptionArgs args)
        {
            _propertyAdded -= (Action<IPropertiesContainer, IProperty>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IPropertiesContainer, IProperty> _propertyRemoved;

        /// <summary>
        /// Implementation of Event Handler.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "PropertyRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnPropertyRemovedAdd(EventInterceptionArgs args)
        {
            if (_propertyRemoved == null || !_propertyRemoved.GetInvocationList().Contains(args.Handler))
            {
                _propertyRemoved += (Action<IPropertiesContainer, IProperty>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        /// <summary>
        /// Implementation of Event Handler.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        [OnEventRemoveHandlerAdvice(Master = nameof(OnPropertyRemovedAdd))]
        public void OnPropertyRemovedRemove(EventInterceptionArgs args)
        {
            _propertyRemoved -= (Action<IPropertiesContainer, IProperty>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IPropertiesContainer, IProperty> _propertyValueChanged;

        /// <summary>
        /// Implementation of Event Handler.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "PropertyValueChanged", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnPropertyValueChangedAdd(EventInterceptionArgs args)
        {
            if (_propertyValueChanged == null || !_propertyValueChanged.GetInvocationList().Contains(args.Handler))
            {
                _propertyValueChanged += (Action<IPropertiesContainer, IProperty>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        /// <summary>
        /// Implementation of Event Handler.
        /// </summary>
        /// <param name="args">Argument of the interception.</param>
        [OnEventRemoveHandlerAdvice(Master = nameof(OnPropertyValueChangedAdd))]
        public void OnPropertyValueChangedRemove(EventInterceptionArgs args)
        {
            _propertyValueChanged -= (Action<IPropertiesContainer, IProperty>)args.Handler;
            args.ProceedRemoveHandler();
        }

        /// <summary>
        /// Implementation of Properties getter.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IProperty> Properties => _properties?.Get()?.AsEnumerable();

        /// <summary>
        /// Implementation of method HasProperty.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public bool HasProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return _properties?.Get()?.Any(x => x.PropertyTypeId == propertyType.Id) ?? false;
        }

        /// <summary>
        /// Implementation of method GetProperty.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IProperty GetProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return _properties?.Get()?.FirstOrDefault(x => x.PropertyTypeId == propertyType.Id);
        }

        /// <summary>
        /// Implementation of method Add.
        /// </summary>
        /// <param name="property">Property to be added.</param>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public void Add(IProperty property)
        {
            var properties = _properties?.Get();
            if (properties == null)
            {
                properties = new AdvisableCollection<IProperty>();
                _properties?.Set(properties);
            }

            properties.Add(property);
        }

        /// <summary>
        /// Implementation of method AddProperty.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 20)]
        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            var model = GetModel();

            IProperty result = null;
            if (model != null)
            {
                result = InternalAddProperty(propertyType, value);

                if (result != null)
                {
                    var schema = model.GetSchema(propertyType.SchemaId);
                    if (schema != null)
                    {
                        schema.PropertyTypeAdded += OnPropertyTypeAdded;
                        schema.PropertyTypeRemoved += OnPropertyTypeRemoved;
                    }
                }
            }

            return result;
        }

        private IProperty InternalAddProperty(IPropertyType propertyType, string value)
        {
            IProperty result = null;

            using (var scope = UndoRedoManager.OpenScope("Add property"))
            {
                var associatedClass = propertyType.GetType().GetCustomAttributes<AssociatedPropertyClassAttribute>()
                    .FirstOrDefault();
                if (associatedClass != null)
                {
                    var associatedClassType = Type.GetType(associatedClass.AssociatedType, false);
                    if (associatedClassType != null)
                    {
                        result = Activator.CreateInstance(associatedClassType, propertyType) as IProperty;
                    }
                }

                if (result != null)
                {
                    var model = GetModel();

                    var properties = _properties?.Get();
                    if (properties == null)
                    { 
                        properties = new AdvisableCollection<IProperty>();
                        _properties?.Set(properties);
                    }
                    result.StringValue = value;

                    UndoRedoManager.Attach(result, model);
                    properties?.Add(result);
                    scope?.Complete();
                }
            }

            if (result != null)
            {
                if (Instance is IPropertiesContainer container)
                    _propertyAdded?.Invoke(container, result);
                result.Changed += OnPropertyChanged;
            }

            return result;
        }

        private void OnPropertyTypeRemoved(IPropertySchema schema, IPropertyType propertyType)
        {
            RemoveProperty(propertyType);
        }

        private void OnPropertyTypeAdded(IPropertySchema schema, IPropertyType propertyType)
        {
            if (schema != null && propertyType != null && !HasProperty(propertyType))
            {
                if (_properties?.Get()?.Any(x => x.PropertyType != null && x.PropertyType.SchemaId == schema.Id) ?? false)
                    InternalAddProperty(propertyType, null);
            }
        }

        /// <summary>
        /// Implementation of method RemoveProperty.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public bool RemoveProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return RemoveProperty(propertyType.Id);
        }

        /// <summary>
        /// Implementation of method RemoveProperty.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public bool RemoveProperty(Guid propertyTypeId)
        {
            var result = false;

            var properties = _properties?.Get()?.Where(x => x.PropertyTypeId == propertyTypeId).ToArray();
            if (properties?.Any() ?? false) 
            {
                using (var scope = UndoRedoManager.OpenScope("Remove property"))
                {
                    foreach (var property in properties)
                    {
                        if (property != null && (_properties?.Get()?.Remove(property) ?? false))
                        {
                            UndoRedoManager.Detach(property);

                            if (Instance is IPropertiesContainer container)
                                _propertyRemoved?.Invoke(container, property);
                            result = true;
                        }
                    }
                             
                    scope?.Complete();
               }
            }

            return result;
        }

        /// <summary>
        /// Implementation of method ClearProperties.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 8)]
        public void ClearProperties()
        {
            var properties = _properties?.Get()?.ToArray();

            if (properties?.Any() ?? false)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove properties"))
                {
                    foreach (var property in properties)
                    {
                        if (property != null)
                        {
                            UndoRedoManager.Detach(property);

                            if (Instance is IPropertiesContainer container)
                                _propertyRemoved?.Invoke(container, property);
                        }
                    }

                    _properties?.Get()?.Clear();
                         
                    scope?.Complete();
               }
            }
        }

        /// <summary>
        /// Implementation of method Apply.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 15)]
        public void Apply(IPropertySchema schema)
        {
            if (Instance is IPropertiesContainer container)
            {
                var eiTemplate = false;
                var pTemplate = false;
                var dsTemplate = false;
                var fTemplate = false;
                var tbTemplate = false;
                if (container is IEntityTemplate entityTemplate)
                {
                    eiTemplate = entityTemplate.EntityType == EntityType.ExternalInteractor;
                    pTemplate = entityTemplate.EntityType == EntityType.Process;
                    dsTemplate = entityTemplate.EntityType == EntityType.DataStore;
                }
                else
                {
                    fTemplate = container is IFlowTemplate;
                    tbTemplate = container is ITrustBoundaryTemplate;
                }

                if (schema.AppliesTo.HasFlag(container.PropertiesScope) ||
                        (eiTemplate && schema.AppliesTo.HasFlag(Scope.ExternalInteractor)) ||
                        (pTemplate && schema.AppliesTo.HasFlag(Scope.Process)) ||
                        (dsTemplate && schema.AppliesTo.HasFlag(Scope.DataStore)) ||
                        (fTemplate && schema.AppliesTo.HasFlag(Scope.DataFlow)) ||
                        (tbTemplate && schema.AppliesTo.HasFlag(Scope.TrustBoundary)))
                {
                    using (var scope = UndoRedoManager.OpenScope("Apply Property Schema"))
                    {
                        var existingProp = container.Properties?.ToArray();
                        var schemaProp = schema.PropertyTypes?.ToArray();
                        var missing = existingProp == null
                            ? schemaProp
                            : schemaProp?.Except(existingProp.Select(x => x.PropertyType)).ToArray();
                        var inExcess = existingProp?.Where(x => x.PropertyType != null &&
                                                                x.PropertyType.SchemaId == schema.Id &&
                                                                !(schemaProp?.Any(y => y.Id == x.PropertyTypeId) ?? false))
                            .Select(x => x.PropertyType).ToArray();

                        if (missing?.Any() ?? false)
                        {
                            foreach (var item in missing)
                            {
                                if (item != null)
                                    container.AddProperty(item, null);
                            }
                        }

                        if (inExcess?.Any() ?? false)
                        {
                            foreach (var item in inExcess)
                            {
                                if (item != null)
                                    container.RemoveProperty(item);
                            }
                        }

                        scope?.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// Implementation of method Unapply.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public void Unapply(IPropertySchema schema)
        {
            if (Instance is IPropertiesContainer container && schema.AppliesTo.HasFlag(container.PropertiesScope))
            {
                using (var scope = UndoRedoManager.OpenScope("Unapply Property Schema"))
                {
                    var existingProp = container.Properties?.ToArray();
                    var schemaProp = schema.PropertyTypes?.ToArray();
                    var toBeRemoved = existingProp?
                        .Where(x => x.PropertyType != null && x.PropertyType.SchemaId == schema.Id)
                        .ToArray();

                    if (toBeRemoved?.Any() ?? false)
                    {
                        foreach (var property in toBeRemoved)
                        {
                            if (property != null)
                                container.RemoveProperty(property.PropertyTypeId);
                        }
                    }

                    scope?.Complete();
                }
            }
        }
        #endregion

        #region Additional methods.
        /// <summary>
        /// Activities to be executed after the deserialization of the object. 
        /// </summary>
        /// <param name="context">Serialization context</param>
        [IntroduceMember(OverrideAction=MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 17, 
            Visibility = Visibility.Assembly)]
        [CopyCustomAttributes(typeof(OnDeserializedAttribute))]
        [OnDeserialized]
        public void PostDeserialization(StreamingContext context)
        {
            var schemas = _properties?.Get()?
                .Select(x => x.PropertyType)
                .Where(x => x != null)
                .Select(x => x.SchemaId)
                .Distinct();

            var model = GetModel();
            if (schemas?.Any() ?? false)
            {
                if (model != null)
                {
                    foreach (var schemaId in schemas)
                    {
                        if (schemaId != null)
                        {
                            var schema = model.GetSchema(schemaId);
                            if (schema != null)
                            {
                                schema.PropertyTypeAdded += OnPropertyTypeAdded;
                                schema.PropertyTypeRemoved += OnPropertyTypeRemoved;
                            }
                        }
                    }
                }
            }

            var properties = _properties?.Get()?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if (property != null)
                    {
                        if (model != null)
                            UndoRedoManager.Attach(property, model);
                        property.Changed += OnPropertyChanged;
                    }
                }
            }

            if (Instance is IPostDeserialization postDeserialization)
                postDeserialization.ExecutePostDeserialization();
        }

        private IThreatModel GetModel()
        {
            IThreatModel result = null;

            if (Instance is IThreatModelChild modelChild)
                result = modelChild.Model;
            else if (Instance is IThreatModel model)
                result = model;

            return result;
        }
        #endregion
    }
}
