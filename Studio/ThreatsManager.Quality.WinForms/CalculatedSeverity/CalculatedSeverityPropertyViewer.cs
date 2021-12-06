using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    public class CalculatedSeverityPropertyViewer : IPropertyViewer
    {
        private readonly IPropertiesContainer _container;
        private readonly IProperty _property;

        public CalculatedSeverityPropertyViewer([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            _container = container;
            _property = property;
        }

        public IEnumerable<IPropertyViewerBlock> Blocks
        {
            get
            {
                IEnumerable<IPropertyViewerBlock> result = null;

                if (_container is IThreatEvent threatEvent)
                {
                    if (threatEvent.Model is IThreatModel model)
                    {
                        var schemaManager = new CalculatedSeverityPropertySchemaManager(model);
                        if (schemaManager.IsCalculatedSeverityEnabled)
                        {
                            var blocks = new List<IPropertyViewerBlock>();

                            var config = schemaManager.GetSeverityCalculationConfig(threatEvent);
                            var calculatedSeverity = threatEvent.GetCalculatedSeverity(config?.Delta ?? 0);
                            if (calculatedSeverity != null && calculatedSeverity.Id != threatEvent.SeverityId)
                            {
                                blocks.Add(new ApplyCalculatedSeverityPropertyViewerBlock(threatEvent, calculatedSeverity));
                            }

                            blocks.Add(new AdjustSeverityPropertyViewerBlock(threatEvent));

                            if (config != null && config.Delta != 0)
                            {
                                blocks.Add(new AdjustFactorPropertyViewerBlock(config));
                                blocks.Add(new AdjustReasonPropertyViewerBlock(config));
                                blocks.Add(new AdjustedByPropertyViewerBlock(config));
                                blocks.Add(new AdjustedOnPropertyViewerBlock(config));
                            }

                            result = blocks;
                        }
                    }
                }

                return result;
            }
        }
    }
}
