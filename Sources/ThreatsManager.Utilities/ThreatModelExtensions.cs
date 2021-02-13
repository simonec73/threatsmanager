using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Extensions to simplify development by adding behaviors to the Threat Model and its components.
    /// </summary>
    public static class ThreatModelExtensions
    {
        /// <summary>
        /// Get the maximum severity applied to the Threat Events derived from the specific Threat Type.
        /// </summary>
        /// <param name="threatType">Threat Type to be analyzed.</param>
        /// <returns>Maximum severity applied to Threat Events derived from the Threat Type.</returns>
        public static ISeverity GetTopSeverity(this IThreatType threatType)
        {
            ISeverity result = null;

            var model = threatType.Model;

            if (model != null)
            {
                var modelTe = model.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id)
                    .OrderByDescending(x => x.SeverityId).FirstOrDefault();
                if (modelTe != null)
                {
                    result = modelTe.Severity;
                }

                var entitiesTe = model.Entities?
                    .Select(e => e.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id)
                        .OrderByDescending(x => x.SeverityId).FirstOrDefault())
                    .Where(x => x != null).ToArray();
                if (entitiesTe?.Any() ?? false)
                {
                    foreach (var entityTe in entitiesTe)
                    {
                        if (result == null || entityTe.SeverityId > result.Id)
                        {
                            result = entityTe.Severity;
                        }
                    }
                }

                var flowsTe = model.DataFlows?
                    .Select(e => e.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id)
                        .OrderByDescending(x => x.SeverityId).FirstOrDefault())
                    .Where(x => x != null).ToArray();
                if (flowsTe?.Any() ?? false)
                {
                    foreach (var flowTe in flowsTe)
                    {
                        if (result == null || flowTe.SeverityId > result.Id)
                        {
                            result = flowTe.Severity;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the maximum severity applied to the Vulnerabilities derived from the specific Weakness.
        /// </summary>
        /// <param name="weakness">Weakness to be analyzed.</param>
        /// <returns>Maximum severity applied to Vulnerabilities derived from the Weakness.</returns>
        public static ISeverity GetTopSeverity(this IWeakness weakness)
        {
            ISeverity result = null;

            var model = weakness.Model;

            if (model != null)
            {
                var modelV = model.Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id)
                    .OrderByDescending(x => x.SeverityId).FirstOrDefault();
                if (modelV != null)
                {
                    result = modelV.Severity;
                }

                var entitiesV = model.Entities?
                    .Select(e => e.Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id)
                        .OrderByDescending(x => x.SeverityId).FirstOrDefault())
                    .Where(x => x != null).ToArray();
                if (entitiesV?.Any() ?? false)
                {
                    foreach (var entityV in entitiesV)
                    {
                        if (result == null || entityV.SeverityId > result.Id)
                        {
                            result = entityV.Severity;
                        }
                    }
                }

                var flowsV = model.DataFlows?
                    .Select(e => e.Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id)
                        .OrderByDescending(x => x.SeverityId).FirstOrDefault())
                    .Where(x => x != null).ToArray();
                if (flowsV?.Any() ?? false)
                {
                    foreach (var flowV in flowsV)
                    {
                        if (result == null || flowV.SeverityId > result.Id)
                        {
                            result = flowV.Severity;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Verifies if the Identity should be picked, based on the filter passed as argument.
        /// </summary>
        /// <param name="identity">Identity to be analyzed.</param>
        /// <param name="filter">Filter to be applied.</param>
        /// <returns>True if any string in the filter is present in any text field of the Identity.</returns>
        /// <remarks>It analyzes the Name, the Description and eventual Text properties.
        /// <para>The search is case-insensitive.</para></remarks>
        public static bool Filter(this IIdentity identity, [Required] string filter)
        {
            var result = (!string.IsNullOrWhiteSpace(identity.Name) &&
                          identity.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                         (!string.IsNullOrWhiteSpace(identity.Description) &&
                          identity.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!result && identity is IPropertiesContainer container)
            {
                var properties = container.Properties?.ToArray();
                if (properties?.Any() ?? false)
                {
                    foreach (var property in properties)
                    {
                        var stringValue = property.StringValue;
                        if ((!string.IsNullOrWhiteSpace(stringValue) &&
                             stringValue.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Verifies if the object should be picked, based on the filter passed as argument.
        /// </summary>
        /// <param name="container">Object to be analyzed.</param>
        /// <param name="description">Optional description to be searched.</param>
        /// <param name="filter">Filter to be applied.</param>
        /// <returns>True if any string in the filter is present in any text field of the object.</returns>
        /// <remarks>It analyzes the Name, the Description and eventual Text properties.
        /// <para>The search is case-insensitive.</para></remarks>
        public static bool Filter(this IPropertiesContainer container, string description, [Required] string filter)
        {
            var name = container.ToString();
            var result = (!string.IsNullOrWhiteSpace(name) &&
                          name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                         (!string.IsNullOrWhiteSpace(description) &&
                          description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!result)
            {
                var properties = container.Properties?.ToArray();
                if (properties?.Any() ?? false)
                {
                    foreach (var property in properties)
                    {
                        var stringValue = property.StringValue;
                        if ((!string.IsNullOrWhiteSpace(stringValue) &&
                             stringValue.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the type of the Entity passed as argument.
        /// </summary>
        /// <param name="entity">Entity for which the Type should be retrieved.</param>
        /// <returns>Entity Type.</returns>
        public static EntityType GetEntityType([NotNull] this IEntity entity)
        {
            EntityType result;

            if (entity is IExternalInteractor)
                result = EntityType.ExternalInteractor;
            else if (entity is IProcess)
                result = EntityType.Process;
            else if (entity is IDataStore)
                result = EntityType.DataStore;
            else
                throw new ArgumentException("Not a known Entity.");

            return result;
        }

        /// <summary>
        /// Get the associations between Threat Events and the Mitigation passed as argument. 
        /// </summary>
        /// <param name="mitigation">Mitigation to be analyzed.</param>
        /// <returns>Enumeration of all the relationships.</returns>
        public static IEnumerable<IThreatEventMitigation> GetThreatEventMitigations(this IMitigation mitigation)
        {
            IEnumerable<IThreatEventMitigation> result = null;

            var model = mitigation?.Model;
            if (model != null)
            {
                List<IThreatEventMitigation> mitigations = new List<IThreatEventMitigation>();

                GetThreatEventMitigations(mitigation, model, mitigations);
                GetThreatEventMitigations(mitigation, model.Entities, mitigations);
                GetThreatEventMitigations(mitigation, model.DataFlows, mitigations);

                result = mitigations;
            }

            return result;
        }

        #region Auxiliary functions.
        private static void GetThreatEventMitigations([NotNull] IMitigation mitigation, IEnumerable<IThreatEventsContainer> containers,
            [NotNull] List<IThreatEventMitigation> list)
        {
            var tecs = containers?.ToArray();
            if (tecs?.Any() ?? false)
            {
                foreach (var tec in tecs)
                    GetThreatEventMitigations(mitigation, tec, list);
            }
        }

        private static void GetThreatEventMitigations([NotNull] IMitigation mitigation, [NotNull] IThreatEventsContainer container,
            [NotNull] List<IThreatEventMitigation> list)
        {
            var tes = container.ThreatEvents?.ToArray();
            if (tes?.Any() ?? false)
            {
                foreach (var te in tes)
                {
                    var m = te.Mitigations?.FirstOrDefault(x => x.MitigationId == mitigation.Id);
                    if (m != null)
                        list.Add(m);
                }
            }
        }
        #endregion
    }
}
