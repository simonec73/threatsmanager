using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels.MitigationsKanban
{
    public partial class MitigationsKanbanPanel : KanbanPanel, IShowThreatModelPanel<Form>, ICustomRibbonExtension, IDesktopAlertAwareExtension
    {
        private bool _loading;
        private RoadmapStatus _filter = RoadmapStatus.NoActionRequired;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public MitigationsKanbanPanel() : base()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;

            InitializePalettes();
            LoadModel();

            DevOpsManager.RefreshDone += DevOpsManagerOnRefreshDone;
        }

        private void DevOpsManagerOnRefreshDone(IThreatModel model, int count)
        {
            if (count > 0 && _model == model)
                LoadModel();
        }

        [Dispatched]
        private void LoadModel()
        {
            try
            {
                _loading = true;
                ClearPalettes();

                var schemaManager = new RoadmapPropertySchemaManager(_model);
                var devOpsSchemaManager = new DevOpsPropertySchemaManager(_model);
                var connector = DevOpsManager.GetConnector(_model); 
                IEnumerable<IMitigation> mitigations;
                if (_filter == RoadmapStatus.NoActionRequired)
                {
                    mitigations = _model?.GetUniqueMitigations()?
                        .Where(x => (schemaManager.GetStatus(x) != RoadmapStatus.NoActionRequired) || 
                                    (connector != null && devOpsSchemaManager.GetDevOpsInfo(x, connector) != null))
                        .OrderBy(x => x.Name).ToArray();
                }
                else
                {
                    mitigations = _model?.GetUniqueMitigations()?
                        .Where(x => schemaManager.GetStatus(x) == _filter)
                        .OrderBy(x => x.Name).ToArray();
                }

                if (mitigations?.Any() ?? false)
                {
                    var summaries = DevOpsManager.GetMitigationsSummary(_model);

                    foreach (var mitigation in mitigations)
                    {
                        if (summaries?.ContainsKey(mitigation) ?? false)
                        {
                            var summary = summaries[mitigation];
                            AddItem(mitigation, summary.Status, summary.AssignedTo);
                        }
                        else
                            AddItem(mitigation, WorkItemStatus.Unknown, null);
                    }

                    RefreshNodes();
                }
            }
            finally
            {
                _loading = false;
            }
        }

        private void InitializePalettes()
        {
            var connector = DevOpsManager.GetConnector(_model);
            var itemStates = connector.GetWorkItemStatesAsync().Result?.ToArray();
            if (itemStates?.Any() ?? false)
            {
                var mappings = connector.WorkItemStateMappings?.ToArray();
                var captions = new List<string>();
                captions.Add(WorkItemStatus.Unknown.GetEnumLabel());
                var labels = itemStates
                    .Where(x => mappings.Any(y => string.CompareOrdinal(x, y.Key) == 0))
                    .Select(x => mappings.Where(y => string.CompareOrdinal(x, y.Key) == 0).First().Value.GetEnumLabel())
                    .Where(x => x != null)
                    .Distinct();
                if (labels.Any())
                    captions.AddRange(labels);
                Initialize(captions);
            }
        }
        #endregion

        public IActionDefinition ActionDefinition => 
            new ActionDefinition(Id, "MitigationsKanban", "Summary Mitigations Kanban", 
            Properties.Resources.standard_mitigations_kanban_big,
            Properties.Resources.standard_mitigations_kanban);

        
        #region Status Management.
        protected override bool SetFirst(object item)
        {
            bool result = false;

            if (!_loading && item is IMitigation mitigation &&
                MessageBox.Show(Form.ActiveForm, "This action is going to remove the Mitigation from the DevOps system. Are you sure?", "Mitigation removal", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                result = Set(mitigation, 0);

                var schemaManager = new DevOpsPropertySchemaManager(_model);
                schemaManager.RemoveDevOpsInfos(mitigation);
            }

            return result;
        }

        protected override bool SetSecond(object item)
        {
            bool result = false;

            if (!_loading && item is IMitigation mitigation)
            {
                result = Set(mitigation, 1);
            }

            return result;
        }

        protected override bool SetThird(object item)
        {
            bool result = false;

            if (!_loading && item is IMitigation mitigation)
            {
                result = Set(mitigation, 2);
            }

            return result;
        }

        protected override bool SetFourth(object item)
        {
            bool result = false;

            if (!_loading && item is IMitigation mitigation)
            {
                result = Set(mitigation, 3);
            }

            return result;
        }

        protected override bool SetFifth(object item)
        {
            bool result = false;

            if (!_loading && item is IMitigation mitigation)
            {
                result = Set(mitigation, 4);
            }

            return result;
        }

        protected override bool SetSixth(object item)
        {
            bool result = false;

            if (!_loading && item is IMitigation mitigation)
            {
                result = Set(mitigation, 5);
            }

            return result;
        }

        private bool Set(IMitigation mitigation, int pos)
        {
            bool result = false;
            var status = GetPaletteWorkItemStatus(pos);

            try
            {
                _loading = true;
                DevOpsManager.SetMitigationsStatusAsync(mitigation, status);
                result = true;
            }
            catch (WorkItemCreationException)
            {
                MoveItem(mitigation, status, WorkItemStatus.Unknown);
                ShowWarning?.Invoke($"Mitigation creation failed.\nPlease wait a few moments and retry, because the problem may be temporary.");
            }
            catch (WorkItemStateChangeException stateChangeException)
            {
                MoveItem(mitigation, stateChangeException.FinalStatus, stateChangeException.InitialStatus);
                ShowWarning?.Invoke($"Mitigation movement failed.\nPlease wait a few moments and retry, because the problem may be temporary.");
            }
            finally
            {
                _loading = false;
            }

            return result;
        }
        #endregion
    }
}
