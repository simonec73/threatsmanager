using System;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[JsonProperty("schema")]
    //protected Guid _schemaId { get; set; }
    //#endregion    

    [PSerializable]
    public class PropertyTypeAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_schemaId))]
        public Property<Guid> _schemaId;
        #endregion

        #region Implementation of interface IPropertyType.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid SchemaId => _schemaId?.Get() ?? Guid.Empty;

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
