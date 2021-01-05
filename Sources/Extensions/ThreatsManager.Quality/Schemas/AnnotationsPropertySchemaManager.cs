using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Quality.Schemas
{
    public class AnnotationsPropertySchemaManager : IInitializableObject
    {
        private readonly IThreatModel _model;
        private static string Annotations = "Annotations";

        public AnnotationsPropertySchemaManager([NotNull] IThreatModel model)
        {
            _model = model;
        }

        public bool IsInitialized => _model != null;

        [InitializationRequired]
        public IPropertySchema GetSchema()
        {
            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace) ?? _model.AddSchema(Annotations, Resources.DefaultNamespace);
            schema.Description = Resources.AnnotationsPropertySchemaDescription;
            schema.Visible = true;
            schema.AppliesTo = Scope.All;
            schema.System = true;
            schema.AutoApply = false;
            schema.NotExportable = true;

            return schema;
        }

        #region Annotations.
        [InitializationRequired]
        public IPropertyType GetAnnotationsPropertyType()
        {
            IPropertyType result = null;

            var schema = GetSchema();
            if (schema != null)
            {
                result = schema.GetPropertyType(Annotations) ?? schema.AddPropertyType(Annotations, PropertyValueType.JsonSerializableObject);
                result.DoNotPrint = true;
                result.Visible = true;
                result.CustomPropertyViewer = "Annotations Property Viewer";
                result.Description = Resources.AnnotationsDescription;
            }

            return result;
        }

        public bool EnableAnnotations([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property == null)
                {
                    result = container.AddProperty(propertyType, null) != null;
                }
            }

            return result;
        }

        public bool DisableAnnotations([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property != null)
                {
                    result = container.RemoveProperty(propertyType);
                }
            }

            return result;
        }

        public IEnumerable<Annotation> GetAnnotations([NotNull] IPropertiesContainer container)
        {
            IEnumerable<Annotation> result = null;

            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Annotations.Annotations annotations)
                {
                    result = annotations.Items;
                }
            }

            return result;
        }

        public void AddAnnotation([NotNull] IPropertiesContainer container, [NotNull] Annotation annotation)
        {
            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property == null)
                {
                    property = container.AddProperty(propertyType, null);
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                    {
                        var annotations = new Annotations.Annotations();
                        annotations.Add(annotation);
                        jsonSerializableObject.Value = annotations;
                    }
                } else if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                {
                    if (!(jsonSerializableObject.Value is Annotations.Annotations annotations))
                    {
                        annotations = new Annotations.Annotations();
                        jsonSerializableObject.Value = annotations;
                    }
                    annotations.Add(annotation);
                }
            }
        }

        public void RemoveAnnotation([NotNull] IPropertiesContainer container, [NotNull] Annotation annotation)
        {
            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Annotations.Annotations annotations)
                {
                    annotations.Remove(annotation);
                }
            }
        }

        public bool HasOpenTopics([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is Annotations.Annotations annotations)
            {
                result = annotations.Items.OfType<TopicToBeClarified>().Any(x => !x.Answered);
            }

            return result;
        }
        #endregion
    }
}
