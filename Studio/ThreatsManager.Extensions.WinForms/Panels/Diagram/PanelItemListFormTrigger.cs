using Northwoods.Go;
using System;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public static class PanelItemListFormTrigger
    {
        static PanelItemListFormTrigger()
        {
            CurrentStatus = PanelsStatus.Normal;
        }

        public static PanelsStatus CurrentStatus { get; private set; }

        public static event Action<PanelsStatus, GoView> PanelStatusUpdated;

        public static void RaiseShowPanels(PanelsStatus status, GoView view)
        {
            CurrentStatus = status;
            PanelStatusUpdated?.Invoke(status, view);
        }
    }
}