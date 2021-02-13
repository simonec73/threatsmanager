using System.Linq;
using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class TablePropertyTypeField : Field
    {
        public TablePropertyTypeField([NotNull] IPropertyType propertyType)
        {
            PropertyType = propertyType;
        }

        public IPropertyType PropertyType { get; private set; }

        public override string Tooltip => PropertyType.Description;

        public override string ToString()
        {
            return $"[From Events] {PropertyType.ToString()}";
        }

        public override string Label => PropertyType.ToString();

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IThreatType threatType)
            {
                var threatEvents = GetAssociatedThreatEvents(threatType)?
                    .Where(x => x.HasProperty(PropertyType) && !string.IsNullOrWhiteSpace(x.GetProperty(PropertyType)?.StringValue))
                    .ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    var table = cell.AddTable();
                    int count = threatEvents.Length;
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
                    header.Cells[1].AddParagraph().AppendText("Value");
                    header.Cells[1].Width = 350;

                    for (int i = 0; i < count; i++)
                    {
                        var row = table.Rows[i + 1];

                        var te = threatEvents[i];
                        var parent = te.Parent;
                        row.Cells[0].AddParagraph().AppendText($"[{te.Model.GetIdentityTypeInitial(parent)}] {parent.Name}");
                        row.Cells[0].Width = 150;
                        var property = te.GetProperty(PropertyType);
                        row.Cells[1].AddParagraph().AppendText(property?.StringValue ?? "N/A");
                        row.Cells[1].Width = 350;
                    }
                }
                else
                {
                    cell.AddParagraph().AppendText("N/A");
                }
            }
        }

        public override bool IsVisible(IIdentity identity)
        {
            return (identity is IThreatType threatType) && (GetAssociatedThreatEvents(threatType)?
                .Where(x => x.HasProperty(PropertyType) && !string.IsNullOrWhiteSpace(x.GetProperty(PropertyType)?.StringValue))
                .Any() ?? false);
        }
    }
}