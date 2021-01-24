using System.Drawing;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class SeverityField : Field
    {
        public override string ToString()
        {
            return "Severity";
        }

        public override string Tooltip => "Severity of the Threat.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IThreatType threatType)
            {
                var severity = GetMaximumSeverity(threatType);
                if (severity != null)
                    AddSeverity(cell, severity);
                else
                    cell.AddParagraph().AppendText("N/A");
            }
            else if (identity is IThreatEvent threatEvent)
                AddSeverity(cell, threatEvent.Severity);
            else
                cell.AddParagraph().AppendText("<UNSUPPORTED>");
        }

        private void AddSeverity([NotNull] WTableCell cell, [NotNull] ISeverity severity)
        {
            AddSeverity(cell, severity.Name, severity.TextColor, severity.BackColor);
        }

        private void AddSeverity([NotNull] WTableCell cell, [Required] string severityName,
            KnownColor textColor, KnownColor backColor)
        {
            var table = cell.AddTable();
            table.ResetCells(1,1);
            table.ApplyStyleForBandedColumns = false;
            table.ApplyStyleForBandedRows = false;
            table.ApplyStyleForFirstColumn = false;
            table.ApplyStyleForHeaderRow = false;
            table.ApplyStyleForLastColumn = false;
            table.ApplyStyleForLastRow = false;
            table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;
            table.TableFormat.IsAutoResized = true;
            table[0, 0].CellFormat.BackColor = Color.FromKnownColor(backColor);
            table[0, 0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            table[0, 0].Width = 75;
            var paragraph = table[0, 0].AddParagraph();
            paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
            var range = paragraph.AppendText(severityName);
            range.CharacterFormat.TextColor = Color.FromKnownColor(textColor);
            range.CharacterFormat.Bold = true;
        }
    }
}