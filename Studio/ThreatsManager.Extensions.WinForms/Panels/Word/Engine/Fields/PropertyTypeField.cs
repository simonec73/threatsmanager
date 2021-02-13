using PostSharp.Patterns.Contracts;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Panels.Word.Engine.Fields
{
    internal class PropertyTypeField : Field
    {
        public PropertyTypeField([NotNull] IPropertyType propertyType)
        {
            PropertyType = propertyType;
        }

        public IPropertyType PropertyType { get; private set; }

        public override string ToString()
        {
            return PropertyType.Name;
        }

        public override string Tooltip => PropertyType.Description;

        public override void InsertContent([NotNull] WTableCell cell, [NotNull] IIdentity identity)
        {
            if (identity is IPropertiesContainer)
            {
                cell.AddParagraph().AppendText(GetContent(identity));
            }
            else
                cell.AddParagraph().AppendText("<UNSUPPORTED>");
        }

        public override bool IsVisible(IIdentity identity)
        {
            return !string.IsNullOrWhiteSpace(GetContent(identity));
        }

        private string GetContent(IIdentity identity)
        {
            string result = null;

            if (identity is IPropertiesContainer container)
            {
                var property = container.GetProperty(PropertyType);
                if (property is IPropertyIdentityReference reference)
                {
                    result = reference.Value?.Name;
                }
                else
                {
                    result = property?.StringValue;
                }
            }

            return result;
        }
    }
}
