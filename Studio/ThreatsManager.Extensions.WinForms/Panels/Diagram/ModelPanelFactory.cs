using System;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Extension("54B649C9-147C-44D6-9007-2ECEBA522356", "Diagram", 10, ExecutionMode.Business)]
    public partial class ModelPanelFactory : IPanelFactory<Form>, IMainRibbonExtension,
        IContextAwareExtension, IPanelFactoryActionsRequestor, IExecutionModeSupport
    {
        private ExecutionMode _executionMode = ExecutionMode.Expert;

        #region IPanelFactory implementation.
        /// <summary>
        /// Model Panel should create a single panel per Diagram.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Multiple;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            IPanel<Form> result = null;
            action = null;

            if (identity is IDiagram diagram)
            {
                result = CreatePanel(diagram);

                var diagramActionDefinition = new DiagramActionDefinition(diagram.Id);
                diagramActionDefinition.Initialize(diagram);

                action = diagramActionDefinition;
            }

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            if (action.Tag is IDiagram diagram)
            {
                return CreatePanel(diagram);
            }
            else
            {
                throw new ArgumentException("The selected action does not open a Diagram.");
            }
        }

        private ModelPanel CreatePanel(IDiagram diagram)
        {
            var result = new ModelPanel(this);
            result.SetExecutionMode(_executionMode);

            if (_actions != null)
                result.SetContextAwareActions(_actions);

            if (diagram != null)
                result.SetDiagram(diagram);

            return result;
        }
        #endregion

        #region Auxiliary functions.
        [InitializationRequired]
        internal bool Delete([NotNull] ModelPanel panel)
        {
            bool result = false;

            if (panel.Diagram is IDiagram diagram && panel.Diagram.Model is IThreatModel model)
            {
                if (model.RemoveDiagram(diagram.Id))
                {
                    PanelDeletionRequired?.Invoke(this, panel);
                    result = true;
                }
            }

            return result;
        }
        #endregion

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
        }
    }
}