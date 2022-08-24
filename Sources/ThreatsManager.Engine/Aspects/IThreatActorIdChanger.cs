using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IThreatActorIdChanger
    {
        Guid GetThreatActorId();
        void SetThreatActorId(Guid newId);
    }
}
