using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //[JsonProperty("modelId")]
    //protected Guid _modelId { get; set; }
    //[Parent] or [Reference]
    //[field: NotRecorded]
    //[field: UpdateThreatModelId]
    //[field: AutoApplySchemas]
    //protected IThreatModel _model { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatModelChildAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_modelId))]
        public Property<Guid> _modelId;

        [ImportMember(nameof(_model))]
        public Property<IThreatModel> _model;
        #endregion

        #region Implementation of interface IThreatModelChid.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public IThreatModel Model
        {
            get
            {
                IThreatModel result = _model?.Get();

                if (result == null)
                {
                    var id = _modelId?.Get() ?? Guid.Empty;
                    if (id != Guid.Empty)
                    { 
                        result = ThreatModelManager.Get(id);
                        if (result != null)
                            _model.Set(result);
                    }
                }

                return result;
            }
        }
        #endregion
    }
}
