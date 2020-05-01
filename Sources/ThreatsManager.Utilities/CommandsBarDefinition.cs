using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    public class CommandsBarDefinition : ICommandsBarDefinition
    {
        public CommandsBarDefinition([Required] string name, [Required] string label, [NotNull] IEnumerable<IActionDefinition> commands)
        {
            Name = name;
            Label = label;
            Commands = commands;
        }

        public string Name { get; }
        public string Label { get; }
        public IEnumerable<IActionDefinition> Commands { get; }
    }
}
