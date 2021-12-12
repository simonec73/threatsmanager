using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Resources = ThreatsManager.Icons.Resources;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Quality.Panels.Annotations
{
    public partial class AnnotationsPanel
    {
        private readonly Guid _id = Guid.NewGuid();
        
        public event Action<string, bool> ChangeCustomActionStatus;

        public Guid Id => _id;
        public string TabLabel => "Annotations";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddNotes", "Add Notes",
                            Properties.Resources.note_text_big_new,
                            Properties.Resources.note_text_new,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "AddTopic", "Add Topic to be Clarified",
                            Properties.Resources.speech_balloon_question_big_new,
                            Properties.Resources.speech_balloon_question_new,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "AddHighlight", "Add Highlight",
                            Properties.Resources.marker_big_new,
                            Properties.Resources.marker_new,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "RemoveNotes", "Remove Notes",
                            Properties.Resources.note_text_big_delete,
                            Properties.Resources.note_text_delete,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "RemoveTopic", "Remove Topic to be Clarified",
                            Properties.Resources.speech_balloon_question_big_delete,
                            Properties.Resources.speech_balloon_question_delete,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "RemoveHighlight", "Remove Highlight",
                            Properties.Resources.marker_big_delete,
                            Properties.Resources.marker_delete,
                            false, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Filter", "Filter", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ShowOpenTopics", "Open Topics Only",
                            Properties.Resources.speech_balloon_question_big,
                            Properties.Resources.speech_balloon_question,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "ShowHighlights", "Highlights Only",
                            Properties.Resources.marker_big,
                            Properties.Resources.marker,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "ShowAll", "Show All",
                            Properties.Resources.question_and_answer_big,
                            Properties.Resources.question_and_answer,
                            false, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ExportOpen", "Export Open Topics",
                            Properties.Resources.xlsx_big,
                            Properties.Resources.xlsx),
                        new ActionDefinition(Id, "ExportAll", "Export All Topics",
                            Properties.Resources.xlsx_big,
                            Properties.Resources.xlsx),
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh List",
                            Resources.refresh_big,
                            Resources.refresh,
                            true, Shortcut.F5),
                    }),
                };

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            var schemaManager = new AnnotationsPropertySchemaManager(_model);

            _right.SuspendLayout();

            switch (action.Name)
            {
                case "AddNotes":
                    if (_selected != null)
                    {
                        var notes = new Annotation();
                        schemaManager.AddAnnotation(_selected, notes);
                        AddButton(notes);
                    }
                    break;
                case "AddTopic":
                    if (_selected != null)
                    {
                        var topic = new TopicToBeClarified();
                        schemaManager.AddAnnotation(_selected, topic);
                        AddButton(topic);
                    }
                    break;
                case "AddHighlight":
                    if (_selected != null)
                    {
                        var high = new Highlight();
                        schemaManager.AddAnnotation(_selected, high);
                        AddButton(high);
                    }
                    break;
                case "RemoveNotes":
                    if (_selected != null && _annotation.Annotation is Annotation annotation &&
                        MessageBox.Show("You are about to remove the currently selected Note. Are you sure?",
                            "Remove Notes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        schemaManager.RemoveAnnotation(_selected, annotation);
                        _annotation.Annotation = null;
                        RemoveButton(annotation);
                    }
                    break;
                case "RemoveTopic":
                    if (_selected != null && _annotation.Annotation is TopicToBeClarified toBeClarified &&
                        MessageBox.Show("You are about to remove the currently selected Topic. Are you sure?",
                        "Remove Topic", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        schemaManager.RemoveAnnotation(_selected, toBeClarified);
                        _annotation.Annotation = null;
                        RemoveButton(toBeClarified);
                    }
                    break;
                case "RemoveHighlight":
                    if (_selected != null && _annotation.Annotation is Highlight highlight &&
                        MessageBox.Show("You are about to remove the currently selected Highlight. Are you sure?",
                            "Remove Highlight", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        schemaManager.RemoveAnnotation(_selected, highlight);
                        _annotation.Annotation = null;
                        RemoveButton(highlight);
                    }
                    break;
                case "ShowOpenTopics":
                    _show = WhatToShow.OpenTopicsOnly;
                    LoadModel();
                    ChangeCustomActionStatus?.Invoke("ShowOpenTopics", false);
                    ChangeCustomActionStatus?.Invoke("ShowHighlights", true);
                    ChangeCustomActionStatus?.Invoke("ShowReviewNotes", true);
                    ChangeCustomActionStatus?.Invoke("ShowAll", true);
                    break;
                case "ShowHighlights":
                    _show = WhatToShow.HighlightsOnly;
                    LoadModel();
                    ChangeCustomActionStatus?.Invoke("ShowOpenTopics", true);
                    ChangeCustomActionStatus?.Invoke("ShowHighlights", false);
                    ChangeCustomActionStatus?.Invoke("ShowReviewNotes", true);
                    ChangeCustomActionStatus?.Invoke("ShowAll", true);
                    break;
                case "ShowAll":
                    _show = WhatToShow.All;
                    LoadModel();
                    ChangeCustomActionStatus?.Invoke("ShowOpenTopics", true);
                    ChangeCustomActionStatus?.Invoke("ShowHighlights", true);
                    ChangeCustomActionStatus?.Invoke("ShowReviewNotes", true);
                    ChangeCustomActionStatus?.Invoke("ShowAll", false);
                    break;
                case "ExportOpen":
                    var saveFileDialog = new SaveFileDialog()
                    {
                        AddExtension = true,
                        AutoUpgradeEnabled = true,
                        CheckFileExists = false,
                        CheckPathExists = true,
                        RestoreDirectory = true,
                        DefaultExt = "csv",
                        Filter = "CSV file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx",
                        Title = "Create a file with open Topics",
                        ValidateNames = true
                    };
                    if (saveFileDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        ExportCsv(saveFileDialog.FileName, true);
                    }
                    break;
                case "ExportAll":
                    var saveFileDialog2 = new SaveFileDialog()
                    {
                        AddExtension = true,
                        AutoUpgradeEnabled = true,
                        CheckFileExists = false,
                        CheckPathExists = true,
                        RestoreDirectory = true,
                        DefaultExt = "csv",
                        Filter = "CSV file (*.csv)|*.csv|Excel file (*.xlsx)|*.xlsx",
                        Title = "Create a file with all Topics",
                        ValidateNames = true
                    };
                    if (saveFileDialog2.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        ExportCsv(saveFileDialog2.FileName, false);
                    }
                    break;
                case "Refresh":
                    LoadModel();
                    break;
            }

            _right.ResumeLayout();
        }

        private void ExportCsv([Required] string fileName, bool openOnly)
        {
            var list = new List<IPropertiesContainer>();
            Add(list, _model.GetExternalInteractors(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetProcesses(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetDataStores(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetFlows(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetTrustBoundaries(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetThreatEvents(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetThreatEventMitigations(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetThreatTypes(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetKnownMitigations(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetStandardMitigations(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetEntityTemplates(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetFlowTemplates(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetTrustBoundaryTemplates(_schemaManager, _propertyType, null, openOnly));
            Add(list, _model.GetDiagrams(_schemaManager, _propertyType, null, openOnly));

            if (list.Any())
            {
                using (var engine = new ExcelReportEngine())
                {
                    var page = engine.AddPage("Report");
                    List<string> fields = new List<string> {"Object Type", "Name", "Context", "Annotation Type",
                        "Annotation Text", "Asked By", "Asked On", "Asked Via", "Answered", "Answer"};
                    engine.AddHeader(page, fields.ToArray());

                    foreach (var item in list)
                    {
                        string objectType = null;
                        string name = null;
                        string context = null;
                        if (item is IIdentity identity)
                        {
                            objectType = _model.GetIdentityTypeName(identity);
                            name = identity.Name;
                        }
                        else if (item is IThreatEventMitigation threatEventMitigation)
                        {
                            objectType = "Threat Event Mitigation";
                            name = threatEventMitigation.Mitigation.Name;
                            context =
                                $"'{threatEventMitigation.ThreatEvent.Name}' on '{threatEventMitigation.ThreatEvent.Parent.Name}'";
                        }
                        else if (item is IThreatTypeMitigation threatTypeMitigation)
                        {
                            objectType = "Threat Type Mitigation";
                            name = threatTypeMitigation.Mitigation.Name;
                            context =
                                $"{threatTypeMitigation.ThreatType.Name}";
                        }

                        var annotations = _schemaManager.GetAnnotations(item)?.ToArray();
                        if (annotations?.Any() ?? false)
                        {
                            foreach (var annotation in annotations)
                            {
                                if (!openOnly ||
                                    (annotation is TopicToBeClarified topic && !topic.Answered))
                                {
                                    string annotationType;
                                    string text = annotation.Text;
                                    string askedBy = null;
                                    string askedOn = null;
                                    string askedVia = null;
                                    string answered = null;
                                    string answers = null;
                                    if (annotation is TopicToBeClarified topicToBeClarified)
                                    {
                                        annotationType = "Topic to be clarified";
                                        askedBy = topicToBeClarified.AskedBy;
                                        askedOn = topicToBeClarified.AskedOn.ToShortDateString();
                                        askedVia = topicToBeClarified.AskedVia;
                                        answered = topicToBeClarified.Answered.ToString();
                                        answers = GetAnswers(topicToBeClarified.Answers);
                                    }
                                    else if (annotation is Highlight highlight)
                                    {
                                        annotationType = "Highlight";
                                    }
                                    else if (annotation is ReviewNote reviewNote)
                                    {
                                        annotationType = null;
                                    }
                                    else
                                    {
                                        annotationType = "Note";
                                    }

                                    if (annotationType != null)
                                        engine.AddRow(page, new[]
                                        {
                                            objectType, name, context, annotationType,
                                            text, askedBy, askedOn, askedVia, answered, answers
                                        });
                                }
                            }
                        }
                    }

                    try
                    {
                        engine.Save(fileName);
                        ShowMessage?.Invoke("File created successfully.");
                    }
                    catch (Exception exc)
                    {
                        ShowWarning?.Invoke(exc.Message);
                    }
                }
            }
            else
            {
                ShowWarning?.Invoke("The file has not been created because it would be empty.");
            }
        }

        private string GetAnswers(IEnumerable<AnnotationAnswer> answers)
        {
            string result = null;

            var list = answers?.ToArray();
            if (list?.Any() ?? false)
            {
                var builder = new StringBuilder();

                foreach (var item in list)
                {
                    builder.AppendLine($"-- By {item.AnsweredBy} on {item.AnsweredOn.ToShortDateString()} via {item.AnsweredVia}");
                    builder.AppendLine(item.Text);
                    builder.AppendLine();
                }

                result = builder.ToString();
            }

            return result;
        }

        private void Add([NotNull] List<IPropertiesContainer> list, IEnumerable<IPropertiesContainer> containers)
        {
            var items = containers?.ToArray();
            if (items?.Any() ?? false)
            {
                list.AddRange(items);
            }
        }
    }
}
