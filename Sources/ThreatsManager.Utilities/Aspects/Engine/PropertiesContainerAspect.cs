using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Patterns.Collections;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("properties")]
    //private IList<IProperty> _properties { get; set; }
    //#endregion    

    [PSerializable]
    public class PropertiesContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_properties))]
        public Property<IList<IProperty>> _properties;

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

        [OnEventRemoveHandlerAdvice(Master = nameof(OnPropertyAddedAdd))]
        public void OnPropertyAddedRemove(EventInterceptionArgs args)
        {
            _propertyAdded -= (Action<IPropertiesContainer, IProperty>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IPropertiesContainer, IProperty> _propertyRemoved;

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

        [OnEventRemoveHandlerAdvice(Master = nameof(OnPropertyRemovedAdd))]
        public void OnPropertyRemovedRemove(EventInterceptionArgs args)
        {
            _propertyRemoved -= (Action<IPropertiesContainer, IProperty>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IPropertiesContainer, IProperty> _propertyValueChanged;

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

        [OnEventRemoveHandlerAdvice(Master = nameof(OnPropertyValueChangedAdd))]
        public void OnPropertyValueChangedRemove(EventInterceptionArgs args)
        {
            _propertyValueChanged -= (Action<IPropertiesContainer, IProperty>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IProperty> Properties => _properties?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public bool HasProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return _properties?.Get()?.Any(x => x.PropertyTypeId == propertyType.Id) ?? false;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IProperty GetProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return _properties?.Get()?.FirstOrDefault(x => x.PropertyTypeId == propertyType.Id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 20)]
        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            var model = GetModel();

            IProperty result = null;
            if (model != null)
            {
                result = InternalAddProperty(model, propertyType, value);

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

        private IProperty InternalAddProperty(IThreatModel model, IPropertyType propertyType, string value)
        {
            IProperty result = null;

            var associatedClass = propertyType.GetType().GetCustomAttributes<AssociatedPropertyClassAttribute>()
                .FirstOrDefault();
            if (associatedClass != null)
            {
                var associatedClassType = Type.GetType(associatedClass.AssociatedType, false);
                if (associatedClassType != null)
                {
                    result = Activator.CreateInstance(associatedClassType, model, propertyType) as IProperty;
                }
            }

            if (result != null)
            {
                var properties = _properties?.Get();
                if (properties == null)
                { 
                    properties = new AdvisableCollection<IProperty>();
                    _properties?.Set(properties);
                }
                result.StringValue = value;
                properties?.Add(result);

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
            var model = GetModel();
            if (model != null && schema != null && propertyType != null && !HasProperty(propertyType))
            {
                if (_properties?.Get()?.Any(x => x.PropertyType != null && x.PropertyType.SchemaId == schema.Id) ?? false)
                    InternalAddProperty(model, propertyType, null);
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public bool RemoveProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            bool result = false;

            var property = GetProperty(propertyType);
            if (property != null)
            {
                result = _properties?.Get()?.Remove(property) ?? false;
                if (result)
                {
                    if (Instance is IPropertiesContainer container)
                        _propertyRemoved?.Invoke(container, property);
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public bool RemoveProperty(Guid propertyTypeId)
        {
            bool result = false;

            var properties = _properties?.Get()?.Where(x => x.PropertyTypeId == propertyTypeId).ToArray();
            if (properties?.Any() ?? false) 
            {
                foreach (var property in properties)
                {
                    if (_properties?.Get()?.Remove(property) ?? false)
                    {
                        if (Instance is IPropertiesContainer container)
                            _propertyRemoved?.Invoke(container, property);
                        result = true;
                    }
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void ClearProperties()
        {
            var properties = _properties?.Get()?.ToArray();

            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if (Instance is IPropertiesContainer container)
                        _propertyRemoved?.Invoke(container, property);
                }

                _properties?.Get()?.Clear();
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void Apply(IPropertySchema schema)
        {
            if (Instance is IPropertiesContainer container && schema.AppliesTo.HasFlag(container.PropertiesScope))
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
                        container.AddProperty(item, null);
                    }
                }

                if (inExcess?.Any() ?? false)
                {
                    foreach (var item in inExcess)
                    {
                        container.RemoveProperty(item);
                    }
                }
            }
        }
        #endregion

        #region Additional methods.
        [IntroduceMember(OverrideAction=MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7, 
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

            if (schemas?.Any() ?? false)
            {
                var model = GetModel();
                if (model != null)
                {
                    foreach (var schemaId in schemas)
                    {
                        var schema = model.GetSchema(schemaId);
                        schema.PropertyTypeAdded += OnPropertyTypeAdded;
                        schema.PropertyTypeRemoved += OnPropertyTypeRemoved;
                    }
                }
            }

            var properties = _properties?.Get()?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    property.Changed += OnPropertyChanged;
                }
            }
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
