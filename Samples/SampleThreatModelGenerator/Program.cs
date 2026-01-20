using System.IO;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Packaging;
using System.Drawing;
using System;

namespace SampleThreatModelGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && !File.Exists(args[0]))
            {
                // initialize the threatmodel
                var loader = new ModelLoader();
                var model = loader.LoadDefaultModel();

                // add diagram
                var diagram = model.AddDiagram("Diagram 1");

                // add external interactor
                var externalinteractor = model.AddEntity<IExternalInteractor>("User");
                diagram.AddShape(externalinteractor, new PointF(-300, 200));

                // add trust boundary
                var trustboundary = model.AddGroup<ITrustBoundary>("Trust Boundary 1");
                diagram.AddGroupShape(trustboundary.Id, new PointF(300, 100), new SizeF(500, 200));

                // add processes and their shapes, parents
                var process1 = model.AddEntity<IProcess>("Web Front-End");
                diagram.AddShape(process1, new PointF(100, 50));
                process1.SetParent(trustboundary);

                var process2 = model.AddEntity<IProcess>("Exposed APIs");
                diagram.AddShape(process2, new PointF(100, 350));
                process2.SetParent(trustboundary);

                var process3 = model.AddEntity<IProcess>("Serverless BL");
                diagram.AddShape(process3, new PointF(500, 50));
                process3.SetParent(trustboundary);

                var datastore = model.AddEntity<IDataStore>("Database");
                diagram.AddShape(datastore, new PointF(500, 350));
                datastore.SetParent(trustboundary);

                var process4 = model.AddEntity<IProcess>("On-premises systems");
                diagram.AddShape(process4, new PointF(900, 50));

                // add dataflows between them
                var dataflow1 = model.AddDataFlow("Get static content", externalinteractor.Id, process1.Id);
                diagram.AddLink(dataflow1);

                var dataflow2 = model.AddDataFlow("Get/set data", externalinteractor.Id, process2.Id);
                diagram.AddLink(dataflow2);

                var dataflow3 = model.AddDataFlow("Get/set data into DB", process2.Id, datastore.Id);
                diagram.AddLink(dataflow3);

                var dataflow4 = model.AddDataFlow("Get data from DB", process3.Id, datastore.Id);
                diagram.AddLink(dataflow4);

                var dataflow5 = model.AddDataFlow("Push to on-premises", process3.Id, process4.Id);
                diagram.AddLink(dataflow5);

                // set output file
                string output_file = args[0];

                // Save the model to a file in JSON format.
                var fileName = output_file;
                var json = ThreatModelManager.Serialize(model);
                var package = Package.Create(fileName);
                package.Add("threatmodel.json", json);
                package.Save();
            }
            else if (args.Length == 1)
            {
                Console.WriteLine($"Specify non-existent filename as {args[0]} already exists.");
            }
            else
            {
                Console.WriteLine("Provide output filename in cmd line args.");
            }
        }
    }
}
