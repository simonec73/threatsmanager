using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        private Action<IThreatEventsContainer, IThreatEvent> _threatEventAddedToDataFlow;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAddedToDataFlow
        {
            add
            {
                if (_threatEventAddedToDataFlow == null || !_threatEventAddedToDataFlow.GetInvocationList().Contains(value))
                {
                    _threatEventAddedToDataFlow += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_threatEventAddedToDataFlow != null) _threatEventAddedToDataFlow -= value;
            }
        }

        private Action<IThreatEventsContainer, IThreatEvent> _threatEventRemovedFromDataFlow;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemovedFromDataFlow
        {
            add
            {
                if (_threatEventRemovedFromDataFlow == null || !_threatEventRemovedFromDataFlow.GetInvocationList().Contains(value))
                {
                    _threatEventRemovedFromDataFlow += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_threatEventRemovedFromDataFlow != null) _threatEventRemovedFromDataFlow -= value;
            }
        }

        [Child]
        [JsonProperty("dataFlows")]
        private IList<IDataFlow> _flows;

        [IgnoreAutoChangeNotification]
        public IEnumerable<IDataFlow> DataFlows => _flows?.AsEnumerable();

        [InitializationRequired]
        public IDataFlow GetDataFlow(Guid id)
        {
            return _flows?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IDataFlow GetDataFlow(Guid sourceId, Guid targetId)
        {
            return _flows?.FirstOrDefault(x => (x.SourceId == sourceId) && (x.TargetId == targetId));
        }

        [InitializationRequired]
        public void Add(IDataFlow dataFlow)
        {
            if (dataFlow is IThreatModelChild child && child.Model != this)
                throw new ArgumentException();

            using (UndoRedoManager.OpenScope("Add Data Flow"))
            {
                if (_flows == null)
                    _flows = new AdvisableCollection<IDataFlow>();

                _flows.Add(dataFlow);
                UndoRedoManager.Attach(dataFlow);
            }
        }

        [InitializationRequired]
        public IDataFlow AddDataFlow([Required] string name, Guid sourceId, Guid targetId)
        {
            IDataFlow result = null;

            if (!(_flows?.Any(x => (x.SourceId == sourceId) && (x.TargetId == targetId)) ?? false))
            {
                result = new DataFlow(name, sourceId, targetId);
                Add(result);
                RegisterEvents(result);
                ChildCreated?.Invoke(result);
            }

            return result;
        }

        [InitializationRequired]
        public IDataFlow AddDataFlow([Required] string name, Guid sourceId, Guid targetId, IFlowTemplate template)
        {
            IDataFlow result = new DataFlow(name, sourceId, targetId)
            {
                _templateId = template?.Id ?? Guid.Empty
            };

            Add(result);
            RegisterEvents(result);
            ChildCreated?.Invoke(result);

            return result;
        }

        private void OnThreatEventRemovedFromDataFlow([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            _threatEventRemovedFromDataFlow?.Invoke(container, threatEvent);
        }

        private void OnThreatEventAddedToDataFlow([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            _threatEventAddedToDataFlow?.Invoke(container, threatEvent);
        }

        public bool RemoveDataFlow(Guid id)
        {
            bool result = false;

            var flow = GetDataFlow(id);

            if (flow != null)
            {
                using (UndoRedoManager.OpenScope("Remove Data Flow"))
                {
                    RemoveRelated(flow);
                    flow.ThreatEventAdded += OnThreatEventAddedToDataFlow;
                    flow.ThreatEventRemoved += OnThreatEventRemovedFromDataFlow;

                    result = _flows.Remove(flow);
                    if (result)
                    {
                        UndoRedoManager.Detach(flow);
                        UnregisterEvents(flow);
                        ChildRemoved?.Invoke(flow);
                    }
                }
            }

            return result;
        }

        private void RemoveRelated([NotNull] IDataFlow flow)
        {
            var diagrams = _diagrams?.ToArray();
            if (diagrams != null)
            {
                foreach (var diagram in diagrams)
                {
                    diagram.RemoveLink(flow.Id);
                }
            }
        }
    }
}