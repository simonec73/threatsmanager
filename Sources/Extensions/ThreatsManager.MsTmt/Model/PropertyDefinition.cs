using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

// ReSharper disable PossibleMultipleEnumeration

namespace ThreatsManager.MsTmt.Model
{
    public class PropertyDefinition
    {
        private readonly string _name;
        private readonly string _label;
        private readonly bool _hideFromUi;
        private readonly List<string> _values;

        public PropertyDefinition(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _name = node.ChildNodes[0].InnerText;
            _label = node.ChildNodes[1].InnerText;
            bool.TryParse(node.ChildNodes[2].InnerText, out _hideFromUi);

            var values = node.ChildNodes[3];
            if (values.HasChildNodes)
            {
                _values = new List<string>(values.ChildNodes
                    .OfType<XmlNode>().Select(x => x.InnerText)
                    .Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }

        public PropertyDefinition(XmlNode node, IEnumerable<string> values)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _name = node.ChildNodes[0].InnerText;
            _label = node.ChildNodes[1].InnerText;
            bool.TryParse(node.ChildNodes[2].InnerText, out _hideFromUi);

            if (values != null && values.Any())
            {
                _values = new List<string>(values);
            }
        }

        public PropertyDefinition(XmlNode node, bool hidden)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _name = node.ChildNodes[0].InnerText;
            _label = node.ChildNodes[1].InnerText;
            _hideFromUi = hidden;

            var values = node.ChildNodes[3];
            if (values.HasChildNodes)
            {
                _values = new List<string>(values.ChildNodes
                    .OfType<XmlNode>().Select(x => x.InnerText)
                    .Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }

        public string Name => _name;

        public string Label => _label;

        public bool HideFromUi => _hideFromUi;

        public IEnumerable<string> Values => _values?.AsReadOnly();

        public override string ToString()
        {
            return _label;
        }
    }
}