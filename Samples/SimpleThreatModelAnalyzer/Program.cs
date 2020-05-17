using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Packaging;
using ThreatsManager.Utilities;

namespace SimpleThreatModelAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    Console.WriteLine($"Opening file '{args[0]}'.");

                    var loader = new ModelLoader();
                    var model = loader.OpenModel(args[0]);

                    if (model != null)
                    {
                        Console.WriteLine($"Name: {model.Name}");
                        Console.WriteLine(
                            $"Count External Interactors: {model.Entities.Count(x => x is IExternalInteractor)}");
                        Console.WriteLine($"Count Processes: {model.Entities.Count(x => x is IProcess)}");
                        Console.WriteLine($"Count Data Stores: {model.Entities.Count(x => x is IDataStore)}");
                    }
                    else
                    {
                        Console.WriteLine("Model has not been loaded.");
                    }
                }
                else
                {
                    throw new FileNotFoundException("Unable to find the specified file.", args[0]);
                }
            }
        }

    }
}
