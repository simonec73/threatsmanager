using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.Panels
{
    static class AnnotationsHelper
    {
        public static IEnumerable<IExternalInteractor> GetExternalInteractors(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.Entities?.OfType<IExternalInteractor>()
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IProcess> GetProcesses(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.Entities?.OfType<IProcess>()
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IDataStore> GetDataStores(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.Entities?.OfType<IDataStore>()
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IDataFlow> GetFlows(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.DataFlows?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<ITrustBoundary> GetTrustBoundaries(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.Groups?.OfType<ITrustBoundary>()
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IThreatEvent> GetThreatEvents(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.GetThreatEvents()?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IThreatEventMitigation> GetThreatEventMitigations(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.GetThreatEventMitigations()?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Mitigation.Name)
                .ThenBy(x => x.ThreatEvent.Name)
                .ThenBy(x => x.ThreatEvent.Parent.Name);
        }

        public static IEnumerable<IThreatType> GetThreatTypes(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.ThreatTypes?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IMitigation> GetKnownMitigations(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.Mitigations?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IThreatTypeMitigation> GetStandardMitigations(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.GetThreatTypeMitigations()?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Mitigation.Name)
                .ThenBy(x => x.ThreatType.Name);
        }

        public static IEnumerable<IEntityTemplate> GetEntityTemplates(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.EntityTemplates?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<IFlowTemplate> GetFlowTemplates(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.FlowTemplates?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

        public static IEnumerable<ITrustBoundaryTemplate> GetTrustBoundaryTemplates(this IThreatModel model, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager, [NotNull] IPropertyType propertyType, bool showOnlyOpenQuestions = false)
        {
            return model.TrustBoundaryTemplates?
                .Where(x => showOnlyOpenQuestions ? schemaManager.HasOpenTopics(x) : x.HasProperty(propertyType))
                .OrderBy(x => x.Name);
        }

    }
}
