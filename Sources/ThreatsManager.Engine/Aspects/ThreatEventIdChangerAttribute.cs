using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IThreatEventIdChanger))]
    internal class ThreatEventIdChangerAttribute : InstanceLevelAspect, IThreatEventIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_threatEventId))]
        public Property<Guid> _threatEventId;
        #endregion

        public Guid GetThreatEventId()
        {
            return _threatEventId?.Get() ?? Guid.Empty;
        }

        public void SetThreatEventId(Guid newId)
        {
            _threatEventId?.Set(newId);
        }
    }
}
