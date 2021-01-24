using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    [Extension("17D7852D-F011-4119-8554-E6031BF1F87D", "Property Schema List", 20, ExecutionMode.Expert)]
    public partial class PropertySchemaListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Property Schema List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new PropertySchemaListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = new ActionDefinition(result.Id, "PropertySchemaList", "Property Schema\nList", Properties.Resources.properties_big,
                Properties.Resources.properties);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new PropertySchemaListPanel();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}