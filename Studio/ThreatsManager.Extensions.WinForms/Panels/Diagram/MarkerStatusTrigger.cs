using System;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public static class MarkerStatusTrigger
    {
        static MarkerStatusTrigger()
        {
            CurrentStatus = MarkerStatus.Full;
        }

        public static MarkerStatus CurrentStatus { get; private set; }

        public static event Action<MarkerStatus> MarkerStatusUpdated;

        public static void RaiseMarkerStatusUpdated(MarkerStatus status)
        {
            CurrentStatus = status;
            MarkerStatusUpdated?.Invoke(status);
        }
    }
}