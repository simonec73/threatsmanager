using System;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoThreatGeneration.Actions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace SampleExtensions.RibbonExtensions
{
    [Extension("B33DB234-2105-4C7B-8D24-242D155CAD64",
        "Simulate Import of Auto threat Gen Rules Context Aware Action", 100, ExecutionMode.Expert)]
    public class GenerateAutoThreatGen : IMainRibbonExtension, IDesktopAlertAwareExtension
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
        public string Bar => "AutoThreatGen";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportATG", "Import Auto Threat Gen", Properties.Resources.Robot,
                Properties.Resources.Robot)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        private const string TestThreatName = "My test threat";
        private const string TestFirstTrustBoundaryTemplate = "A first Trust Boundary Template";
        private const string TestSecondTrustBoundaryTemplate = "A second Trust Boundary Template";

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "ImportATG":
                        using (var scope = UndoRedoManager.OpenScope("Import Auto Threat Gen simulation"))
                        {
                            // 1st step: get the test threat type.
                            var threatType = threatModel.ThreatTypes?
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, TestThreatName) == 0) ??
                                threatModel.AddThreatType(TestThreatName, threatModel.GetSeverity(50));

                            // 2nd step: get the first Trust Boundary Template.
                            var template1 = threatModel.TrustBoundaryTemplates?
                                 .FirstOrDefault(x => string.CompareOrdinal(x.Name, TestFirstTrustBoundaryTemplate) == 0) ??
                                threatModel.AddTrustBoundaryTemplate(TestFirstTrustBoundaryTemplate, null);

                            // 3nd step: get the second Trust Boundary Template.
                            var template2 = threatModel.TrustBoundaryTemplates?
                                 .FirstOrDefault(x => string.CompareOrdinal(x.Name, TestSecondTrustBoundaryTemplate) == 0) ??
                                threatModel.AddTrustBoundaryTemplate(TestSecondTrustBoundaryTemplate, null);

                            // 4th step: create a simple generation rule and assign it to the threat type.
                            var rule = new SelectionRule();
                            var andNode = new AndRuleNode();
                            rule.Root = andNode;
                            andNode.Children.Add(new EnterTrustBoundaryTemplateRuleNode("Flow", template1));
                            andNode.Children.Add(new ExitTrustBoundaryTemplateRuleNode("Flow", template2));

                            // 5th step: add the rule to the threat type.
                            threatType.SetRule(rule);

                            scope?.Complete();
                        }

                        ShowMessage?.Invoke("Import Azure Subscription succeeded.");

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Import Azure Subscription failed.");
            }
        }
    }
}