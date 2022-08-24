using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;

namespace ThreatsManager.Engine.Aspects
{
    [PSerializable]
    [IntroduceInterface(typeof(ISeverityIdChanger))]
    internal class SeverityIdChangerAttribute : InstanceLevelAspect, ISeverityIdChanger
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_severityId))]
        public Property<int> _severityId;
        #endregion

        public int GetSeverityId()
        {
            return _severityId?.Get() ?? 0;
        }

        public void SetSeverityId(int newId)
        {
            _severityId?.Set(newId);
        }
    }
}
