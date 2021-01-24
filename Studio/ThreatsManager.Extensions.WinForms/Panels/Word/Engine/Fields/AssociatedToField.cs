using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class AssociatedToField : Field
    {
        public override string ToString()
        {
            return "Associated To";
        }

        public override string Tooltip => "Associated entity.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IThreatEvent threatEvent)
            {
                var typeName = threatEvent.Model.GetIdentityTypeInitial(threatEvent.Parent);
                cell.AddParagraph().AppendText($"[{typeName}] {threatEvent.Parent.Name}");
            }
            else
            {
                cell.AddParagraph().AppendText("<UNSUPPORTED>");
            }
        }
    }
}