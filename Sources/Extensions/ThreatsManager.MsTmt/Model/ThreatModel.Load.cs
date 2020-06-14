using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public partial class ThreatModel
    {
        #region Constructors.
        public ThreatModel([Required] string fileName)
        {
            _document = new XmlDocument();
            _document.Load(fileName);
            GetPages();
            GetFlows();
            GetElements();
            RetrieveThreatCategories();
            RetrieveThreatProperties();
            RetrieveThreatTypesInfo();
            var threats = GetThreats();

            if (threats != null)
            {
                AnalyzeThreats(threats);
            }
        }
        #endregion

        #region Analyze entities.
        private void GetPages()
        {
            var nodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='DrawingSurfaceList']/*[local-name()='DrawingSurfaceModel']");
            if (nodes != null)
            {
                Guid id;
                string name;

                foreach (XmlNode node in nodes)
                {
                    if (Guid.TryParse(node.SelectSingleNode("Guid")?.InnerText, out id))
                    {
                        name = node.SelectSingleNode("Header")?.InnerText;
                        if (name != null)
                        {
                            _pages.Add(id, name);
                            _pagesGuids.Add(name, id);
                        }
                    }
                }
            }
        }

        private void GetFlows()
        {
            var nodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='DrawingSurfaceList']/*[local-name()='DrawingSurfaceModel']/*[local-name()='Lines']/*[local-name()='KeyValueOfguidanyType']");
            if (nodes != null)
            {
                Guid key;
                XmlNode valueNode;
                IEnumerable<XmlNode> properties;
                Guid sourceId;
                Guid targetId;

                foreach (XmlNode node in nodes)
                {
                    if (Guid.TryParse(node.ChildNodes[0].InnerText, out key))
                    {
                        valueNode = node.ChildNodes[1];
                        properties = null;
                        if (valueNode.Attributes
                            .OfType<XmlAttribute>(
                            ).Any(x => (string.CompareOrdinal(x.LocalName, "type") == 0) &&
                                       (string.CompareOrdinal(x.Value, "Connector") == 0)))
                        {
                            properties = valueNode.ChildNodes[2].OfType<XmlNode>();
                        }

                        var header = node.ParentNode?.ParentNode?.ChildNodes
                            .OfType<XmlNode>().FirstOrDefault(x => string.CompareOrdinal(x.LocalName, "Header") == 0);

                        if (Guid.TryParse(valueNode.ChildNodes[8].InnerText, out sourceId) &&
                            Guid.TryParse(valueNode.ChildNodes[11].InnerText, out targetId) &&
                            properties != null && header != null)
                            _flows.Add(key, new FlowInfo(key, sourceId, targetId, header.InnerText, properties));
                    }
                }
            }
        }

        private void AnalyzeThreats([NotNull] IEnumerable<Threat> threats)
        {
            _threats = new Dictionary<string, Threat>();
            foreach (var threat in threats)
            {
                AnalyzeThreat(threat);
            }

            _maxThreatPerTypeCount = _threatsPerType.Values.Max(x => x.Count);
            _avgThreatPerTypeCount = _threatsPerType.Values.Average(x => x.Count);
        }

        private void GetElements()
        {
            var pageNodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='DrawingSurfaceList']/*[local-name()='DrawingSurfaceModel']");
            if (pageNodes != null)
            {
                foreach (XmlNode pageNode in pageNodes)
                {
                    var page = pageNode.ChildNodes[5].InnerText;

                    var nodes = pageNode.ChildNodes[4];
                    
                    foreach (XmlNode node in nodes)
                    {
                        Guid key;
                        if (Guid.TryParse(node.ChildNodes[0].InnerText, out key))
                        {
                            ElementType type;

                            XmlNode valueNode = node.ChildNodes[1];
                            if (valueNode.Attributes != null && valueNode.Attributes.Count > 1 &&
                                Enum.TryParse(valueNode.Attributes[1].Value, false, out type))
                            {
                                XmlNode propertiesNode = valueNode.ChildNodes[2];
                                var properties = propertiesNode.ChildNodes;
                                int left = Int32.Parse(valueNode.ChildNodes[5].InnerText);
                                int top = Int32.Parse(valueNode.ChildNodes[8].InnerText);
                                _elements.Add(key, new ElementInfo(page, top, left, properties.OfType<XmlNode>(), type));
                            }
                        }
                    }
                }
            }
        }

        private void RetrieveThreatCategories()
        {
            var nodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='KnowledgeBase']/*[local-name()='ThreatCategories']/*[local-name()='ThreatCategory']/*[local-name()='Name']");
            _categories.AddRange(nodes.OfType<XmlNode>().Select(x => x.InnerText));
        }

        private void RetrieveThreatProperties()
        {
            var nodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='KnowledgeBase']/*[local-name()='ThreatMetaData']/*[local-name()='PropertiesMetaData']/*[local-name()='ThreatMetaDatum']");

            foreach (XmlNode node in nodes)
            {
                if (string.CompareOrdinal(node.ChildNodes[0].InnerText, "UserThreatCategory") == 0)
                {
                    _propertyDefinitions.Add(new PropertyDefinition(node, _categories));
                }
                else if (string.CompareOrdinal(node.ChildNodes[0].InnerText, "InteractionString") == 0)
                {
                    _propertyDefinitions.Add(new PropertyDefinition(node, true));
                }
                else
                {
                    _propertyDefinitions.Add(new PropertyDefinition(node));
                }
            }
        }

        private void RetrieveThreatTypesInfo()
        {
            var nodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='KnowledgeBase']/*[local-name()='ThreatTypes']/*[local-name()='ThreatType']");
            string key;
            string name;
            string priority;
            string description;

            foreach (XmlNode node in nodes)
            {
                key = node.ChildNodes[4].InnerText;
                name = node.ChildNodes[7].InnerText;
                description = node.ChildNodes[2].InnerText;
                priority = GetPriority(node.ChildNodes[5]);

                _threatTypeProperties.Add(key, node.ChildNodes[5].ChildNodes.OfType<XmlNode>().Select(x => new PropertyDefinition(x)));

                _threatTypeNames.Add(key, name);
                _threatTypeIDs.Add(name, key);
                _threatTypeDescriptions.Add(key, description);
                _threatTypePriorities.Add(key, priority);
                _threatsPerType.Add(key, new List<Threat>());
            }
        }

        private string GetPriority([NotNull] XmlNode nodeChildNode)
        {
            return nodeChildNode.ChildNodes[2].ChildNodes[3].InnerText;
        }

        private IEnumerable<Threat> GetThreats()
        {
            var nodes = _document.SelectNodes("/*[local-name() = 'ThreatModel']/*[local-name() = 'ThreatInstances']/*[local-name() = 'KeyValueOfstringThreatpc_P0_PhOB']");

            return nodes?.OfType<XmlNode>().Select(x => new Threat(this, x));
        }

        private void AnalyzeThreat([NotNull] Threat threat)
        {
            _threats.Add(threat.Key, threat);

            var threatType = threat.TypeId;
            List<Threat> list;
            if (_threatsPerType.ContainsKey(threatType))
            {
                list = _threatsPerType[threatType];
            }
            else
            {
                if (_threatsPerType.ContainsKey(threat.TypeId))
                    list = _threatsPerType[threat.TypeId];
                else
                {
                    list = new List<Threat>();
                    _threatsPerType.Add(threat.TypeId, list);
                }
            }
            list.Add(threat);
        }
        #endregion
    }
}
