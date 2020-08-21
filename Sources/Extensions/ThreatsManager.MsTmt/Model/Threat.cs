using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using PostSharp.Patterns.Contracts;
using ThreatsManager.MsTmt.Properties;

namespace ThreatsManager.MsTmt.Model
{
#pragma warning disable CS0067
    public class Threat
    {
        private class ParameterInfo
        {
            private readonly XmlNode _node;

            public ParameterInfo([NotNull] ThreatModel model, [NotNull] XmlNode node)
            {
                _node = node;
                Name = node.ChildNodes[0].InnerText;
                Label = model.Properties.FirstOrDefault(x => string.CompareOrdinal(x.Name, Name) == 0)?.Label;
                Value = node.ChildNodes[1].InnerText;
            }

            public string Name { get; private set; }

            public string Label { get; private set; }

            public string Value { get; private set; }
        }

        #region Member Variables.
        private readonly ThreatModel _model;

        private readonly string _key;
        private readonly XmlNode _node;
        private XmlNode _changedByNode;
        private Guid _drawingSurfaceGuid;
        private Guid _sourceGuid;
        private Guid _targetGuid;
        private Guid _flowGuid;
        private XmlNode _modifiedAtNode;
        private readonly Dictionary<string, ParameterInfo> _parameters = new Dictionary<string, ParameterInfo>();
        private string _typeId;
        private XmlNode _stateNode;
        private bool _customThreatType;
        #endregion

        #region Constructor.
        public Threat([NotNull] ThreatModel model, [NotNull] XmlNode node)
        {
            _model = model;
            _node = node;
            _key = node.ChildNodes[0].InnerText;
            AnalyzeValue(node.ChildNodes[1]);
        }
        #endregion

        #region Events.
        public static event Action<Threat, string, string> Changed;
        public static event Action<Threat> Removed;
        public static event Action<Threat> Added;
        #endregion

        #region Properties.
        public string Key => _key;

        public string ChangedBy
        {
            get => _changedByNode.InnerText;
            private set
            {
                _changedByNode.InnerText = value;

                var attribute = _changedByNode.Attributes?.OfType<XmlAttribute>()
                    .FirstOrDefault(x => string.CompareOrdinal(x.LocalName, "nil") == 0);
                if (attribute != null)
                {
                    _changedByNode.Attributes.Remove(attribute);
                }
            }
        }

        public Guid PageGuid => _drawingSurfaceGuid;

        public Guid SourceGuid => _sourceGuid;

        public Guid TargetGuid => _targetGuid;

        public Guid FlowGuid => _flowGuid;

        public DateTime ModifiedAt
        {
            get => DateTime.Parse(_modifiedAtNode.InnerText);
            private set => _modifiedAtNode.InnerText = value.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public ThreatState State
        {
            get => (ThreatState)Enum.Parse(typeof(ThreatState), _stateNode.InnerText);
            private set => _stateNode.InnerText = value.ToString();
        }

        public IEnumerable<string> ParameterNames => _parameters.Keys;

        public IEnumerable<string> ParameterLabels => _parameters.Values.Select(x => x.Label);

        public XmlNode Node => _node;
        #endregion

        #region Public methods.
        public string GetValue([Required] string name)
        {
            if (_parameters.ContainsKey(name))
                return _parameters[name].Value;
            else
                return null;
        }

        public string GetValueFromLabel([Required] string label)
        {
            return _parameters.Values.FirstOrDefault(x => string.CompareOrdinal(x.Label, label) == 0)?.Value;
        }

        public string TypeId => _customThreatType ? Guid.Empty.ToString() : _typeId;

        public override string ToString()
        {
            return GetValue(Resources.ThreatTitle);
        }
        #endregion

        #region Private methods.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void AnalyzeValue([NotNull] XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.LocalName)
                {
                    case "ChangedBy":
                        _changedByNode = node;
                        break;
                    case "DrawingSurfaceGuid":
                        Guid.TryParse(node.InnerText, out _drawingSurfaceGuid);
                        break;
                    case "FlowGuid":
                        Guid.TryParse(node.InnerText, out _flowGuid);
                        break;
                    case "SourceGuid":
                        Guid.TryParse(node.InnerText, out _sourceGuid);
                        break;
                    case "TargetGuid":
                        Guid.TryParse(node.InnerText, out _targetGuid);
                        break;
                    case "ModifiedAt":
                        _modifiedAtNode = node;
                        break;
                    case "Properties":
                        AnalyzeProperties(node);
                        break;
                    case "TypeId":
                        _typeId = node.InnerText;
                        break;
                    case "State":
                        _stateNode = node;
                        break;
                }
            }
        }

        private void AnalyzeProperties(XmlNode node)
        {
            var properties = _model.Properties; // Proprietà che occorre garantire.
            var children = node.ChildNodes;     // Nodo con le proprietà esistenti.
            int countProperties = properties.Count();
            int countChildren = children.Count;
            int currentPropertyIndex = 0;   // Puntatore alla proprietà da garantire.
            int currentChildIndex = 0;      // Puntatore al nodo figlio corrente.

            PropertyDefinition currentProperty = properties.ElementAt(currentPropertyIndex);
            if (currentProperty != null && currentChildIndex < countChildren)
            {
                XmlNode currentChild = children[currentChildIndex];
                var currentParameterInfo = new ParameterInfo(_model, currentChild);

                while (currentPropertyIndex < countProperties)
                {
                    if (string.CompareOrdinal(currentProperty.Name, currentParameterInfo.Name) == 0)
                    {
                        // La proprietà corrente corrisponde al nodo corrente.
                        _parameters.Add(currentParameterInfo.Name, currentParameterInfo);

                        currentPropertyIndex++; // Passiamo alla prossima proprietà.
                        if (currentPropertyIndex == countProperties)
                            break;
                        currentChildIndex++; // Passiamo al prossimo nodo.
                        if (currentChildIndex == countChildren)
                            break;
                        currentProperty = properties.ElementAt(currentPropertyIndex);
                        currentChild = children[currentChildIndex];
                        currentParameterInfo = new ParameterInfo(_model, currentChild);
                    }
                    else
                    {
                        do
                        {
                            // La proprietà non è presente: occorre aggiungerla.
                            var newChild = children[0].CloneNode(true);
                            newChild.ChildNodes[0].InnerText = currentProperty.Name;
                            newChild.ChildNodes[1].InnerText = string.Empty;
                            var parameterInfo = new ParameterInfo(_model, newChild);

                            // La nuova proprietà va aggiunta prima del prossimo nodo.
                            node.InsertBefore(newChild, currentChild);
                            _parameters.Add(parameterInfo.Name, parameterInfo);

                            // A questo punto, occorre spostare sia il contatore delle Proprietà sia quello dei nodi,
                            //  anche se lo spostamento del contatore dei nodi non comporta l'individuazione di un altro nodo.
                            currentPropertyIndex++;
                            if (currentPropertyIndex == countProperties)
                                break;
                            currentProperty = properties.ElementAt(currentPropertyIndex);
                            currentChildIndex++;
                            countChildren++; // Abbiamo aggiunto un nodo.
                        } while (string.CompareOrdinal(currentProperty.Name, currentParameterInfo.Name) != 0);
                    }
                }
            }
        }
        #endregion
    }
}
