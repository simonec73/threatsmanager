﻿using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.StatusInfoProviders
{
    [Extension("C9C8D7DF-EFA7-40B0-BC51-F6FDBB34E316", "Note Counter Status Info Provider", 150, ExecutionMode.Simplified)]
    public class NoteCounter : IStatusInfoProviderExtension
    {
        private IThreatModel _model;
        private AnnotationsPropertySchemaManager _schemaManager;
        private IPropertyType _propertyType;
        
        public event Action<string, string> UpdateInfo;

        public void Initialize([NotNull] IThreatModel model)
        {
            if (_model != null)
            {
                Dispose();
            }

            _model = model;
            _schemaManager = new AnnotationsPropertySchemaManager(model);
            _propertyType = _schemaManager.GetAnnotationsPropertyType();
            _model.PropertyAdded += PropertyUpdated;
            _model.PropertyRemoved += PropertyUpdated;
            _model.PropertyValueChanged += PropertyUpdated;
            _model.ChildPropertyAdded += ChildPropertyUpdated;
            _model.ChildPropertyChanged += ChildPropertyUpdated;
            _model.ChildPropertyChanged += ChildPropertyUpdated;
        }

        private void PropertyUpdated(IPropertiesContainer container, IProperty property)
        {
            if (property.PropertyTypeId == _propertyType.Id)
                Update();
        }

        private void ChildPropertyUpdated(IIdentity container, IPropertyType propertyType, IProperty property)
        {
            if (propertyType.Id == _propertyType.Id)
                Update();
        }

        public string CurrentStatus
        {
            get
            {
                var count = Count(_model) + Count(_model.Entities) + Count(_model.DataFlows) + Count(_model.Groups) + Count(_model.Diagrams) +
                            Count(_model.ThreatEvents) +Count(_model.Mitigations) + Count(_model.ThreatActors) +
                            Count(_model.EntityTemplates) + Count(_model.FlowTemplates) + Count(_model.TrustBoundaryTemplates);

                return $"Notes: {count}";
            }
        }

        private int Count(IEnumerable<IPropertiesContainer> containers)
        {
            var result = 0;

            var list = containers?.ToArray();
            if (list?.Any() ?? false)
            {
                foreach (var item in list)
                {
                    result += Count(item);
                }
            }

            return result;
        }

        private int Count(IPropertiesContainer container)
        {
            var result = 0;

            if (container != null)
            {
                result = _schemaManager.GetAnnotations(container)?
                    .Count(x => !(x is AnnotationAnswer || x is Highlight || x is ReviewNote || x is TopicToBeClarified)) ?? 0;
            }

            return result;
        }

        public string Description => "Counter of the Notes defined in the Threat Model.";

        private void Update()
        {
            UpdateInfo?.Invoke(this.GetExtensionId(), CurrentStatus);
        }

        public override string ToString()
        {
            return "Notes Counter";
        }
 
        public void Dispose()
        {
            _model.PropertyAdded -= PropertyUpdated;
            _model.PropertyRemoved -= PropertyUpdated;
            _model.PropertyValueChanged -= PropertyUpdated;
            _model.ChildPropertyAdded -= ChildPropertyUpdated;
            _model.ChildPropertyChanged -= ChildPropertyUpdated;
            _model.ChildPropertyChanged -= ChildPropertyUpdated;
            _schemaManager = null;
            _propertyType = null;
            _model = null;
        }
    }
}
