using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IThreatActorIdChanger))]
    internal class ThreatActorIdChangerAttribute : InstanceLevelAspect, IThreatActorIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_actorId))]
        public Property<Guid> _actorId;
        #endregion

        public Guid GetThreatActorId()
        {
            return _actorId?.Get() ?? Guid.Empty;
        }

        public void SetThreatActorId(Guid newId)
        {
            _actorId?.Set(newId);
        }
    }
}
