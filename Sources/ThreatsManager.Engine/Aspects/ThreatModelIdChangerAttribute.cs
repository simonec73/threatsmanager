using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IThreatModelIdChanger))]
    internal class ThreatModelIdChangerAttribute : InstanceLevelAspect, IThreatModelIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_modelId))]
        public Property<Guid> _modelId;
        #endregion

        public Guid GetThreatModelId()
        {
            return _modelId?.Get() ?? Guid.Empty;
        }

        public void SetThreatModelId(Guid newId)
        {
            _modelId?.Set(newId);
        }
    }
}
