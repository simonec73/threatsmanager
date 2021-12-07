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
            schema.Priority = 1000;

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
                result.DoNotPrint = false;
                result.Visible = true;
                result.CustomPropertyViewer = "Annotations Property Viewer";
                result.Description = Resources.AnnotationsDescription;
            }

            return result;
        }

        public bool AreAnnotationsEnabled([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                result = container.GetProperty(propertyType) != null;
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

        public void RemoveAnnotations([NotNull] IPropertiesContainer container)
        {
            var propertyType = GetAnnotationsPropertyType();
            if (propertyType != null)
            {
                var property = container.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Annotations.Annotations annotations)
                {
                    annotations.Clear();
                }
            }
        }

        public bool HasNotes([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is Annotations.Annotations annotations)
            {
                result = annotations.Items.Any(x => !(x is Highlight) && !(x is TopicToBeClarified) && !(x is ReviewNote) && !(x is AnnotationAnswer));
            }

            return result;
        }

        public bool HasTopics([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is Annotations.Annotations annotations)
            {
                result = annotations.Items.OfType<TopicToBeClarified>().Any();
            }

            return result;
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
        
        public bool HasClosedTopics([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is Annotations.Annotations annotations)
            {
                result = annotations.Items.OfType<TopicToBeClarified>().Any(x => x.Answered);
            }

            return result;
        }

        public bool HasHighlights([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is Annotations.Annotations annotations)
            {
                result = annotations.Items.OfType<Highlight>().Any();
            }

            return result;
        }

        public bool HasReviewNotes([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var propertyType = GetAnnotationsPropertyType();
            if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                jsonSerializableObject.Value is Annotations.Annotations annotations)
            {
                result = annotations.Items.OfType<ReviewNote>().Any();
            }

            return result;
        }
        #endregion
    }
}
