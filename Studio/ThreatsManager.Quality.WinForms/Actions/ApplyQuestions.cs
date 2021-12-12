using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Panels.Annotations;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Actions
{
#pragma warning disable 0067
    [Extension("933C0A34-F437-4070-A8C4-DC744C187A3B", "Apply Questions", 102, ExecutionMode.Simplified)]
    public class ApplyQuestions : IMainRibbonExtension, IDesktopAlertAwareExtension, IAsker
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.View;
        public string Bar => "Questions";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ApplyQuestions", "Generate Topics to be Clarified", Properties.Resources.speech_balloon_question_big_new,
                Properties.Resources.speech_balloon_question_new)
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
                    case "ApplyQuestions":
                        Ask?.Invoke(this, threatModel, Resources.ApplyTopicsAutoGenRules_Caption, Resources.ApplyTopicsAutoGenRules_Confirm, false, RequestOptions.OkCancel);
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Automatic Threat Event generation failed.\nPlease close the document without saving it.");
                throw;
            }
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Ok && context is IThreatModel threatModel)
            {
                if (Generate(threatModel))
                    ShowMessage?.Invoke("Topics to be Clarified generated successfully.");
                else
                {
                    ShowWarning?.Invoke("No Topic to be Clarified has been generated.");
                }
            }
        }

        private bool Generate([NotNull] IThreatModel model)
        {
            var result = false;

            var schemaManager = new QuestionsPropertySchemaManager(model);
            var questions = schemaManager.GetQuestions()?.ToArray();
            if (questions?.Any() ?? false)
            {
                var asm = new AnnotationsPropertySchemaManager(model);
                var pt = asm.GetAnnotationsPropertyType();

                var ei = model.Entities?.OfType<IExternalInteractor>().OrderBy(x => x.Name);
                var p = model.Entities?.OfType<IProcess>().OrderBy(x => x.Name);
                var ds = model.Entities?.OfType<IDataStore>().OrderBy(x => x.Name);
                var f = model.DataFlows?.OrderBy(x => x.Name);
                var tb = model.Groups?.OfType<ITrustBoundary>().OrderBy(x => x.Name);
                var te = model.GetThreatEvents()?.OrderBy(x => x.Name);
                var tem = model.GetThreatEventMitigations()?
                    .OrderBy(x => x.Mitigation.Name)
                    .ThenBy(x => x.ThreatEvent.Name)
                    .ThenBy(x => x.ThreatEvent.Parent.Name);
                var tt = model.ThreatTypes?.OrderBy(x => x.Name);
                var km = model.Mitigations?.OrderBy(x => x.Name);
                var sm = model.GetThreatTypeMitigations()?
                    .OrderBy(x => x.Mitigation.Name)
                    .ThenBy(x => x.ThreatType.Name);
                var et = model.EntityTemplates?.OrderBy(x => x.Name);
                var ft = model.FlowTemplates?.OrderBy(x => x.Name);
                var tbt = model.TrustBoundaryTemplates?.OrderBy(x => x.Name);

                foreach (var question in questions)
                {
                    Generate(question, ei, asm);
                    Generate(question, p, asm);
                    Generate(question, ds, asm);
                    Generate(question, f, asm);
                    Generate(question, tb, asm);
                    Generate(question, te, asm);
                    Generate(question, tem, asm);
                    Generate(question, tt, asm);
                    Generate(question, km, asm);
                    Generate(question, sm, asm);
                    Generate(question, et, asm);
                    Generate(question, ft, asm);
                    Generate(question, tbt, asm);
                    Generate(question, model, asm);
                }

                result = true;
            }

            return result;
        }

        private void Generate([NotNull] Question question, 
            IEnumerable<IPropertiesContainer> containers, AnnotationsPropertySchemaManager schemaManager)
        {
            var items = containers?.ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    Generate(question, item, schemaManager);
                }
            }
        }

        private void Generate([NotNull] Question question, [NotNull] IPropertiesContainer container, 
            [NotNull] AnnotationsPropertySchemaManager schemaManager)
        {
            if (question.Rule?.Evaluate(container) ?? false)
            {
                schemaManager.AddAnnotation(container, new TopicToBeClarified()
                {
                    Text = question.Text
                });
            }
        }
    }
}