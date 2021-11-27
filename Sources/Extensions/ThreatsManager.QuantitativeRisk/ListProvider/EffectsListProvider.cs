using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.ListProvider
{
    [Extension("24233F79-60EB-4E03-AFE4-13CB0A938393", "Effects", 10, ExecutionMode.Pioneer)]
    public class EffectsListProvider : IListProviderExtension
    {
        public IEnumerable<IListItem> GetAvailableItems(string context)
        {
            List<IListItem> result = new List<IListItem>();

            var values = Enum.GetValues(typeof(Effect));
            foreach (var value in values)
            {
                var effect = (Effect) value;
                result.Add(new ListItem(effect.ToString(), effect.GetEnumLabel()));
            }

            return result;
        }
    }
}
