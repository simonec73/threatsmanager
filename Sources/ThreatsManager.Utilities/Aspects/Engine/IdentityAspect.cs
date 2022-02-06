using System;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //[JsonProperty("id")]
    //protected Guid _id { get; set; }
    //[JsonProperty("name")]
    //protected string _name { get; set; }
    //[JsonProperty("description")]
    //protected string _description { get; set; }
    //#endregion

    /// <summary>
    /// Attribute applied to automatically inject the code needed by classes implementing IIdentity.
    /// </summary>
    /// <remarks>PostSharp is required to make this attribute effective.</remarks>
    [PSerializable]
    public class IdentityAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_id))]
        public Property<Guid> _id;

        [ImportMember(nameof(_name))]
        public Property<string> _name;

        [ImportMember(nameof(_description))]
        public Property<string> _description;
        #endregion

        #region Implementation of interface IIdentity.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid Id => _id?.Get() ?? Guid.Empty;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,LinesOfCodeAvoided = 1)]
        public string Name 
        { 
            get => _name?.Get();
            set => _name?.Set(value); 
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public string Description
        {
            get => _description?.Get();
            set => _description?.Set(value);
        }
        #endregion
    }
}
