using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IMitigationIdChanger
    {
        Guid GetMitigationId();
        void SetMitigationId(Guid newId);
    }
}
