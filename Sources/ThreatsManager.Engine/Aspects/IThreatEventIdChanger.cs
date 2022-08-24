using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IThreatEventIdChanger
    {
        Guid GetThreatEventId();
        void SetThreatEventId(Guid newId);
    }
}
