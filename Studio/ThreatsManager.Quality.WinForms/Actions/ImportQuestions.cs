﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Panels.Annotations;
using ThreatsManager.Quality.Properties;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Actions
{
#pragma warning disable 0067
    [Extension("8F2DB8C8-4EA9-406E-8519-4FD5E643570E", "ImportQuestions", 101, ExecutionMode.Simplified)]
    public class ImportQuestions : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.View;
        public string Bar => "Questions";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportQuestions", "Import Questions", Properties.Resources.speech_balloon_question_big_floppy_disk,
                Properties.Resources.speech_balloon_question_floppy_disk)
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
                    case "ImportQuestions":
                        var openFileDialog = new OpenFileDialog()
                        {
                            DefaultExt = "tmq",
                            Filter = "Questions file (*.tmq)|*.tmq",
                            RestoreDirectory = true,
                            Title = "Import Questions",

                        };
                        if (openFileDialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            using (var file = File.OpenRead(openFileDialog.FileName))
                            {
                                using (var ms = new MemoryStream())
                                {
                                    file.CopyTo(ms);
                                    var jsonText = Encoding.Unicode.GetString(ms.ToArray());
                                    var questions =
                                        JsonConvert.DeserializeObject<ICollection<Question>>(jsonText, new JsonSerializerSettings()
                                        {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                                            TypeNameHandling = TypeNameHandling.All,
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
                                            SerializationBinder = new KnownTypesBinder(),
                                            MissingMemberHandling = MissingMemberHandling.Ignore
                                        })?.ToArray();

                                    if (questions?.Any() ?? false)
                                    {
                                        var schemaManager = new QuestionsPropertySchemaManager(threatModel);
                                        var existing = schemaManager.GetQuestions()?.ToArray();

                                        foreach (var question in questions)
                                        {
                                            if (!(existing?.Any(x =>
                                                string.CompareOrdinal(x.Text, question.Text) == 0) ?? false))
                                            {
                                                schemaManager.AddQuestion(question);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Question Import failed.\nThe Threat Model may have some questions applied.");
                throw;
            }
        }
    }
}