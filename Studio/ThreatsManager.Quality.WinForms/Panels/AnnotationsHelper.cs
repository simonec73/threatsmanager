using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.Panels
{
    static class AnnotationsHelper
    {
        public static bool HasNotesWithText(this IPropertiesContainer container, [NotNull] AnnotationsPropertySchemaManager schemaManager, string filter)
        {
            return string.IsNullOrWhiteSpace(filter) ||
                   (schemaManager.GetAnnotations(container)?
                       .Any(x => x.Text != null && x.Text.ToLower().Contains(filter.ToLower()) || 
                                 (x is TopicToBeClarified topic &&
                                  (topic.Answers?.Any(y => y.Text != null && y.Text.ToLower().Contains(filter.ToLower())) ?? false))) ?? false);
        }

        public static IEnumerable<IExternalInteractor> GetExternalInteractors(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.Entities?.OfType<IExternalInteractor>()
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IProcess> GetProcesses(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.Entities?.OfType<IProcess>()
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IDataStore> GetDataStores(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.Entities?.OfType<IDataStore>()
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IDataFlow> GetFlows(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.DataFlows?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<ITrustBoundary> GetTrustBoundaries(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.Groups?.OfType<ITrustBoundary>()
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IThreatEvent> GetThreatEvents(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.GetThreatEvents()?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IThreatEventMitigation> GetThreatEventMitigations(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.GetThreatEventMitigations()?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Mitigation.Name)
                .ThenBy(x => x.ThreatEvent.Name)
                .ThenBy(x => x.ThreatEvent.Parent.Name);
        }

        public static IEnumerable<IThreatType> GetThreatTypes(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.ThreatTypes?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IMitigation> GetKnownMitigations(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.Mitigations?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IThreatTypeMitigation> GetStandardMitigations(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.GetThreatTypeMitigations()?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Mitigation.Name)
                .ThenBy(x => x.ThreatType.Name);
        }

        public static IEnumerable<IEntityTemplate> GetEntityTemplates(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.EntityTemplates?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IFlowTemplate> GetFlowTemplates(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.FlowTemplates?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<ITrustBoundaryTemplate> GetTrustBoundaryTemplates(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.TrustBoundaryTemplates?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IDiagram> GetDiagrams(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, 
            string filter, bool showOnlyOpenQuestions = false)
        {
            return model.Diagrams?
                .Where(x => x.HasNotesWithText(schemaManager, filter) && (showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType)))
                .OrderBy(x => x.Name);
        }
    }
}
