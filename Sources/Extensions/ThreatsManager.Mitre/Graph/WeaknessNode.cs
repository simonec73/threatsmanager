using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Cwe;
using ThreatsManager.Utilities;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class WeaknessNode : Node
    {
        public WeaknessNode()
        {

        }

        internal WeaknessNode([NotNull] MitreGraph graph, [NotNull] WeaknessType weakness) : base(graph, "CWE", weakness.ID)
        {
            if (weakness.Status == StatusEnumeration.Deprecated || weakness.Status == StatusEnumeration.Obsolete)
                throw new ArgumentException(Properties.Resources.InvalidStatus, "weakness");

            Name = weakness.Name;
            Description = weakness.Description;
            ExtendedDescription = weakness.Extended_Description.ConvertToString();
            if (Enum.TryParse<Evaluation>(weakness.Likelihood_Of_Exploit.ToString(), out var likelihood))
                Likelihood = likelihood;
            else
                Likelihood = Evaluation.Unknown;

            #region Add relationships.
            var relWeaknesses = weakness.Related_Weaknesses?.ToArray();
            if (relWeaknesses?.Any() ?? false)
            {
                foreach (var w in relWeaknesses)
                {
                    if (Enum.TryParse<RelationshipType>(w.Nature.ToString(), out var relType))
                    {
                        AddRelationship(relType, "CWE", w.CWE_ID);
                    }
                }
            }

            var relExamples = weakness.Observed_Examples?.ToArray();
            if (relExamples?.Any() ?? false)
            {
                foreach (var e in relExamples)
                {
                    AddRelationship(RelationshipType.Abstracts, "CVE", e.Reference);

                    var node = graph.CreateNode(e);
                    node?.AddRelationship(RelationshipType.IsAnExampleOf, this);
                }
            }

            var relAttackPatterns = weakness.Related_Attack_Patterns?.ToArray();
            if (relAttackPatterns?.Any() ?? false)
            {
                foreach (var a in relAttackPatterns)
                {
                    AddRelationship(RelationshipType.IsLeveragedBy, "CAPEC", a.CAPEC_ID);
                }
            }
            #endregion

            #region Add Contexts.
            var architectures = weakness.Applicable_Platforms?.Architecture?.ToArray();
            if (architectures?.Any() ?? false)
            {
                foreach (var a in architectures)
                {
                    AddContext(ContextType.Architecture, a.Class.ToString(), a.Name.ToString());
                }
            }

            var languages = weakness.Applicable_Platforms?.Language?.ToArray();
            if (languages?.Any() ?? false)
            {
                foreach (var l in languages)
                {
                    AddContext(ContextType.Language, l.Class.ToString(), l.Name.ToString());
                }
            }

            var operatingSystems = weakness.Applicable_Platforms?.Operating_System?.ToArray();
            if (operatingSystems?.Any() ?? false)
            {
                foreach (var os in operatingSystems)
                {
                    AddContext(ContextType.OperatingSystem, os.Class.ToString(), os.Name.ToString());
                }
            }

            var technologies = weakness.Applicable_Platforms?.Technology?.ToArray();
            if (technologies?.Any() ?? false)
            {
                foreach (var tech in technologies)
                {
                    AddContext(ContextType.Technology, tech.Class.ToString(), tech.Name.ToString());
                }
            }
            #endregion

            #region Add Consequences.
            var consequences = weakness.Common_Consequences?.ToArray();
            if (consequences?.Any() ?? false)
            {
                foreach (var consequence in consequences)
                {
                    AddConsequence(consequence);
                }
            }
            #endregion

            #region Add Detection Methods.
            var detectionMethods = weakness.Detection_Methods?.ToArray();
            if (detectionMethods?.Any() ?? false)
            {
                foreach (var detectionMethod in detectionMethods)
                {
                    AddDetectionMethod(detectionMethod);
                }
            }
            #endregion

            #region Add Potential Mitigations.
            var potentialMitigations = weakness.Potential_Mitigations?.ToArray();
            if (potentialMitigations?.Any() ?? false)
            {
                foreach (var potentialMitigation in potentialMitigations)
                {
                    AddPotentialMitigations(potentialMitigation);
                }
            }
            #endregion

            #region Add Taxonomy Mappings.
            var taxonomyMappings = weakness.Taxonomy_Mappings?.ToArray();
            if (taxonomyMappings?.Any() ?? false)
            {
                foreach (var taxonomyMapping in taxonomyMappings)
                {
                    AddTaxonomyMapping(taxonomyMapping);
                }
            }
            #endregion
        }

        [JsonProperty("extDesc")]
        public string ExtendedDescription { get; private set; }
        
        [JsonProperty("likelihood")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Evaluation Likelihood { get; private set; }

        [JsonProperty("contexts")]
        public List<Context> Contexts { get; private set; }

        [JsonProperty("consequences")]
        public List<Consequence> Consequences { get; private set; }

        [JsonProperty("detectionMethods")]
        public List<DetectionMethod> DetectionMethods { get; private set; }

        [JsonProperty("mitigations")]
        public List<PotentialMitigation> PotentialMitigations { get; private set; }

        [JsonProperty("taxonomyMappings")]
        public List<TaxonomyMapping> TaxonomyMappings { get; private set; }

        #region Private member functions.
        private void AddContext(ContextType contextType, string className, string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(name))
            {
                if (Contexts == null)
                    Contexts = new List<Context>();

                Contexts.Add(new Context(contextType, className, name));
            }
        }

        private void AddConsequence(CommonConsequencesTypeConsequence consequence)
        {
            if (consequence != null)
            {
                IEnumerable<string> scopes = null;
                IEnumerable<string> impacts = null;
                Evaluation likelihood = Evaluation.Unknown;

                var s = consequence.Scope?.ToArray();
                if (s?.Any() ?? false)
                {
                    scopes = s.Select(x => x.GetXmlEnumLabel());
                }

                var i = consequence.Impact?.ToArray();
                if (i?.Any() ?? false)
                {
                    impacts = i.Select(x => x.GetXmlEnumLabel());
                }

                if (Enum.TryParse<Evaluation>(consequence.Likelihood.ToString(), out var l))
                {
                    likelihood = l;
                }

                var notes = consequence.Note?.ConvertToString();

                if (Consequences == null)
                    Consequences = new List<Consequence>();

                Consequences.Add(new Consequence(scopes, impacts, likelihood, notes));
            }
        }

        private void AddDetectionMethod(DetectionMethodsTypeDetection_Method detectionMethod)
        {
            if (detectionMethod != null)
            {
                if (DetectionMethods == null)
                    DetectionMethods = new List<DetectionMethod>();

                DetectionMethods.Add(new DetectionMethod(detectionMethod.Method.GetXmlEnumLabel(), 
                    detectionMethod.Description.ConvertToString(), detectionMethod.Effectiveness.GetXmlEnumLabel()));
            }
        }

        private void AddPotentialMitigations(PotentialMitigationsTypeMitigation mitigation)
        {
            if (mitigation != null)
            {
                if (PotentialMitigations == null)
                    PotentialMitigations = new List<PotentialMitigation>();

                PotentialMitigations.Add(new PotentialMitigation(mitigation.Phase.Select(x => x.ToString()),
                    mitigation.Strategy.ToString(), mitigation.Description.ConvertToString(), mitigation.Effectiveness.ToString()));
            }
        }

        private void AddTaxonomyMapping(TaxonomyMappingsTypeTaxonomy_Mapping taxonomyMapping)
        {
            if (taxonomyMapping != null)
            {
                if (TaxonomyMappings == null)
                    TaxonomyMappings = new List<TaxonomyMapping>();

                TaxonomyMappings.Add(new TaxonomyMapping(taxonomyMapping.Taxonomy_Name.GetXmlEnumLabel(),
                    taxonomyMapping.Entry_ID, taxonomyMapping.Entry_Name, taxonomyMapping.Mapping_Fit == TaxonomyMappingFitEnumeration.Exact));
            }
        }
        #endregion
    }
}