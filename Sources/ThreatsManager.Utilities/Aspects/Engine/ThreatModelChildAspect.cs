using System;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //protected Guid _modelId { get; set; }
    //protected IThreatModel _model { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatModelChildAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Family)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("modelId")]
        public Guid _modelId { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrIgnore, 
            LinesOfCodeAvoided = 0, Visibility = Visibility.Family)]
        public IThreatModel _model { get; set; }
        #endregion

        #region Implementation of interface IThreatModelChid.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatModel Model => _model ?? (_model = ThreatModelManager.Get(_modelId));
        #endregion
    }
}
