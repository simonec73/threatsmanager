using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using System;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(IAssociatedIdChanger))]
    internal class AssociatedIdChangerAttribute : InstanceLevelAspect, IAssociatedIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_associatedId))]
        public Property<Guid> _associatedId;
        #endregion

        public Guid GetAssociatedId()
        {
            return _associatedId?.Get() ?? Guid.Empty;
        }

        public void SetAssociatedId(Guid newId)
        {
            _associatedId?.Set(newId);
        }
    }
}
