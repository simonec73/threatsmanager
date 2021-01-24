using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
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

            Initialize(EnumExtensions.GetEnumLabels<WorkItemStatus>());
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;

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
                IEnumerable<IMitigation> mitigations;
                if (_filter == RoadmapStatus.NoActionRequired)
                {
                    mitigations = _model?.GetUniqueMitigations()?
                        .Where(x => schemaManager.GetStatus(x) != RoadmapStatus.NoActionRequired)
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
                    var states = DevOpsManager.GetMitigationsStatus(_model);

                   foreach (var mitigation in mitigations)
                    {
                        if (states?.ContainsKey(mitigation) ?? false)
                            AddItem(mitigation, states[mitigation]);
                        else
                            AddItem(mitigation, WorkItemStatus.Unknown);
                    }

                    RefreshNodes();
                }
            }
            finally
            {
                _loading = false;
            }
        }
        #endregion

        public IActionDefinition ActionDefinition => 
            new ActionDefinition(Id, "MitigationsKanban", "Summary Mitigations Kanban", 
            Properties.Resources.standard_mitigations_kanban_big,
            Properties.Resources.standard_mitigations_kanban);

        
        #region Status Management.
        protected override void SetFirst(object item)
        {
            if (!_loading && item is IMitigation mitigation)
            {
                Set(mitigation, 0);
            }
        }

        protected override void SetSecond(object item)
        {
            if (!_loading && item is IMitigation mitigation)
            {
                Set(mitigation, 1);
            }
        }

        protected override void SetThird(object item)
        {
            if (!_loading && item is IMitigation mitigation)
            {
                Set(mitigation, 2);
            }
        }

        protected override void SetFourth(object item)
        {
            if (!_loading && item is IMitigation mitigation)
            {
                Set(mitigation, 3);
            }
        }

        protected override void SetFifth(object item)
        {
            if (!_loading && item is IMitigation mitigation)
            {
                Set(mitigation, 4);
            }
        }

        protected override void SetSixth(object item)
        {
            if (!_loading && item is IMitigation mitigation)
            {
                Set(mitigation, 5);
            }
        }

        private void Set(IMitigation mitigation, int pos)
        {
            var status = GetPaletteWorkItemStatus(pos);

            try
            {
                _loading = true;
                DevOpsManager.SetMitigationsStatusAsync(mitigation, status);
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
        }
        #endregion
    }
}
