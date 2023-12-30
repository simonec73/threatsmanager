using System;
using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Policies
{
    public class MaxExecutionModePolicy : Policy
    {
        protected override string PolicyName => "MaxExecutionMode";

        public ExecutionMode? MaxExecutionMode
        {
            get
            {
                ExecutionMode? result = null;

                var fromRegistry = Value as string;
                if (fromRegistry != null && Enum.TryParse<ExecutionMode>(fromRegistry, out var maxExecutionMode))
                {
                    result = maxExecutionMode;
                }

                return result;
            }
        }
    }
}
