using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IAssociatedIdChanger
    {
        Guid GetAssociatedId();
        void SetAssociatedId(Guid newId);
    }
}
