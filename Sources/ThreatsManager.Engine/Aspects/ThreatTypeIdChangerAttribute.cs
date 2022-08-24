using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IThreatTypeIdChanger))]
    internal class ThreatTypeIdChangerAttribute : InstanceLevelAspect, IThreatTypeIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_threatTypeId))]
        public Property<Guid> _threatTypeId;
        #endregion

        public Guid GetThreatTypeId()
        {
            return _threatTypeId?.Get() ?? Guid.Empty;
        }

        public void SetThreatTypeId(Guid newId)
        {
            _threatTypeId?.Set(newId);
        }
    }
}
