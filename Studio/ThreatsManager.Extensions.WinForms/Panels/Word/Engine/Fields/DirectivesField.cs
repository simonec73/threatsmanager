using System.Linq;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class DirectivesField : Field
    {
        public override bool SecondPass => true;

        public override string Tooltip => "Mitigation Directives.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IMitigation mitigation)
            {
                var tems = GetAssociatedThreatEvents(mitigation)?.Where(x => !string.IsNullOrWhiteSpace(x.Directives)).ToArray();

                if (tems?.Any() ?? false)
                {
                    var sorted = tems.OrderByDescending(x => x.Strength, new StrengthComparer()).ToArray();
                    var table = cell.AddTable();
                    int count = sorted.Length;
                    table.ResetCells(count + 1, 2);
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
                    header.Cells[1].AddParagraph().AppendText("Directives");
                    header.Cells[1].Width = 350;

                    for (int i = 0; i < count; i++)
                    {
                        var row = table.Rows[i + 1];

                        var tem = sorted[i];
                        var parent = tem.ThreatEvent.Parent;
                        row.Cells[0].AddParagraph()
                            .AppendText($"[{mitigation.Model.GetIdentityTypeInitial(parent)}] {parent.Name}");
                        row.Cells[0].Width = 150;
                        row.Cells[1].Width = 350;
                        row.Cells[1].AddParagraph().AppendText(tem.Directives);
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
            return "Directives";
        }

        public override bool IsVisible(IIdentity identity)
        {
            return identity is IMitigation mitigation && 
                   (GetAssociatedThreatEvents(mitigation)?.Where(x => !string.IsNullOrWhiteSpace(x.Directives)).Any() ?? false);
        }
    }
}