using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[JsonProperty("originalId")]
    //private Guid _originalId { get; set; }
    //[JsonProperty("overridden")]
    //private bool _overridden { get; set; }
    //[Reference]
    //[field: NonSerialized]
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
        [ImportMember(nameof(_originalId))]
        public Property<Guid> _originalId;

        [ImportMember(nameof(_originalId))]
        public Property<IProperty> _original;

        [ImportMember(nameof(_overridden))]
        public Property<bool> _overridden;
        #endregion

        #region Implementation of interface IShadowProperty.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 8)]
        public IProperty Original
        {
            get
            {
                IProperty result = _original?.Get();
                if (result == null)
                {
                    var originalId = _originalId?.Get() ?? Guid.Empty;
                    if (originalId != Guid.Empty)
                    {
                        result = (Instance as IThreatModelChild)?.Model?.FindProperty(originalId);
                        if (result != null)
                            _original?.Set(result);
                    }
                }

                return result;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public bool IsOverridden => _overridden?.Get() ?? false;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public void RevertToOriginal()
        {
            if (IsOverridden)
            {
                _overridden?.Set(false);
                InvokeChanged();
            }
        }
        #endregion
    }
}
