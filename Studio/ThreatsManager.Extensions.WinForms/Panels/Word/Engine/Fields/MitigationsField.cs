using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal abstract class MitigationsField : Field
    {
        protected virtual MitigationStatus Status => MitigationStatus.Undefined;

        public override bool SecondPass => true;

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            var mitigations = GetMitigations(identity);

            if (mitigations?.Any() ?? false)
            {
                var sorted = mitigations.OrderBy(x => x.ThreatEvent.Parent.Name)
                    .ThenByDescending(x => x.Strength, new StrengthComparer()).ToArray();
                var table = cell.AddTable();
                int count = sorted.Length;
                table.ResetCells(count + 1, 4);
                table.ApplyStyleForBandedColumns = false;
                table.ApplyStyleForBandedRows = false;
                table.ApplyStyleForFirstColumn = false;
                table.ApplyStyleForHeaderRow = true;
                table.ApplyStyleForLastColumn = false;
                table.ApplyStyleForLastRow = false;
                table.TableFormat.IsAutoResized = true;

                var header = table.Rows[0];
                header.Cells[0].AddParagraph().AppendText("Object");
                header.Cells[0].Width = 150;
                header.Cells[1].AddParagraph().AppendText("Mitigation");
                header.Cells[1].Width = 200;
                header.Cells[2].AddParagraph().AppendText("Severity");
                header.Cells[2].Width = 75;
                header.Cells[3].AddParagraph().AppendText("Strength");
                header.Cells[3].Width = 75;

                for (int i = 0; i < count; i++)
                {
                    var row = table.Rows[i + 1];

                    var mitigation = sorted[i];
                    var te = mitigation.ThreatEvent;
                    var parent = te.Parent;
                    row.Cells[0].AddParagraph().AppendText($"[{te.Model.GetIdentityTypeInitial(parent)}] {parent.Name}");
                    row.Cells[0].Width = 150;
                    var bookmark = cell.Document.Bookmarks.FindByName(mitigation.Mitigation.Id.ToString("N"));
                    if (bookmark != null)
                    {
                        row.Cells[1].AddParagraph()
                            .AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText, bookmark.BookmarkStart, 
                                true, false, false, null);
                    }
                    else
                    {
                        row.Cells[1].AddParagraph().AppendText(mitigation.Mitigation.Name);
                    }
                    row.Cells[1].Width = 200;
                    row.Cells[2].AddParagraph().AppendText(mitigation.ThreatEvent.Severity.Name);
                    row.Cells[2].Width = 75;
                    row.Cells[3].AddParagraph().AppendText(mitigation.Strength.Name);
                    row.Cells[3].Width = 75;
                }
            }
            else
            {
                cell.AddParagraph().AppendText("N/A");
            }
        }

        public override bool IsVisible(IIdentity identity)
        {
            var mitigations = GetMitigations(identity);

            return mitigations?.Any() ?? false;
        }

        private List<IThreatEventMitigation> GetMitigations(IIdentity identity)
        {
            List<IThreatEventMitigation> mitigations = new List<IThreatEventMitigation>();
            if (identity is IThreatType threatType)
            {
                var threatEvents = GetAssociatedThreatEvents(threatType);

                var list = threatEvents?.ToArray();
                if (list?.Any() ?? false)
                {
                    foreach (var te in list)
                    {
                        var me = te.Mitigations?.Where(x => x.Status == Status).ToArray();
                        if (me?.Any() ?? false)
                            mitigations.AddRange(me);
                    }
                }
            }
            else if (identity is IThreatEvent threatEvent)
            {
                var me = threatEvent.Mitigations?.Where(x => x.Status == Status).ToArray();
                if (me?.Any() ?? false)
                    mitigations.AddRange(me);
            }

            return mitigations;
        }
    }
}