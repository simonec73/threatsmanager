using PostSharp.Patterns.Contracts;
using System.Xml.Linq;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps
{
    public class DevOpsField : IDevOpsField
    {
        public DevOpsField()
        {

        }

        public DevOpsField([Required] string id, [Required] string label)
        {
            Id = id;
            Label = label;
        }

        public string Id { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Label) ? ThreatModelManager.Undefined : Label;
        }
    }
}
