using System;
using System.Drawing;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Extensions
{
    internal class DiagramActionDefinition : IActionDefinition//, INotifyPropertyChanged
    {
        private IDiagram _diagram;

        public DiagramActionDefinition(Guid id)
        {
            Id = id;
        }

        public void Initialize([PostSharp.Patterns.Contracts.NotNull] IDiagram diagram)
        {
            _diagram = diagram;
            // ReSharper disable once SuspiciousTypeConversion.Global
            //((INotifyPropertyChanged) diagram).PropertyChanged += OnDiagramPropertyChanged;
        }

        public Guid Id { get; }
        public string Name => Id.ToString("N");
        public string Label => _diagram?.Name;
        public Bitmap Icon => Resources.model;
        public Bitmap SmallIcon => Resources.model_small;
        public bool Enabled => true;
        public object Tag => _diagram;
        public Shortcut Shortcut => Shortcut.None;
        public string Tooltip => "Show the selected Diagram.";

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void OnDiagramPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (sender is IDiagram diagram && string.CompareOrdinal(e.PropertyName, "Name") == 0)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Label"));
        //    }
        //}
    }
}