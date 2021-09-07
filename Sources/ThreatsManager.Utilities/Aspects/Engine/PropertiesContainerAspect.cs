using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //private List<IProperty> _properties { get; set; }
    //#endregion    

    [PSerializable]
    public class PropertiesContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("properties")]
        public List<IProperty> _properties { get; set; }

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

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IPropertiesContainer, IProperty> PropertyAdded
        {
            add
            {
                if (_propertyAdded == null || !_propertyAdded.GetInvocationList().Contains(value))
                {
                    _propertyAdded += value;
                }
            }
            remove { _propertyAdded -= value; }
        }

        private Action<IPropertiesContainer, IProperty> _propertyRemoved;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved
        {
            add
            {
                if (_propertyRemoved == null || !_propertyRemoved.GetInvocationList().Contains(value))
                {
                    _propertyRemoved += value;
                }
            }
            remove { _propertyRemoved -= value; }
        }

        private Action<IPropertiesContainer, IProperty> _propertyValueChanged;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged
        {
            add
            {
                if (_propertyValueChanged == null || !_propertyValueChanged.GetInvocationList().Contains(value))
                {
                    _propertyValueChanged += value;
                }
            }
            remove { _propertyValueChanged -= value; }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IProperty> Properties => _properties?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public bool HasProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return _properties?.Any(x => x.PropertyTypeId == propertyType.Id) ?? false;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IProperty GetProperty(IPropertyType propertyType)
        {
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));

            return _properties?.FirstOrDefault(x => x.PropertyTypeId == propertyType.Id);
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
                if (_properties == null)
                    _properties = new List<IProperty>();
                result.StringValue = value;
                _properties.Add(result);

                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
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
                if (_properties?.Any(x => x.PropertyType != null && x.PropertyType.SchemaId == schema.Id) ?? false)
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
                result = _properties?.Remove(property) ?? false;
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
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

            var properties = _properties?.Where(x => x.PropertyTypeId == propertyTypeId).ToArray();
            if (properties?.Any() ?? false) 
            {
                foreach (var property in properties)
                {
                    if (_properties.Remove(property))
                    {
                        if (Instance is IPropertiesContainer container)
                            _propertyRemoved?.Invoke(container, property);
                        result = true;
                    }
                }

                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void ClearProperties()
        {
            var properties = _properties?.ToArray();

            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if (Instance is IPropertiesContainer container)
                        _propertyRemoved?.Invoke(container, property);
                }

                _properties?.Clear();
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
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
            var schemas = _properties?
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

            var properties = _properties?.ToArray();
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
