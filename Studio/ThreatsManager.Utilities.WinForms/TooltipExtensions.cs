using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Utilities.WinForms
{
    public static class TooltipExtensions
    {
        public static SuperTooltipInfo GetSuperTooltipInfo([NotNull] this IThreatModel model, 
            [NotNull] IIdentity identity, bool canJump = true)
        {
            IEnumerable<IDiagram> diagrams = null;

            string suffix = null;
            if (identity is IEntity)
            {
                diagrams = model.Diagrams?.Where(x => x.GetEntityShape(identity.Id) != null)
                    .OrderBy(x => x.Name).ToArray();
            } else if (identity is IDataFlow dataFlow)
            {
                var suffixBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(dataFlow.Source?.Name))
                    suffixBuilder.Append(
                        $"Source: <b>[{model.GetIdentityTypeInitial(dataFlow.Source)}] {dataFlow.Source.Name}</b>");
                if (!string.IsNullOrEmpty(dataFlow.Target?.Name))
                {
                    if (suffixBuilder.Length > 0)
                        suffixBuilder.Append("<br/>");
                    suffixBuilder.Append(
                        $"Target: <b>[{model.GetIdentityTypeInitial(dataFlow.Target)}] {dataFlow.Target.Name}</b>");
                }

                suffix = suffixBuilder.ToString();
                diagrams = model.Diagrams?.Where(x => x.GetLink(dataFlow.Id) != null)
                    .OrderBy(x => x.Name).ToArray();
            } else if (identity is ITrustBoundary trustBoundary)
            {
                diagrams = model.Diagrams?.Where(x => x.GetGroupShape(trustBoundary.Id) != null)
                    .OrderBy(x => x.Name).ToArray();
            }

            string body = null;
            if (string.IsNullOrWhiteSpace(suffix))
            {
                if (!string.IsNullOrWhiteSpace(identity.Description))
                {
                    body = identity.Description.Replace("\n", "<br/>");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(identity.Description))
                {
                    body = $"{identity.Description.Replace("\n", "<br/>")}<br/><br/>{suffix}";
                }
                else
                {
                    body = suffix;
                }
            }
            

            string footer = null;
            if (diagrams?.Any() ?? false)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Found in Diagram(s):");
                foreach (var diagram in diagrams)
                {
                    if (canJump)
                        builder.AppendLine($"<br/><a href='{diagram.Id}'>{diagram.Name}</a>");
                    else
                        builder.AppendLine($"<br/>{diagram.Name}");
                }
                footer = builder.ToString();
            }
            return new SuperTooltipInfo($"[{model.GetIdentityTypeInitial(identity)}] {identity.Name}", 
                footer, body, identity.GetImage(ImageSize.Big), null, eTooltipColor.Office2003);
        }

    }
}
