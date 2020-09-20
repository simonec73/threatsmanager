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

        public PropertyDefinition(XmlNode node, bool isTemplate)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("m", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

            _name = isTemplate ? node.SelectSingleNode("Name")?.InnerText :
                node.SelectSingleNode("m:Name", nsManager)?.InnerText;
            _label = isTemplate ? node.SelectSingleNode("Label")?.InnerText :
                node.SelectSingleNode("m:Label", nsManager)?.InnerText;
            bool.TryParse(isTemplate ? node.SelectSingleNode("HideFromUI")?.InnerText :
                node.SelectSingleNode("m:HideFromUI", nsManager)?.InnerText, out _hideFromUi);

            var values = isTemplate ? node.SelectSingleNode("Values") :
                node.SelectSingleNode("m:Values", nsManager);
            if (values?.HasChildNodes ?? false)
            {
                _values = new List<string>(values.ChildNodes
                    .OfType<XmlNode>().Select(x => x.InnerText)
                    .Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }

        public PropertyDefinition(XmlNode node, IEnumerable<string> values, bool isTemplate)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("m", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

            _name = isTemplate ? node.SelectSingleNode("Name")?.InnerText :
                node.SelectSingleNode("m:Name", nsManager)?.InnerText;
            _label = isTemplate ? node.SelectSingleNode("Label")?.InnerText :
                node.SelectSingleNode("m:Label", nsManager)?.InnerText;
            bool.TryParse(isTemplate ? node.SelectSingleNode("HideFromUI")?.InnerText :
                node.SelectSingleNode("m:HideFromUI", nsManager)?.InnerText, out _hideFromUi);

            if (values != null && values.Any())
            {
                _values = new List<string>(values);
            }
        }

        public PropertyDefinition(XmlNode node, bool hidden, bool isTemplate)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("m", "http://schemas.datacontract.org/2004/07/ThreatModeling.Model");

            _name = isTemplate ? node.SelectSingleNode("Name")?.InnerText :
                node.SelectSingleNode("m:Name", nsManager)?.InnerText;
            _label = isTemplate ? node.SelectSingleNode("Label")?.InnerText :
                node.SelectSingleNode("m:Label", nsManager)?.InnerText;
            _hideFromUi = hidden;

            var values = isTemplate ? node.SelectSingleNode("Values") :
                node.SelectSingleNode("m:Values", nsManager);
            if (values?.HasChildNodes ?? false)
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