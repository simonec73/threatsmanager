using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Engine.Aspects;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class SeverityDefinition : ISeverity, IInitializableObject
    {
        public SeverityDefinition()
        {
            
        }

        public SeverityDefinition([Range(0, 100)] int id, [Required] string name) : this()
        {
            Id = id;
            Name = name;
            Visible = true;
        }

        public bool IsInitialized => Model != null;
        
        #region Default implementation.
        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IProperty> Properties { get; }
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        public IProperty GetProperty(IPropertyType propertyType)
        {
            return null;
        }

        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            return null;
        }

        public bool RemoveProperty(IPropertyType propertyType)
        {
            return false;
        }

        public bool RemoveProperty(Guid propertyTypeId)
        {
            return false;
        }

        public void ClearProperties()
        {
        }

        public void Apply(IPropertySchema schema)
        {
        }

        public void Unapply(IPropertySchema schema)
        {
        }

        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }
        #endregion

        #region Additional placeholders required.
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.Severity;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("visible", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        [JsonProperty("textColor")]
        [JsonConverter(typeof(StringEnumConverter))]
        private KnownColor? _textColor { get; set; }

        [property: NotRecorded]
        [SafeForDependencyAnalysis]
        public KnownColor TextColor
        {
            get
            {
                KnownColor result = KnownColor.Black;

                if (_textColor.HasValue)
                    result = _textColor.Value;
                else
                {
                    if (Enum.TryParse<DefaultSeverity>(Id.ToString(), out var severity))
                    {
                        result = severity.GetEnumTextColor();
                    }
                }

                return result;
            }

            set
            {
                if (!_textColor.HasValue || value != _textColor.Value)
                {
                    _textColor = value;
                }
            }
        }

        [JsonProperty("backColor")]
        [JsonConverter(typeof(StringEnumConverter))]
        private KnownColor? _backColor { get; set; }

        [property: NotRecorded]
        [SafeForDependencyAnalysis]
        public KnownColor BackColor
        {
            get
            {
                KnownColor result = KnownColor.White;

                if (_backColor.HasValue)
                    result = _backColor.Value;
                else
                {
                    if (Enum.TryParse<DefaultSeverity>(Id.ToString(), out var severity))
                    {
                        result = severity.GetEnumBackColor();
                    }
                }

                return result;
            }

            set
            {
                if (!_backColor.HasValue || value != _backColor.Value)
                {
                    _backColor = value;
                }
            }
        }

        public ISeverity Clone(ISeveritiesContainer container)
        {
            SeverityDefinition result = null;

            if (container is IThreatModel model)
            {
                result = new SeverityDefinition
                {
                    Id = Id, 
                    Name = Name, 
                    Description = Description,
                    _model = model, 
                    _modelId = model.Id,
                    Visible = Visible,
                    _textColor = _textColor,
                    _backColor = _backColor
                };
                container.Add(result);
                this.CloneProperties(result);
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            var comparer = new SeverityComparer();
            return comparer.Compare(this, obj as ISeverity);
        }

        public int CompareTo(ISeverity obj)
        {
            var comparer = new SeverityComparer();
            return comparer.Compare(this, obj);
        }
 
        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion
    }
}