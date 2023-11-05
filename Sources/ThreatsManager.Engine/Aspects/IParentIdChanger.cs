using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IParentIdChanger
    {
        Guid GetParentId();
        void SetParentId(Guid newId);
    }
}
