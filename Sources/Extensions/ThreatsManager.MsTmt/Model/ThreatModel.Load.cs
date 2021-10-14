using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.MsTmt.Model
{
    public partial class ThreatModel
    {
        private const string Undefined = "<undefined>";

        #region Constructors.
        public ThreatModel([Required] string fileName)
        {
            _document = new XmlDocument();
            _document.Load(fileName);
            var isTemplate = fileName.EndsWith(".tb7", StringComparison.InvariantCultureIgnoreCase);
            GetEntityTypes(isTemplate);
            if (!isTemplate)
            {
                GetPages();
                GetFlows();
                GetElements();
            }
            RetrieveThreatCategories(isTemplate);
            RetrieveThreatProperties(isTemplate);
            RetrieveThreatTypesInfo(isTemplate);

            if (!isTemplate)
            {
                var threats = GetThreats();

                if (threats != null)
                {
                    AnalyzeThreats(threats);
                }
            }
        }
        #endregion

        #region Analyze knowledgebase.
        private void GetEntityTypes(bool isTemplate)
        {
            GetEntityTypes(isTemplate, true, _document.SelectNodes(
                $"{(isTemplate ? "" : "/*[local-name()='ThreatModel']")}/*[local-name()='KnowledgeBase']/*[local-name()='GenericElements']/*[local-name()='ElementType']")?.OfType<XmlNode>());
            GetEntityTypes(isTemplate, false, _document.SelectNodes(
                    $"{(isTemplate ? "" : "/*[local-name()='ThreatModel']")}/*[local-name()='KnowledgeBase']/*[local-name()='StandardElements']/*[local-name()='ElementType']")?.OfType<XmlNode>());
        }
        
        private void GetEntityTypes(bool isTemplate, bool isGeneric, IEnumerable<XmlNode> nodes)
        {
            var nodeList = nodes?.ToArray();
            if (nodeList != null)
            {
                var nsManager = new XmlNamespaceManager(_document.NameTable);
                nsManager.AddNamespace("a", "http://schemas.datacontract.org/2004/07/ThreatModeling.KnowledgeBase");

                string id;
                string parent;
                string name;
                string description;
                bool hidden;
                Bitmap image;
                ElementType elementType;
                IEnumerable<XmlNode> attributes;

                foreach (XmlNode node in nodeList)
                {
                    if (bool.TryParse(isTemplate ? node.SelectSingleNode("Hidden")?.InnerText :
                        node.SelectSingleNode("a:Hidden", nsManager)?.InnerText, out hidden) && !hidden)
                    {
                        id = isTemplate ? node.SelectSingleNode("ID")?.InnerText :
                            node.SelectSingleNode("a:Id", nsManager)?.InnerText;
                        parent = isTemplate ? node.SelectSingleNode("ParentElement")?.InnerText : 
                            node.SelectSingleNode("a:ParentId", nsManager)?.InnerText;

                        name = isTemplate ? node.SelectSingleNode("Name")?.InnerText :
                            node.SelectSingleNode("a:Name", nsManager)?.InnerText;
                        description = isTemplate ? node.SelectSingleNode("Description")?.InnerText.Trim() :
                            node.SelectSingleNode("a:Description", nsManager)?.InnerText.Trim();
                        image = null;
                        var imageText = isTemplate ? node.SelectSingleNode("Image")?.InnerText :
                            node.SelectSingleNode("a:ImageSource", nsManager)?.InnerText;
                        if (imageText != null)
                        {
                            try
                            {
                                var imageBytes = Convert.FromBase64String(imageText);
                                if ((imageBytes?.Length ?? 0) > 0)
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                        stream.Write(imageBytes, 0, imageBytes.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        image = (Bitmap)Image.FromStream(stream);
                                    }
                                }
                            }
                            catch (FormatException)
                            {
                                // We can ignore: the image is not set correctly.
                            }
                        }

                        var representation = isTemplate ? node.SelectSingleNode("Representation")?.InnerText :
                            node.SelectSingleNode("a:Representation", nsManager)?.InnerText;
                        switch (representation)
                        {
                            case "Rectangle":
                                elementType = ElementType.StencilRectangle;
                                break;

                            case "Ellipse":
                                elementType = ElementType.StencilEllipse;
                                break;

                            case "ParallelLines":
                                elementType = ElementType.StencilParallelLines;
                                break;
                            case "Line":
                                elementType = ElementType.Connector;
                                break;
                            case "LineBoundary":
                                elementType = ElementType.LineBoundary;
                                break;
                            case "BorderBoundary":
                                elementType = ElementType.BorderBoundary;
                                break;
                            case "Inherited":
                                elementType = _elementTypes.Values
                                    .FirstOrDefault(x => string.CompareOrdinal(parent, x.TypeId) == 0)?.ElementType ?? 
                                              _flowTypes.Values
                                    .FirstOrDefault(x => string.CompareOrdinal(parent, x.TypeId) == 0)?.ElementType ??
                                    ElementType.Undefined;
                                break;
                            default:
                                elementType = ElementType.Undefined;
                                break;
                        }

                        attributes = isTemplate ? node.SelectSingleNode("Attributes")?.OfType<XmlNode>().ToArray() :
                            node.SelectSingleNode("a:Attributes", nsManager)?.OfType<XmlNode>().ToArray();
                        if (!string.IsNullOrWhiteSpace(id) && elementType != ElementType.Undefined)
                        {
                            var elementTypeInfo = new ElementTypeInfo(elementType, 
                                id, parent, name, description, image, 
                                attributes, isGeneric, isTemplate);
                            if (elementType == ElementType.Connector)
                            {
                                if (!_flowTypes.ContainsKey(id))
                                    _flowTypes.Add(id, elementTypeInfo);
                            }
                            else
                            {
                                if (!_elementTypes.ContainsKey(id))
                                    _elementTypes.Add(id, elementTypeInfo);
                            }
                        }
                    }
                }
            }
        }

        private void RetrieveThreatCategories(bool isTemplate)
        {
            var nodes =
                _document.SelectNodes(
                    $"{(isTemplate ? "" : "/*[local-name()='ThreatModel']")}/*[local-name()='KnowledgeBase']/*[local-name()='ThreatCategories']/*[local-name()='ThreatCategory']/*[local-name()='Name']")
                    .OfType<XmlNode>().ToArray();
            _categories.AddRange(nodes.Select(x => x.InnerText));
        }

        private void RetrieveThreatProperties(bool isTemplate)
        {
            var nodes =
                _document.SelectNodes(
                    $"{(isTemplate ? "" : "/*[local-name()='ThreatModel']")}/*[local-name()='KnowledgeBase']/*[local-name()='ThreatMetaData']/*[local-name()='PropertiesMetaData']/*[local-name()='ThreatMetaDatum']")
                    .OfType<XmlNode>().ToArray();

            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("ab", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model.Abstracts");
            nsManager.AddNamespace("mo", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

            foreach (XmlNode node in nodes)
            {
                var name = isTemplate ? node.SelectSingleNode("Name")?.InnerText :
                    node.SelectSingleNode("mo:Name", nsManager)?.InnerText;

                if (string.CompareOrdinal(name, "UserThreatCategory") == 0)
                {
                    _propertyDefinitions.Add(new PropertyDefinition(node, _categories, isTemplate));
                }
                else if (string.CompareOrdinal(name, "InteractionString") == 0)
                {
                    _propertyDefinitions.Add(new PropertyDefinition(node, true, isTemplate));
                }
                else
                {
                    _propertyDefinitions.Add(new PropertyDefinition(node, isTemplate));
                }
            }
        }

        private void RetrieveThreatTypesInfo(bool isTemplate)
        {
            var nodes =
                _document.SelectNodes(
                    $"{(isTemplate ? "" : "/*[local-name()='ThreatModel']")}/*[local-name()='KnowledgeBase']/*[local-name()='ThreatTypes']/*[local-name()='ThreatType']")
                    .OfType<XmlNode>()
                    .ToArray();
            string key;
            string name;
            string priority;
            string description;
            XmlNode properties;
            string includeFilter;
            string excludeFilter;
            IEnumerable<PropertyDefinition> propertyDefinitions;

            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("a", "http://schemas.datacontract.org/2004/07/ThreatModeling.KnowledgeBase");

            foreach (XmlNode node in nodes)
            {
                key = isTemplate ? node.SelectSingleNode("Id")?.InnerText :
                    node.SelectSingleNode("a:Id", nsManager)?.InnerText;
                name = Cleanup(isTemplate ? node.SelectSingleNode("ShortTitle")?.InnerText :
                    node.SelectSingleNode("a:ShortTitle", nsManager)?.InnerText);
                description = Cleanup(isTemplate ? node.SelectSingleNode("Description")?.InnerText.Trim() :
                    node.SelectSingleNode("a:Description", nsManager)?.InnerText.Trim());
                properties = isTemplate ? node.SelectSingleNode("PropertiesMetaData") :
                    node.SelectSingleNode("a:PropertiesMetaData", nsManager);
                priority = GetPriority(properties, isTemplate);
                includeFilter = isTemplate
                    ? node.SelectSingleNode("GenerationFilters/Include")?.InnerText
                    : node.SelectSingleNode("a:GenerationFilters/a:Include", nsManager)?.InnerText;
                excludeFilter = isTemplate
                    ? node.SelectSingleNode("GenerationFilters/Exclude")?.InnerText
                    : node.SelectSingleNode("a:GenerationFilters/a:Exclude", nsManager)?.InnerText;
                propertyDefinitions = properties?.OfType<XmlNode>().Select(x => new PropertyDefinition(x, isTemplate));

                _threatTypes.Add(new ThreatType(key, name, description, priority, includeFilter, excludeFilter, propertyDefinitions));

                _threatTypeIDs.Add(name, key);
                _threatsPerType.Add(key, new List<Threat>());
            }
        }

        private string GetPriority(XmlNode nodeChildNode, bool isTemplate)
        {
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("m", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

            return isTemplate ? nodeChildNode?.SelectSingleNode("ThreatMetaDatum/Name[contains(text(),'Priority')]")?.ParentNode?.SelectSingleNode("Values")?.InnerText :
                nodeChildNode?.SelectSingleNode("m:ThreatMetaDatum/m:Name[contains(text(),'Priority')]", nsManager)?.ParentNode?.SelectSingleNode("m:Values", nsManager)?.InnerText;
        }
        #endregion

        #region Analyze entities.
        private void GetPages()
        {
            var nodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='DrawingSurfaceList']/*[local-name()='DrawingSurfaceModel']");

            var nsManager = new XmlNamespaceManager(_document.NameTable);
            nsManager.AddNamespace("mo", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");
            nsManager.AddNamespace("ab", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model.Abstracts");

            if (nodes != null)
            {
                Guid id;
                string name;

                foreach (XmlNode node in nodes)
                {
                    if (Guid.TryParse(node.SelectSingleNode("ab:Guid", nsManager)?.InnerText, out id))
                    {
                        name = node.SelectSingleNode("mo:Header", nsManager)?.InnerText;
                        if (string.IsNullOrWhiteSpace(name))
                            name = Undefined;
                        if (!_pagesGuids.ContainsKey(name))
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

                var nsManager = new XmlNamespaceManager(new NameTable());
                nsManager.AddNamespace("a", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
                nsManager.AddNamespace("ab", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model.Abstracts");
                nsManager.AddNamespace("mo", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

                foreach (XmlNode node in nodes)
                {
                    if (Guid.TryParse(node.SelectSingleNode("a:Key", nsManager)?.InnerText, out key))
                    {
                        valueNode = node.SelectSingleNode("a:Value", nsManager);
                        if (valueNode != null)
                        {
                            properties = null;
                            if (valueNode?.Attributes != null && valueNode.Attributes.Count > 1 &&
                                Enum.TryParse<ElementType>(valueNode.Attributes.GetNamedItem("type", "http://www.w3.org/2001/XMLSchema-instance")?.Value, false, out var type))
                            {
                                properties = Cleanup(valueNode.SelectSingleNode("ab:Properties", nsManager)?.OfType<XmlNode>());
                                var page = node.ParentNode?.ParentNode?.SelectSingleNode("mo:Header", nsManager)?.InnerText;
                                if (string.IsNullOrWhiteSpace(page))
                                {
                                    page = Undefined;
                                }
                                var typeId = valueNode.SelectSingleNode("ab:TypeId", nsManager)?.InnerText;

                                if (properties != null && !string.IsNullOrWhiteSpace(page) && !string.IsNullOrWhiteSpace(typeId))
                                {
                                    switch (type)
                                    {
                                        case ElementType.Connector:
                                            if (Guid.TryParse(
                                                    valueNode.SelectSingleNode("ab:SourceGuid", nsManager)?.InnerText,
                                                    out sourceId) &&
                                                Guid.TryParse(
                                                    valueNode.SelectSingleNode("ab:TargetGuid", nsManager)?.InnerText,
                                                    out targetId))
                                                _flows.Add(key, new FlowInfo(key, sourceId, targetId, typeId, page, properties));
                                            break;
                                        case ElementType.LineBoundary:
                                            _elements.Add(key, new ElementInfo(page, 0, 0, 0, 0, type, typeId, properties));
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AnalyzeThreats([PostSharp.Patterns.Contracts.NotNull] IEnumerable<Threat> threats)
        {
            _threats = new Dictionary<string, Threat>();
            foreach (var threat in threats)
            {
                AnalyzeThreat(threat);
            }
        }

        private void GetElements()
        {
            var pageNodes =
                _document.SelectNodes(
                    "/*[local-name()='ThreatModel']/*[local-name()='DrawingSurfaceList']/*[local-name()='DrawingSurfaceModel']");
            if (pageNodes != null)
            {
                var nsManager = new XmlNamespaceManager(new NameTable());
                nsManager.AddNamespace("a", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
                nsManager.AddNamespace("ab", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model.Abstracts");
                nsManager.AddNamespace("mo", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

                foreach (XmlNode pageNode in pageNodes)
                {
                    var page = pageNode.SelectSingleNode("mo:Header", nsManager)?.InnerText;
                    if (string.IsNullOrWhiteSpace(page))
                        page = Undefined;
                    var nodes = pageNode.SelectSingleNode("mo:Borders", nsManager);

                    if (nodes != null)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            Guid key;
                            if (Guid.TryParse(node.SelectSingleNode("a:Key", nsManager)?.InnerText, out key))
                            {
                                XmlNode valueNode = node.SelectSingleNode("a:Value", nsManager);
                                if (valueNode?.Attributes != null && valueNode.Attributes.Count > 1 &&
                                    Enum.TryParse<ElementType>(
                                        valueNode.Attributes.GetNamedItem("type",
                                            "http://www.w3.org/2001/XMLSchema-instance")?.Value, false, out var type) &&
                                    !IsAnnotationNode(valueNode, nsManager))
                                {
                                    XmlNode propertiesNode = valueNode.SelectSingleNode("ab:Properties", nsManager);
                                    int left = Int32.Parse(
                                        valueNode.SelectSingleNode("ab:Left", nsManager)?.InnerText ?? "0");
                                    int top = Int32.Parse(valueNode.SelectSingleNode("ab:Top", nsManager)?.InnerText ?? "0");
                                    int width = Int32.Parse(
                                        valueNode.SelectSingleNode("ab:Width", nsManager)?.InnerText ?? "0");
                                    int height =
                                        Int32.Parse(
                                            valueNode.SelectSingleNode("ab:Height", nsManager)?.InnerText ?? "0");
                                    var typeId = valueNode.SelectSingleNode("ab:TypeId", nsManager)?.InnerText;

                                    var properties = Cleanup(propertiesNode?.ChildNodes.OfType<XmlNode>());

                                    _elements.Add(key,
                                        new ElementInfo(page, top, left, width, height, type, typeId, properties));
                                }
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<XmlNode> Cleanup(IEnumerable<XmlNode> nodes)
        {
            return nodes?.Where(x =>
                string.CompareOrdinal(
                    x.Attributes?.GetNamedItem("type", "http://www.w3.org/2001/XMLSchema-instance")?.Value,
                    "b:HeaderDisplayAttribute") != 0);
        }

        private string Cleanup(string text)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                result = text
                    .Replace("{source.Name}", "Source")
                    .Replace("{target.Name}", "Target")
                    .Replace("{flow.Name}", "Flow");
            }

            return result;
        }

        private bool IsAnnotationNode([PostSharp.Patterns.Contracts.NotNull] XmlNode node, [PostSharp.Patterns.Contracts.NotNull] XmlNamespaceManager nsManager)
        {
            return string.CompareOrdinal(node.SelectSingleNode("ab:GenericTypeId", nsManager)?.InnerText, "GE.A") == 0;
        }

        private IEnumerable<Threat> GetThreats()
        {
            var nodes = _document.SelectNodes("/*[local-name() = 'ThreatModel']/*[local-name() = 'ThreatInstances']/*[local-name() = 'KeyValueOfstringThreatpc_P0_PhOB']");

            return nodes?.OfType<XmlNode>().Select(x => new Threat(this, x));
        }

        private void AnalyzeThreat([PostSharp.Patterns.Contracts.NotNull] Threat threat)
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
