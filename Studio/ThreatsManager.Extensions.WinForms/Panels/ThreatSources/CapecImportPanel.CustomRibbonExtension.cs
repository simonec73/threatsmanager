using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
#pragma warning disable CS0067
    public partial class CapecImportPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Capec Import";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("Import", "Import", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ImportChecked", "Import Checked Threats",
                            Resources.threat_types_big,
                            Resources.threat_types),
                        new ActionDefinition(Id, "ImportSelected", "Import Selected Threat",
                            Resources.threat_type_big,
                            Resources.threat_type)
                    }),
                    new CommandsBarDefinition("Selection", "Selection", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "CheckAll", "Check All Threats",
                            Properties.Resources.threats_selection_big,
                            Properties.Resources.threats_selection),
                        new ActionDefinition(Id, "CheckBranch", "Check the Selected Branch",
                        Properties.Resources.threat_selection_big,
                        Properties.Resources.threat_selection),
                        new ActionDefinition(Id, "Clear", "Clear Selections",
                            Properties.Resources.clear_big,
                            Properties.Resources.clear)
                    }),
                };

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;

            try
            {
                switch (action.Name)
                {
                    case "ImportChecked":
                        text = "Import of checked Threats";
                        var checkedNodes = _catalog.CheckedNodes;
                        if (checkedNodes != null)
                            ImportThreats(checkedNodes);
                        break;
                    case "ImportSelected":
                        text = "Import of selected Threat";
                        var selectedNodes = _catalog.SelectedNodes;
                        if (selectedNodes != null)
                            ImportThreats(selectedNodes);
                        break;
                    case "CheckAll":
                        _catalog.CheckAll();
                        break;
                    case "CheckBranch":
                        _catalog.CheckBranch();
                        break;
                    case "Clear":
                        text = "Reset of selections";
                        _catalog.ResetSelections();
                        break;
                }

                if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                if (text != null)
                    ShowWarning?.Invoke($"{text} has failed.");
            }
        }

        private void ImportThreats([NotNull] IEnumerable<ThreatSourceNode> nodes)
        {
            var propertySchemaManager = new CapecPropertySchemaManager(_model);
            var propertySchema = propertySchemaManager.GetSchema();
            var threatsSchemaManager = new ThreatsPropertySchemaManager(_model);
            var threatsSchema = threatsSchemaManager.GetSchema();

            var hiddenProperties = _catalog.HiddenProperties.ToArray();
            var hpPropertyType = propertySchemaManager.GetHiddenPropertiesPropertyType();
            if (hpPropertyType != null)
            {
                var property = _model.GetProperty(hpPropertyType);
                if (property is IPropertyTokens propertyTokens)
                {
                    propertyTokens.Value = hiddenProperties;
                }
                else
                {
                    _model.AddProperty(hpPropertyType, hiddenProperties.TagConcat());
                }
            }

            var unkownSeverity = _model.GetMappedSeverity(0);

            foreach (var node in nodes)
            {
                var threatType = _model.AddThreatType(node.Name, unkownSeverity);
                if (threatType != null)
                {
                    var properties = node.Properties.Where(x => !hiddenProperties.Contains(x.Key)).ToArray();
                    if (properties.Any())
                    {
                        foreach (var property in properties)
                        {
                            if (!IsSpecialProperty(property.Key, property.Value, threatType))
                            {
                                var propertyType =
                                    propertySchema.GetPropertyType(property.Key) ??
                                    propertySchema.AddPropertyType(property.Key, PropertyValueType.String);
                                if (propertyType != null)
                                {
                                    var p = threatType.GetProperty(propertyType);
                                    if (p != null)
                                        p.StringValue = property.Value;
                                    else
                                        threatType.AddProperty(propertyType, property.Value);
                                }
                            }
                        }
                    }

                    var keywords = _catalog.GetKeywords(node.Id)?.ToArray();
                    if (keywords?.Any() ?? false)
                    {
                        var keywordsPT = threatsSchema.GetPropertyType("Keywords") ??
                            threatsSchema.AddPropertyType("Keywords", PropertyValueType.Tokens);
                        if (keywordsPT != null)
                        {
                            var keywordsP = threatType.GetProperty(keywordsPT);
                            if (keywordsP != null)
                                keywordsP.StringValue = keywords.TagConcat();
                            else
                                threatType.AddProperty(keywordsPT, keywords.TagConcat());
                        }
                    }
                }
            }
        }

        private bool IsSpecialProperty([Required] string propertyName, string propertyValue, [NotNull] IThreatType threatType)
        {
            bool result = false;

            switch (propertyName)
            {
                case "Name":
                    result = true;
                    break;
                case "Summary":
                    threatType.Description = propertyValue;
                    result = true;
                    break;
                case "Severity":
                    if (Enum.TryParse<DefaultSeverity>(propertyValue, out var severity))
                    {
                        threatType.Severity = _model.GetMappedSeverity((int) severity);
                        result = true;
                    }
                    break;
            }

            return result;
        }
    }
}