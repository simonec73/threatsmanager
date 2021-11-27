using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A cell.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value of the cell.</param>
        /// <param name="textColor">[Optional] Color of the text. If missing, the default value will be used.</param>
        /// <param name="backColor">[Optional] Color of the background. If missing, the default value will be used.</param>
        /// <param name="bold">[Optional] Indicates if the text must be bold.</param>
        /// <param name="centered">[Optional] Indicates if the text must be centered.</param>
        public Cell(string value, KnownColor? textColor = null, KnownColor? backColor = null, bool bold = false, bool centered = false)
        {
            Value = value;
            TextColor = textColor;
            BackColor = backColor;
            Bold = bold;
            Centered = centered;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="children">Children objects.</param>
        public Cell(IEnumerable<Line> children)
        {
            Children = children;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value of the cell to be used if no LinkIds correspond to an actual object.</param>
        /// <param name="prefix">Prefix to compose the final text.</param>
        /// <param name="suffix">Suffix to compose the final text.</param>
        /// <param name="linkIds">Identifiers of items in a List to which the text should be linked to.</param>
        /// <param name="textColor">[Optional] Color of the text. If missing, the default value will be used.</param>
        /// <param name="backColor">[Optional] Color of the background. If missing, the default value will be used.</param>
        /// <param name="bold">[Optional] Indicates if the text must be bold.</param>
        /// <param name="centered">[Optional] Indicates if the text must be centered.</param>
        public Cell(string value, string prefix, string suffix, IEnumerable<Guid> linkIds, 
            KnownColor? textColor = null, KnownColor? backColor = null, bool bold = false, bool centered = false) : 
            this(value, textColor, backColor, bold, centered)
        {
            LinkIds = linkIds;
            Prefix = prefix;
            Suffix = suffix;
        }

        /// <summary>
        /// Creates a new ItemRow for a given Property.
        /// </summary>
        /// <param name="container">Container of the Property.</param>
        /// <param name="property">Property to be analyzed.</param>
        /// <returns>The created ItemRow.</returns>
        public static Cell Create([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            Cell result = null;

            var propertyType = property.PropertyType;

            if (property is IPropertyIdentityReference propertyIdentityReference)
            {
                if (propertyIdentityReference.Value is IIdentity identity && identity is IThreatModelChild child)
                {
                    result = new Cell($"{identity.Name}", 
                        $"[{child.Model.GetIdentityTypeInitial(identity)}] ", null, 
                        new[] { identity.Id });
                }
            }
            else if (property is IPropertyJsonSerializableObject propertyJsonSerializableObject)
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
                                result = new Cell(blocks.First().Text);
                            }
                        }
                    }
                }
            }
            else if (property is IPropertyList propertyList)
            {
                result = new Cell(propertyList.Value?.ToString()?.TrimEnd(' ', '\r', '\n'));
            }

            return result;
        }

        /// <summary>
        /// Value of the cell.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Prefix to compose the final text.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Suffix to compose the final text.
        /// </summary>
        public string Suffix { get; private set; }

        /// <summary>
        /// Color of the Text.
        /// </summary>
        public KnownColor? TextColor { get; private set; }

        /// <summary>
        /// Color of the Background.
        /// </summary>
        public KnownColor? BackColor { get; private set; }

        /// <summary>
        /// Specifies if the text must be bold.
        /// </summary>
        public bool Bold { get; private set; }

        /// <summary>
        /// Specifies if the text must be centered.
        /// </summary>
        public bool Centered { get; private set; }

        /// <summary>
        /// Identifier of items in a List to which the content of the cell should be linked to.
        /// </summary>
        /// <remarks>The Ids are listed in priority order: the first which is a match is applied.</remarks>
        public IEnumerable<Guid> LinkIds { get; private set; }

        /// <summary>
        /// Children for the current cell.
        /// </summary>
        /// <remarks>If Children is not empty, then the Cell is to be represented as a bulleted list of its children.</remarks>
        public IEnumerable<Line> Children { get; private set; }
    }
}