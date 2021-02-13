using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class FlowTypeField : Field
    {
        public override string ToString()
        {
            return "Flow Type";
        }

        public override string Tooltip => "Type of the flow.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IDataFlow flow)
                cell.AddParagraph().AppendText(flow.FlowType.GetEnumLabel());
        }

        public override bool IsVisible(IIdentity identity)
        {
            return identity is IDataFlow;
        }
    }
}