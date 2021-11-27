using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Threat Model Manager.
    /// </summary>
    /// <remarks>Utility class to manage serialization and deserialization of Threat Models, instances and model locations.</remarks>
    public static class ThreatModelManager
    {
        private static readonly List<IThreatModel> _instances = new List<IThreatModel>();

        private static readonly Dictionary<Guid, string> _locations = new Dictionary<Guid, string>();

        /// <summary>
        /// Standard color used for everything is "good", like Mitigations.
        /// </summary>
        public static Color StandardColor = Color.FromArgb(0x01, 0x73, 0xC7);

        /// <summary>
        /// Color used for everything is "bad", like Threats and Threat Actors.
        /// </summary>
        public static Color ThreatsColor = Color.FromArgb(0xE5, 0x39, 0x35);

        /// <summary>
        /// Creates a new Default Instance of the Threat Model.
        /// </summary>
        /// <returns>Threat Model created.</returns>
        public static IThreatModel GetDefaultInstance()
        {
            IThreatModel result = null;

            var type = Type.GetType("ThreatsManager.Engine.Manager, ThreatsManager.Engine", false);
            var property = type?.GetProperty("Instance");
            if (property != null)
            {
                var instance = property.GetValue(null);
                result = type.InvokeMember("CreateNewThreatModel", BindingFlags.InvokeMethod, null, instance,
                    new object[] { "Default Threat Model" }) as IThreatModel;

                if (result != null)
                {
                    _instances.Add(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the Threat Model whose ID is passed as argument from the list of known Threat Models.
        /// </summary>
        /// <param name="id">Identifier of the Threat Model.</param>
        /// <returns>Instance of the Threat Model, having the specified ID.
        /// If a Threat Model with the searched ID does not exist, the method returns <code>null</code>.</returns>
        public static IThreatModel Get(Guid id)
        {
            return _instances.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Removes a Threat Model from the list of known Threat Models.
        /// </summary>
        /// <param name="id">Identifier of the Threat Model.</param>
        /// <returns>True if the Threat Model has been found and removed, otherwise false.</returns>
        public static bool Remove(Guid id)
        {
            bool result = false;

            var model = Get(id);

            if (model != null)
            {
                (model as IDisposable)?.Dispose();
                result = _instances.Remove(model);
            }

            return result;
        }
        
        /// <summary>
        /// Deserialize a Threat Model and adds it to the list of known Threat Models.
        /// </summary>
        /// <param name="json">Serialized Json of the Threat Model, as byte array.</param>
        /// <param name="ignoreMissingMembers">Optional flag to specify to ignore information that is unknown.</param>
        /// <param name="newThreatModelId">Optional identifier to be used for the Threat Model replacing its configured one.</param>
        /// <returns>The deserialized Threat Model.</returns>
        public static IThreatModel Deserialize([NotNull] byte[] json, 
            bool ignoreMissingMembers = false, Guid? newThreatModelId = null)//, bool addToKnownInstances = true)
        {
            IThreatModel result = null;

            if (json.Length > 0)
            {
                string jsonText = null;

                if (json[0] == 0xFF)
                    jsonText = Encoding.Unicode.GetString(json, 2, json.Length - 2);
                else
                    jsonText = Encoding.Unicode.GetString(json);

                if (newThreatModelId.HasValue && newThreatModelId != Guid.Empty)
                {
                    var parsed = JObject.Parse(jsonText);
                    var id = parsed.GetValue("id")?.ToObject<string>();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        jsonText = jsonText.Replace(id, newThreatModelId?.ToString("D"));
                    }
                }

                var binder = new KnownTypesBinder();

                using (var textReader = new StringReader(jsonText))
                using (var reader = new JsonTextReader(textReader))
                {
                    var serializer = new JsonSerializer
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        SerializationBinder = binder,
                        MaxDepth = 128,
                        MissingMemberHandling = ignoreMissingMembers
                            ? MissingMemberHandling.Ignore
                            : MissingMemberHandling.Error
                    };
                    result = serializer.Deserialize<IThreatModel>(reader);
                }

                if (result != null)
                {
                    try
                    {
                        if (!binder.HasUnknownTypes)
                            result.ResetDirty();

                        result.SuspendDirty();

                        if (_instances.Any(x => x.Id == result.Id))
                        {
                            throw new ExistingModelException(result);
                        }
                        else
                        {
                            result.Cleanup();
                            result.PropertySchemasNormalization();

                            _instances.Add(result);

                            var method = result.GetType()
                                .GetMethod("RegisterEvents", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[] { }, null);
                            if (method != null)
                                method.Invoke(result, null);

                            var processors = ExtensionUtils.GetExtensions<IPostLoadProcessor>()?.ToArray();
                            if (processors?.Any() ?? false)
                            {
                                foreach (var processor in processors)
                                {
                                    processor.Process(result);
                                }
                            }
                        }
                    }
                    finally
                    {
                        result.ResumeDirty();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Serializes a Threat Model as Json.
        /// </summary>
        /// <param name="model">Threat Model to be serialized.</param>
        /// <returns>Serialized Json of the Threat Model, as byte array.</returns>
        public static byte[] Serialize([NotNull] IThreatModel model)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.All, MaxDepth = 128, Formatting = Formatting.Indented};
                serializer.Serialize(writer, model);
            }

            var buf = Encoding.Unicode.GetBytes(sb.ToString());

            var result = new byte[buf.Length + 2];
            result[0] = 0xFF;
            result[1] = 0xFE;
            buf.CopyTo(result, 2);

            return result;
        }

        /// <summary>
        /// Get the location of a known Threat Model.
        /// </summary>
        /// <param name="model">Threat Model for which to get the location.</param>
        /// <returns>Location of the Threat Model.</returns>
        public static string GetLocation(this IThreatModel model)
        {
            if (model == null || !_locations.TryGetValue(model.Id, out var result))
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Sets the location of a known Threat Model.
        /// </summary>
        /// <param name="model">Threat Model for which to set the location.</param>
        /// <param name="location">Location of the Threat Model.</param>
        /// <remarks>If <paramref name="location"/> is <code>null</code>,
        /// then the stored location for the Threat Model is forgot.</remarks>
        public static void SetLocation(this IThreatModel model, string location)
        {
            if (model != null)
            {
                if (_locations.ContainsKey(model.Id))
                {
                    if (location == null)
                        _locations.Remove(model.Id);
                    else
                        _locations[model.Id] = location;
                }
                else
                {
                    if (location != null)
                        _locations[model.Id] = location;
                }
            }
        }

        #region Post-deserialize normalization activities.

        private static void PropertySchemasNormalization(this IThreatModel model)
        {
            var propertySchemas = model.Schemas?.ToArray();
            if (propertySchemas?.Any() ?? false)
            {
                foreach (var schema in propertySchemas)
                {
                    schema.SetModelId(model.Id);

                    var propertyTypes = schema.PropertyTypes?.ToArray();
                    if (propertyTypes?.Any() ?? false)
                    {
                        foreach (var propertyType in propertyTypes)
                            propertyType.SetModelId(model.Id);
                    }
                }
            }
        }

        private static void SetModelId(this object item, Guid modelId)
        {
            PropertyInfo property = item.GetType().GetProperty("_modelId", BindingFlags.NonPublic | BindingFlags.Instance);
            property?.GetSetMethod(true).Invoke(item, new object[] { modelId });
        }

        private static void Cleanup(this IThreatModel model)
        {
            model.CleanProperties(model);
            model.CleanThreatEvents(model);

            var dataFlows = model.DataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var flow in dataFlows)
                {
                    flow.CleanProperties(model);
                    flow.CleanThreatEvents(model);
                }
            }

            var diagrams = model.Diagrams?.ToArray();
            if (diagrams?.Any() ?? false)
            {
                foreach (var diagram in diagrams)
                {
                    diagram.CleanProperties(model);

                    var entityShapes = diagram.Entities?.ToArray();
                    if (entityShapes?.Any() ?? false)
                    {
                        foreach (var entityShape in entityShapes)
                            entityShape.CleanProperties(model);
                    }

                    var groupShapes = diagram.Groups?.ToArray();
                    if (groupShapes?.Any() ?? false)
                    {
                        foreach (var groupShape in groupShapes)
                            groupShape.CleanProperties(model);
                    }
                }
            }

            var entities = model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    entity.CleanProperties(model);
                    entity.CleanThreatEvents(model);
                }
            }

            var entityTemplates = model.EntityTemplates?.ToArray();
            if (entityTemplates?.Any() ?? false)
            {
                foreach (var entityTemplate in entityTemplates)
                {
                    entityTemplate.CleanProperties(model);
                }
            }

            var flowTemplates = model.FlowTemplates?.ToArray();
            if (flowTemplates?.Any() ?? false)
            {
                foreach (var flowTemplate in flowTemplates)
                {
                    flowTemplate.CleanProperties(model);
                }
            }

            var trustBoundaryTemplates = model.TrustBoundaryTemplates?.ToArray();
            if (trustBoundaryTemplates?.Any() ?? false)
            {
                foreach (var trustBoundaryTemplate in trustBoundaryTemplates)
                {
                    trustBoundaryTemplate.CleanProperties(model);
                }
            }

            var groups = model.Groups?.ToArray();
            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    group.CleanProperties(model);
                }
            }

            var mitigations = model.Mitigations?.ToArray();
            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                {
                    mitigation.CleanProperties(model);
                }
            }

            var severities = model.Severities?.ToArray();
            if (severities?.Any() ?? false)
            {
                foreach (var severity in severities)
                    severity.CleanProperties(model);
            }

            var strengths = model.Strengths?.ToArray();
            if (strengths?.Any() ?? false)
            {
                foreach (var strength in strengths)
                    strength.CleanProperties(model);
            }

            var actors = model.ThreatActors?.ToArray();
            if (actors?.Any() ?? false)
            {
                foreach (var actor in actors)
                {
                    actor.CleanProperties(model);
                }
            }

            var threatEvents = model.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    threatEvent.CleanProperties(model);

                    var teMitigations = threatEvent.Mitigations?.ToArray();
                    if (teMitigations?.Any() ?? false)
                    {
                        foreach (var teMitigation in teMitigations)
                        {
                            teMitigation.CleanProperties(model);
                        }
                    }

                    var teScenarios = threatEvent.Scenarios?.ToArray();
                    if (teScenarios?.Any() ?? false)
                    {
                        foreach (var teScenario in teScenarios)
                        {
                            teScenario.CleanProperties(model);
                        }
                    }
                }
            }

            var threatTypes = model.ThreatTypes?.ToArray();
            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    threatType.CleanProperties(model);
                }
            }
        }

        private static void CleanProperties(this IPropertiesContainer container, [NotNull] IThreatModel model)
        {
            var properties = container?.Properties?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    var propertyType = model.GetPropertyType(property.PropertyTypeId);
                    if (propertyType == null)
                        container.RemoveProperty(property.PropertyTypeId);
                }
            }
        }

        private static void CleanThreatEvents(this IThreatEventsContainer container, [NotNull] IThreatModel model)
        {
            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                    threatEvent.CleanProperties(model);
            }
        }
        #endregion
    }
}
