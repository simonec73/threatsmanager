using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Diagram;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("7FB3040F-4380-4AE4-849F-CB0D7996D768", "List Diagrams Placeholder", 38, ExecutionMode.Business)]
    public class ListDiagramsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListDiagrams";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListDiagramsPlaceholder();
        }
    }

    public class ListDiagramsPlaceholder : IListPlaceholder
    {
        public string Name => "Diagrams";
        public string Label => "Diagrams";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.model_small;

        public bool Tabular => false;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            return null;
        }

        public IEnumerable<ListItem> GetList([NotNull] IThreatModel model)
        {
            IEnumerable<ListItem> result = null;

            var diagrams = model.Diagrams?.ToArray();

            if (diagrams?.Any() ?? false)
            {
                var list = new List<ListItem>();

                var currentMarkerStatus = MarkerStatusTrigger.CurrentStatus;
                MarkerStatusTrigger.RaiseMarkerStatusUpdated(MarkerStatus.Hidden);

                try
                {
                    foreach (var diagram in diagrams)
                    {
                        var panel = new ModelPanel();
                        panel.SetDiagram(diagram);
                        var imageRow = new ImageRow("Diagram", panel.GetMetafile(), $"The '{diagram.Name}' scenario.");
                        var textRow = new TextRow("Description", diagram.Description);

                        list.Add(new ListItem(diagram.Name, diagram.Id, new ItemRow[]{imageRow, textRow}));
                    }

                }
                finally
                {
                    MarkerStatusTrigger.RaiseMarkerStatusUpdated(currentMarkerStatus);
                }

                result = list;
            }

            return result;
        }
    }
}