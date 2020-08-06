using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.Entities;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [JsonProperty("flowTemplates")]
        private List<IFlowTemplate> _flowTemplates;

        public IEnumerable<IFlowTemplate> FlowTemplates => _flowTemplates?.AsReadOnly();

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

            if (_flowTemplates == null)
                _flowTemplates = new List<IFlowTemplate>();

            _flowTemplates.Add(flowTemplate);
 
            SetDirty();
            ChildCreated?.Invoke(flowTemplate);
        }

        [InitializationRequired]
        public IFlowTemplate AddFlowTemplate([Required] string name, string description, IDataFlow source = null)
        {
            var result = new FlowTemplate(this, name)
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
                result = _flowTemplates.Remove(template);
                if (result)
                {
                    SetDirty();
                    ChildRemoved?.Invoke(template);
                }
            }

            return result;
        }
    }
}