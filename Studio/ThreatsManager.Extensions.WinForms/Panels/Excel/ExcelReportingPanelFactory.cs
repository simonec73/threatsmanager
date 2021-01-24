using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Excel
{
    [Extension("04AB929D-B8FA-4B5A-A11B-80CCB9368F03", "Excel Reporting", 25, ExecutionMode.Simplified)]
    public partial class ExcelReportingPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The External Interactors List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new ExcelReportingPanel();

            action = new ActionDefinition(result.Id, "ExcelReport", "Excel Reporting", Properties.Resources.xlsx_big,
                Properties.Resources.xlsx);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new ExcelReportingPanel();
        }
        #endregion
    }
}