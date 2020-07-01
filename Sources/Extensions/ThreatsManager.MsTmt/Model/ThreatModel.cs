using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public partial class ThreatModel
    {
        #region Private member variables.
        /// <summary>
        /// Xml DOM Document for the source document.
        /// </summary>
        private readonly XmlDocument _document;
        /// <summary>
        /// Threat Events loaded from the document.
        /// </summary>
        private Dictionary<string, Threat> _threats;
        /// <summary>
        /// Element Types (Entity Types + flows definitions)
        /// </summary>
        private readonly Dictionary<string, ElementTypeInfo> _elementTypes = new Dictionary<string, ElementTypeInfo>();
        /// <summary>
        /// Hierarchy of the Element Types.
        /// </summary>
        /// <remarks>The key contains the Element Key of the parent, and the value represents the Element Key for the children.</remarks>
        private readonly Dictionary<string, List<string>> _hierarchy = new Dictionary<string, List<string>>();

        private readonly List<ThreatType> _threatTypes = new List<ThreatType>();
        private readonly Dictionary<string, string> _threatTypeIDs = new Dictionary<string, string>();
        private readonly Dictionary<string, List<Threat>> _threatsPerType = new Dictionary<string, List<Threat>>();

        #region Threat Model information.
        private readonly Dictionary<Guid, string> _pages = new Dictionary<Guid, string>();
        private readonly Dictionary<string, Guid> _pagesGuids = new Dictionary<string, Guid>();

        private readonly Dictionary<Guid, FlowInfo> _flows = new Dictionary<Guid, FlowInfo>();
        private readonly Dictionary<Guid, ElementInfo> _elements = new Dictionary<Guid, ElementInfo>(); 
        private readonly List<string> _categories = new List<string>(); 
        private readonly List<PropertyDefinition> _propertyDefinitions = new List<PropertyDefinition>();
        private readonly Dictionary<string, List<string>> _propertyEntityTypes = new Dictionary<string, List<string>>();
        #endregion
        #endregion

        #region Public properties.
        public IEnumerable<ElementTypeInfo> ElementTypes => _elementTypes.Values;

        public IDictionary<string, List<Threat>> ThreatsPerType => _threatsPerType;

        public IEnumerable<FlowInfo> Flows => _flows.Values;

        public IDictionary<Guid, ElementInfo> Elements => _elements;
        #endregion

        #region Public member functions: entity management.
        public IEnumerable<PropertyDefinition> Properties => _propertyDefinitions.AsReadOnly();

        public ThreatType GetThreatType([Required] string threatTypeId)
        {
            return _threatTypes.FirstOrDefault(x => string.CompareOrdinal(threatTypeId, x.Key) == 0);
        }

        public IEnumerable<string> GetElementTypesForProperty([Required] string propertyKey)
        {
            return _propertyEntityTypes
                .Where(x => string.CompareOrdinal(propertyKey, x.Key) == 0)
                .Select(x => x.Value).FirstOrDefault()?.ToArray();
        }

        public string GetPropertyName([Required] string elementType, [Required] string propertyKey)
        {
            return _elementTypes.Values
                .FirstOrDefault(x => string.CompareOrdinal(x.Name, elementType) == 0)?
                .Properties?
                .FirstOrDefault(x => string.CompareOrdinal(x.Key, propertyKey) == 0)?
                .Name;
        }

        public IEnumerable<ElementTypeInfo> GetChildren([Required] string parentKey)
        {
            IEnumerable<ElementTypeInfo> result = null;

            if (_hierarchy.TryGetValue(parentKey, out var list))
            {
                List<ElementTypeInfo> found = new List<ElementTypeInfo>();

                var items = list
                    .Select(x => _elementTypes
                        .Where(y => string.CompareOrdinal(y.Key, x) == 0)
                        .Select(y => y.Value)
                        .FirstOrDefault())
                    .Where(x => x != null)?
                    .ToArray();

                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        found.Add(item);
                        var itemItems = GetChildren(item.TypeId)?.ToArray();
                        if (itemItems?.Any() ?? false)
                            found.AddRange(itemItems);
                    }
                }

                result = found.ToArray();
            }

            return result;
        }
        #endregion
    }
}