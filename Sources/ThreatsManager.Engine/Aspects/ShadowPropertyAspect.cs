using System;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private Guid _originalId { get; set; }
    //private bool _overridden { get; set; }
    //private IProperty _original { get; set; }
    //#endregion    

    [PSerializable]
    public class ShadowPropertyAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("InvokeChanged", IsRequired = true)]
        public Action InvokeChanged;
        #endregion

        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("originalId")]
        public Guid _originalId { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 0, Visibility = Visibility.Private)]
        public IProperty _original { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("overridden")]
        public bool _overridden { get; set; }
        #endregion

        #region Implementation of interface IShadowProperty.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IProperty Original => _original ?? (_original = (Instance as IThreatModelChild)?.Model?.FindProperty(_originalId));

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public bool IsOverridden => _overridden;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public void RevertToOriginal()
        {
            if (_overridden)
            {
                _overridden = false;
                InvokeChanged();
            }
        }
        #endregion
    }
}
