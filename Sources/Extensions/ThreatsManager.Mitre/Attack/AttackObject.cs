using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Attack
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AttackObject
    {
        [JsonProperty("created")]
        public DateTime Created;

        [JsonProperty("modified")]
        public DateTime Modified;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("created_by_ref")]
        public string CreatedByRef;

        [JsonProperty("kill_chain_phases")]
        public IEnumerable<KillChainPhase> KillChainPhases;

        [JsonProperty("external_references")]
        public IEnumerable<ExternalReference> ExternalReferences;

        [JsonProperty("x_mitre_platforms")]
        public IEnumerable<string> Platforms;

        [JsonProperty("x_mitre_is_subtechnique")]
        public bool IsSubtechnique;

        [JsonProperty("x_mitre_version")]
        public string Version;

        [JsonProperty("x_mitre_permissions_required")]
        public IEnumerable<string> PermissionsRequired;

        [JsonProperty("x_mitre_detection")]
        public string Detection;

        [JsonProperty("x_mitre_deprecated")]
        public bool Deprecated;

        [JsonProperty("revoked")]
        public bool Revoked;

        [JsonProperty("source_ref")]
        public string Source;

        [JsonProperty("target_ref")]
        public string Target;

        [JsonProperty("relationship_type")]
        public string Relationship;

        public string AttackId => ExternalReferences?.FirstOrDefault(x => string.CompareOrdinal(x.Source, "mitre-attack") == 0)?.ExternalId ?? Id;

        public string URL => ExternalReferences?.FirstOrDefault(x => string.CompareOrdinal(x.Source, "mitre-attack") == 0)?.Url;
    }
}