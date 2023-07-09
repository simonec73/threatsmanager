using Newtonsoft.Json;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using System;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public abstract class UnaryRuleNode : SelectionRuleNode
    {
        [JsonProperty("child", TypeNameHandling = TypeNameHandling.Objects)]
        [Child]
        private SelectionRuleNode _child { get; set; }

        [property: NotRecorded]
        public SelectionRuleNode Child
        {
            get => _child;

            set
            {
                _child = value;
                if (_child != null)
                    _child.ModelId = ModelId;
            }
        }

        public override Guid ModelId 
        { 
            get => base.ModelId; 
            set
            {
                base.ModelId = value;
                if (_child != null) _child.ModelId = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
