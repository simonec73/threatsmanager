using PostSharp.Patterns.Contracts;
using SampleWinFormExtensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace SampleWinFormExtensions.Initializers
{
    /// <summary>
    /// Initializers are called every time a new Threat Model is created. 
    /// </summary>
    /// <remarks>Initializers implement interface IInitializer.</remarks>
    [Extension("0C8C1CC5-B0F8-4D52-9874-4E9D73801AAD",
        "Definitions Schema Initializer", 10, ExecutionMode.Business)]
    public class DefinitionsSchemaInitializer : IInitializer
    {
        public void Initialize([NotNull] IThreatModel model)
        {
            var schemaManager = new DefinitionsPropertySchemaManager(model);

            var propertyType = schemaManager.DefinitionsPropertyType;
            if (propertyType != null)
            {
                var property = model.GetProperty(propertyType) ?? model.AddProperty(propertyType, null);

                if (property is IPropertyJsonSerializableObject jsonProperty)
                {
                    var definitions = new DefinitionContainer();
                    definitions.SetDefinition("Threat Model",
                        "Threat Modeling is a process to understand potential threat events to a system, determine risks from those threats, and establish appropriate mitigations.");
                    definitions.SetDefinition("Threat Events",
                        "Potential attack scenarios to the system. Who performs those attack scenarios is called Threat Actor.");
                    definitions.SetDefinition("Risks",
                        "Potential loss caused by the occurring of a Threat Event. It is typically expressed in monetary terms.");
                    definitions.SetDefinition("Mitigations",
                        "Actions that may decrease the Risk associated to a Threat Event.");

                    jsonProperty.Value = definitions;
                }
            }
        }
    }
}