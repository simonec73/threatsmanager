using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.AutoGenRules.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class ExitTrustBoundaryRuleNode : SelectionRuleNode
    {
        public ExitTrustBoundaryRuleNode()
        {
        }

        public ExitTrustBoundaryRuleNode([Required] string name, bool value)
        {
            Name = name;
            Value = value;
        }

        [JsonProperty("value")]
        public bool Value { get; set; }

        public override bool Evaluate([NotNull] object context)
        {
            bool result = false;

            if (context is IDataFlow flow)
            {
                result = InternalEvaluate(flow);
            }

            return result;
        }

        private bool InternalEvaluate([NotNull] IIdentity identity)
        {
            return (GetExitingTrustBoundaries(identity)?.Any() ?? false) == Value;
        }

        public override string ToString()
        {
            return Value ? "Flow exits a Trust Boundary" : "Flow does not exits any Trust Boundary";
        }
    }
}
