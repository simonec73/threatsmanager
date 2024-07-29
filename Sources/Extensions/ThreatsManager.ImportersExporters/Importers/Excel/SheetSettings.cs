using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class SheetSettings
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("objectType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// The hit policy defines how the engine must behave if an object already exists.
        /// </summary>
        [JsonProperty("hitPolicy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HitPolicy HitPolicy { get; set; }

        /// <summary>
        /// Remove all existing objects with the specified ObjectType, before importing.
        /// </summary>
        [JsonProperty("forceClear")]
        public bool ForceClear { get; set; }

        /// <summary>
        /// Index of the first row to import.
        /// </summary>
        [JsonProperty("firstRow")]
        public int FirstRow { get; set; }

        /// <summary>
        /// Name of the Schema containing information about the Level.
        /// </summary>
        /// <remarks>It is used only if the Object Type is SpecializedMitigation
        /// because it allows to identify the correct parent mitigation.</remarks>
        [JsonProperty("levelSchemaName")]
        public string LevelSchemaName { get; set; }

        /// <summary>
        /// Namespace of the Schema containing information about the Level.
        /// </summary>
        /// <remarks>It is used only if the Object Type is SpecializedMitigation
        /// because it allows to identify the correct parent mitigation.</remarks>
        [JsonProperty("levelSchemaNs")]
        public string LevelSchemaNamespace { get; set; }

        /// <summary>
        /// Name of the Property containing information about the Level.
        /// </summary>
        /// <remarks>It is used only if the Object Type is SpecializedMitigation
        /// because it allows to identify the correct parent mitigation.</remarks>
        [JsonProperty("levelPropertyName")]
        public string LevelPropertyName { get; set; }

        /// <summary>
        /// Name of the Target Item Template associated with the object.
        /// </summary>
        /// <remarks>If not defined, no Item Template will be created.</remarks>
        [JsonProperty("targetItem")]
        public string TargetItem { get; set; }

        /// <summary>
        /// List of the columns to be imported.
        /// </summary>
        [JsonProperty("columns")]
        public List<ColumnSettings> Columns { get; set; }

        /// <summary>
        /// If true, then the default behavior is to exclude the row, otherwise it is to include it.
        /// Filters will determine if it must be imported or not.
        /// </summary>
        [JsonProperty("defaultExclude")]
        public bool DefaultExclude { get; set; }

        [JsonProperty("filters")]
        public List<FilterSettings> Filters { get; set; }

        /// <summary>
        /// Import calculate settings.
        /// </summary>
        [JsonProperty("calculate")]
        public List<CalculateSettings> Calculate { get; set; }

        /// <summary>
        /// Control Type Associations.
        /// </summary>
        [JsonProperty("controlTypes")]
        public ControlTypeAssociationsDefinition ControlTypeAssociations { get; set; } 

        /// <summary>
        /// Mappings used to link threat types to mitigations.
        /// </summary>
        [JsonProperty("threatTypeMitigations")]
        public ThreatTypeMitigationMappingsDefinition ThreatTypeMitigationMappings { get; set; }

        /// <summary>
        /// List of mitigations to be removed.
        /// </summary>
        [JsonProperty("removedMitigations")]
        public ItemsToBeRemovedDefinition MitigationsToBeRemoved { get; set; }
    }
}