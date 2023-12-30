using ThreatsManager.Utilities;

namespace ThreatsManager.Policies
{
    internal class TelemetryPolicy : Policy
    {
        protected override string PolicyName => "Telemetry";

        public bool? Telemetry => BoolValue;
    }
}
