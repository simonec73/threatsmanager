using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Quality.Panels.QuestionList
{
#pragma warning disable CS0067
    public partial class QuestionListPanel
    {
        private readonly Guid _id = Guid.NewGuid();
        public event Action<string, bool> ChangeCustomActionStatus;

        public Guid Id => _id;
        public string TabLabel => "Question List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddQuestion", "Add Question",
                            Properties.Resources.speech_balloon_question_big_new,
                            Properties.Resources.speech_balloon_question_new),
                        new ActionDefinition(Id, "RemoveQuestion", "Remove Question",
                            Properties.Resources.speech_balloon_question_big_delete,
                            Properties.Resources.speech_balloon_question_delete,
                            false, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Save", "Save", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Save", "Save Questions",
                            Properties.Resources.floppy_disk_big,
                            Properties.Resources.floppy_disk),
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
            switch (action.Name)
            {
                case "AddQuestion":
                    var question = new Question();
                    _schemaManager.AddQuestion(question);
                    AddGridRow(question, _grid.PrimaryGrid);
                    break;
                case "RemoveQuestion":
                    if (_currentRow?.Tag is Question question2)
                    {
                        var text = question2.Text?.Trim().TrimEnd('?');
                        if ((text?.Length ?? 0) > 15)
                            text = $"{text.Substring(0, 15)}...";
                        if (MessageBox.Show($"Are you sure you want to delete question '{text}'?", 
                            "Remove Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                        {
                            _schemaManager.RemoveQuestion(question2);
                            _currentRow.Cells[0].PropertyChanged -= OnQuestionCellChanged;
                            _grid.PrimaryGrid.Rows.Remove(_currentRow);
                            _currentRow = null;
                            ChangeCustomActionStatus?.Invoke("RemoveQuestion", false);
                        }
                    }
                    break;
                case "Save":
                    if (_saveFile.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        if (File.Exists(_saveFile.FileName))
                            File.Delete(_saveFile.FileName);

                        using (var file = File.OpenWrite(_saveFile.FileName))
                        {
                            using (var writer = new BinaryWriter(file))
                            {
                                var questions = _schemaManager.GetQuestions();

                                StringBuilder sb = new StringBuilder();
                                StringWriter sw = new StringWriter(sb);

                                using(JsonWriter jtw = new JsonTextWriter(sw))
                                {
                                    var serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented};
                                    serializer.Serialize(jtw, questions);
                                }

                                var serialization = Encoding.Unicode.GetBytes(sb.ToString());
                    
                                writer.Write((byte)0xFF);
                                writer.Write((byte)0xFE);
                                writer.Write(serialization);
                            }
                        }
                    }
                    break;
                case "Refresh":
                    LoadModel();
                    break;
            }
        }
    }
}