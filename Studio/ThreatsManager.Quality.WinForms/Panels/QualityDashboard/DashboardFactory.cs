using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    [Extension("7D784F04-B1DE-4777-B785-5762F1113BB4", "Quality Dashboard", 10, ExecutionMode.Simplified)]
    public partial class DashboardFactory : IPanelFactory<Form>, IMainRibbonExtension, 
        IContextAwareExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new Dashboard();
            if (_actions != null)
                result.SetContextAwareActions(_actions);

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            var result = new Dashboard();
            if (_actions != null)
                result.SetContextAwareActions(_actions);
            return result;
        }
        #endregion
    }
}