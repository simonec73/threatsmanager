using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using IEntity = ThreatsManager.Interfaces.ObjectModel.Entities.IEntity;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class SourceField : Field
    {
        public override string ToString()
        {
            return "Source";
        }

        public override string Tooltip => "Source of the flow.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IDataFlow flow && flow.Source is IEntity source && source.Name != null)
                cell.AddParagraph().AppendText(source.Name.Trim());
        }

        public override bool IsVisible(IIdentity identity)
        {
            return (identity is IDataFlow flow) && !string.IsNullOrWhiteSpace(flow.Source?.Name);
        }
    }
}