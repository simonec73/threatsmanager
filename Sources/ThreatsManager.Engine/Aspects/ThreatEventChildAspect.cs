using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[JsonProperty("threatEventId")]
    //private Guid _threatEventId { get; set; }
    //[Reference]
    //[field: NotRecorded]
    //[field: UpdateId("Id", "_threatEventId")]
    //private IThreatEvent _threatEvent { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatEventChildAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_threatEventId))]
        public Property<Guid> _threatEventId;

        [ImportMember(nameof(_threatEvent))]
        public Property<IThreatEvent> _threatEvent;
        #endregion

        #region Implementation of interface IThreatEventChild.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public IThreatEvent ThreatEvent
        {
            get
            {
                var model = (Instance as IThreatModelChild)?.Model;

                var threatEvent = _threatEvent?.Get();
                if (threatEvent == null)
                {
                    var threatEventId =  _threatEventId?.Get() ?? Guid.Empty;
                    if (threatEventId != Guid.Empty)
                    { 
                        threatEvent = model?.FindThreatEvent(threatEventId);
                        if (threatEvent != null)
                            _threatEvent?.Set(threatEvent);
                    }
                }

                return threatEvent;
            }
        }
        #endregion
    }
}
