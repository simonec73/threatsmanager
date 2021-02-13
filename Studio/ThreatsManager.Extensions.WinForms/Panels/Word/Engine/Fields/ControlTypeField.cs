using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class ControlTypeField : Field
    {
        public override string ToString()
        {
            return "Control Type";
        }

        public override string Tooltip => "Control Type of the Mitigation.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IMitigation mitigation)
                cell.AddParagraph().AppendText(mitigation.ControlType.GetEnumLabel());
            else
                cell.AddParagraph().AppendText("<UNSUPPORTED>");
        }
    }
}