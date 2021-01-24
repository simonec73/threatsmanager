using System.Linq;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class AffectedThreatsField : Field
    {
        public override bool SecondPass => true;

        public override string Tooltip => "Threats affected by the Mitigation.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IMitigation mitigation)
            {
                var tems = GetAssociatedThreatEvents(mitigation)?.ToArray();

                if (tems?.Any() ?? false)
                {
                    var sorted = tems.OrderBy(x => x.ThreatEvent.Parent.Name).ToArray();
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
                    header.Cells[1].AddParagraph().AppendText("Threat");
                    header.Cells[1].Width = 150;
                    header.Cells[2].AddParagraph().AppendText("Strength");
                    header.Cells[2].Width = 100;
                    header.Cells[3].AddParagraph().AppendText("Status");
                    header.Cells[3].Width = 100;

                    for (int i = 0; i < count; i++)
                    {
                        var row = table.Rows[i + 1];

                        var tem = sorted[i];
                        var parent = tem.ThreatEvent.Parent;
                        row.Cells[0].AddParagraph()
                            .AppendText($"[{mitigation.Model.GetIdentityTypeInitial(parent)}] {parent.Name}");
                        row.Cells[0].Width = 150;
                        var bookmarkTe = cell.Document.Bookmarks.FindByName(tem.ThreatEvent.Id.ToString("N"));
                        if (bookmarkTe != null)
                        {
                            row.Cells[1].AddParagraph()
                                .AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText, bookmarkTe.BookmarkStart,
                                    true, false, false, null);
                        }
                        else
                        {
                            var bookmarkTt = cell.Document.Bookmarks.FindByName(tem.ThreatEvent.ThreatType.Id.ToString("N"));
                            if (bookmarkTt != null)
                            {
                                row.Cells[1].AddParagraph()
                                    .AppendCrossReference(ReferenceType.Bookmark, ReferenceKind.ContentText, bookmarkTt.BookmarkStart,
                                        true, false, false, null);
                            }
                            else
                            {
                                row.Cells[1].AddParagraph().AppendText(tem.ThreatEvent.Name);
                            }
                        }
                    
                        row.Cells[1].Width = 150;
                        row.Cells[2].AddParagraph().AppendText(tem.Strength?.Name ?? "N/A");
                        row.Cells[2].Width = 100;
                        row.Cells[3].AddParagraph().AppendText(tem.Status.GetEnumLabel());
                        row.Cells[3].Width = 100;
                    }
                }
                else
                {
                    cell.AddParagraph().AppendText("N/A");
                }
            }
        }

        public override string ToString()
        {
            return "Affected Threats";
        }

        public override bool IsVisible(IIdentity identity)
        {
            return identity is IMitigation mitigation && 
                   (GetAssociatedThreatEvents(mitigation)?.Any() ?? false);
        }
    }
}