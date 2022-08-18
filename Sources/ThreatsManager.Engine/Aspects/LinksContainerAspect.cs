﻿using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("links")]
    //private IList<ILink> _links { get; set; }
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(NotifyPropertyChangedAttribute))]
    public class LinksContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_links))]
        public Property<IList<ILink>> _links;
        #endregion

        #region Implementation of interface ILinksContainer.
        private Action<ILinksContainer, ILink> _linkAdded;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "LinkAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnLinkAddedAdd(EventInterceptionArgs args)
        {
            if (_linkAdded == null || !_linkAdded.GetInvocationList().Contains(args.Handler))
            {
                _linkAdded += (Action<ILinksContainer, ILink>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnLinkAddedAdd))]
        public void OnLinkAddedRemove(EventInterceptionArgs args)
        {
            _linkAdded -= (Action<ILinksContainer, ILink>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<ILinksContainer, IDataFlow> _linkRemoved;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "LinkRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnLinkRemovedAdd(EventInterceptionArgs args)
        {
            if (_linkRemoved == null || !_linkRemoved.GetInvocationList().Contains(args.Handler))
            {
                _linkRemoved += (Action<ILinksContainer, IDataFlow>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnLinkRemovedAdd))]
        public void OnLinkRemovedRemove(EventInterceptionArgs args)
        {
            _linkRemoved -= (Action<ILinksContainer, IDataFlow>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<ILink> Links => _links?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public ILink GetLink(Guid dataFlowId)
        {
            return _links?.Get()?.FirstOrDefault(x => x.AssociatedId == dataFlowId);
        }
        
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void Add(ILink link)
        {
            if (link == null)
                throw new ArgumentNullException(nameof(link));

            using (UndoRedoManager.OpenScope("Add link for Flow"))
            {
                var links = _links?.Get();
                if (links == null)
                {
                    links = new AdvisableCollection<ILink>();
                    _links?.Set(links);
                }

                UndoRedoManager.Attach(link);
                if (link is IUndoable undoable)
                    undoable.IsUndoEnabled = true;
                links.Add(link);
                if (Instance is ILinksContainer container)
                {
                    _linkAdded?.Invoke(container, link);
                }
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public ILink AddLink(IDataFlow dataFlow)
        {
            if (dataFlow == null)
                throw new ArgumentNullException(nameof(dataFlow));

            ILink result = null;

            if (GetLink(dataFlow.Id) == null)
            {
                result = new Link(dataFlow);

                Add(result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public bool RemoveLink(Guid dataFlowId)
        {
            bool result;

            var link = GetLink(dataFlowId);

            using (UndoRedoManager.OpenScope("Remove link for Flow"))
            {
                result = _links?.Get()?.Remove(link) ?? false;
                if (result)
                {
                    UndoRedoManager.Detach(link);
                    if (link is IUndoable undoable)
                        undoable.IsUndoEnabled = false;
                    if (link.DataFlow is IDataFlow flow && Instance is ILinksContainer container)
                        _linkRemoved?.Invoke(container, flow);
                }
            }

            return result;
        }
        #endregion
    }
}