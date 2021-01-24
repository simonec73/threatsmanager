using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    [Extension("99EC3AAB-9346-4EF2-8A39-858D130D0A17", "Known Mitigation List", 11, ExecutionMode.Simplified)]
    public partial class KnownMitigationListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The External Interactors List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new KnownMitigationListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "KnownMitigationList", "Known Mitigation\nList", Resources.standard_mitigations_big,
                Resources.standard_mitigations);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new KnownMitigationListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}