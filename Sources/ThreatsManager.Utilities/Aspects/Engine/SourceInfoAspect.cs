using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Serialization;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities.Aspects.Engine
{
    //#region Additional placeholders required.
    //[JsonProperty("sourceTMId")]
    //protected Guid _sourceTMId { get; set; }
    //[JsonProperty("sourceTMName")]
    //protected string _sourceTMName { get; set; }
    //[JsonProperty("versionId")]
    //protected string _versionId { get; set; }
    //[JsonProperty("versionAuthor")]
    //protected string _versionAuthor { get; set; }
    //#endregion

    /// <summary>
    /// Attribute applied to automatically inject the code needed by classes implementing ISourceInfo.
    /// </summary>
    /// <remarks>PostSharp is required to make this attribute effective.</remarks>
    [PSerializable]
    public class SourceInfoAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        /// <summary>
        /// Import for member _sourceTMId from the associated object.
        /// </summary>
        [ImportMember(nameof(_sourceTMId))]
        public Property<Guid> _sourceTMId;

        /// <summary>
        /// Import for member _sourceTMName from the associated object.
        /// </summary>
        [ImportMember(nameof(_sourceTMName))]
        public Property<string> _sourceTMName;

        /// <summary>
        /// Import for member _versionId from the associated object.
        /// </summary>
        [ImportMember(nameof(_versionId))]
        public Property<string> _versionId;

        /// <summary>
        /// Import for member _versionAuthor from the associated object.
        /// </summary>
        [ImportMember(nameof(_versionAuthor))]
        public Property<string> _versionAuthor;
        #endregion

        #region Implementation of interface ISourceInfo.
        /// <summary>
        /// Implementation of the SourceTMId getter.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public Guid SourceTMId => _sourceTMId?.Get() ?? Guid.Empty;

        /// <summary>
        /// Implementation of the SourceTMName getter.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public string SourceTMName => _sourceTMName?.Get();

        /// <summary>
        /// Implementation of the VersionId getter.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public string VersionId => _versionId?.Get();

        /// <summary>
        /// Implementation of the VersionAuthor getter.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public string VersionAuthor => _versionAuthor?.Get();

        /// <summary>
        /// Implementation of method SetSourceInfo.
        /// </summary>
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public void SetSourceInfo(IThreatModel source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (var scope = UndoRedoManager.OpenScope("Set Source Info"))
            {
                _sourceTMId?.Set(source.Id);
                _sourceTMName?.Set(source.Name);
                var current = source.CurrentVersion;
                if (current != null)
                {
                    _versionId?.Set(current.VersionId);
                    _versionAuthor?.Set(current.VersionAuthor);
                }
                scope?.Complete();
            }
        }
        #endregion
    }
}
