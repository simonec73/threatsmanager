using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IWeaknessIdChanger))]
    internal class WeaknessIdChangerAttribute : InstanceLevelAspect, IWeaknessIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_weaknessId))]
        public Property<Guid> _weaknessId;
        #endregion

        public Guid GetWeaknessId()
        {
            return _weaknessId?.Get() ?? Guid.Empty;
        }

        public void SetWeaknessId(Guid newId)
        {
            _weaknessId?.Set(newId);
        }
    }
}
