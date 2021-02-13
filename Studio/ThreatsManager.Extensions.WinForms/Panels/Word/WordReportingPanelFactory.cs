using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word
{
    [Extension("00A70EA9-0C47-4DCF-8BB6-A4833B86A42F", "Word Reporting", 20, ExecutionMode.Business)]
    public partial class WordReportingPanelFactory : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
    {
        #region IPanelFactory implementation.
        /// <summary>
        /// The Word Reporting List Panel Factory should create a single instance.
        /// </summary>
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new WordReportingPanel();

            action = new ActionDefinition(result.Id, "WordReporting", "Word Reporting", Properties.Resources.docx_big,
                Properties.Resources.docx);

            return result;
        }

        public IPanel<Form> Create([NotNull] IActionDefinition action)
        {
            return new WordReportingPanel();
        }
        #endregion
    }
}