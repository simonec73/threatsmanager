using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Utilities;
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
            IPropertySchema result;

            using (var scope = UndoRedoManager.OpenScope($"Get '{Annotations}' schema"))
            {
                result = _model.GetSchema(Annotations, Resources.DefaultNamespace) ?? _model.AddSchema(Annotations, Resources.DefaultNamespace);
                result.Description = Resources.AnnotationsPropertySchemaDescription;
                result.Visible = true;
                result.AppliesTo = Scope.All;
                result.System = true;
                result.AutoApply = false;
                result.NotExportable = true;
                result.Priority = 1000;
                scope?.Complete();
            }

            return result;
        }

        #region Annotations.
        [InitializationRequired]
        public IPropertyType GetAnnotationsPropertyType()
        {
            IPropertyType result = null;

            using (var scope = UndoRedoManager.OpenScope("Get Annotations property type"))
            {
                var schema = GetSchema();
                if (schema != null)
                {
                    result = schema.GetPropertyType(Annotations) ?? schema.AddPropertyType(Annotations, PropertyValueType.JsonSerializableObject);
                    result.DoNotPrint = false;
                    result.Visible = true;
                    result.CustomPropertyViewer = "Annotations Property Viewer";
                    result.Description = Resources.AnnotationsDescription;

                    scope?.Complete();
                }
            }

            return result;
        }

        public bool AnnotationsEnabled => _model.GetSchema(Annotations, Resources.DefaultNamespace) != null;

        public bool AreAnnotationsEnabled([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    result = container.GetProperty(propertyType) != null;
                }
            }

            return result;
        }

        public bool EnableAnnotations([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Enable Annotations"))
            {
                var propertyType = GetAnnotationsPropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType);
                    if (property == null)
                    {
                        result = container.AddProperty(propertyType, null) != null;
                        scope?.Complete();
                    }
                }
            }

            return result;
        }

        public bool DisableAnnotations([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            using (var scope = UndoRedoManager.OpenScope("Disable Annotations"))
            {
                var propertyType = GetAnnotationsPropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType);
                    if (property != null)
                    {
                        result = container.RemoveProperty(propertyType);
                        scope?.Complete();
                    }
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
            using (var scope = UndoRedoManager.OpenScope("Add Annotation"))
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
                            scope?.Complete();
                        }
                    }
                    else if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                    {
                        if (!(jsonSerializableObject.Value is Annotations.Annotations annotations))
                        {
                            annotations = new Annotations.Annotations();
                            jsonSerializableObject.Value = annotations;
                        }
                        annotations.Add(annotation);
                        scope?.Complete();
                    }
                }
            }
        }

        public void RemoveAnnotation([NotNull] IPropertiesContainer container, [NotNull] Annotation annotation)
        {
            using (var scope = UndoRedoManager.OpenScope("Remove Annotation"))
            {
                var propertyType = GetAnnotationsPropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType);
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        annotations.Remove(annotation);
                        scope?.Complete();
                    }
                }
            }
        }

        public void RemoveAnnotations([NotNull] IPropertiesContainer container)
        {
            using (var scope = UndoRedoManager.OpenScope("Remove Annotations"))
            {
                var propertyType = GetAnnotationsPropertyType();
                if (propertyType != null)
                {
                    var property = container.GetProperty(propertyType);
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        annotations.Clear();
                        scope?.Complete();
                    }
                }
            }
        }

        public bool HasAnnotations([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?.Any() ?? false;
                    }
                }
            }

            return result;
        }

        public bool HasNotes([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?
                            .Any(x => !(x is Highlight) && !(x is TopicToBeClarified) && !(x is ReviewNote) && !(x is AnnotationAnswer))
                            ?? false;
                    }
                }
            }

            return result;
        }

        public bool HasTopics([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?.OfType<TopicToBeClarified>().Any() ?? false;
                    }
                }
            }

            return result;
        }

        public bool HasOpenTopics([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?.OfType<TopicToBeClarified>().Any(x => !x.Answered) ?? false;
                    }
                }
            }

            return result;
        }
        
        public bool HasClosedTopics([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?.OfType<TopicToBeClarified>().Any(x => x.Answered) ?? false;
                    }
                }
            }

            return result;
        }

        public bool HasHighlights([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?.OfType<Highlight>().Any() ?? false;
                    }
                }
            }

            return result;
        }

        public bool HasReviewNotes([NotNull] IPropertiesContainer container)
        {
            bool result = false;

            var schema = _model.GetSchema(Annotations, Resources.DefaultNamespace);
            if (schema != null)
            {
                var propertyType = schema.GetPropertyType(Annotations);
                if (propertyType != null)
                {
                    if (container.GetProperty(propertyType) is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Annotations.Annotations annotations)
                    {
                        result = annotations.Items?.OfType<ReviewNote>().Any() ?? false;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
