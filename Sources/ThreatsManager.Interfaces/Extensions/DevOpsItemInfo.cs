using System.Text.RegularExpressions;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Information related to an item stored in the DevOps system.
    /// </summary>
    public class DevOpsItemInfo
    {
        public DevOpsItemInfo([RegularExpression(@"(?<Id>[\d]+)#(?<Name>.*)")] string name)
        {
            var regex = new Regex(@"(?<Id>[\d]+)#(?<Name>.*)");
            var match = regex.Match(name);
            if (match.Success && int.TryParse(match.Groups["Id"].Value, out var id))
            {
                Id = id;
                Name = match.Groups["Name"].Value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier of the Item.</param>
        /// <param name="name">Name of the Item.</param>
        public DevOpsItemInfo([Positive] int id, [Required] string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Identifier of the Item.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Name of the Item.
        /// </summary>
        public string Name { get; }

        public string Serialize()
        {
            return $"{Id}#{Name}";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}