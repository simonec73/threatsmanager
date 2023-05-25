using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel;
using System.Drawing;

namespace MicroSvcsThreatsMapper.Libraries
{
    internal class ThreatModelLoader
    {
        public IThreatModel LoadThreatModel(string file_name, IThreatModel threatModel)
        {
            var microsvcs = new MicroSvcsLoader().LoadJSONFile(file_name);

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
                diagram.AddGroupShape(aws_trustboundary.Id, new PointF(0, 0), new SizeF(2000, 1000));

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
                    diagram.AddShape(nodeprocess, new PointF(node_idx * 500 - 1500, -300));
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
                        diagram.AddShape(podprocess, new PointF(pod_idx * 500 - 1200, node_idx * 200));
                        podprocess.SetParent(openshift_trustboundary);
                        var nodepod_link = threatModel.AddDataFlow("Flow", nodeprocess.Id, podprocess.Id);
                        diagram.AddLink(nodepod_link);
                        var pjpod_link = threatModel.AddDataFlow("Flow", pjprocess.Id, podprocess.Id);
                        diagram.AddLink(pjpod_link);
                    }
                }
            }
            return threatModel;
        }
    }
}
