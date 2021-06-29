using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ThreatsManager.MsTmt.Model
{
    public abstract class Info
    {
        private readonly List<Property> _properties = new List<Property>();
        private const string Undefined = "<undefined>";

        protected Info(IEnumerable<XmlNode> nodes)
        {
            var nodesArray = nodes?.ToArray();
            if (nodesArray?.Any() ?? false)
            {
                var document = nodesArray.FirstOrDefault()?.OwnerDocument;
                if (document != null)
                {
                    var nsManager = new XmlNamespaceManager(document.NameTable);
                    nsManager.AddNamespace("b", "http://schemas.datacontract.org/2004/07/ThreatModeling.KnowledgeBase");

                    foreach (XmlNode node in nodesArray)
                    {
                        var propertyKey = node.SelectSingleNode("b:Name", nsManager)?.InnerText;
                        var propertyName = node.SelectSingleNode("b:DisplayName", nsManager)?.InnerText;
                        var propertyValue = node.SelectSingleNode("b:Value", nsManager)?.InnerText;
                        if (String.CompareOrdinal(propertyName, "Name") == 0)
                        {
                            Name = string.IsNullOrWhiteSpace(propertyValue) ? Undefined : propertyValue;
                        }
                        else
                        {
                            var nodeType =
                                node.Attributes?.GetNamedItem("type", "http://www.w3.org/2001/XMLSchema-instance");
                            PropertyType propertyType = PropertyType.String;
                            IEnumerable<string> values = null;
                            bool ignore = false;
                            if (nodeType != null)
                            {
                                switch (nodeType.Value)
                                {
                                    case "b:HeaderDisplayAttribute":
                                        if (string.CompareOrdinal(propertyName, "Configurable Attributes") != 0 &&
                                            string.CompareOrdinal(propertyName, "Custom Attributes") != 0 &&
                                            string.CompareOrdinal(propertyName, "Predefined Static Attributes") != 0)
                                        {
                                            propertyValue = propertyName;
                                            propertyName = "Entity Type";
                                        }
                                        else
                                        {
                                            ignore = true;
                                        }

                                        break;
                                    case "b:StringDisplayAttribute":
                                        break;
                                    case "b:BooleanDisplayAttribute":
                                        propertyType = PropertyType.Boolean;
                                        break;
                                    case "b:ListDisplayAttribute":
                                        propertyType = PropertyType.List;
                                        var selectedIndex = node.SelectSingleNode("b:SelectedIndex", nsManager)?.InnerText;
                                        values = node.SelectSingleNode("b:Value", nsManager)?
                                            .ChildNodes
                                            .OfType<XmlNode>()
                                            .Select(x => x.InnerText)
                                            .ToArray();
                                        if (int.TryParse(selectedIndex, out var index) && (values?.Count() ?? 0) > index)
                                        {
                                            propertyValue = values.ElementAt(index);
                                        }
                                        break;
                                }
                            }

                            if (!ignore)
                            {
                                _properties.Add(new Property()
                                {
                                    Key = propertyKey,
                                    Name = propertyName,
                                    Value = propertyValue,
                                    Type = propertyType,
                                    Values = values
                                });
                            }
                        }
                    }
                }
            }
        }

        public string Name { get; private set; }

        public IEnumerable<Property> Properties => _properties;
    }
}
