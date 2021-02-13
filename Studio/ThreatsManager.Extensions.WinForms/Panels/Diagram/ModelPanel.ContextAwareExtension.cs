using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public partial class ModelPanel
    {
        private IEnumerable<IContextAwareAction> _actions;

        public Scope SupportedScopes => Scope.All;

        
        public event Action<IPanelFactory, IIdentity> OpenPanel;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            _actions = actions.ToArray();
            _graph.SetContextAwareActions(_actions);
            ThreatEventListForm.SetActions(actions.Where(x => x.Scope.HasFlag(Scope.ThreatEvent)));

            foreach (var action in _actions)
            {
                if (action is IIdentityAddingRequiredAction identityAddingRequiredAction)
                    identityAddingRequiredAction.IdentityAddingRequired += AddIdentity;

                if (action is IRefreshGroupBorderRequiredAction refreshGroupBorderRequiredAction)
                    refreshGroupBorderRequiredAction.RefreshGroupBorderRequired += RefreshGroupBorder;

                if (action is IEntityGroupRemovingRequiredAction entityGroupRemovingRequiredAction)
                    entityGroupRemovingRequiredAction.EntityGroupRemovingRequired += RemoveEntityGroup;

                if (action is IDataFlowAddingRequiredAction dataFlowAddingRequiredAction)
                {
                    dataFlowAddingRequiredAction.DataFlowAddingRequired += AddDataFlow;
                }

                if (action is IDataFlowRemovingRequiredAction dataFlowRemovingRequiredAction)
                    dataFlowRemovingRequiredAction.DataFlowRemovingRequired += RemoveDataFlow;

                if (action is ICommandsBarContextAwareAction commandsBarContextAwareAction)
                {
                    var commandsBar = commandsBarContextAwareAction.CommandsBar;
                    if (commandsBar != null)
                    {
                        if (_commandsBarContextAwareActions == null)
                            _commandsBarContextAwareActions = new Dictionary<string, List<ICommandsBarDefinition>>();
                        List<ICommandsBarDefinition> list;
                        if (_commandsBarContextAwareActions.ContainsKey(commandsBar.Name))
                            list = _commandsBarContextAwareActions[commandsBar.Name];
                        else
                        {
                            list = new List<ICommandsBarDefinition>();
                            _commandsBarContextAwareActions.Add(commandsBar.Name, list);
                        }

                        list.Add(commandsBar);
                    }
                }

                if (action is IPanelOpenerExtension panelCreationRequired)
                    panelCreationRequired.OpenPanel += CreatePanel;

                if (action is IRemoveIdentityFromModelRequiredAction removeIdentityFromModelRequiredAction)
                    removeIdentityFromModelRequiredAction.IdentityRemovingRequired += RemoveIdentityFromModel;
            }
        }

        public void RemoveActionEvents()
        {
            if (_actions?.Any() ?? false)
            {
                foreach (var action in _actions)
                {
                    if (action is IIdentityAddingRequiredAction identityAddingRequiredAction)
                        identityAddingRequiredAction.IdentityAddingRequired -= AddIdentity;

                    if (action is IRefreshGroupBorderRequiredAction refreshGroupBorderRequiredAction)
                        refreshGroupBorderRequiredAction.RefreshGroupBorderRequired -= RefreshGroupBorder;

                    if (action is IEntityGroupRemovingRequiredAction entityGroupRemovingRequiredAction)
                        entityGroupRemovingRequiredAction.EntityGroupRemovingRequired -= RemoveEntityGroup;

                    if (action is IDataFlowAddingRequiredAction dataFlowAddingRequiredAction)
                        dataFlowAddingRequiredAction.DataFlowAddingRequired -= AddDataFlow;

                    if (action is IDataFlowRemovingRequiredAction dataFlowRemovingRequiredAction)
                        dataFlowRemovingRequiredAction.DataFlowRemovingRequired -= RemoveDataFlow;

                    if (action is IPanelOpenerExtension panelCreationRequired)
                        panelCreationRequired.OpenPanel -= CreatePanel;

                    if (action is IRemoveIdentityFromModelRequiredAction removeIdentityFromModelRequiredAction)
                        removeIdentityFromModelRequiredAction.IdentityRemovingRequired -= RemoveIdentityFromModel;
                }
            }
        }

        [InitializationRequired]
        private void RemoveIdentityFromModel([NotNull] IIdentity identity)
        {
            if (identity is IEntity entity)
            {
                _diagram.Model?.RemoveEntity(entity.Id);
            }

            if (identity is IGroup group)
            {
                _diagram.Model?.RemoveGroup(group.Id);
            }

        }

        private void CreatePanel([NotNull] IPanelFactory factory, [NotNull] IIdentity identity)
        {
            OpenPanel?.Invoke(factory, identity);
        }

        private void RemoveEntityGroup([NotNull] IShape shape)
        {
            if (shape is IEntityShape entityShape &&
                (_diagram.Entities?.Contains(entityShape) ?? false) &&
                entityShape.Identity is IEntity entity)
            {
                var graphEntity = GetEntity(entity);
                if (graphEntity != null)
                {
                    var selection = new GoSelection(null);
                    selection.Add(graphEntity);
                    _graph.DeleteSelection(selection);
                }
            } else if (shape is IGroupShape groupShape && 
                (_diagram.Groups?.Contains(groupShape) ?? false) &&
                groupShape.Identity is IGroup group)
            {
                var graphGroup = GetGroup(group);
                if (graphGroup != null)
                {
                    var selection = new GoSelection(null);
                    selection.Add(graphGroup);
                    _graph.DeleteSelection(selection);
                }
            }
        }

        private void RemoveDataFlow([NotNull] ILink link)
        {
            var graphLink = GetLink(link);
            if (graphLink != null)
            {
                var selection = new GoSelection(null);
                selection.Add(graphLink);
                _graph.DeleteSelection(selection);
            }
        }

        private void AddDataFlow([NotNull] IDiagram diagram, [NotNull] IDataFlow dataFlow)
        {
            if (diagram == _diagram && !_graph.IsDisposed)
            {
                var link = _diagram.AddLink(dataFlow);
                if (link != null)
                    AddLink(link);
            }
        }

        private void AddIdentity([NotNull] IDiagram diagram, [NotNull] IIdentity identity, 
            PointF relative, SizeF size)
        {
            if (diagram == _diagram && !_graph.IsDisposed)
            {
                var mouse = _graph.PointToClient(Control.MousePosition);
                PointF position = new PointF(mouse.X + relative.X, mouse.Y + relative.Y);

                IShape shape = null;
                if (identity is IEntity entity)
                {
                    //shape = _diagram.AddShape(entity, position);
                    GraphOnCreateIdentity(entity.Id, position);
                }
                else if (identity is IGroup group)
                {
                    shape = _diagram.AddShape(group, position, size);
                }

                if (shape != null)
                    AddShape(shape);
            }
        }
         
        private void RefreshGroupBorder([NotNull] IDiagram diagram, [NotNull] IGroup group)
        {
            if (Diagram == _diagram)
            {
                var graphGroup = GetGroup(group);
                graphGroup?.RefreshBorder();
            }
        }
    }
}