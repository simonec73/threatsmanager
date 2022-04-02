using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[JsonProperty("id")]
    //protected Guid _id { get; set; }
    //[JsonProperty("propertyTypeId")]
    //protected Guid _propertyTypeId { get; set; }
    //[JsonProperty("readOnly")]
    //protected bool _readOnly { get; set; }
    //#endregion

    [PSerializable]
    public class PropertyAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_id))]
        public Property<Guid> _id;

        [ImportMember(nameof(_propertyTypeId))]
        public Property<Guid> _propertyTypeId;

        [ImportMember(nameof(_readOnly))]
        public Property<bool> _readOnly;
        #endregion

        #region Implementation of interface IProperty.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public IPropertyType PropertyType
        {
            get 
            {
                IPropertyType result = null;

                var propertyTypeId = _propertyTypeId?.Get() ?? Guid.Empty;
                var model = (Instance as IThreatModelChild)?.Model;
                if (propertyTypeId != Guid.Empty && model != null)
                {
                    result = model.GetPropertyType(propertyTypeId);
                }

                return result;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid Id => _id?.Get() ?? Guid.Empty;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public bool ReadOnly
        {
            get
            {
                return _readOnly?.Get() ?? false;
            }

            set
            {
                _readOnly?.Set(value);
            }
        }
        #endregion
    }
}
