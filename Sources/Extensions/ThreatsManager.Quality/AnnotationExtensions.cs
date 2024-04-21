using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality
{
    public static class AnnotationExtensions
    {
        public static IEnumerable<IPropertiesContainer> GetContainersWithAnnotations(this IThreatModel model)
        {
            IEnumerable<IPropertiesContainer> result = null;

            var containers = new List<IPropertiesContainer>();
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            var entities = model?.Entities?.Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (entities?.Any() ?? false)
            {
                containers.AddRange(entities);
            }
            var flows = model?.DataFlows?.Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (flows?.Any() ?? false)
            {
                containers.AddRange(flows);
            }
            var groups = model?.Groups?.Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (groups?.Any() ?? false)
            {
                containers.AddRange(groups);
            }
            var threatEvents = model?.GetThreatEvents()?.Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (threatEvents?.Any() ?? false)
            {
                containers.AddRange(threatEvents);
            }
            var threatEventMitigations = model?.GetThreatEventMitigations()?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (threatEventMitigations?.Any() ?? false)
            {
                containers.AddRange(threatEventMitigations);
            }
            var threatTypes = model?.ThreatTypes?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (threatEventMitigations?.Any() ?? false)
            {
                containers.AddRange(threatEventMitigations);
            }
            var mitigations = model?.Mitigations?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (mitigations?.Any() ?? false)
            {
                containers.AddRange(mitigations);
            }
            var threatTypeMitigations = model?.GetThreatTypeMitigations()?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (threatTypeMitigations?.Any() ?? false)
            {
                containers.AddRange(threatTypeMitigations);
            }
            var entityTenplates = model?.EntityTemplates?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (entityTenplates?.Any() ?? false)
            {
                containers.AddRange(entityTenplates);
            }
            var flowTemplates = model?.FlowTemplates?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (flowTemplates?.Any() ?? false)
            {
                containers.AddRange(flowTemplates);
            }
            var trustBoundaryTemplates = model?.TrustBoundaryTemplates?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (trustBoundaryTemplates?.Any() ?? false)
            {
                containers.AddRange(trustBoundaryTemplates);
            }
            var diagrams = model?.Diagrams?
                .Where(x => schemaManager.HasAnnotations(x)).ToArray();
            if (diagrams?.Any() ?? false)
            {
                containers.AddRange(diagrams);
            }
            if (schemaManager.HasAnnotations(model))
            {
                containers.Add(model);
            }

            if (containers.Any())
            {
                result = containers;
            }

            return result;
        }

        public static IEnumerable<IPropertiesContainer> GetContainersWithNotes(this IThreatModel model)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            return GetContainersWithAnnotations(model)?.Where(x => schemaManager.HasNotes(x));
        }

        public static IEnumerable<IPropertiesContainer> GetContainersWithTopics(this IThreatModel model)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            return GetContainersWithAnnotations(model)?.Where(x => schemaManager.HasTopics(x));
        }

        public static IEnumerable<IPropertiesContainer> GetContainersWithOpenTopics(this IThreatModel model)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            return GetContainersWithAnnotations(model)?.Where(x => schemaManager.HasOpenTopics(x));
        }

        public static IEnumerable<IPropertiesContainer> GetContainersWithClosedTopics(this IThreatModel model)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            return GetContainersWithAnnotations(model)?.Where(x => schemaManager.HasClosedTopics(x));
        }

        public static IEnumerable<IPropertiesContainer> GetContainersWithHighlights(this IThreatModel model)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            return GetContainersWithAnnotations(model)?.Where(x => schemaManager.HasHighlights(x));
        }

        public static IEnumerable<IPropertiesContainer> GetContainersWithReviewNotes(this IThreatModel model)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(model);

            return GetContainersWithAnnotations(model)?.Where(x => schemaManager.HasReviewNotes(x));
        }

        public static IEnumerable<Annotation> GetAnnotations(this IPropertiesContainer container)
        {
            IEnumerable<Annotation> result = null;

            IThreatModel model = null;
            if (container is IThreatModelChild child)
            {
                model = child.Model;
            }
            else if (container is IThreatModel model1)
            {
                model = model1;
            }

            if (model != null)
            {
                var schemaManager = new AnnotationsPropertySchemaManager(model);
                result = schemaManager.GetAnnotations(container);
            }

            return result;
        }

        public static IEnumerable<Annotation> GetNotes(this IPropertiesContainer container)
        {
            return GetAnnotations(container)?
                .Where(x => !(x is Highlight) && !(x is TopicToBeClarified) 
                    && !(x is ReviewNote) && !(x is AnnotationAnswer));
        }

        public static IEnumerable<TopicToBeClarified> GetTopics(this IPropertiesContainer container)
        {
            return GetAnnotations(container)?.OfType<TopicToBeClarified>();
        }

        public static IEnumerable<TopicToBeClarified> GetOpenTopics(this IPropertiesContainer container)
        {
            return GetTopics(container)?.Where(x => !x.Answered);
        }

        public static IEnumerable<TopicToBeClarified> GetClosedTopics(this IPropertiesContainer container)
        {
            return GetTopics(container)?.Where(x => x.Answered);
        }

        public static IEnumerable<Highlight> GetHighlights(this IPropertiesContainer container)
        {
            return GetAnnotations(container)?.OfType<Highlight>();
        }
    }
}
