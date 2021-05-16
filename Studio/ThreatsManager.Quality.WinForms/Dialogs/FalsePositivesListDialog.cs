using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Dialogs
{
    public partial class FalsePositivesListDialog : Form
    {
        private readonly IThreatModel _model;

        public FalsePositivesListDialog()
        {
            InitializeComponent();
        }

        public FalsePositivesListDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;
        }

        public bool StatusChanged { get; private set; }

        private void FalsePositivesList_Load(object sender, EventArgs e)
        {
            var propertyType = new QualityPropertySchemaManager(_model).GetFalsePositivePropertyType();
            var analyzers = QualityAnalyzersManager.QualityAnalyzers?
                .Where(x => HasFalsePositives(x, propertyType))
                .ToArray();
            if (analyzers?.Any() ?? false)
            {
                _qualityAnalyzers.Items.AddRange(analyzers);
            }
        }

        private void _qualityAnalyzers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var analyzer = _qualityAnalyzers.SelectedItem as IQualityAnalyzer;

            _falsePositives.PrimaryGrid.Rows.Clear();

            if (analyzer != null)
            {
                var propertyType = new QualityPropertySchemaManager(_model).GetFalsePositivePropertyType();

                if (propertyType != null)
                {
                    AddItem(analyzer, _model, propertyType);
                    AddItems(analyzer, _model.Entities, propertyType);
                    AddItems(analyzer, _model.DataFlows, propertyType);
                    AddItems(analyzer, _model.Groups, propertyType);
                    AddItems(analyzer, _model.Diagrams, propertyType);
                }
            }
        }

        private bool HasFalsePositives([NotNull] IQualityAnalyzer analyzer, 
            [NotNull] IPropertyType propertyType)
        {
            return HasFalsePositives(analyzer, _model, propertyType) ||
                   HasFalsePositives(analyzer, _model.Entities, propertyType) ||
                   HasFalsePositives(analyzer, _model.DataFlows, propertyType) ||
                   HasFalsePositives(analyzer, _model.Groups, propertyType) ||
                   HasFalsePositives(analyzer, _model.Diagrams, propertyType);
        }

        private void AddItems([NotNull] IQualityAnalyzer analyzer, 
            IEnumerable<IPropertiesContainer> containers,
            [NotNull] IPropertyType propertyType)
        {
            if (containers?.Any() ?? false)
            {
                foreach (var container in containers)
                    AddItem(analyzer, container, propertyType);
            }
        }

        private void AddItem([NotNull] IQualityAnalyzer analyzer, 
            [NotNull] IPropertiesContainer container,
            [NotNull] IPropertyType propertyType)
        {
            var property = container.GetProperty(propertyType) as IPropertyJsonSerializableObject;
            if (property != null)
            {
                var list = property.Value as FalsePositiveList;
                var finding = list?.FalsePositives.FirstOrDefault(x =>
                    string.CompareOrdinal(x.QualityInitializerId, analyzer.GetExtensionId()) == 0);

                if (finding != null)
                    _falsePositives.PrimaryGrid.Rows.Add(new GridRow(container.ToString(), finding.Reason, finding.Author, finding.Timestamp)
                    {
                        Tag = container
                    });
            }

            if (container is IThreatEventsContainer threatEventsContainer)
            {
                var threatEvents = threatEventsContainer.ThreatEvents?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    foreach (var te in threatEvents)
                        AddItem(analyzer, te, propertyType);
                }
            }
        }

        private bool HasFalsePositives([NotNull] IQualityAnalyzer analyzer, 
            IEnumerable<IPropertiesContainer> containers,
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            if (containers?.Any() ?? false)
            {
                foreach (var container in containers)
                {
                    if (HasFalsePositives(analyzer, container, propertyType))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private bool HasFalsePositives([NotNull] IQualityAnalyzer analyzer, 
            [NotNull] IPropertiesContainer container,
            [NotNull] IPropertyType propertyType)
        {
            bool result = false;

            var property = container.GetProperty(propertyType) as IPropertyJsonSerializableObject;
            if (property != null)
            {
                var list = property.Value as FalsePositiveList;
                var finding = list?.FalsePositives.FirstOrDefault(x =>
                    string.CompareOrdinal(x.QualityInitializerId, analyzer.GetExtensionId()) == 0);

                if (finding != null)
                    result = true;
            }

            if (!result && container is IThreatEventsContainer threatEventsContainer)
            {
                var threatEvents = threatEventsContainer.ThreatEvents?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    foreach (var te in threatEvents)
                    {
                        if (HasFalsePositives(analyzer, te, propertyType))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private void _remove_Click(object sender, EventArgs e)
        {
            Remove(_falsePositives.PrimaryGrid.SelectedCells.OfType<GridCell>().Select(x => x.GridRow));
        }

        private void _removeAll_Click(object sender, EventArgs e)
        {
            Remove(_falsePositives.PrimaryGrid.Rows.OfType<GridRow>());
        }

        private void Remove(IEnumerable<GridRow> rows)
        {
            var list = rows?.ToArray();
            if ((list?.Any() ?? false) && _qualityAnalyzers.SelectedItem is IQualityAnalyzer analyzer)
            {
                var schemaManager = new QualityPropertySchemaManager(_model);

                foreach (var row in list)
                {
                    if (row.Tag is IPropertiesContainer container)
                    {
                        schemaManager.ResetFalsePositive(container, analyzer);
                        row.GridPanel.Rows.Remove(row);
                    }
                }

                StatusChanged = true;
            }
        }
    }
}
