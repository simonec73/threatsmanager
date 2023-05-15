using System;
using System.Drawing;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using System.Windows.Forms;

namespace MicroSvcsThreatsMapper.RibbonExtensions
{
    [Extension("45636082-DB4B-4E9A-8D25-06384EFA104F",
        "Load Micro Services Context Aware Action", 100, ExecutionMode.Expert)]
    public class ImportMicroServices : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Import;
        public string Bar => "Importing";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportMicroSvc", "Import Micro Services JSON", Properties.Resources.OpenShift_Clusters,
                Properties.Resources.OpenShift_Clusters)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "ImportMicroSvc":
                        var dialog = new OpenFileDialog()
                        {
                            AddExtension = true,
                            AutoUpgradeEnabled = true,
                            CheckFileExists = true,
                            CheckPathExists = true,
                            DefaultExt = "json",
                            DereferenceLinks = true,
                            Filter = "Micro Services JSON file (*.json)|*.json",
                            FilterIndex = 0,
                            Title = "Select Micro Services JSON file to load",
                            RestoreDirectory = true
                        };
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            var microsvcs = new MicroSvcsLoader().LoadJSONFile(dialog.FileName);

                            // project index
                            int pj_idx = 0;
                            foreach (var msvcpj in microsvcs)
                            {
                                // increment the idx
                                pj_idx++;

                                // add diagram
                                var diagram = threatModel.AddDiagram($"{pj_idx}. {msvcpj.Name}");

                                // add required trust boundaries
                                var aws_trustboundary = threatModel.AddGroup<ITrustBoundary>("AWS Ava Zone");
                                diagram.AddGroupShape(aws_trustboundary.Id, new PointF(0,0), new SizeF(2000, 1000));

                                var openshift_trustboundary = threatModel.AddGroup<ITrustBoundary>("OpenShift Trust Boundary");
                                diagram.AddGroupShape(openshift_trustboundary.Id, new PointF(0, 200), new SizeF(2000, 800));

                                // set tb parent
                                openshift_trustboundary.SetParent(aws_trustboundary);

                                var pjprocess = threatModel.AddEntity<IProcess>(msvcpj.Name);
                                diagram.AddShape(pjprocess, new PointF(-1500, -100));
                                pjprocess.SetParent(openshift_trustboundary);

                                // node index
                                int node_idx = 0;
                                foreach (var msvcnode in msvcpj.Nodes)
                                {
                                    // increment the idx
                                    node_idx++;

                                    var nodeprocess = threatModel.AddEntity<IProcess>(msvcnode.Name);
                                    diagram.AddShape(nodeprocess, new PointF(node_idx*500-1500, -300));
                                    nodeprocess.SetParent(aws_trustboundary);
                                    var nodepj_link = threatModel.AddDataFlow("Flow", nodeprocess.Id, pjprocess.Id);
                                    diagram.AddLink(nodepj_link);

                                    // pod index
                                    int pod_idx = 0;
                                    foreach (var msvcpod in msvcnode.Pods)
                                    {
                                        // increment the idx
                                        pod_idx++;

                                        var podprocess = threatModel.AddEntity<IProcess>(msvcpod);
                                        diagram.AddShape(podprocess, new PointF(pod_idx*500-1200, node_idx*200));
                                        podprocess.SetParent(openshift_trustboundary);
                                        var nodepod_link = threatModel.AddDataFlow("Flow", nodeprocess.Id, podprocess.Id);
                                        diagram.AddLink(nodepod_link);
                                        var pjpod_link = threatModel.AddDataFlow("Flow", pjprocess.Id, podprocess.Id);
                                        diagram.AddLink(pjpod_link);
                                    }
                                }
                            }

                            ShowMessage?.Invoke("Import Micro Services ThreatModel succeeded.");
                        }

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Import Micro Services ThreatModel failed.");
            }
        }
    }
}
