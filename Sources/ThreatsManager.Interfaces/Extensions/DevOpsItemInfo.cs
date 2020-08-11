using System.Text.RegularExpressions;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Information related to an item stored in the DevOps system.
    /// </summary>
    public class DevOpsItemInfo
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the Item.</param>
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

        /// <summary>
        /// Serialize the content of the object.
        /// </summary>
        /// <returns>Serialized object.</returns>
        public string Serialize()
        {
            return $"{Id}#{Name}";
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}