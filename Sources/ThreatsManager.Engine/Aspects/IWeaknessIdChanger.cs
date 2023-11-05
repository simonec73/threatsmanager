using System;

namespace ThreatsManager.Engine.Aspects
{
    internal interface IWeaknessIdChanger
    {
        Guid GetWeaknessId();
        void SetWeaknessId(Guid newId);
    }
}
