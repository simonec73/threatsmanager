using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Element Types for Entity Types.
        /// </summary>
        private readonly Dictionary<string, ElementTypeInfo> _elementTypes = new Dictionary<string, ElementTypeInfo>();
        /// <summary>
        /// Element Types foe flows definitions.
        /// </summary>
        private readonly Dictionary<string, ElementTypeInfo> _flowTypes = new Dictionary<string, ElementTypeInfo>();
 
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
        #endregion
        #endregion

        #region Public properties.
        public IEnumerable<ElementTypeInfo> ElementTypes => _elementTypes.Values;

        public IEnumerable<ElementTypeInfo> FlowTypes => _flowTypes.Values;

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

        public IEnumerable<ElementTypeInfo> GetElementTypesForProperty([Required] string propertyKey)
        {
            IEnumerable<ElementTypeInfo> result = _elementTypes.Values
                .Where(x => x.IsGeneric && (x.Properties?.Any(y => string.CompareOrdinal(propertyKey, y.Key) == 0) ?? false))
                .ToArray();

            if (!result.Any())
            {
                result = _elementTypes.Values
                    .Where(x => !x.IsGeneric && (x.Properties?.Any(y => string.CompareOrdinal(propertyKey, y.Key) == 0) ?? false))
                    .ToArray();
            }

            return result;
        }

        public IEnumerable<ElementTypeInfo> GetFlowTypesForProperty([Required] string propertyKey)
        {
            IEnumerable<ElementTypeInfo> result = _flowTypes.Values
                .Where(x => x.IsGeneric && (x.Properties?.Any(y => string.CompareOrdinal(propertyKey, y.Key) == 0) ?? false))
                .ToArray();

            if (!result.Any())
            {
                result = _flowTypes.Values
                    .Where(x => !x.IsGeneric && (x.Properties?.Any(y => string.CompareOrdinal(propertyKey, y.Key) == 0) ?? false))
                    .ToArray();
            }

            return result;
        }
        #endregion
    }
}