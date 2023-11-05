using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IThreatTypeIdChanger
    {
        Guid GetThreatTypeId();
        void SetThreatTypeId(Guid newId);
    }
}
