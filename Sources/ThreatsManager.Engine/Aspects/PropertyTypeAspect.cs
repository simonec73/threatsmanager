using System;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //protected Guid _schemaId { get; set; }
    //#endregion    

    [PSerializable]
    public class PropertyTypeAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Family)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("schema")]
        public Guid _schemaId { get; set; }
        #endregion

        #region Implementation of interface IPropertyType.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid SchemaId => _schemaId;

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("priority")]
        public int Priority { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("doNotPrint")]
        public bool DoNotPrint { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute),
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("readOnly")]
        public bool ReadOnly { get; set; }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("propertyViewer")]
        public string CustomPropertyViewer { get; set; }
        #endregion
    }
}
