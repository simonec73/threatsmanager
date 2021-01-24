using System;
using System.Drawing;
using System.Linq;
using System.Text;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    public class RoadmapMitigation : GoTextNode
    {
        private IMitigation _mitigation;

        public RoadmapMitigation([NotNull] IMitigation mitigation, Effectiveness effectiveness)
        {
            _mitigation = mitigation;

            Label = new GoText()
            {
                DragsNode = true,
                Selectable = false,
                Text = mitigation.Name, 
                Editable = false,
                Width = 200,
                Wrapping = false,
                StringTrimming = StringTrimming.EllipsisCharacter,
                FontSize = 9
            };
            switch (effectiveness)
            {
                case Effectiveness.Unknown:
                    Label.TextColor = Color.White;
                    Shape.BrushColor = ThreatModelManager.StandardColor;
                    break;
                case Effectiveness.Minor:
                    Label.TextColor = Color.Black;
                    Shape.BrushColor = Color.White;
                    Shape.PenColor = Color.DarkGreen;
                    break;
                case Effectiveness.Average:
                    Label.TextColor = Color.Black;
                    Shape.BrushColor = Color.LimeGreen;
                    Shape.PenColor = Color.LimeGreen;
                    break;
                case Effectiveness.Major:
                    Label.TextColor = Color.White;
                    Shape.BrushColor = Color.DarkGreen;
                    Shape.PenColor = Color.DarkGreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectiveness), effectiveness, null);
            }

            var builder = new StringBuilder();
            builder.AppendLine(mitigation.Name);
            if (effectiveness != Effectiveness.Unknown)
                builder.AppendLine($"Effectiveness: {effectiveness.ToString()}");
            var schemaManager = new EffortPropertySchemaManager(mitigation.Model);
            if (schemaManager.IsEffortSupported)
                builder.AppendLine($"Estimated Effort: {CalculateMaxEffort(schemaManager).ToString()}");

            ToolTipText = builder.ToString();
        }

        public IMitigation Mitigation => _mitigation;

        private Effort CalculateMaxEffort([NotNull] EffortPropertySchemaManager schemaManager)
        {
            Effort result = Effort.Undefined;

            var model = _mitigation.Model;
            var propertyType = schemaManager.GetPropertyType();
            if (model != null && propertyType != null)
            {
                CalculateMaxEffort(model, propertyType);

                var entities = model.Entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entity in entities)
                    {
                        var effort = CalculateMaxEffort(entity, propertyType);
                        if (effort > result)
                            result = effort;
                    }
                }

                var flows = model.DataFlows?.ToArray();
                if (flows?.Any() ?? false)
                {
                    foreach (var flow in flows)
                    {
                        var effort = CalculateMaxEffort(flow, propertyType);
                        if (effort > result)
                            result = effort;
                    }
                }
            }

            return result;
        }

        private Effort CalculateMaxEffort([NotNull] IThreatEventsContainer container, 
            [NotNull] IPropertyType propertyType)
        {
            Effort result = Effort.Undefined;

            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    var effort = threatEvent.Mitigations?
                        .FirstOrDefault(x => x.MitigationId == _mitigation.Id)?
                        .GetProperty(propertyType)?.StringValue?.GetEnumValue<Effort>();

                    if (effort.HasValue && effort > result)
                        result = effort.Value;
                }
            }

            return result;
        }
    }
}
