using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Abstract class used to represent an Item Row.
    /// </summary>
    public abstract class ItemRow 
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">The label to be used for the row.</param>
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

        /// <summary>
        /// Creates a new ItemRow for a given Property.
        /// </summary>
        /// <param name="container">Container of the Property.</param>
        /// <param name="property">Property to be analyzed.</param>
        /// <returns>The created ItemRow.</returns>
        public static ItemRow Create([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            ItemRow result = null;

            var propertyType = property.PropertyType;

            if (property is IPropertyArray propertyArray)
            {
                result = new ListRow(propertyType.Name, propertyArray.Value?.Select(x => new Line(x?.TrimEnd(' ', '\r', '\n'))));
            } else if (property is IPropertyIdentityReference propertyIdentityReference)
            {
                if (propertyIdentityReference.Value is IIdentity identity && identity is IThreatModelChild child)
                {
                    result = new TextRow(propertyType.Name,
                        $"{identity.Name}",
                        $"[{child.Model.GetIdentityTypeInitial(identity)}] ", null, new[] {identity.Id});
                }
                else
                {
                    result = new EmptyRow(propertyType.Name);
                }
            } else if (property is IPropertyJsonSerializableObject propertyJsonSerializableObject)
            {
                var propertyViewerId = propertyJsonSerializableObject.PropertyType.CustomPropertyViewer;
                if (!string.IsNullOrWhiteSpace(propertyViewerId))
                {
                    var factory = ExtensionUtils.GetExtensionByLabel<IPropertyViewerFactory>(propertyViewerId);
                    if (factory != null)
                    {
                        var propertyViewer = factory.CreatePropertyViewer(container, property);
                        if (propertyViewer != null)
                        {
                            var blocks = propertyViewer.Blocks?.Where(x => x.Printable).ToArray();
                            if (blocks?.Any() ?? false)
                            {
                                if (blocks.Length == 1)
                                {
                                    var block = blocks[0];
                                    result = new TextRow(block.Label, block.Text);
                                }
                                else
                                {
                                    result = new TableRow(property.PropertyType.Name, new[]
                                    {
                                        new TableColumn("Property", 150),
                                        new TableColumn("Value", 350)
                                    }, GetCells(blocks));
                                }
                            }
                        }
                    }
                }
            } else if (property is IPropertyList propertyList)
            {
                result = new TextRow(propertyType.Name, propertyList.Value?.ToString()?.TrimEnd(' ', '\r', '\n'));
            } else if (property is IPropertyListMulti propertyListMulti)
            {
                result = new ListRow(propertyType.Name, propertyListMulti.Values?.Select(x => new Line(x.ToString()?.TrimEnd(' ', '\r', '\n'))));
            } else
            {
                result = new TextRow(propertyType.Name, property.StringValue?.TrimEnd(' ', '\r', '\n'));
            }

            if (result == null)
                result = new EmptyRow(propertyType.Name);

            return result;
        }

        private static IEnumerable<Cell> GetCells(IEnumerable<IPropertyViewerBlock> blocks)
        {
            IEnumerable<Cell> result = null;

            var list = blocks?.ToArray();
            if (list?.Any() ?? false)
            {
                var cells = new List<Cell>();

                foreach (var item in list)
                {
                    cells.Add(new Cell(item.Label));
                    cells.Add(new Cell(item.Text?.TrimEnd(' ', '\r', '\n')));
                }

                result = cells;
            }

            return result;
        }
    }
}
