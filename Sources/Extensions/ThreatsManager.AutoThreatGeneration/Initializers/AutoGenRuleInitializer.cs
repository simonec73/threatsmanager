using System.Reflection;
using ThreatsManager.AutoThreatGeneration.Engine;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Initializers
{
    [Extension("1A18A128-2C14-4C46-8CF6-1C25A62B9A3E", "Automatic Threat Event Generation Initializer", 10, ExecutionMode.Business)]
    public class AutoGenRuleInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(MitigationSelectionRule));

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.AndRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.AndRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.BooleanRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.BooleanRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.ComparisonOperator", 
                typeof(ThreatsManager.AutoGenRules.Engine.ComparisonOperator));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.ComparisonRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.ComparisonRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.CrossTrustBoundaryRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.CrossTrustBoundaryRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.EnumValueRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.EnumValueRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.NaryRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.NaryRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.NotRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.NotRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.OrRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.OrRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.SelectionRule", 
                typeof(ThreatsManager.AutoGenRules.Engine.SelectionRule));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.SelectionRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.SelectionRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.UnaryRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.UnaryRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.HasIncomingRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.HasIncomingRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.HasOutgoingRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.HasOutgoingRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.TruismRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.TruismRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.EntityTemplateRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.EntityTemplateRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.ExternalInteractorTemplateRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.ExternalInteractorTemplateRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.ProcessTemplateRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.ProcessTemplateRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.DataStoreTemplateRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.DataStoreTemplateRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.FlowTemplateRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.FlowTemplateRuleNode));
            KnownTypesBinder.AddEquivalence(assemblyName, "ThreatsManager.AutoThreatGeneration.Engine.TrustBoundaryTemplateRuleNode", 
                typeof(ThreatsManager.AutoGenRules.Engine.TrustBoundaryTemplateRuleNode));
        }
    }
}