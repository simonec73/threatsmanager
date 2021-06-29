using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CategoryNode : Node
    {
        public CategoryNode()
        {

        }

        internal CategoryNode([NotNull] MitreGraph graph, [NotNull] Cwe.CategoryType category) : base(graph, "CWE", category.ID)
        {
            if (category.Status == Cwe.StatusEnumeration.Deprecated || category.Status == Cwe.StatusEnumeration.Obsolete)
                throw new ArgumentException(Resources.InvalidStatus, "category");

            Name = category.Name;
            Description = category.Summary.ConvertToString();
            if (Enum.TryParse<Status>(category.Status.ToString(), out var status))
                Status = status;

            #region Add relationships.
            var count = category.Relationships?.Items?.Length ?? 0;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var rel = category.Relationships.ItemsElementName[i] == Cwe.ItemsChoiceType1.Has_Member
                        ? RelationshipType.ParentOf
                        : RelationshipType.ChildOf;
                     AddRelationship(rel, "CWE", category.Relationships.Items[i].CWE_ID, category.Relationships.Items[i].View_ID);
                }
            }
            #endregion

            #region Add Taxonomy Mappings.
            var taxonomyMappings = category.Taxonomy_Mappings?.ToArray();
            if (taxonomyMappings?.Any() ?? false)
            {
                foreach (var taxonomyMapping in taxonomyMappings)
                {
                    AddTaxonomyMapping(taxonomyMapping);
                }
            }
            #endregion
        }

        internal CategoryNode([NotNull] MitreGraph graph, [NotNull] Capec.CategoryType category) : base(graph, "CAPEC", category.ID)
        {
            if (category.Status == Capec.StatusEnumeration.Deprecated || category.Status == Capec.StatusEnumeration.Obsolete)
                throw new ArgumentException(Resources.InvalidStatus, "category");

            Name = category.Name;
            Description = category.Summary.ConvertToString();
            if (Enum.TryParse<Status>(category.Status.ToString(), out var status))
                Status = status;

            #region Add relationships.
            var parents = category.Relationships?.Member_Of?.ToArray();
            if (parents?.Any() ?? false)
            {
                foreach (var parent in parents)
                {
                    AddRelationship(RelationshipType.ChildOf, "CAPEC", parent.CAPEC_ID);
                }
            }

            var children = category.Relationships?.Has_Member?.ToArray();
            if (children?.Any() ?? false)
            {
                foreach (var child in children)
                {
                    AddRelationship(RelationshipType.ParentOf, "CAPEC", child.CAPEC_ID);
                }
            }
            #endregion

            #region Add Taxonomy Mappings.
            var taxonomyMappings = category.Taxonomy_Mappings?.ToArray();
            if (taxonomyMappings?.Any() ?? false)
            {
                foreach (var taxonomyMapping in taxonomyMappings)
                {
                    AddTaxonomyMapping(taxonomyMapping);
                }
            }
            #endregion
        }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; private set; }

        [JsonProperty("taxonomyMappings")]
        public List<TaxonomyMapping> TaxonomyMappings { get; private set; }

        private void AddTaxonomyMapping(Cwe.TaxonomyMappingsTypeTaxonomy_Mapping taxonomyMapping)
        {
            if (taxonomyMapping != null)
            {
                if (TaxonomyMappings == null)
                    TaxonomyMappings = new List<TaxonomyMapping>();

                TaxonomyMappings.Add(new TaxonomyMapping(taxonomyMapping.Taxonomy_Name.GetXmlEnumLabel(),
                    taxonomyMapping.Entry_ID, taxonomyMapping.Entry_Name, taxonomyMapping.Mapping_Fit == Cwe.TaxonomyMappingFitEnumeration.Exact));
            }
        }

        private void AddTaxonomyMapping(Capec.TaxonomyMappingsTypeTaxonomy_Mapping taxonomyMapping)
        {
            if (taxonomyMapping != null)
            {
                if (TaxonomyMappings == null)
                    TaxonomyMappings = new List<TaxonomyMapping>();

                TaxonomyMappings.Add(new TaxonomyMapping(taxonomyMapping.Taxonomy_Name.GetXmlEnumLabel(),
                    taxonomyMapping.Entry_ID, taxonomyMapping.Entry_Name, taxonomyMapping.Mapping_Fit == Capec.TaxonomyMappingFitEnumeration.Exact));
            }
        }
    }
}
