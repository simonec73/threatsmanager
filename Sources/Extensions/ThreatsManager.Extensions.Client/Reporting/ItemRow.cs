using System.ComponentModel.Design;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Abstract class used to represent an Item Row.
    /// </summary>
    public abstract class ItemRow 
    {
        protected ItemRow(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Flag specifying if the row should be visible or not.
        /// </summary>
        public abstract bool Visible { get; }

        /// <summary>
        /// Label of the Row.
        /// </summary>
        public string Label { get; private set; }

        public static ItemRow Create([NotNull] IProperty property)
        {
            ItemRow result = null;

            var propertyType = property.PropertyType;

            if (property is IPropertyArray propertyArray)
            {
                result = new ListRow(propertyType.Name, propertyArray.Value);
            } else if (property is IPropertyIdentityReference propertyIdentityReference && 
                       propertyIdentityReference.Value is IIdentity identity && identity is IThreatModelChild child)
            {
                result = new TextRow(propertyType.Name,
                    $"[{child.Model.GetIdentityTypeInitial(identity)}] {identity.Name}");
            } else if (property is IPropertyJsonSerializableObject propertyJsonSerializableObject)
            {
                // TODO: print something out of the object.
            } else if (property is IPropertyList propertyList)
            {
                result = new TextRow(propertyType.Name, propertyList.Value?.Label);
            } else if (property is IPropertyListMulti propertyListMulti)
            {
                result = new ListRow(propertyType.Name, propertyListMulti.Values?.Select(x => x.Label));
            } else
            {
                result = new TextRow(propertyType.Name, property.StringValue);
            }

            return result;
        }
    }
}
