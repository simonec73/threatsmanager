using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Model
{
    public class Info
    {
        private readonly List<Property> _properties = new List<Property>();

        public Info(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Resources.ArgumentNotDefined, nameof(name));

            Name = name;
        }

        public Info(IEnumerable<XmlNode> nodes)
        {
            foreach (XmlNode node in  nodes)
            {
                string propertyName = node.ChildNodes[0].InnerText;
                string propertyValue = node.ChildNodes[2].InnerText;
                if (String.CompareOrdinal(propertyName, "Name") == 0)
                {
                    Name = propertyValue;
                }
                else
                {
                    var nodeType = node.Attributes?.GetNamedItem("type", "http://www.w3.org/2001/XMLSchema-instance");
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
                                var selectedIndex = node.SelectSingleNode("*[local-name()='SelectedIndex']");
                                int index;
                                if (int.TryParse(selectedIndex?.InnerText, out index))
                                {
                                    propertyValue = node.ChildNodes[2].ChildNodes[index].InnerText;
                                }

                                values = node.ChildNodes[2].ChildNodes.OfType<XmlNode>().Select(x => x.InnerText);
                                break;
                        }
                    }

                    if (!ignore)
                    {
                        _properties.Add(new Property() {
                            Name = propertyName,
                            Value = propertyValue,
                            Type = propertyType,
                            Values = values
                        });
                    }
                }
            }
        }

        public string Name { get; private set; }

        public IEnumerable<Property> Properties => _properties;
    }

    public class Property
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public PropertyType Type { get; set; }
        public IEnumerable<string> Values { get; set; }
    }

    public enum PropertyType
    {
        String,
        Boolean,
        List
    }
}
