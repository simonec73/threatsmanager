using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Help;

namespace ThreatsManager.Extensions.Panels.Learning
{
#pragma warning disable CS0067
    public partial class LearningPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Learning";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                List<ICommandsBarDefinition> result = null;

                var levels = LearningManager.Instance.SupportedLevels?.ToArray();
                if (levels?.Any() ?? false)
                {
                    result = new List<ICommandsBarDefinition>()
                    {
                        new CommandsBarDefinition("Levels", "Levels", 
                            levels.Select(x => new ActionDefinition(Guid.NewGuid(), x.ToString(), x.GetEnumLabel(),
                                x.GetLevelImage(ImageSize.Big), x.GetLevelImage(ImageSize.Medium), true))),
                        new CommandsBarDefinition("Export", "Export", 
                            new []
                            {
                                new ActionDefinition(Guid.NewGuid(), "OpenWeb", "Open in Browser", 
                                    Properties.Resources.window_environment_big, Properties.Resources.window_environment),
                                new ActionDefinition(Guid.NewGuid(), "CopyUrl", "Copy URL", 
                                    Properties.Resources.copy_big, Properties.Resources.copy)

                            })
                    };
                }

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            //string text = null;
            //bool warning = false;

            switch (action.Name)
            {
                case "OpenWeb":
                    if (_tree.SelectedNode?.Tag is Page page)
                    {
                        ProcessStartInfo sInfo = new ProcessStartInfo(page.Url);
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                        Process.Start(sInfo);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    }
                    break;
                case "CopyUrl":
                    if (_tree.SelectedNode?.Tag is Page page2)
                    {
                        Clipboard.SetText(page2.Url);
                        ShowMessage?.Invoke("URL copied successfully.");
                    }
                    break;
                default:
                    var level = action.Name.GetEnumValue<LearningLevel>();
                    var topics = LearningManager.Instance.GetTopics(level)?.ToArray();

                    _tree.Nodes.Clear();

                    if (topics?.Any() ?? false)
                    {
                        var backStyle = new DevComponents.DotNetBar.ElementStyle()
                        {
                            Border = DevComponents.DotNetBar.eStyleBorderType.Solid,
                            BorderColor = ThreatModelManager.StandardColor,
                            BackColor = Color.White,
                            TextColor = Color.Black
                        };

                        foreach (var topic in topics)
                        {
                            Node parent = new Node(topic.Name)
                            {
                                Image = Resources.book,
                                ImageExpanded = Resources.book_open,
                                StyleSelected = backStyle
                            };
                            _tree.Nodes.Add(parent);

                            var lessons = topic.Lessons?.ToArray();
                            if (lessons?.Any() ?? false)
                            {
                                foreach (var lesson in lessons)
                                {
                                    var child = new Node(lesson.Name)
                                    {
                                        Image = level.GetLevelImage(ImageSize.Medium),
                                        Tooltip = lesson.Description,
                                        StyleSelected = backStyle,
                                        Tag = lesson
                                    };
                                    child.NodeClick += Child_NodeClick;
                                    parent.Nodes.Add(child);
                                }
                            }
                        }

                        _tree.ExpandAll();
                    }
                    break;
            }
        }

        private void Child_NodeClick(object sender, EventArgs e)
        {
            if (sender is Node node && node.Tag is Page lesson)
            {
                _browser.CoreWebView2.Navigate(lesson.Url);
            }
        }
    }
}