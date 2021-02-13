using System;
using System.Collections.Generic;
using System.IO;
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

namespace ThreatsManager.Quality.Panels.ReviewNotes
{
    public partial class ReviewNotesPanel
    {
        private readonly Guid _id = Guid.NewGuid();
        
        public event Action<string, bool> ChangeCustomActionStatus;

        public Guid Id => _id;
        public string TabLabel => "Review Notes";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddReviewNote", "Add Review Note",
                            Properties.Resources.clipboard_check_edit_big_new,
                            Properties.Resources.clipboard_check_edit_new,
                            false, Shortcut.None),
                        new ActionDefinition(Id, "RemoveReviewNote", "Remove Review Note",
                            Properties.Resources.clipboard_check_edit_big_delete,
                            Properties.Resources.clipboard_check_edit_delete,
                            false, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Export", "Export",
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
                case "AddReviewNote":
                    if (_selected != null)
                    {
                        var review = new ReviewNote();
                        schemaManager.AddAnnotation(_selected, review);
                        AddButton(review);
                    }
                    break;
                case "RemoveReviewNote":
                    if (_selected != null && _annotation.Annotation is ReviewNote reviewNote &&
                        MessageBox.Show("You are about to remove the currently selected Review Note. Are you sure?",
                            "Remove Review Note", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        schemaManager.RemoveAnnotation(_selected, reviewNote);
                        _annotation.Annotation = null;
                        RemoveButton(reviewNote);
                    }
                    break;
                case "Export":
                    var saveFileDialog = new SaveFileDialog()
                    {
                        AddExtension = true,
                        AutoUpgradeEnabled = true,
                        CheckFileExists = false,
                        CheckPathExists = true,
                        RestoreDirectory = true,
                        DefaultExt = "csv",
                        Filter = "CSV file (*.csv)|*.csv",
                        Title = "Create CSV file",
                        ValidateNames = true
                    };
                    if (saveFileDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        ExportCsv(saveFileDialog.FileName);
                    }
                    break;
                case "Refresh":
                    LoadModel();
                    break;
            }

            _right.ResumeLayout();
        }

        private void ExportCsv([Required] string fileName)
        {
            var list = new List<IPropertiesContainer>();
            Add(list, _model.GetExternalInteractors(_schemaManager, _propertyType));
            Add(list, _model.GetProcesses(_schemaManager, _propertyType));
            Add(list, _model.GetDataStores(_schemaManager, _propertyType));
            Add(list, _model.GetFlows(_schemaManager, _propertyType));
            Add(list, _model.GetTrustBoundaries(_schemaManager, _propertyType));
            Add(list, _model.GetThreatEvents(_schemaManager, _propertyType));
            Add(list, _model.GetThreatEventMitigations(_schemaManager, _propertyType));
            Add(list, _model.GetThreatTypes(_schemaManager, _propertyType));
            Add(list, _model.GetKnownMitigations(_schemaManager, _propertyType));
            Add(list, _model.GetStandardMitigations(_schemaManager, _propertyType));
            Add(list, _model.GetEntityTemplates(_schemaManager, _propertyType));
            Add(list, _model.GetFlowTemplates(_schemaManager, _propertyType));
            Add(list, _model.GetTrustBoundaryTemplates(_schemaManager, _propertyType));

            if (list?.Any() ?? false)
            {
                using (var file = new System.IO.FileStream(fileName, FileMode.Create))
                {
                    using (var writer = new StreamWriter(file, Encoding.UTF8))
                    {
                        writer.WriteLine("Object Type,Name,Context,Review Note");

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
                            } else if (item is IThreatTypeMitigation threatTypeMitigation)
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
                                    if (annotation is ReviewNote)
                                    {
                                        writer.WriteLine(
                                            $"\"{objectType}\",\"{name}\",\"{context}\",\"{annotation.Text}\"");
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
