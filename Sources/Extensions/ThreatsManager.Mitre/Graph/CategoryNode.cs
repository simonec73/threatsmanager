using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Mitre.Cwe;
using ThreatsManager.Mitre.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CategoryNode : Node
    {
        internal CategoryNode([NotNull] MitreGraph graph, [NotNull] Cwe.CategoryType category) : base(graph, "CWE", category.ID)
        {
            if (category.Status == StatusEnumeration.Deprecated || category.Status == StatusEnumeration.Obsolete)
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
                    var rel = category.Relationships.ItemsElementName[i] == ItemsChoiceType1.Has_Member
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

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; private set; }

        [JsonProperty("taxonomyMappings")]
        public List<TaxonomyMapping> TaxonomyMappings { get; private set; }

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
    }
}
