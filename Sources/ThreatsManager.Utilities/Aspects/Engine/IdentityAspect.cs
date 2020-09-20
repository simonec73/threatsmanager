using System;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //protected Guid _id { get; set; }
    //#endregion

    /// <summary>
    /// Attribute applied to automatically inject the code needed by classes implementing IIdentity.
    /// </summary>
    /// <remarks>PostSharp is required to make this attribute effective.</remarks>
    [PSerializable]
    public class IdentityAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Family)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("id")]
        public Guid _id { get; set; }
        #endregion

        #region Implementation of interface IIdentity.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid Id => _id;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail,LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("description")]
        public string Description { get; set; }
        #endregion
    }
}
