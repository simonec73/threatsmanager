using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private Guid _threatEventId { get; set; }
    //private IThreatEvent _threatEvent { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatEventChildAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("threatEventId")]
        public Guid _threatEventId { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 0, Visibility = Visibility.Private)]
        public IThreatEvent _threatEvent { get; set; }
        #endregion

        #region Implementation of interface IThreatEventChild.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 4)]
        public IThreatEvent ThreatEvent
        {
            get
            {
                var model = (Instance as IThreatModelChild)?.Model;

                if (_threatEvent == null)
                {
                    _threatEvent = model?.FindThreatEvent(_threatEventId);
                }

                return _threatEvent;
            }
        }
        #endregion
    }
}
