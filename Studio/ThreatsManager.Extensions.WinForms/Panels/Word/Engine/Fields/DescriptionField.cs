using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class DescriptionField : Field
    {
        public override string ToString()
        {
            return "Description";
        }

        public override string Tooltip => "Description of the entity.";

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            cell.AddParagraph().AppendText(identity.Description?.Trim());
        }

        public override bool IsVisible(IIdentity identity)
        {
            return !string.IsNullOrWhiteSpace(identity?.Description);
        }
    }
}