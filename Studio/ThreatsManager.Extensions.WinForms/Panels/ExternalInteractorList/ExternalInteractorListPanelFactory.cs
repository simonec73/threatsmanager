using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ExternalInteractorList
{
    [Extension("261936FC-C969-43AA-A2F1-8F805ECB4BD2", "External Interactor List", 21, ExecutionMode.Simplified)]
    public partial class ExternalInteractorListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The External Interactors List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ExternalInteractorListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "ExternalInteractorList", "External Interactor\nList", Resources.external_big,
                Resources.external);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new ExternalInteractorListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}