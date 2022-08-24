using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IParentIdChanger))]
    internal class ParentIdChangerAttribute : InstanceLevelAspect, IParentIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_parentId))]
        public Property<Guid> _parentId;
        #endregion

        public Guid GetParentId()
        {
            return _parentId?.Get() ?? Guid.Empty;
        }

        public void SetParentId(Guid newId)
        {
            _parentId?.Set(newId);
        }
    }
}
