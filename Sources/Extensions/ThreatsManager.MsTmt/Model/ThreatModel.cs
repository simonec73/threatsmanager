using System;
using System.Collections.Generic;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public partial class ThreatModel
    {
        #region Private member variables.
        private readonly XmlDocument _document;
        private Dictionary<string, Threat> _threats;
        private readonly Dictionary<Guid, string> _pages = new Dictionary<Guid, string>();
        private readonly Dictionary<string, Guid> _pagesGuids = new Dictionary<string, Guid>();
        private readonly Dictionary<Guid, FlowInfo> _flows = new Dictionary<Guid, FlowInfo>();
        private readonly Dictionary<Guid, ElementInfo> _elements = new Dictionary<Guid, ElementInfo>(); 
        private readonly List<string> _categories = new List<string>(); 
        private readonly List<PropertyDefinition> _propertyDefinitions = new List<PropertyDefinition>();
        private readonly Dictionary<string, string> _threatTypeNames = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _threatTypeIDs = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _threatTypeDescriptions = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _threatTypePriorities = new Dictionary<string, string>();
        private readonly Dictionary<string, IEnumerable<PropertyDefinition>> _threatTypeProperties = new Dictionary<string,  IEnumerable<PropertyDefinition>>();
        private readonly Dictionary<string, List<Threat>> _threatsPerType = new Dictionary<string, List<Threat>>();
        private readonly Dictionary<string, decimal> _currentPriorityEvaluation = new Dictionary<string, decimal>();
        private readonly Dictionary<string, int> _currentBuckets = new Dictionary<string, int>();
        private int _maxThreatPerTypeCount;
        private double _avgThreatPerTypeCount;
        const decimal CDefaultBigThreatTypesPremium = 1.1m;
        const decimal CDefaultBigThreatThreshold = 0.5m;
        #endregion

        #region Public properties.
        public IEnumerable<Threat> Threats => _threats.Values;

        public IDictionary<string, List<Threat>> ThreatsPerType => _threatsPerType;

        public IEnumerable<FlowInfo> Flows => _flows.Values;

        public IDictionary<Guid, ElementInfo> Elements => _elements;
        #endregion

        #region Public member functions: entity management.
        public Threat GetThreat([Required] string key)
        {
            return _threats.ContainsKey(key) ? _threats[key] : null;
        }

        public string GetPageName(Guid pageId)
        {
            if (_pages.ContainsKey(pageId))
                return _pages[pageId];
            else
                return null;
        }

        public Guid GetPageId([Required] string pageName)
        {
            if (_pagesGuids.ContainsKey(pageName))
                return _pagesGuids[pageName];
            else
                return Guid.Empty;
        }

        public FlowInfo GetFlowInfo(Guid flowId)
        {
            if (_flows.ContainsKey(flowId))
                return _flows[flowId];
            else
                return null;
        }

        public string GetThreatName([Required] string typeId)
        {
            if (_threatTypeNames.ContainsKey(typeId))
                return _threatTypeNames[typeId];
            else
                return null;
        }

        public ElementInfo GetElementInfo(Guid elementId)
        {
            if (_elements.ContainsKey(elementId))
                return _elements[elementId];
            else
                return null;
        }

        public IEnumerable<PropertyDefinition> Properties => _propertyDefinitions.AsReadOnly();

        public IEnumerable<string> Categories => _categories.AsReadOnly();

        public string GetThreatTypeName([Required] string threatTypeId)
        {
            if (_threatTypeNames.TryGetValue(threatTypeId, out var result))
                return result;
            else
                return null;
        }

        public string GetThreatTypeDescription(string threatTypeId)
        {
            if (_threatTypeDescriptions.TryGetValue(threatTypeId, out var result))
                return result;
            else
                return null;
        }

        public string GetThreatTypePriority(string threatTypeId)
        {
            if (_threatTypePriorities.TryGetValue(threatTypeId, out var result))
                return result;
            else
                return null;
        }

        public IEnumerable<PropertyDefinition> GetThreatTypeProperties(string threatTypeId)
        {
            if (_threatTypeProperties.TryGetValue(threatTypeId, out var result))
                return result;
            else
                return null;
        }
        #endregion
    }
}