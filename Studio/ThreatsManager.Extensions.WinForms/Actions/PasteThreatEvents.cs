using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("1619C3FB-356E-49C7-9548-0E828928F426", "Paste Threat Events Context Aware Action", 25, ExecutionMode.Simplified)]
    public class PasteThreatEvents : IIdentityContextAwareAction, IDesktopAlertAwareExtension
    {
        public Scope Scope => Scope.Entity | Scope.Group | Scope.DataFlow;
        public string Label => "Paste Threat Events";
        public string Group => "Copy&Paste";
        public Bitmap Icon => null;
        public Bitmap SmallIcon => null;
        public Shortcut Shortcut => Shortcut.None;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public bool Execute(object item)
        {
            return (item is IIdentity identity) && Execute(identity);
        }

        public bool IsVisible(object item)
        {
            return true;
        }

        public bool Execute(IIdentity identity)
        {
            bool result = false;

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (identity is IThreatEventsContainer destContainer)
            {
                if (Clipboard.GetDataObject() is DataObject dataObject && 
                    dataObject.GetDataPresent("ThreatEventsInfo") &&
                    dataObject.GetData("ThreatEventsInfo") is string info)
                {
                    var deserialized = JsonConvert.DeserializeObject<ThreatEventsInfo>(info, new JsonSerializerSettings()
                    {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
                        TypeNameHandling = TypeNameHandling.All
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
                    });

                    if (deserialized?.ThreatEvents?.Any() ?? false)
                    {
                        foreach (var threatEvent in deserialized.ThreatEvents)
                        {
                            if (!(destContainer.ThreatEvents?.Any(x => x.ThreatTypeId == threatEvent.ThreatTypeId) ?? false))
                            {
                                AddThreatEvent(threatEvent, destContainer);
                                result = true;
                            }
                        }
                    }
                }
            } 

            if (result)
                ShowMessage?.Invoke("Threat Events have been copied successfully.");
            else
                ShowWarning?.Invoke("No Threat Event has been copied to the selected object.");

            return result;
        }

        private void AddThreatEvent([NotNull] IThreatEvent source, [NotNull] IThreatEventsContainer container)
        {
            var threatEvent = container.AddThreatEvent(source.ThreatType);
            threatEvent.Name = source.Name;
            threatEvent.Description = source.Description;
            threatEvent.Severity = source.Severity;
            source.CloneProperties(threatEvent);

            var scenarios = source.Scenarios?.ToArray();
            if (scenarios?.Any() ?? false)
            {
                foreach (var scenario in scenarios)
                {
                    var newScenario =
                        threatEvent.AddScenario(scenario.Actor, scenario.Severity, scenario.Name);
                    newScenario.Description = scenario.Description;
                    newScenario.Motivation = scenario.Motivation;
                    scenario.CloneProperties(newScenario);
                }
            }

            var mitigations = source.Mitigations?.ToArray();
            if (mitigations?.Any() ?? false)
            {
                foreach (var mitigation in mitigations)
                {
                    var newMitigation = threatEvent.AddMitigation(mitigation.Mitigation, mitigation.Strength,
                        mitigation.Status, mitigation.Directives);
                    mitigation.CloneProperties(newMitigation);
                }
            }
        }
    }
}