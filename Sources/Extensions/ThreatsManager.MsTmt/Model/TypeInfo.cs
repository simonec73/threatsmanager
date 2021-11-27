using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ThreatsManager.MsTmt.Model
{
    public abstract class TypeInfo
    {
        private readonly List<Property> _properties = new List<Property>();

        protected TypeInfo(IEnumerable<XmlNode> nodes, bool isTemplate)
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
                        string propertyKey = isTemplate ? node.SelectSingleNode("Name")?.InnerText :
                            node.SelectSingleNode("b:Name", nsManager)?.InnerText;
                        string propertyName = isTemplate ? node.SelectSingleNode("DisplayName")?.InnerText :
                            node.SelectSingleNode("b:DisplayName", nsManager)?.InnerText;
                        var nodeType = isTemplate ? node.SelectSingleNode("Type")?.InnerText :
                            node.SelectSingleNode("b:Type", nsManager)?.InnerText;
                        if (string.CompareOrdinal(nodeType,  "List") == 0)
                        {
                            var nodeValues = isTemplate ? node.SelectSingleNode("AttributeValues") : 
                                node.SelectSingleNode("b:AttributeValues", nsManager);
                            var values = nodeValues?
                                .ChildNodes
                                .OfType<XmlNode>()
                                .Select(x => x.InnerText)
                                .ToArray(); 

                            _properties.Add(new Property()
                            {
                                Key = propertyKey,
                                Name = propertyName,
                                Type = PropertyType.List,
                                Values = values,
                                Value = values?.FirstOrDefault()
                            });
                        }
                    }
                }
            }
        }

        public IEnumerable<Property> Properties => _properties;
    }
}
