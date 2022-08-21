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
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("flowTemplates")]
        private IList<IFlowTemplate> _flowTemplates;

        [IgnoreAutoChangeNotification]
        public IEnumerable<IFlowTemplate> FlowTemplates => _flowTemplates?.AsEnumerable();

        [InitializationRequired]
        public IFlowTemplate GetFlowTemplate(Guid id)
        {
            return _flowTemplates?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public void Add([NotNull] IFlowTemplate flowTemplate)
        {
            if (flowTemplate is IThreatModelChild child && child.Model != this)
                throw new ArgumentException();

            using (var scope = UndoRedoManager.OpenScope("Add Flow Tempalte"))
            {
                if (_flowTemplates == null)
                    _flowTemplates = new AdvisableCollection<IFlowTemplate>();

                _flowTemplates.Add(flowTemplate);
                UndoRedoManager.Attach(flowTemplate);
                scope.Complete();

                ChildCreated?.Invoke(flowTemplate);
            }
        }

        [InitializationRequired]
        public IFlowTemplate AddFlowTemplate([Required] string name, string description, IDataFlow source = null)
        {
            var result = new FlowTemplate(name)
            {
                Description = description,
                FlowType = source?.FlowType ?? FlowType.ReadWriteCommand
            };
            source.CloneProperties(result);
            Add(result);

            return result;
        }

        [InitializationRequired(false)]
        public bool RemoveFlowTemplate(Guid id)
        {
            bool result = false;

            var template = GetFlowTemplate(id);

            if (template != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Flow Template"))
                {
                    result = _flowTemplates.Remove(template);
                    if (result)
                    {
                        UndoRedoManager.Detach(template);
                        scope.Complete();

                        ChildRemoved?.Invoke(template);
                    }
                }
            }

            return result;
        }
    }
}