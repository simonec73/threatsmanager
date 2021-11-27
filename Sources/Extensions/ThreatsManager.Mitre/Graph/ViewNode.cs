using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ViewNode : Node
    {
        public ViewNode()
        {

        }

        internal ViewNode([NotNull] MitreGraph graph, [NotNull] Cwe.ViewType view) : base(graph, "CWE", view.ID)
        {
            if (view.Status == Cwe.StatusEnumeration.Deprecated || view.Status == Cwe.StatusEnumeration.Obsolete)
                throw new ArgumentException(Properties.Resources.InvalidStatus, "view");
            if (view.Type != Cwe.ViewTypeEnumeration.Graph)
                throw new ArgumentException(Properties.Resources.InvalidViewType, "view");

            Name = view.Name;
            Description = view.Objective.ConvertToString();
            if (Enum.TryParse<Status>(view.Status.ToString(), out var status))
                Status = status;

            #region Add relationships.
            var count = view.Members?.Items?.Length ?? 0;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var rel = view.Members.ItemsElementName[i] == Cwe.ItemsChoiceType1.Has_Member
                        ? RelationshipType.ParentOf
                        : RelationshipType.ChildOf;
                    AddRelationship(rel, "CWE", view.Members.Items[i].CWE_ID, view.Members.Items[i].View_ID);
                }
            }
            #endregion

            #region Add audience.

            var audience = view.Audience?.ToArray();
            if (audience?.Any() ?? false)
            {
                if (Audience == null)
                    Audience = new List<Audience>();

                foreach (var sh in audience)
                {
                    Audience.Add(new Audience(sh.Type.GetXmlEnumLabel(), sh.Description));
                }
            }
            #endregion
        }

        internal ViewNode([NotNull] MitreGraph graph, [NotNull] Capec.ViewType view) : base(graph, "CAPEC", view.ID)
        {
            if (view.Status == Capec.StatusEnumeration.Deprecated || view.Status == Capec.StatusEnumeration.Obsolete)
                throw new ArgumentException(Properties.Resources.InvalidStatus, "view");
            if (view.Type != Capec.ViewTypeEnumeration.Graph)
                throw new ArgumentException(Properties.Resources.InvalidViewType, "view");

            Name = view.Name;
            Description = view.Objective.ConvertToString();
            if (Enum.TryParse<Status>(view.Status.ToString(), out var status))
                Status = status;

            #region Add relationships.
            var parents = view.Members?.Member_Of?.ToArray();
            if (parents?.Any() ?? false)
            {
                foreach (var parent in parents)
                {
                    AddRelationship(RelationshipType.ChildOf, "CAPEC", parent.CAPEC_ID);
                }
            }

            var children = view.Members?.Has_Member?.ToArray();
            if (children?.Any() ?? false)
            {
                foreach (var child in children)
                {
                    AddRelationship(RelationshipType.ParentOf, "CAPEC", child.CAPEC_ID);
                }
            }
            #endregion

            #region Add audience.
            var audience = view.Audience?.ToArray();
            if (audience?.Any() ?? false)
            {
                if (Audience == null)
                    Audience = new List<Audience>();

                foreach (var sh in audience)
                {
                    Audience.Add(new Audience(sh.Type.GetXmlEnumLabel(), sh.Description.ConvertToString()));
                }
            }
            #endregion
        }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; private set; }

        [JsonProperty("audience")]
        public List<Audience> Audience { get; private set; }
    }
}
