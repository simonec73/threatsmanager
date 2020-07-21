using System.ComponentModel.Composition;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Initializers
{
    [Export(typeof(IExtensionInitializer))]
    [ExportMetadata("Id", "1A18A128-2C14-4C46-8CF6-1C25A62B9A3E")]
    [ExportMetadata("Label", "Automatic Threat Event Generation Initializer")]
    [ExportMetadata("Priority", 10)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Business)]
    public class AutoGenRuleInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(AndRuleNode));
            KnownTypesBinder.AddKnownType(typeof(BooleanRuleNode));
            KnownTypesBinder.AddKnownType(typeof(ComparisonOperator));
            KnownTypesBinder.AddKnownType(typeof(ComparisonRuleNode));
            KnownTypesBinder.AddKnownType(typeof(CrossTrustBoundaryRuleNode));
            KnownTypesBinder.AddKnownType(typeof(EnumValueRuleNode));
            KnownTypesBinder.AddKnownType(typeof(NaryRuleNode));
            KnownTypesBinder.AddKnownType(typeof(NotRuleNode));
            KnownTypesBinder.AddKnownType(typeof(OrRuleNode));
            KnownTypesBinder.AddKnownType(typeof(SelectionRule));
            KnownTypesBinder.AddKnownType(typeof(SelectionRuleNode));
            KnownTypesBinder.AddKnownType(typeof(UnaryRuleNode));
            KnownTypesBinder.AddKnownType(typeof(MitigationSelectionRule));
            KnownTypesBinder.AddKnownType(typeof(HasIncomingRuleNode));
            KnownTypesBinder.AddKnownType(typeof(HasOutgoingRuleNode));
            KnownTypesBinder.AddKnownType(typeof(TruismRuleNode));
            KnownTypesBinder.AddKnownType(typeof(EntityTemplateRuleNode));
            KnownTypesBinder.AddKnownType(typeof(FlowTemplateRuleNode));
            KnownTypesBinder.AddKnownType(typeof(TrustBoundaryTemplateRuleNode));
        }
    }
}