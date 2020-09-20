using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private List<ILink> _links { get; set; }
    //#endregion    

    [PSerializable]
    public class LinksContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("links")]
        public List<ILink> _links { get; set; }
        #endregion

        #region Implementation of interface ILinksContainer.
        private Action<ILinksContainer, ILink> _linkAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<ILinksContainer, ILink> LinkAdded
        {
            add
            {
                if (_linkAdded == null || !_linkAdded.GetInvocationList().Contains(value))
                {
                    _linkAdded += value;
                }
            }
            remove
            {
                _linkAdded -= value;
            }
        }

        private Action<ILinksContainer, IDataFlow> _linkRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
        public event Action<ILinksContainer, IDataFlow> LinkRemoved
        {
            add
            {
                if (_linkRemoved == null || !_linkRemoved.GetInvocationList().Contains(value))
                {
                    _linkRemoved += value;
                }
            }
            remove
            {
                _linkRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<ILink> Links => _links?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public ILink GetLink(Guid dataFlowId)
        {
            return _links?.FirstOrDefault(x => x.AssociatedId == dataFlowId);
        }
        
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 5)]
        public void Add(ILink link)
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            if (_links == null)
                _links = new List<ILink>();

            _links.Add(link);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 13)]
        public ILink AddLink(IDataFlow dataFlow)
        {
            if (dataFlow == null)
                throw new ArgumentNullException(nameof(dataFlow));

            ILink result = null;

            if (GetLink(dataFlow.Id) == null)
            {
                if (_links == null)
                    _links = new List<ILink>();
                result = new Link(dataFlow);
                _links.Add(result);
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
                if (Instance is ILinksContainer container)
                    _linkAdded?.Invoke(container, result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveLink(Guid dataFlowId)
        {
            bool result = false;

            var link = _links?.FirstOrDefault(x => x.AssociatedId == dataFlowId);
            if (link != null)
            {
                result = _links.Remove(link);
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (link.DataFlow is IDataFlow flow && Instance is ILinksContainer container)
                        _linkRemoved?.Invoke(container, flow);
                }
            }

            return result;
        }
        #endregion
    }
}