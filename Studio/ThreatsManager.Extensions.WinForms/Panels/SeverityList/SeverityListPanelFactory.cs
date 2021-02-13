using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.SeverityList
{
    [Extension("D3ED5A71-CCEF-43A4-88C1-9CA8111C3652", "Severity List", 31, ExecutionMode.Expert)]
    public partial class SeverityListPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Severity List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new SeverityListPanel();
            action = new ActionDefinition(result.Id, "SeverityList", "Severity\nList", Resources.severity_big,
                Resources.severity);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new SeverityListPanel();
        }
        #endregion
    }
}