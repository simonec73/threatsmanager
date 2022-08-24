using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IThreatModelIdChanger
    {
        Guid GetThreatModelId();
        void SetThreatModelId(Guid newId);
    }
}
