using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Extensions used for cloning parts of the Threat Model.
    /// </summary>
    public static class CloningExtensions
    {
        /// <summary>
        /// Clone Properties between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CloneProperties(this IPropertiesContainer source, 
            [NotNull] IPropertiesContainer target)
        {
            var properties = source?.Properties?.ToArray();
            if ((properties?.Any() ?? false) && 
                target is IThreatModelChild child &&
                child.Model is IThreatModel model &&
                properties.FirstOrDefault()?.Model is IThreatModel sourceModel)
            {
                foreach (var property in properties)
                {
                    if (property.PropertyType is IPropertyType sourcePropertyType &&
                        sourceModel.GetSchema(sourcePropertyType.SchemaId) is IPropertySchema sourceSchema)
                    {
                        if (model.GetSchema(sourceSchema.Name, sourceSchema.Namespace) is IPropertySchema targetSchema &&
                            !targetSchema.NotExportable)
                        {
                            var propertyType = targetSchema.GetPropertyType(sourcePropertyType.Name);
                            if (propertyType != null)
                            {
                                var propertyTarget = target.GetProperty(propertyType);
                                if (propertyTarget == null)
                                    target.AddProperty(propertyType, property.StringValue);
                                else
                                    propertyTarget.StringValue = property.StringValue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clone Threat Events between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void CloneThreatEvents(this IThreatEventsContainer source,
            [NotNull] IThreatEventsContainer target)
        {
            var threatEvents = source?.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    threatEvent.Clone(target);
                }
            }
        }

        /// <summary>
        /// Clone Property Types between Containers.
        /// </summary>
        /// <param name="source">Source Container.</param>
        /// <param name="target">Target Container.</param>
        public static void ClonePropertyTypes(this IPropertyTypesContainer source,
            [NotNull] IPropertyTypesContainer target)
        {
            var propertyTypes = source?.PropertyTypes?.ToArray();
            if (propertyTypes?.Any() ?? false)
            {
                foreach (var propertyType in propertyTypes)
                {
                    propertyType.Clone(target);
                }
            }
        }

        /// <summary>
        /// Merge Property Types between Containers.
        /// </summary>
        /// <param name="target">Target Container.</param>
        /// <param name="source">Source Container.</param>
        public static void MergePropertyTypes(this IPropertyTypesContainer target,
            [NotNull] IPropertyTypesContainer source)
        {
            var propertyTypes = source?.PropertyTypes?.ToArray();
            if (propertyTypes?.Any() ?? false)
            {
                foreach (var propertyType in propertyTypes)
                {
                    var targetPropertyType = target.GetPropertyType(propertyType.Name);
                    if (targetPropertyType == null)
                        propertyType.Clone(target);
                }
            }
        }

        /// <summary>
        /// Clone Properties between Containers.
        /// </summary>
        /// <param name="target">Target Container.</param>
        /// <param name="source">Source Container.</param>
        public static void MergeProperties(this IPropertiesContainer target, 
            [NotNull] IPropertiesContainer source)
        {
            var properties = source?.Properties?.ToArray();
            if ((properties?.Any() ?? false) && 
                source is IThreatModelChild sourceChild &&
                sourceChild.Model is IThreatModel sourceModel &&
                target is IThreatModelChild targetChild &&
                targetChild.Model is IThreatModel targetModel)
            {
                foreach (var property in properties)
                {
                    var sourcePropertyType = property.PropertyType;
                    if (sourcePropertyType != null)
                    {
                        var sourceSchema = sourceModel.GetSchema(sourcePropertyType.SchemaId);
                        if (sourceSchema != null)
                        {
                            var targetSchema = targetModel.GetSchema(sourceSchema.Name, sourceSchema.Namespace) ??
                                               sourceSchema.Clone(targetModel);

                            var targetPropertyType = targetSchema.GetPropertyType(sourcePropertyType.Name) ??
                                                     sourcePropertyType.Clone(targetSchema);

                            var targetProperty = target.GetProperty(targetPropertyType);
                            if (targetProperty == null)
                            {
                                target.AddProperty(targetPropertyType, property.StringValue);
                            }
                            else
                            {
                                if (targetProperty is IPropertyJsonSerializableObject jsonSerializableObject &&
                                    jsonSerializableObject.Value is IMergeable mergeable &&
                                    property is IPropertyJsonSerializableObject sourceJsonSerializableObject &&
                                    sourceJsonSerializableObject.Value is IMergeable sourceMergeable)
                                {
                                    mergeable.Merge(sourceMergeable);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
