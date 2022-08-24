using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IMitigationIdChanger))]
    internal class MitigationIdChangerAttribute : InstanceLevelAspect, IMitigationIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_mitigationId))]
        public Property<Guid> _mitigationId;
        #endregion

        public Guid GetMitigationId()
        {
            return _mitigationId?.Get() ?? Guid.Empty;
        }

        public void SetMitigationId(Guid newId)
        {
            _mitigationId?.Set(newId);
        }
    }

    [PSerializable]
    [IntroduceInterface(typeof(IStrengthIdChanger))]
    internal class StrengthIdChangerAttribute : InstanceLevelAspect, IStrengthIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_strengthId))]
        public Property<int> _strengthId;
        #endregion

        public int GetStrengthId()
        {
            return _strengthId?.Get() ?? 0;
        }

        public void SetStrengthId(int newId)
        {
            _strengthId?.Set(newId);
        }
    }
}
