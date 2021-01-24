using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.ItemTemplateList
{
    [Extension("9CE89064-492F-4683-84CD-E47F400E9151", "Item Template List", 15, ExecutionMode.Simplified)]
    public partial class ItemTemplateListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, 
        IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Item Template Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ItemTemplateListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "ItemTemplateList", "Item Template\nList", Properties.Resources.rubber_stamp_big,
                Properties.Resources.rubber_stamp);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new ItemTemplateListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}