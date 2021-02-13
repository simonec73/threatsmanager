using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class AssociatedObjectsField : Field
    {
        public override string ToString()
        {
            return "Affected Objects";
        }

        public override string Tooltip => "Entities and Flows affected by the Threat.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            List<string> objects = new List<string>();

            if (identity is IThreatType threatType)
            {
                var threatEvents = GetAssociatedThreatEvents(threatType);

                var list = threatEvents?.ToArray();
                if (list?.Any() ?? false)
                {
                    var model = threatType.Model;
                    foreach (var te in list)
                    {
                        var ao = te.Parent;
                        if (ao != null)
                            objects.Add($"[{model.GetIdentityTypeInitial(ao)}] {ao.Name} ({te.Severity.Name})");
                    }
                }
            }

            foreach (var line in objects)
            {                    
                var paragraph = cell.AddParagraph();
                paragraph.ListFormat.ApplyDefBulletStyle();
                paragraph.AppendText(line);
            }
        }
    }
}